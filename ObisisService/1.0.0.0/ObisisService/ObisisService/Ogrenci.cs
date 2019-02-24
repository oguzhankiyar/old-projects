using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using ObisisService.MobileServiceReference;
using System.Data;
using HtmlAgilityPack;

namespace ObisisService
{
    [DataContract]
    public class Ogrenci
    {
        [DataMember]
        public string OgrenciNo { get; set; }
        [DataMember]
        public string Sifre { get; set; }
        [DataMember]
        public string AdSoyad { get; set; }
        [DataMember]
        public string Fakulte { get; set; }
        [DataMember]
        public string Bolum { get; set; }
        [DataMember]
        public int Sinif { get; set; }
        [DataMember]
        public double GANO { get; set; }
        [DataMember]
        public ICollection<Donem> Donemler { get; set; }

        public Ogrenci(string OgrenciNo = null, string Sifre = null)
        {
            this.OgrenciNo = OgrenciNo;
            this.Sifre = Sifre;
            this.Donemler = new HashSet<Donem>();

            if (!string.IsNullOrEmpty(OgrenciNo))
            {
                ObisisMobileServiceClient client = new ObisisMobileServiceClient();
                OgrenciResult or = client.OgrenciDonemGetir(OgrenciNo);
                DataTable dt = or.Data.Tables[0];
                foreach (DataRow item in dt.Rows)
                {
                    this.Donemler.Add(new Donem(this, item["DONEM_ADI"].ToString(), item["OGRETIM_YILI"].ToString(), Convert.ToInt32(item["OGRETIM_YILI_KODU"]), Convert.ToInt32(item["DONEM_NO"])));
                }
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(Function.GetHTML("http://obisis.erciyes.edu.tr/Default.aspx?tabInd=3&tabNo=5", OgrenciNo, Sifre));
                HtmlNode node = document.DocumentNode;
                try
                {
                    this.AdSoyad = Function.TidyText(node.SelectSingleNode("//span[@id='Banner1_Kullanici1_txtAdiSoyadi']").InnerText);
                    this.Fakulte = Function.TidyText(node.SelectSingleNode("//span[@id='Banner1_Kullanici1_txtFakulteAdi']").InnerText);
                    this.Bolum = Function.TidyText(node.SelectSingleNode("//span[@id='Banner1_Kullanici1_txtBolumAdi']").InnerText);
                    string SinifGano = node.SelectSingleNode("//span[@id='Banner1_Kullanici1_txtSinifSeneGano']").InnerText;
                    this.Sinif = Convert.ToInt32(SinifGano.Split('(')[1].Split('.')[0]);
                    this.GANO = Convert.ToDouble(SinifGano.Split(':')[1]);
                }
                catch (Exception) { }
            }
        }
    }
}