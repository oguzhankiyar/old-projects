using System;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using OK.CargoTracking.Model;

namespace OK.CargoTracking.Helper
{
    public class ArasCargo : ICargoFactory
    {
        private string Code;
        private string UrlFormat = "http://appl-srv.araskargo.com.tr/CargoInfoV3.aspx?code={0}";
        private string DetailsUrl, MovementsUrl;
        private Tracking Tracking = new Tracking();

        public async Task<BaseResponse> GetResponseAsync(string code)
        {
            this.Code = code;
            string url = string.Format(this.UrlFormat, this.Code);
            string result = await Functions.GetHtmlSourceAsync(url);
            if (!GetURLs(result))
                return new BaseResponse() { Status = false };

            result = await Functions.GetHtmlSourceAsync(DetailsUrl);
            GetTrackingDetails(result);

            result = await Functions.GetHtmlSourceAsync(MovementsUrl);
            GetMovements(result);

            return new BaseResponse() { Result = Tracking };
        }

        private void GetTrackingDetails(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var node = doc.DocumentNode;
            Tracking.Code = this.Code;
            Tracking.Factory = CargoFactories.Aras;
            Tracking.ShippedUnit = Functions.TidyText(node.SelectNodeById("LabelIlkCikis").InnerText);
            Tracking.ShippedAt = Extensions.GetDate(node.SelectNodeById("cikis_tarihi").InnerText);
            Tracking.ArrivalUnit = Functions.TidyText(node.SelectNodeById("varis_subesi").InnerText);
            Tracking.LastState = Functions.TidyText(node.SelectNodeById("Son_Durum").InnerText);
            if (!string.IsNullOrEmpty(node.SelectNodeById("Teslim_Alan").InnerText))
            {
                Tracking.DeliveredBy = Functions.TidyText(node.SelectNodeById("Teslim_Alan").InnerText);
                Tracking.DeliveredAt = Extensions.GetDate(node.SelectNodeById("Teslim_Tarihi").InnerText);
            }
            if (Tracking.LastState.Contains("Teslim Edildi"))
                Tracking.IsDelivered = true;
        }

        private void GetMovements(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var node = doc.DocumentNode;
            var nodes = node.SelectNodeById("transactionsDataGrid").SelectNodesByTag("tr");
            for (int i = 1; i < nodes.Count(); i++)
            {
                var item = nodes[i] as HtmlNode;
                var movement = new Movement();
                movement.Location = Functions.TidyText(item.SelectNodesByTag("td")[2].InnerText) + " - " + Functions.TidyText(item.SelectNodesByTag("td")[1].InnerText);
                movement.Date = Extensions.GetDate(item.SelectNodesByTag("td")[0].InnerText);
                movement.State = Functions.TidyText(item.SelectNodesByTag("td")[3].InnerText);
                Tracking.Movements.Add(movement);
            }
        }

        private bool GetURLs(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var node = doc.DocumentNode;
            if (!node.InnerText.Contains("Kayıt Bulunamadı"))
            {
                DetailsUrl = "http://appl-srv.araskargo.com.tr/" + node.SelectNodeById("tabs").SelectNodesByTag("a")[0].Attributes["href"].Value.Replace("&amp;", "&");
                MovementsUrl = "http://appl-srv.araskargo.com.tr/" + node.SelectNodeById("tabs").SelectNodesByTag("a")[1].Attributes["href"].Value.Replace("&amp;", "&");
                return true;
            }
            return false;
        }

    }
}
