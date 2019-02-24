using System;
using System.Threading.Tasks;
using OK.CargoTracking.Model;
using System.Net;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace OK.CargoTracking.Helper
{
    public class UpsCargo : ICargoFactory
    {
        private string Code;
        private string UrlFormat = "http://ups.com.tr/WaybillSorgu.aspx?Waybill={0}";
        private static CookieContainer myCookieContainer = new CookieContainer();
        private Tracking Tracking = new Tracking();

        public async Task<BaseResponse> GetResponseAsync(string code)
        {
            this.Code = code;
            string url = string.Format(this.UrlFormat, this.Code);

            string result = await Functions.GetHtmlSourceAsync(url);
            GetTrackingDetails(result);

            /*
            using (var handler = new HttpClientHandler() { CookieContainer = myCookieContainer })
            using (var client = new HttpClient(handler))
            {
                string postData = GetPostData(result);
                var headers = new Dictionary<string, string>();
                headers.Add("X-MicrosoftAjax", "Delta=true");
                headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.131 Safari/537.36");
                result = await Functions.GetHtmlSourceAsync(client, url, new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded; charset=UTF-8"), headers);
            }

            GetMovements(result);*/

            if (Tracking == null)
                return new BaseResponse() { Status = false };
            else
                return new BaseResponse() { Result = Tracking };
        }

        private string GetPostData(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var node = doc.DocumentNode;

            string viewState = node.SelectNodeById("__VIEWSTATE").Attributes["value"].Value;
            string eventTarget = "ctl00$MainContent$LinkButtonSonIslemGoster";
            string eventValidation = node.SelectNodeById("__EVENTVALIDATION").Attributes["value"].Value;
            string eventArgument = "";

            return string.Format(
                    "__VIEWSTATE={0}&__EVENTVALIDATION={1}&__EVENTTARGET={2}&__EVENTARGUMENT={3}&loc={4}&results={5}&view={6}",
                    WebUtility.UrlEncode(viewState),
                    WebUtility.UrlEncode(eventValidation),
                    WebUtility.UrlEncode(eventTarget),
                    WebUtility.UrlEncode(eventArgument),
                    WebUtility.UrlEncode("tr_TR"),
                    WebUtility.UrlEncode("25"),
                    WebUtility.UrlEncode("both"));
        }

        private void GetTrackingDetails(string html)
        {
            if (html.Contains("Takip Numarasına ait bilgi bulunamamaktadır"))
            {
                Tracking = null;
                return;
            }

            if (!html.Contains("ctl00_MainContent_PanelSorguSonucTeslimEdilmedi"))
                Tracking.IsDelivered = true;

            string subfix = "";
            if (!Tracking.IsDelivered)
                subfix = "TeslimEdilmedi";

            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var node = doc.DocumentNode;
            Tracking.Code = this.Code;
            Tracking.Factory = CargoFactories.Ups;
            Tracking.ShippedUnit = Functions.TidyText(node.SelectNodeById("ctl00_MainContent_Label" + subfix + "AliciIl").InnerText) + " - " + Functions.TidyText(node.SelectNodeById("ctl00_MainContent_Label" + subfix + "AliciIlce").InnerText);
            if (node.SelectNodeById("ctl00_MainContent_Label3") != null)
                Tracking.LastState = Functions.TidyText(node.SelectNodeById("ctl00_MainContent_Label3").InnerText);
            if (node.SelectNodeById("ctl00_MainContent_LabelTeslimAlan") != null)
            {
                Tracking.DeliveredBy = Functions.TidyText(node.SelectNodeById("ctl00_MainContent_LabelTeslimAlan").InnerText);
                Tracking.DeliveredAt = Extensions.GetDate(node.SelectNodeById("ctl00_MainContent_LabelTeslimTarihi").InnerText);
            }

            if (node.SelectNodeById("ctl00_MainContent_DataListSonIslem") != null)
                GetMovements(html);
        }

        private void GetMovements(string result)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            var node = doc.DocumentNode;
            var col = node.SelectNodeById("ctl00_MainContent_DataListSonIslem").SelectNodesByTag("table");
            for (int i = 1; i < col.Count; i++)
            {
                var item = col[i];
                var movement = new Movement();
                movement.Date = Convert.ToDateTime(item.SelectNodesByTag("td")[0].InnerText);
                movement.State = Functions.TidyText(item.SelectNodesByTag("td")[1].InnerText);
                movement.Location = Functions.TidyText(item.SelectNodesByTag("td")[2].InnerText);
                Tracking.Movements.Add(movement);
            }
        }
    }
}
