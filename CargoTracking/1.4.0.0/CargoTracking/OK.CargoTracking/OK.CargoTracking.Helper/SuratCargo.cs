using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OK.CargoTracking.Model;
using HtmlAgilityPack;

namespace OK.CargoTracking.Helper
{
    public class SuratCargo : ICargoFactory
    {
        private string Code;
        private string UrlFormat = "http://suratkargo.com.tr/?p=kargom_nerede_post&TakipNo={0}";
        private Tracking Tracking = new Tracking();

        public async Task<BaseResponse> GetResponseAsync(string code)
        {
            this.Code = code;
            string url = string.Format(this.UrlFormat, this.Code);

            string result = await Functions.GetHtmlSourceAsync(url);
            GetTrackingDetails(result);

            if (Tracking == null)
                return new BaseResponse() { Status = false };
            else
                return new BaseResponse() { Result = Tracking };
        }

        private void GetTrackingDetails(string html)
        {
            if (html.Contains("Kayıt Bulunamadı"))
            {
                Tracking = null;
                return;
            }

            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var node = doc.DocumentNode;

            Tracking.Code = this.Code;
            Tracking.Factory = CargoFactories.Surat;
            Tracking.ShippedUnit = Functions.TidyText(node.SelectNodeByClass("online_bg_orta").SelectNodesByTag("div")[1].SelectNodesByTag("div")[13].InnerText);
            Tracking.ShippedAt = Extensions.GetDate(node.SelectNodesByTag("tr")[1].SelectNodesByTag("td")[1].InnerHtml.Replace("<br>", " "));
            Tracking.ArrivalUnit = Functions.TidyText(node.SelectNodeByClass("online_bg_orta").SelectNodesByTag("div")[1].SelectNodesByTag("div")[31].InnerText);
            
            if (!string.IsNullOrEmpty(node.SelectNodesByTag("tr")[1].SelectNodesByTag("td")[5].InnerText))
            {
                Tracking.DeliveredBy = Functions.TidyText(node.SelectNodesByTag("tr")[1].SelectNodesByTag("td")[5].InnerText);
                Tracking.DeliveredAt = Extensions.GetDate(node.SelectNodesByTag("tr")[1].SelectNodesByTag("td")[3].InnerHtml.Replace("<br>", " "));
            }
            
            Tracking.LastState = Functions.TidyText(node.SelectNodesByTag("tr")[1].SelectNodesByTag("td")[6].InnerText);
            if (node.SelectNodesByTag("tr")[1].SelectNodesByTag("td")[6].InnerHtml.Contains("Teslim Edildi"))
                Tracking.IsDelivered = true;
            var movement = new Movement();
            movement.Date = Extensions.GetDate(node.SelectNodesByTag("tr")[1].SelectNodesByTag("td")[3].InnerHtml.Replace("<br>", " "));
            movement.Location = Functions.TidyText(node.SelectNodesByTag("tr")[1].SelectNodesByTag("td")[8].InnerText);
            movement.State = Functions.TidyText(node.SelectNodesByTag("tr")[1].SelectNodesByTag("td")[7].InnerText);
            Tracking.Movements.Add(movement);
        }
    }
}
