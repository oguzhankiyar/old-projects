using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Biletall.Helper;
using Biletall.Helper.Entities;
using Biletall.Helper.Parsings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TicketApp.Models
{
    public static class Logger
    {
        private static Object _lock = new Object();
        private static List<string> _buffer { get; set; }
        private static int _maxSize = 1000;

        static Logger()
        {
            _buffer = new List<string>();
            loadTodayLog();
        }

        public static void WriteLine(Exception e)
        {
            if (e == null)
            {
                WriteLine("an error occured");
                return;
            }

            WriteLine("EXCEPTION {0} {1}\n STACK TRACE {2}", e.Message, e.InnerException != null ? " HAS INNER EXCEPTION" : "", e.StackTrace);
            if (e.InnerException != null)
            {
                WriteLine(e.InnerException);
            }
            Logger.SendTodayLog();
        }

        public static void WriteLine(string format, params object[] args)
        {
            string s = string.Format(format, args);
            WriteLine(s);
        }

        public static void WriteLine(string line)
        {
            var sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss"));
            sb.Append(" - ");
            sb.Append(line);

            lock (_lock)
            {
                _buffer.Add(sb.ToString());
            }

            if (_buffer.Count() == _maxSize)
                _buffer.RemoveAt(0);

            if (System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debug.WriteLine(sb.ToString());
            }
        }

        internal static void HeaderChanged(string text)
        {
            WriteLine("header changed: {0}", text);
        }

        internal static void Clicked(string controlName)
        {
            WriteLine("clicked: {0}", controlName);
        }

        internal static void Navigated(Uri uri)
        {
            WriteLine("navigated: {0}", uri.OriginalString);
        }

        internal static void NavigationFailed(Exception exp)
        {
            WriteLine("navigation failed");
            WriteLine(exp);
        }

        internal static void MethodCalled(string name, object[] parameters = null)
        {
            if (parameters != null)
            {
                string paramsText = "";
                for (int i = 0; i < parameters.Length; i++)
                {
                    paramsText += "param" + (i + 1) + ": " + getObjectString(parameters[i]);
                }
                WriteLine("method called: {0}, params: {1}", name, paramsText);
            }
            else
            {
                WriteLine("method called: {0}", name);
            }
        }

        internal static void SelectionChanged(string control, object selection)
        {
            WriteLine("selection changed: {0}, selection: {1}", control, getObjectString(selection));
        }

        internal static void ValueChanged(string control, object value)
        {
            WriteLine("value changed: {0}, value: {1}", control, getObjectString(value));
        }

        internal static void RequestCanceled()
        {
            WriteLine("request canceled");
        }

        internal static void RequestCalled(string name, object[] parameters)
        {
            string paramsText = "";
            for (int i = 0; i < parameters.Length; i++)
            {
                paramsText += "param" + (i + 1) + ": " + getObjectString(parameters[i]);
            }
            WriteLine("request called: {0}, params: {1}", name, paramsText);
        }

        internal static void RequestParsed(object obj)
        {
            WriteLine("request parsed: {0}", getObjectString(obj));
        }

        internal static void RequestReturned(string response)
        {
            WriteLine("request returned: {0}", response);
        }

        internal static void RequestCompleted()
        {
            WriteLine("request completed");
        }

        internal static void ResultParsed(object response)
        {
            WriteLine("result parsed: {0}", getObjectString(response));
        }

        internal static void RequestFailed(Exception exp)
        {
            WriteLine("request failed");
            WriteLine(exp);
        }

        internal static void UnhandledException(Exception exp)
        {
            Logger.WriteLine("unhandled exception");
            Logger.WriteLine(exp);
        }

        public static void SaveStoredLog()
        {
            var sb = new StringBuilder();

            if (_buffer != null)
            {
                foreach (string s in _buffer)
                {
                    sb.AppendLine(s);
                }
            }
            
            var logDataSaver = new DataSaver<string>();
            logDataSaver.SaveMyData(sb.ToString(), "logs");
        }

        private static void loadTodayLog()
        {
            
            var logDataSaver = new DataSaver<string>();
            var oldTodayLog = logDataSaver.LoadMyData("logs");
            if (oldTodayLog != null)
            {
                foreach (var log in oldTodayLog.Split('\n'))
                {
                    _buffer.Add(log);
                }
            }
        }

        private static string getObjectString(object obj)
        {
            if (obj == null)
                return null;

            if (obj.GetType() == typeof(Ticket))
            {
                var ticket = obj as Ticket;
                var newTicket = new Ticket();
                newTicket.ActionType = ticket.ActionType;
                newTicket.Bill = ticket.Bill;
                if (!string.IsNullOrEmpty(ticket.CreditCard.OwnerName))
                    newTicket.CreditCard = new CreditCard() { OwnerName = "XXXX XXXX", CVC = "XXX", ExpirationMonth = 1, ExpirationYear = 1900, Number = "XXXX XXXX XXXX XXXX" };
                else
                    newTicket.CreditCard = new CreditCard();
                newTicket.Is3DBuyingActivated = ticket.Is3DBuyingActivated;
                newTicket.Is3DBuyingRequired = ticket.Is3DBuyingRequired;
                newTicket.IsBirthdayRequired = ticket.IsBirthdayRequired;
                newTicket.IsPassportRequired = ticket.IsPassportRequired;
                newTicket.Journeys = ticket.Journeys;
                newTicket.Passengers = ticket.Passengers;
                newTicket.PNR = ticket.PNR;
                newTicket.Price = ticket.Price;
                newTicket.Type = ticket.Type;
                return JsonConvert.SerializeObject(newTicket);
            }
            return JsonConvert.SerializeObject(obj);
        }

        static byte[] _byteArray;
        internal static void SendTodayLog()
        {
            Logger.SaveStoredLog();
            Database.SavedData.IsSentLogs = false;
            if (App.IsInternetAvailable)
                SendLog();
        }

        private static void SendLog()
        {
            try
            {
                var logDataSaver = new DataSaver<string>();
                var oldTodayLog = logDataSaver.LoadMyData("logs");

                if (oldTodayLog != null)
                {
                    var obj = new JObject();
                    obj.Add("version", Constraints.ApiVersion);
                    obj.Add("username", Constraints.ApiUsername);
                    obj.Add("password", Constraints.ApiPassword);
                    var jsonReq = new JObject();
                    jsonReq.Add("type", ApiAction.SendLog);
                    var logObj = new JObject();
                    var deviceObj = new JObject();
                    deviceObj.Add("id", DeviceHelper.Id);
                    deviceObj.Add("name", DeviceHelper.Name);
                    deviceObj.Add("platform", DeviceHelper.Platform);
                    deviceObj.Add("appVersion", DeviceHelper.AppVersion);
                    logObj.Add("device", deviceObj);
                    logObj.Add("actions", oldTodayLog);
                    jsonReq.Add("log", logObj);
                    obj.Add("request", jsonReq);

                    _byteArray = Encoding.UTF8.GetBytes(obj.ToString());
                    var request = HttpWebRequest.Create(ApiHelper.ApiUrl) as HttpWebRequest;
                    request.Method = "POST";
                    request.ContentType = "application/json; charset=UTF-8";
                    request.BeginGetRequestStream(SendDataCallBack, request);
                }
            }
            catch { }
        }

        private static void SendDataCallBack(IAsyncResult result)
        {
            try
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
            catch { }
        }

        private static void GetResponseCallBack(IAsyncResult result)
        {
            try
            {
                var request = result.AsyncState as HttpWebRequest;
                if (request != null)
                {
                    var response = request.EndGetResponse(result);
                    var reader = new StreamReader(response.GetResponseStream());
                    var jsonResult = reader.ReadToEnd();

                    var root = JObject.Parse(jsonResult);
                    if (Convert.ToBoolean(root["status"].ToString()))
                    {
                        Database.SavedData.IsSentLogs = true;
                        //RemoveLogs();
                    }
                }
            }
            catch { }
        }

        private static void RemoveLogs()
        {
            var logDataSaver = new DataSaver<string>();
            logDataSaver.SaveMyData(string.Empty, "logs");
        }
    }
}
