using SocialObisis.ObisisServiceReference;
using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Text;

namespace SocialObisis.Models
{
    public class Obisis
    {
        public ICollection<Duyuru> Duyurular;
        public Obisis()
        {
            this.Duyurular = new HashSet<Duyuru>();
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(GetHTML("http://obisis.erciyes.edu.tr"));
            HtmlNodeCollection collection;
            if (HttpContext.Current.Session["sifre"] == null)
                collection = document.DocumentNode.SelectNodes("//table[@id='ctl01_dlDuyuru']//tr//td");
            else
                collection = document.DocumentNode.SelectNodes("//table[@id='ctl00_dlDuyuru']//tr//td");
            foreach (var col in collection)
            {
                if (col.SelectSingleNode(".//td[@id='HtmlIcerik']") != null)
                {
                    this.Duyurular.Add(new Duyuru()
                    {
                        Baslik = col.SelectSingleNode(".//span[1]").InnerText,
                        Icerik = col.SelectSingleNode(".//td[@id='HtmlIcerik']").InnerText
                    });
                }
            }
        }
        public static void AddLoginLog(string text)
        {
            try
            {
                TextWriter tw = new StreamWriter(HttpContext.Current.Request.MapPath("/") + "LoginLog.txt", true, Encoding.UTF8);
                tw.WriteLine(text);
                tw.Close();
            }
            catch (Exception) { }
        }
        public static string GetHTML(string url, string ogrNo = null, string sifre = null)
        {
            CookieContainer _CookieContainer_ = new CookieContainer();
            if (ogrNo != null || sifre != null)
            {
                string firstURL = "http://obisis.erciyes.edu.tr";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(firstURL);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader stream = new StreamReader(response.GetResponseStream());
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(stream.ReadToEnd());
                HtmlNodeCollection collection = document.DocumentNode.SelectNodes("//input");
                string postData = string.Format("__VIEWSTATE={0}&__EVENTVALIDATION={1}&ctl00$txtboxOgrenciNo={2}&ctl00$txtBoxSifre={3}&ctl00$btnLogin={4}", HttpUtility.UrlEncode(collection[1].Attributes["value"].Value), HttpUtility.UrlEncode(collection[2].Attributes["value"].Value), ogrNo, sifre, HttpUtility.UrlEncode("Giriş"));

                HttpWebRequest _request = (HttpWebRequest)WebRequest.Create(firstURL);
                _request.CookieContainer = _CookieContainer_;
                _request.Method = "POST";
                _request.ContentType = "application/x-www-form-urlencoded";
                _request.KeepAlive = true;
                byte[] byteArray = Encoding.ASCII.GetBytes(postData);
                _request.ContentLength = byteArray.Length;
                Stream newStream = _request.GetRequestStream(); //open connection
                newStream.Write(byteArray, 0, byteArray.Length); // Send the data.
                newStream.Close();
                HttpWebResponse getResponse = (HttpWebResponse)_request.GetResponse();
            }
            HttpWebRequest _req = (HttpWebRequest)WebRequest.Create(url);
            _req.CookieContainer = _CookieContainer_;
            HttpWebResponse _res = (HttpWebResponse)_req.GetResponse();
            StreamReader _stream = new StreamReader(_res.GetResponseStream());
            return (_stream.ReadToEnd());
        }
        public static string TidyText(string text)
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
            return text.Trim();
        }
        public static double CalculateGANO(Ogrenci ogr, IEnumerable<Ders> lessons)
        {
            double agirlikli = 0;
            double kredi = 0;
            foreach (var item in ogr.Donemler)
            {
                if (item.Dersler != lessons)
                {
                    foreach (var ders in item.Dersler.Where(x => x.OrtalamayaEtki))
                    {
                        if (lessons.Where(x => x.Kod == ders.Kod && x.OrtalamayaEtki).Count() == 0)
                        {
                            kredi += ders.Kredi;
                            agirlikli += ders.BasariKatsayisi * ders.Kredi;
                        }
                    }
                }
                else
                {
                    foreach (var ders in lessons.Where(x => x.OrtalamayaEtki))
                    {
                        kredi += ders.Kredi;
                        agirlikli += CalculateGrade(ders) * ders.Kredi;
                    }
                }
            }
            double gano = kredi == 0 ? 0 : agirlikli / kredi;
            return Math.Ceiling(gano * 100) / 100;
        }
        public static int Ortalama(Ders ders)
        {
            int vizeOrtalama = ders.Vize1;
            int final = ders.Final;
            if (ders.Vize2 != -1 && ders.Vize3 != -1)
                vizeOrtalama = (ders.Vize1 + ders.Vize2 + ders.Vize3) / 3;
            else if (ders.Vize2 != -1)
                vizeOrtalama = (ders.Vize1 + ders.Vize2) / 2;
            if (ders.Butunleme != -1)
                final = ders.Butunleme;
            return Convert.ToInt32(Math.Ceiling(vizeOrtalama * 0.4 + final * 0.6));
        }
        public static string CalculateMark(Ders ders)
        {
            int ortalama = Ortalama(ders);
            if (!ders.OrtalamayaEtki && ortalama >= 50)
                return "GT";
            else if (!ders.OrtalamayaEtki && ortalama < 50)
                return "ST";
            else if (ders.Final == -1)
                return "";
            else if (ortalama >= 85)
                return "AA";
            else if (ortalama >= 75)
                return "BA";
            else if (ortalama >= 65)
                return "BB";
            else if (ortalama >= 57)
                return "CB";
            else if (ortalama >= 50)
                return "CC";
            else if (ortalama >= 45)
                return "CD";
            else if (ortalama >= 40)
                return "DD";
            else if (ortalama >= 30)
                return "FD";
            else
                return "FF";
        }
        public static double CalculateGrade(Ders ders)
        {
            int ortalama = Ortalama(ders);
            if (ortalama >= 85)
                return 4.0;
            else if (ortalama >= 75)
                return 3.5;
            else if (ortalama >= 65)
                return 3.0;
            else if (ortalama >= 57)
                return 2.5;
            else if (ortalama >= 50)
                return 2.0;
            else if (ortalama >= 45)
                return 1.5;
            else if (ortalama >= 40)
                return 1.0;
            else if (ortalama >= 30)
                return 0.5;
            else
                return 0.0;
        }

