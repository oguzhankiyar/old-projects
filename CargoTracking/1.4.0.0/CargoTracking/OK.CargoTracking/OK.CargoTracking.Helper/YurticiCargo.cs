using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OK.CargoTracking.Model;
using HtmlAgilityPack;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace OK.CargoTracking.Helper
{
    public class YurticiCargo : ICargoFactory
    {
        private string Code;
        private string UrlFormat = "http://selfservis.yurticikargo.com/reports/SSWDocumentDetail.aspx?DocId={0}";
        private string MovementsUrlFormat = "http://selfservis.yurticikargo.com/ajaxpro/SelfService.Web.reports.SSWDocumentDetail,SelfService.Web.ashx";
        private Tracking Tracking = new Tracking();

        public async Task<BaseResponse> GetResponseAsync(string code)
        {
            this.Code = code;
            string url = string.Format(this.UrlFormat, this.Code);

            string result = await Functions.GetHtmlSourceAsync(url, null, null, new ISO88599Encoding());
            GetTrackingDetails(result);
            
            string postData = JsonConvert.SerializeObject(new { docId = this.Code });
            var headers = new Dictionary<string, string>();
            headers.Add("X-Ajaxpro-Method", "GetCargoHistory");
            headers.Add("User-Agent", "runscope/0.1");
            result = await Functions.GetHtmlSourceAsync(this.MovementsUrlFormat, new StringContent(postData), headers);
            GetMovements(result);

            if (Tracking == null)
                return new BaseResponse() { Status = false };
            else
                return new BaseResponse() { Result = Tracking };
        }
        
        public void GetTrackingDetails(string html)
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
            Tracking.Factory = CargoFactories.Yurtici;
            var nodes = node.SelectNodesByTag("tr");

            for (int i = 0; i < nodes.Count; i++)
            {
                var item = nodes[i];
                string text = item.SelectNodesByTag("td")[1].InnerText;
                if (i == 5)
                    Tracking.ShippedUnit = Functions.TidyText(text);
                else if (i == 4)
                    Tracking.ArrivalUnit = Functions.TidyText(text);
                else if (i == 0)
                    Tracking.ShippedAt = Extensions.GetDate(text.Replace(" -", ""));
                else if (i == 16)
                    Tracking.DeliveredAt = Extensions.GetDate(text.Replace(" -", ""));
                else if (i == 7)
                    Tracking.DeliveredBy = Functions.TidyText(text);
                else if (i == 14)
                    Tracking.LastState = Functions.TidyText(text);
            }
            if (Tracking.LastState.Contains("Teslim Edildi"))
                Tracking.IsDelivered = true;
        }

        public void GetMovements(string result)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            var node = doc.DocumentNode;
            var col = node.SelectNodesByTag("table")[0].SelectNodesByTag("tr");
            for (int i = 1; i < col.Count; i++)
            {
                var item = col[i];
                var movement = new Movement();
                movement.Date = Extensions.GetDate(item.SelectNodesByTag("td")[1].InnerText + " " + item.SelectNodesByTag("td")[2].InnerText);
                movement.State = Functions.TidyText(item.SelectNodesByTag("td")[3].InnerText);
                movement.Location = Functions.TidyText(item.SelectNodesByTag("td")[5].InnerText) + " - " +
                                      Functions.TidyText(item.SelectNodesByTag("td")[6].InnerText) + " " +
                                      Functions.TidyText(item.SelectNodesByTag("td")[7].InnerText);
                Tracking.Movements.Insert(0, movement);
            }
        }
    }
}
