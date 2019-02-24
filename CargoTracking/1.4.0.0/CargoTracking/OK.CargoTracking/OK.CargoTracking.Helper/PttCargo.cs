using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OK.CargoTracking.Model;
using Newtonsoft.Json.Linq;

namespace OK.CargoTracking.Helper
{
    public class PttCargo : ICargoFactory
    {
        private string Code;
        private string UrlFormat
        {
            get
            {
                if (this.IsInternational)
                    return "https://wap.ptt.gov.tr/cepptt/android/posta/yurtDisiKargoSorgula?barkod={0}";
                return "https://wap.ptt.gov.tr/cepptt/android/posta/gonderisorgu2?barkod={0}";
            }
        }
        private Tracking Tracking = new Tracking();
        public bool IsInternational;

        public async Task<BaseResponse> GetResponseAsync(string code)
        {
            this.Code = code;
            string url = string.Format(this.UrlFormat, this.Code);

            var headers = new Dictionary<string, string>();
            headers.Add("User-Agent", "android");
            string result = await Functions.GetHtmlSourceAsync(url, null, headers);
            GetTrackingDetails(result);

            if (Tracking == null)
                return new BaseResponse() { Status = false };
            else
                return new BaseResponse() { Result = Tracking };
        }

        private void GetTrackingDetails(string html)
        {
            if (this.IsInternational)
            {
                var obj = JObject.Parse(html);
                if (obj["sonucAciklama"].ToString().ToLower().Contains("bulunamadı"))
                {
                    Tracking = null;
                    return;
                }

                Tracking.Code = this.Code;
                Tracking.Factory = CargoFactories.PttInternational;
                Tracking.DeliveredBy = Functions.TidyText(obj["teslim_alan"].ToString());
                Tracking.ShippedUnit = " - ";
                Tracking.ArrivalUnit = " - ";
                Tracking.ShippedAt = null;
                try
                {
                    JArray array = obj["dongu"] as JArray;
                    for (int i = 0; i < array.Count; i++)
                    {
                        var obj2 = array[i] as JObject;
                        Tracking.ShippedUnit = Functions.TidyText(array[0]["islemYeri"].ToString());
                        Tracking.ArrivalUnit = !string.IsNullOrEmpty(obj["teslim_alan"].ToString()) ? Functions.TidyText(array[array.Count - 1]["islemYeri"].ToString()) : " - ";
                        Tracking.ShippedAt = Convert.ToDateTime(array[0]["tarih"].ToString());
                        Tracking.LastState = array[array.Count - 1]["yapilanIslem"].ToString();
                        var movement = new Movement();
                        try
                        {
                            movement.Location = Functions.TidyText(obj2["ofis"].ToString()) + " - " + Functions.TidyText(obj2["islemYeri"].ToString());
                        }
                        catch { }
                        movement.Date = Convert.ToDateTime(obj2["tarih"].ToString());
                        movement.State = obj2["yapilanIslem"].ToString();
                        if (movement.State.ToLower().Contains("teslim edildi"))
                            Tracking.IsDelivered = true;
                        Tracking.Movements.Insert(0, movement);
                    }
                }
                catch (Exception) { }
            }
            else
            {
                var obj = JObject.Parse(html);
                if (obj["sonucAciklama"].ToString().ToLower().Contains("bulunamadı"))
                {
                    Tracking = null;
                    return;
                }

                Tracking.Code = this.Code;
                Tracking.Factory = CargoFactories.Ptt;
                Tracking.DeliveredBy = Functions.TidyText(obj["TESALAN"].ToString());
                Tracking.ShippedUnit = Functions.TidyText(obj["IMERK"].ToString());
                Tracking.ArrivalUnit = Functions.TidyText(obj["VMERK"].ToString());
                string dateString = obj["ITARIH"].ToString();
                Tracking.ShippedAt = Extensions.GetDate(dateString.Substring(6) + "." + dateString.Substring(4, 2) + "." + dateString.Substring(0, 4));
                try
                {
                    JArray array = obj["dongu"] as JArray;
                    for (int i = 0; i < array.Count; i++)
                    {
                        var obj2 = array[i] as JObject;
                        var movement = new Movement();
                        try
                        {
                            movement.Location = Functions.TidyText(obj2["IMERK"].ToString());
                        }
                        catch { }
                        movement.Date = Extensions.GetDate(obj2["ITARIH"].ToString());
                        movement.State = obj2["ISLEM"].ToString();
                        if (movement.State.ToLower().Contains("teslim edildi"))
                            Tracking.IsDelivered = true;
                        Tracking.Movements.Insert(0, movement);
                    }
                }
                catch (Exception) { }
            }
        }
    }
}
