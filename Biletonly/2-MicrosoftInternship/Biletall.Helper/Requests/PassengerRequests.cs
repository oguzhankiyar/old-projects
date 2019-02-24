using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Biletall.Helper.Entities;
using Biletall.Helper.Enums;
using Biletall.Helper.Parsings;

namespace Biletall.Helper.Requests
{
    public class PassengerRequests
    {
        public static Action<BaseResponse<bool>> OnVerificationCompleted { get; set; }
        public static Action<BaseResponse<bool>> OnListVerificationCompleted { get; set; }
        private static byte[] _byteArray;
        private static int _completedCount = 0;

        public static void VerifyPassengers(List<Passenger> passengers)
        {
            Global.OnRequestCalled("PassengerRequests.VerifyPassengers", new object[] { passengers });
            _completedCount = 0;
            if (passengers.Any())
            {
                PassengerRequests.OnVerificationCompleted = (response) =>
                {
                    _completedCount++;
                    if (!response.Result && OnListVerificationCompleted != null)
                    {
                        var passengerResponse = new BaseResponse<bool> { Result = false };
                        Global.OnResultParsed(passengerResponse);
                        OnListVerificationCompleted(passengerResponse);
                        Global.OnRequestCompleted();
                    }
                    else if (_completedCount == passengers.Count() && OnListVerificationCompleted != null)
                    {
                        var passengerResponse = new BaseResponse<bool> { Result = true };
                        Global.OnResultParsed(passengerResponse);
                        OnListVerificationCompleted(passengerResponse);
                        Global.OnRequestCompleted();
                    }
                    else
                        PassengerRequests.VerifyPassenger(passengers[_completedCount]);
                };
                PassengerRequests.VerifyPassenger(passengers[0]);
            }
        }

        public static void VerifyPassenger(Passenger passenger)
        {
            Global.OnRequestCalled("PassengerRequests.VerifyPassenger", new object[] { passenger });
            try
            {
                if (!FirstControl(passenger) && OnVerificationCompleted != null)
                    OnVerificationCompleted(new BaseResponse<bool>() { Result = false });
                else
                    GetResultWithPost(passenger);
            }
            catch (Exception)
            {
                if (OnVerificationCompleted != null)
                    OnVerificationCompleted(new BaseResponse<bool>() { Result = true });
            }
        }

        private static bool FirstControl(Passenger passenger)
        {
            try
            {
                string tckn = passenger.TCKN.ToString();
                if (tckn.Length != 11 || tckn[0] == '0')
                    return false;

                int number1 = Convert.ToInt32(tckn[0].ToString());
                int number2 = Convert.ToInt32(tckn[1].ToString());
                int number3 = Convert.ToInt32(tckn[2].ToString());
                int number4 = Convert.ToInt32(tckn[3].ToString());
                int number5 = Convert.ToInt32(tckn[4].ToString());
                int number6 = Convert.ToInt32(tckn[5].ToString());
                int number7 = Convert.ToInt32(tckn[6].ToString());
                int number8 = Convert.ToInt32(tckn[7].ToString());
                int number9 = Convert.ToInt32(tckn[8].ToString());
                int number10 = Convert.ToInt32(tckn[9].ToString());
                int number11 = Convert.ToInt32(tckn[10].ToString());

                int firstTotal = number1 + number3 + number5 + number7 + number9;
                int secondTotal = number2 + number4 + number6 + number8;
                int tenthNumber = (firstTotal * 7 - secondTotal) % 10;
                if (tckn[9].ToString() != tenthNumber.ToString())
                    return false;

                int eleventhNumber = (firstTotal + secondTotal + tenthNumber) % 10;
                if (tckn[10].ToString() != eleventhNumber.ToString())
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static void GetResultWithPost(Passenger passenger)
        {
            if (string.IsNullOrEmpty(passenger.FirstName) || string.IsNullOrEmpty(passenger.LastName) || passenger.Birthday == null)
            {
                if (OnVerificationCompleted != null)
                    OnVerificationCompleted(new BaseResponse<bool>() { Result = false });
                return;
            }

            string firstName = passenger.FirstName.Replace("i", "İ").ToUpper();
            string lastName = passenger.LastName.Replace("i", "İ").ToUpper();

            var sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.Append("<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">");
            sb.Append("<soap:Body>");
            sb.Append("<TCKimlikNoDogrula xmlns=\"http://tckimlik.nvi.gov.tr/WS\">");
            sb.Append("<TCKimlikNo>" + passenger.TCKN + "</TCKimlikNo>");
            sb.Append("<Ad>" + firstName + "</Ad>");
            sb.Append("<Soyad>" + lastName + "</Soyad>");
            sb.Append("<DogumYili>" + ((DateTime)passenger.Birthday).Year + "</DogumYili>");
            sb.Append("</TCKimlikNoDogrula>");
            sb.Append("</soap:Body>");
            sb.Append("</soap:Envelope>");

            _byteArray = Encoding.UTF8.GetBytes(sb.ToString());

            var request = HttpWebRequest.Create("https://tckimlik.nvi.gov.tr/Service/KPSPublic.asmx");
            request.ContentType = "text/xml; charset=utf-8";
            request.Method = "POST";
            request.BeginGetRequestStream(SendDataCallBack, request);
        }

        private static void SendDataCallBack(IAsyncResult result)
        {
            var request = result.AsyncState as HttpWebRequest;
            if (request != null)
            {
                using (var stream = request.EndGetRequestStream(result))
                {
                    stream.Write(_byteArray, 0, _byteArray.Length);
                    stream.Close();
                }
                request.BeginGetResponse(GetResponseCallBack, request);
            }
        }

        private static void GetResponseCallBack(IAsyncResult result)
        {
            var request = result.AsyncState as HttpWebRequest;
            if (request != null)
            {
                var response = request.EndGetResponse(result);
                var reader = new StreamReader(response.GetResponseStream());
                var xmlResult = reader.ReadToEnd();

                Global.Invoke(() =>
                {
                    if (OnVerificationCompleted != null)
                    {
                        var state = XElement.Parse(xmlResult).Value == "true";
                        Global.OnResultParsed(state);
                        OnVerificationCompleted(new BaseResponse<bool>() { Result = state });
                        Global.OnRequestCompleted();
                    }
                });
            }
        }
    }
}
