using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OK.CargoTracking.Model;
using HtmlAgilityPack;
using System.Net.Http;

namespace OK.CargoTracking.Helper
{
    public class MngCargo : ICargoFactory
    {
        private string Code;
        private string UrlFormat = "http://service.mngkargo.com.tr/iactive/popup/kargotakip.asp?k={0}";
        private Tracking Tracking = new Tracking();

        public async Task<BaseResponse> GetResponseAsync(string code)
        {
            this.Code = code;
            string url = string.Format(this.UrlFormat, this.Code);

            string result = await Functions.GetHtmlSourceAsync(url, null, null, new ISO88599Encoding());
            GetTrackingDetails(result);

            if (Tracking == null)
                return new BaseResponse() { Status = false };
            else
                return new BaseResponse() { Result = Tracking };
        }

        private void GetTrackingDetails(string html)
        {
            if (html.Contains("Aradığınız Alım Belgesi Numarasına Ait Kayıt Bulunamadı.") ||
                html.Contains("Giriş Yaptığınız Numara Geçerli Bir Fatura Numarası Değildir.") ||
                html.Contains("Aradığınız Gönderi Numarasına Ait Kayıt Bulunamadı."))
            {
                Tracking = null;
                return;
            }

            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var node = doc.DocumentNode;

            Tracking.Code = this.Code;
            Tracking.Factory = CargoFactories.Mng;
            Tracking.ShippedUnit = Functions.TidyText(node.SelectNodeByClass("tkp-fatura-birim").InnerText);
            Tracking.ArrivalUnit = Functions.TidyText(node.SelectNodeByClass("tkp-teslim-birim").InnerText);
            Tracking.ShippedAt = Extensions.GetDate(node.SelectNodeByClass("tkp-fatura-tarih").InnerText);
            
            if (node.SelectNodeByClass("tkp-teslim-tarih").InnerText.Trim() != "-")
            {
                string dateString = node.SelectNodeByClass("tkp-teslim-tarih").InnerText + " " + node.SelectNodeByClass("tkp-teslim-saat").InnerText;
                Tracking.DeliveredAt = Extensions.GetDate(dateString);
                Tracking.DeliveredBy = Functions.TidyText(node.SelectNodeByClass("tkp-teslim-alan").InnerText);
            }
            try
            {
                var movement = new Movement();
                if (node.SelectNodeByClass("tkp-teslim-tarih").InnerText.Trim() != "-")
                {
                    movement.Location = Tracking.ArrivalUnit;
                    movement.Date = Tracking.DeliveredAt;
                    movement.State = "Teslim edildi.";
                    Tracking.Movements.Add(movement);
                    Tracking.IsDelivered = true;
                }
                Tracking.LastState = Functions.TidyText(node.SelectNodeByClass("tkp-hareket-aciklama").InnerText);
                movement = new Movement();
                if (node.SelectNodeByClass("tkp-hareket-tarih").InnerText.Trim() != "-")
                    movement.Date = Extensions.GetDate(node.SelectNodeByClass("tkp-hareket-tarih").InnerText.Trim() + " " + node.SelectNodeByClass("tkp-hareket-saat").InnerText.Trim());
                else
                    movement.Date = null;
                if (!string.IsNullOrEmpty(node.SelectNodeByClass("tkp-hareket-tur").InnerText))
                    movement.State = Functions.TidyText(node.SelectNodeByClass("tkp-hareket-tur").InnerText);
                Tracking.Movements.Add(movement);
            }
            catch (Exception) { }
        }
    }
}
