using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OK.CargoTracking.Helper
{
    public static class Extensions
    {
        public static HtmlNode SelectNodeById(this HtmlNode node, string id)
        {
            var tagElements = node.Descendants().ToList();
            return tagElements.SingleOrDefault(x => x.Id == id);
        }

        public static HtmlNode SelectNodeByClass(this HtmlNode node, string className)
        {
            var tagElements = node.Descendants().ToList();
            return tagElements.SingleOrDefault(x => x.Attributes["class"] != null && x.Attributes["class"].Value == className);
        }

        public static List<HtmlNode> SelectNodesByTag(this HtmlNode node, string tag)
        {
            return node.Descendants().Where(x => x.OriginalName == tag).ToList();
        }

        public static DateTime? GetDate(string str)
        {
            DateTime date;
            string[] formats = { "dd/MM/yyyy", "dd/MM/yyyy HH:mm", "dd/MM/yyyy HH:mm:ss", "dd.MM.yyyy", "dd.MM.yyyy HH:mm", "dd.MM.yyyy HH:mm:ss" };

            if (DateTime.TryParseExact(str, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date))
                return date;
            return null;
        }
    }
}
