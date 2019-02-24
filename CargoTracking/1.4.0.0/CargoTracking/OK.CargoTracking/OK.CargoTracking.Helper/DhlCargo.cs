using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OK.CargoTracking.Model;
using HtmlAgilityPack;

namespace OK.CargoTracking.Helper
{
    public class DhlCargo : ICargoFactory
    {
        private string Code;
        private string UrlFormat = "http://www.dhl.com.tr/content/tr/tr/express/tracking.shtml?brand=DHL&AWB={0}";
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
            if (html.Contains("Talebiniz için herhangi bir sonuç bulunamadı"))
            {
                Tracking = null;
                return;
            }
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var node = doc.DocumentNode;
            doc.LoadHtml(node.SelectNodeById("table" + this.Code + "']").InnerHtml);
            node = doc.DocumentNode;

            Tracking.Code = this.Code;
            Tracking.Factory = CargoFactories.Dhl;
            Tracking.ShippedUnit = Functions.TidyText(node.SelectNodeByClass("tophead").SelectNodesByTag("th")[2].SelectNodesByTag("span")[0].InnerText);
            Tracking.ArrivalUnit = Functions.TidyText(node.SelectNodeByClass("tophead").SelectNodesByTag("th")[2].SelectNodesByTag("span")[1].InnerText);
            var col = node.SelectNodesByTag("tr");
            DateTime? currentDate = null;
            int j = 0;
            for (int i = 2; i < col.Count; i++)
            {
                var item = col[i];
                if (item.ParentNode.Name == "thead" && !string.IsNullOrEmpty(item.InnerText.Replace("\r", "").Replace("\n", "").Replace("\t", "")))
                {
                    currentDate = DateTime.Now.Date; //item.SelectNodesByTag("th")[0].InnerText.Trim();
                }
                else if (item.ParentNode.Name == "tbody")
                {
                    j++;
                    var movement = new Movement();
                    movement.Location = item.SelectNodesByTag("td")[2].InnerText.Trim();
                    movement.State = item.SelectNodesByTag("td")[1].InnerText.Trim();
                    movement.Date = DateTime.Now.Date; //currentDate + " " + item.SelectNodesByTag("td")[3].InnerText.Replace("at", ", ").Replace("  ", " ").Trim();
                    Tracking.Movements.Insert(0, movement);
                    if (j == 1)
                    {
                        Tracking.LastState = movement.State;
                        if (Tracking.LastState.Contains("teslim edildi"))
                        {
                            try
                            {
                                Tracking.DeliveredBy = Functions.TidyText(Tracking.LastState.Split(':')[1].Trim());
                                Tracking.DeliveredAt = movement.Date;
                            }
                            catch (Exception) { }
                        }
                    }
                }
            }
            Tracking.ShippedAt = currentDate;
        }
    }
}