        public class Donem
        {
            public Donem(Ogrenci ogrenci, string donem_adi, string ogretim_yili, int yil_kodu, int donem_no)
            {
                if (!string.IsNullOrEmpty(ogrenci.OgrenciNo))
                {
                    this.Dersler = new HashSet<Ders>();
                    this.Adi = donem_adi;
                    this.No = donem_no;
                    this.OgretimYili = ogretim_yili;
                    this.OgretimYiliKodu = yil_kodu;
                    this.Ogrenci = ogrenci;
                    ObisisMobileServiceClient client = new ObisisMobileServiceClient();
                    OgrenciResult or = client.OgrenciBilgiGetir(ogrenci.OgrenciNo, ogrenci.Sifre, yil_kodu, donem_no);
                    DataTable dt = or.Data.Tables[0];
                    HtmlDocument document = new HtmlDocument();
                    try
                    {
                        document.LoadHtml(GetHTML("http://obisis.erciyes.edu.tr/Default.aspx?tabInd=4&tabNo=8", ogrenci.OgrenciNo, ogrenci.Sifre));
                        this.GANO = Convert.ToDouble(document.DocumentNode.SelectSingleNode("//*[contains(text(), '" + this.OgretimYili + " " + this.Adi + "')]").InnerText.Split(':')[1].Trim());
                    }
                    catch (Exception)
                    {
                        try
                        {
                            document.LoadHtml(GetHTML("http://obisis.erciyes.edu.tr/Default.aspx?tabInd=5&tabNo=8", ogrenci.OgrenciNo, ogrenci.Sifre));
                            this.GANO = Convert.ToDouble(document.DocumentNode.SelectSingleNode("//*[contains(text(), '" + this.OgretimYili + " " + this.Adi + "')]").InnerText.Split(':')[1].Trim());
                        }
                        catch (Exception)
                        {
                            document.LoadHtml(GetHTML("http://obisis.erciyes.edu.tr/Default.aspx?tabInd=6&tabNo=8", ogrenci.OgrenciNo, ogrenci.Sifre));
                            this.GANO = Convert.ToDouble(document.DocumentNode.SelectSingleNode("//*[contains(text(), '" + this.OgretimYili + " " + this.Adi + "')]").InnerText.Split(':')[1].Trim());
                        }
                    }
                    int i = 1;
                    foreach (DataRow item in dt.Rows)
                    {
                        HtmlNode node = document.DocumentNode.SelectSingleNode("//tr[contains(td[1], '" + this.OgretimYili + " " + this.Adi + "')]/following-sibling::tr[" + i++ + "]");
                        string ads = node.SelectSingleNode("./td[2]").InnerText;
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
                            DersAdi = item["DERS_ADI_TR"].ToString(),
                            Vize1 = string.IsNullOrEmpty(item["VIZE1"].ToString()) ? -1 : Convert.ToInt32(item["VIZE1"]),
                            Vize2 = string.IsNullOrEmpty(item["VIZE2"].ToString()) ? -1 : Convert.ToInt32(item["VIZE2"]),
                            Vize3 = string.IsNullOrEmpty(item["VIZE3"].ToString()) ? -1 : Convert.ToInt32(item["VIZE3"]),
                            Final = string.IsNullOrEmpty(item["FINAL"].ToString()) ? -1 : Convert.ToInt32(item["FINAL"]),
                            Butunleme = string.IsNullOrEmpty(item["BUTUNLEME"].ToString()) ? -1 : Convert.ToInt32(item["BUTUNLEME"]),
                            Ortalama = string.IsNullOrEmpty(item["ORTALAMA"].ToString()) ? 0 : Convert.ToDouble(item["ORTALAMA"]),
                            HarfNotu = item["HARF_NOTU"].ToString(),
                            Durum = item["GECTI_KALDI"].ToString(),
                            Kod = kod,
                            Kredi = kredi,
                            OrtalamayaEtki = ortalamayaEtki,
                            BasariKatsayisi = basariKatsayisi,
                            Sinif = sinif,
                            Donem = this,
                            Ogrenci = ogrenci
                        });
                    }
                }
            }
            public int No { get; set; }
            public string Adi { get; set; }
            public string OgretimYili { get; set; }
            public int OgretimYiliKodu { get; set; }
            public double GANO { get; set; }
            public virtual ICollection<Ders> Dersler { get; set; }
            public virtual Ogrenci Ogrenci { get; set; }
        };
        public class Ders
        {
            public string Kod { get; set; }
            public string DersAdi { get; set; }
            public int Vize1 { get; set; }
            public int Vize2 { get; set; }
            public int Vize3 { get; set; }
            public int Final { get; set; }
            public int Butunleme { get; set; }
            public double Ortalama { get; set; }
            public string HarfNotu { get; set; }
            public double BasariKatsayisi { get; set; }
            public string Durum { get; set; }
            public int Sinif { get; set; }
            public double Kredi { get; set; }
            public bool OrtalamayaEtki { get; set; }
            public virtual Donem Donem { get; set; }
            public virtual Ogrenci Ogrenci { get; set; }
        };
        public class Ogrenci
        {
            public Ogrenci(string ogrNo = null, string sifre = null)
            {
                this.OgrenciNo = ogrNo;
                this.Sifre = sifre;
                this.Donemler = new HashSet<Donem>();
                this.Dersler = new HashSet<Ders>();
                if (!string.IsNullOrEmpty(ogrNo))
                {
                    ObisisMobileServiceClient client = new ObisisMobileServiceClient();
                    OgrenciResult or = client.OgrenciDonemGetir(ogrNo);
                    DataTable dt = or.Data.Tables[0];
                    foreach (DataRow item in dt.Rows)
                    {
                        this.Donemler.Add(new Donem(this, item["DONEM_ADI"].ToString(), item["OGRETIM_YILI"].ToString(), Convert.ToInt32(item["OGRETIM_YILI_KODU"]), Convert.ToInt32(item["DONEM_NO"])));
                    }
                    foreach (Donem donem in this.Donemler)
                    {
                        foreach (Ders ders in donem.Dersler)
                        {
                            this.Dersler.Add(ders);
                        }
                    }

                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(GetHTML("http://obisis.erciyes.edu.tr/Default.aspx?tabInd=3&tabNo=5", ogrNo, sifre));
                    HtmlNode node = document.DocumentNode;
                    try
                    {
                        this.AdSoyad = TidyText(node.SelectSingleNode("//span[@id='Banner1_Kullanici1_txtAdiSoyadi']").InnerText);
                        this.Fakulte = TidyText(node.SelectSingleNode("//span[@id='Banner1_Kullanici1_txtFakulteAdi']").InnerText);
                        this.Bolum = TidyText(node.SelectSingleNode("//span[@id='Banner1_Kullanici1_txtBolumAdi']").InnerText);
                        string SinifGano = node.SelectSingleNode("//span[@id='Banner1_Kullanici1_txtSinifSeneGano']").InnerText;
                        this.Sinif = Convert.ToInt32(SinifGano.Split('(')[1].Split('.')[0]);
                        this.GANO = Convert.ToDouble(SinifGano.Split(':')[1]);
                        this.GerekenGANO = Convert.ToDouble(node.SelectSingleNode("//table[@id='ctl00_dgMezuniyetDurum']/tr[2]/td[1]").InnerText.Replace("Ortalama en az", "").Replace("olmalı", "").Trim());
                        this.MezuniyetKredisi = Convert.ToDouble(node.SelectSingleNode("//table[@id='ctl00_dgMezuniyetDurum']/tr[7]/td[1]").InnerText.Replace("Mezuniyet kredisi", "").Replace("olmalı", "").Trim()) / 100;
                        this.ToplamKredi = Convert.ToDouble(node.SelectSingleNode("//table[@id='ctl00_dgMezuniyetDurum']/tr[7]/td[2]").InnerText.Replace("Toplam krediniz", "").Trim()) / 100;
                        this.StajDurum = node.SelectSingleNode("//table[@id='ctl00_dgMezuniyetDurum']/tr[6]/td[3]").InnerHtml == "<img src=\"/images/Valid.gif\">";
                    }
                    catch (Exception) { }
                }
            }
            public string OgrenciNo { get; set; }
            [DataType(DataType.Password)]
            public string Sifre { get; set; }
            public string AdSoyad { get; set; }
            public string Fakulte { get; set; }
            public string Bolum { get; set; }
            public int Sinif { get; set; }
            public double GANO { get; set; }
            public double GerekenGANO { get; set; }
            public double MezuniyetKredisi { get; set; }
            public double ToplamKredi { get; set; }
            public bool StajDurum { get; set; }
            public ICollection<Donem> Donemler { get; set; }
            public ICollection<Ders> Dersler { get; set; }
        };
        public class Duyuru
        {
            public string Baslik { get; set; }
            public string Icerik { get; set; }
        };
    };
}