using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CargoTracking.Models
{
    public class Function
    {
        public static string TidyText(string text)
        {
            try
            {
                text = HttpUtility.HtmlDecode(text);
                text = text.Replace("&nbsp", " ");
                text = text.Trim();
                text = text.Replace("-", " - ").Replace("/", " / ").Replace("  ", "");
                if (!string.IsNullOrEmpty(text.Replace("-", "").Replace("/", "").Trim()))
                {
                    string[] split = text.Split(' ');
                    text = "";
                    for (int i = 0; i < split.Length; i++)
                    {
                        char[] chars = split[i].ToCharArray();
                        split[i] = chars[0].ToString().ToUpper();
                        for (int j = 1; j < chars.Length; j++)
                        {
                            split[i] += chars[j].ToString().ToLower();
                        }
                        text += split[i] + " ";
                    }
                    text = text.Replace("VE", "ve");
                }
                return text.Trim();
            }
            catch (Exception)
            {
                return text.Trim();
            }
        }
    }
}
