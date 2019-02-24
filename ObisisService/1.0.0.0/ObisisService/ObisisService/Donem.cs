using HtmlAgilityPack;
using ObisisService.MobileServiceReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ObisisService
{
    [DataContract]
    public class Donem
    {
        [DataMember]
        public int No { get; set; }
        [DataMember]
        public string Adi { get; set; }
        [DataMember]
        public string OgretimYili { get; set; }
        [DataMember]
        public int OgretimYiliKodu { get; set; }
        [DataMember]
        public double GANO { get; set; }
        [DataMember]
        public virtual ICollection<Ders> Dersler { get; set; }

        public Donem(Ogrenci ogrenci, string donem_adi, string ogretim_yili, int yil_kodu, int donem_no)
        {
            if (!string.IsNullOrEmpty(ogrenci.OgrenciNo))
            {
                this.Dersler = new HashSet<Ders>();
                this.Adi = Function.TidyText(donem_adi);
                this.No = donem_no;
                this.OgretimYili = ogretim_yili;
                this.OgretimYiliKodu = yil_kodu;
                ObisisMobileServiceClient client = new ObisisMobileServiceClient();
                OgrenciResult or = client.OgrenciBilgiGetir(ogrenci.OgrenciNo, ogrenci.Sifre, yil_kodu, donem_no);
                DataTable dt = or.Data.Tables[0];
                HtmlDocument document = new HtmlDocument();
                try
                {
                    document.LoadHtml(Function.GetHTML("http://obisis.erciyes.edu.tr/Default.aspx?tabInd=4&tabNo=8", ogrenci.OgrenciNo, ogrenci.Sifre));
                    this.GANO = Convert.ToDouble(document.DocumentNode.SelectSingleNode("//*[contains(text(), '" + ogretim_yili + " " + donem_adi + "')]").InnerText.Split(':')[1].Trim());
                }
                catch (Exception)
                {
                    try
                    {
                        document.LoadHtml(Function.GetHTML("http://obisis.erciyes.edu.tr/Default.aspx?tabInd=5&tabNo=8", ogrenci.OgrenciNo, ogrenci.Sifre));
                        this.GANO = Convert.ToDouble(document.DocumentNode.SelectSingleNode("//*[contains(text(), '" + ogretim_yili + " " + donem_adi + "')]").InnerText.Split(':')[1].Trim());
                    }
                    catch (Exception)
                    {
                        document.LoadHtml(Function.GetHTML("http://obisis.erciyes.edu.tr/Default.aspx?tabInd=6&tabNo=8", ogrenci.OgrenciNo, ogrenci.Sifre));
                        this.GANO = Convert.ToDouble(document.DocumentNode.SelectSingleNode("//*[contains(text(), '" + ogretim_yili + " " + donem_adi + "')]").InnerText.Split(':')[1].Trim());
                    }
                }
                int i = 1;
                foreach (DataRow item in dt.Rows)
                {
                    HtmlNode node = document.DocumentNode.SelectSingleNode("//tr[contains(td[1], '" + ogretim_yili + " " + donem_adi + "')]/following-sibling::tr[" + i++ + "]");
                    double kredi = 0.0;
                    int sinif = 1;
                    bool ortalamayaEtki = true;
                    double basariKatsayisi = 0.0;
                    string kod = "";
                    string text = node.InnerText;
                    if (node != null)
                    {
                        kod = node.SelectSingleNode("./td[1]").InnerText;
                        kredi = Convert.ToDouble(node.SelectSingleNode("./td[6]").InnerText.Trim());
                        sinif = Convert.ToInt32(node.SelectSingleNode("./td[3]").InnerText);
                        ortalamayaEtki = node.SelectSingleNode("./td[8]").InnerText != "&nbsp;" && node.SelectSingleNode("./td[9]").InnerText == "&nbsp;" ? false : true;
                        basariKatsayisi = node.SelectSingleNode("./td[9]").InnerText == "&nbsp;" ? 0.0 : Convert.ToDouble(node.SelectSingleNode("./td[9]").InnerText.Trim());
                    }
                    this.Dersler.Add(new Ders()
                    {
                        Adi = Function.TidyText(item["DERS_ADI_TR"].ToString()),
                        Vize1 = string.IsNullOrEmpty(item["VIZE1"].ToString()) ? (int?)null : Convert.ToInt32(item["VIZE1"]),
                        Vize2 = string.IsNullOrEmpty(item["VIZE2"].ToString()) ? (int?)null : Convert.ToInt32(item["VIZE2"]),
                        Vize3 = string.IsNullOrEmpty(item["VIZE3"].ToString()) ? (int?)null : Convert.ToInt32(item["VIZE3"]),
                        Final = string.IsNullOrEmpty(item["FINAL"].ToString()) ? (int?)null : Convert.ToInt32(item["FINAL"]),
                        Butunleme = string.IsNullOrEmpty(item["BUTUNLEME"].ToString()) ? (int?)null : Convert.ToInt32(item["BUTUNLEME"]),
                        Ortalama = string.IsNullOrEmpty(item["ORTALAMA"].ToString()) ? (int?)null : Convert.ToInt32(item["ORTALAMA"]),
                        HarfNotu = string.IsNullOrEmpty(item["HARF_NOTU"].ToString()) ? null : item["HARF_NOTU"].ToString(),
                        Durum = string.IsNullOrEmpty(item["GECTI_KALDI"].ToString()) ? null : item["GECTI_KALDI"].ToString(),
                        Kod = kod,
                        Kredi = kredi,
                        OrtalamayaEtki = ortalamayaEtki,
                        BasariKatsayisi = basariKatsayisi,
                        Sinif = sinif
                    });
                }
            }
        }
    }
}