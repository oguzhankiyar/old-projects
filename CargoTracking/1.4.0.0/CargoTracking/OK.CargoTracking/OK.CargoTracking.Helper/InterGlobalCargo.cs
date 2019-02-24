using System;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using OK.CargoTracking.Model;

namespace OK.CargoTracking.Helper
{
    public class InterGlobalCargo : ICargoFactory
    {
        private string Code;
        private string UrlFormat = "http://www.globalkargo.com/kargotakip.aspx?tno={0}";
        private Tracking Tracking = new Tracking();

        public async Task<BaseResponse> GetResponseAsync(string code)
        {
            this.Code = code;
            string url = string.Format(this.UrlFormat, this.Code);
            string result = await Functions.GetHtmlSourceAsync(url);

            if (result.Contains("Track Information not Found"))
                return new BaseResponse() { Status = false };
            GetTrackingDetails(result);
            
            return new BaseResponse() { Result = Tracking };
        }

        private void GetTrackingDetails(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var node = doc.DocumentNode;
            Tracking.Code = this.Code;
            Tracking.Factory = CargoFactories.InterGlobal;
            /*Tracking.ShippedUnit = Functions.TidyText(node.SelectNodeById("LabelIlkCikis").InnerText);
            Tracking.ShippedAt = Extensions.GetDate(node.SelectNodeById("cikis_tarihi").InnerText);
            Tracking.ArrivalUnit = Functions.TidyText(node.SelectNodeById("varis_subesi").InnerText);
            Tracking.LastState = Functions.TidyText(node.SelectNodeById("Son_Durum").InnerText);*/

            var nodes = node.SelectNodeById("kargo_takip_sonuc").SelectNodesByTag("tr");
            for (int i = 0; i < nodes.Count(); i++)
            {
                var item = nodes[i] as HtmlNode;
                var movement = new Movement();
                movement.Date = Extensions.GetDate(item.SelectNodesByTag("span")[0].InnerText);
                movement.State = Functions.TidyText(item.SelectNodesByTag("span")[1].InnerText);
                Tracking.Movements.Add(movement);
            }
            if (Tracking.Movements.Any())
                Tracking.LastState = Tracking.Movements.First().State;
            
            if (Tracking.LastState.Contains("Teslim Alındı"))
                Tracking.IsDelivered = true;
        }
    }
}
