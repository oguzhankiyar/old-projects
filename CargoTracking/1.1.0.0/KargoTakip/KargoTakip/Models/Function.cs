using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace KargoTakip.Models
{
    class Function
    {
        public static void GecmiseEkle(Takip takip)
        {
            if (Data.Gecmis.Where(x => x.Kod == takip.Kod).Count() == 0)
                Data.Gecmis.Add(takip);
            else
                Data.Gecmis.SingleOrDefault(x => x.Kod == takip.Kod).SonGiris = DateTime.Now;
            DataSaver<List<Takip>> _dataSaver = new DataSaver<List<Takip>>();
            _dataSaver.SaveMyData(Data.Gecmis, "Gecmis");
        }
        public static void GecmisiYukle()
        {
            DataSaver<List<Takip>> _dataSaver = new DataSaver<List<Takip>>();
            Data.Gecmis = _dataSaver.LoadMyData("Gecmis") == null ? new List<Takip>() : _dataSaver.LoadMyData("Gecmis");
        }
        public static void GecmistenSil(Takip takip)
        {
            try
            {
                Data.Gecmis.Remove(takip);
            }
            catch(Exception)
            {

            }
            DataSaver<List<Takip>> _dataSaver = new DataSaver<List<Takip>>();
            _dataSaver.SaveMyData(Data.Gecmis, "Gecmis");
        }
        public static string TidyText(string text)
        {
            text = HttpUtility.HtmlDecode(text);
            text = text.Replace("&nbsp", " ");
            text = text.Trim();
            text = text.Replace("-", " - ").Replace("/", " / ").Replace("  ", "");
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
            return text.Trim();
        }
    }
}
