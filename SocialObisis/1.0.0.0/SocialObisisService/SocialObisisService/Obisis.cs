using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Text;
using SocialObisisService.ObisisServiceReference;
using System.Runtime.Serialization;

namespace SocialObisisService
{
    [DataContract]
    public class Person
    {
        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public int Age { get; set; }

        public Person(string firstName = "", string lastName = "", int age = 0)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Age = age;
        }
    }
    [DataContract]
    public class Obisis
    {
        public Obisis()
        {
            this.Duyurular = new HashSet<Duyuru>();
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://obisis.erciyes.edu.tr");
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
            StreamReader stream = new StreamReader(webResponse.GetResponseStream());
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.Load(stream);
            HtmlNodeCollection collection = document.DocumentNode.SelectNodes("//table[@id='ctl01_dlDuyuru']//tr//td");
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
        public ICollection<Duyuru> Duyurular;
        [DataContract]
        public class Donem
        {
            public Donem(Ogrenci ogrenci, int yil_kodu, int donem_no)
            {
                if (!string.IsNullOrEmpty(ogrenci.OgrenciNo))
                {
                    this.Dersler = new HashSet<Ders>();
                    this.No = donem_no;
                    this.OgretimYiliKodu = yil_kodu;
                    this.Ogrenci = ogrenci;
                    ObisisMobileServiceClient client = new ObisisMobileServiceClient();
                    OgrenciResult or = client.OgrenciBilgiGetir(ogrenci.OgrenciNo, ogrenci.Sifre, yil_kodu, donem_no);
                    DataTable dt = or.Data.Tables[0];
                    foreach (DataRow item in dt.Rows)
                    {
                        this.Dersler.Add(new Ders()
                        {
                            DersAdi = item["DERS_ADI_TR"].ToString(),
                            Vize1 = string.IsNullOrEmpty(item["VIZE1"].ToString()) ? -1 : Convert.ToInt32(item["VIZE1"]),
                            Vize2 = string.IsNullOrEmpty(item["VIZE2"].ToString()) ? -1 : Convert.ToInt32(item["VIZE2"]),
                            Vize3 = string.IsNullOrEmpty(item["VIZE3"].ToString()) ? -1 : Convert.ToInt32(item["VIZE3"]),
                            Final = string.IsNullOrEmpty(item["FINAL"].ToString()) ? -1 : Convert.ToInt32(item["FINAL"]),
                            Butunleme = string.IsNullOrEmpty(item["BUTUNLEME"].ToString()) ? -1 : Convert.ToInt32(item["BUTUNLEME"]),
                            Ortalama = string.IsNullOrEmpty(item["FINAL"].ToString()) ? 0 : Convert.ToDouble(item["ORTALAMA"]),
                            HarfNotu = item["HARF_NOTU"].ToString(),
                            Durum = item["GECTI_KALDI"].ToString(),
                            Donem = this,
                            Ogrenci = ogrenci
                        });
                    }
                }
            }
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
            [DataMember]
            public virtual Ogrenci Ogrenci { get; set; }
        };
        [DataContract]
        public class Ders
        {
            [DataMember]
            public string DersAdi { get; set; }
            [DataMember]
            public int Vize1 { get; set; }
            [DataMember]
            public int Vize2 { get; set; }
            [DataMember]
            public int Vize3 { get; set; }
            [DataMember]
            public int Final { get; set; }
            [DataMember]
            public int Butunleme { get; set; }
            [DataMember]
            public double Ortalama { get; set; }
            [DataMember]
            public string HarfNotu { get; set; }
            [DataMember]
            public string Durum { get; set; }
            [DataMember]
            public virtual Donem Donem { get; set; }
            [DataMember]
            public virtual Ogrenci Ogrenci { get; set; }
        };
        [DataContract]
        public class Ogrenci
        {
            public Ogrenci(string ogrNo = "", string sifre = "")
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
                        this.Donemler.Add(new Donem(this, Convert.ToInt32(item["OGRETIM_YILI_KODU"]), Convert.ToInt32(item["DONEM_NO"]))
                        {
                            Adi = item["DONEM_ADI"].ToString(),
                            OgretimYili = item["OGRETIM_YILI"].ToString()
                        });
                    }
                    foreach (Donem donem in this.Donemler)
                    {
                        foreach (Ders ders in donem.Dersler)
                        {
                            this.Dersler.Add(ders);
                        }
                    }

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://obisis.erciyes.edu.tr");
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string strResponse = reader.ReadToEnd();
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(strResponse);
                    HtmlNodeCollection collection = doc.DocumentNode.SelectNodes("//input");

                    string getUrl = "http://obisis.erciyes.edu.tr";
                    string postData = string.Format("__VIEWSTATE={0}&__EVENTVALIDATION={1}&ctl00$txtboxOgrenciNo={2}&ctl00$txtBoxSifre={3}&ctl00$btnLogin={4}", HttpUtility.UrlEncode(collection[1].Attributes["value"].Value), HttpUtility.UrlEncode(collection[2].Attributes["value"].Value), ogrNo, sifre, HttpUtility.UrlEncode("Giriş"));
                    HttpWebRequest getRequest = (HttpWebRequest)WebRequest.Create(getUrl);
                    getRequest.CookieContainer = new CookieContainer();
                    getRequest.Method = "POST";
                    getRequest.ContentType = "application/x-www-form-urlencoded";

                    byte[] byteArray = Encoding.ASCII.GetBytes(postData);
                    getRequest.ContentLength = byteArray.Length;
                    Stream newStream = getRequest.GetRequestStream(); //open connection
                    newStream.Write(byteArray, 0, byteArray.Length); // Send the data.
                    newStream.Close();

                    HttpWebResponse getResponse = (HttpWebResponse)getRequest.GetResponse();
                    StreamReader sr = new StreamReader(getResponse.GetResponseStream());
                    HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                    document.LoadHtml(sr.ReadToEnd());
                    HtmlNode node = document.DocumentNode;
                    this.AdSoyad = node.SelectSingleNode("//span[@id='Banner1_Kullanici1_txtAdiSoyadi']").InnerText;
                    this.Fakulte = node.SelectSingleNode("//span[@id='Banner1_Kullanici1_txtFakulteAdi']").InnerText;
                    this.Bolum = node.SelectSingleNode("//span[@id='Banner1_Kullanici1_txtBolumAdi']").InnerText;
                    string SinifGano = node.SelectSingleNode("//span[@id='Banner1_Kullanici1_txtSinifSeneGano']").InnerText;
                    this.Sinif = Convert.ToInt32(SinifGano.Split('(')[1].Split('.')[0]);
                    this.GANO = Convert.ToDouble(SinifGano.Split(':')[1]);
                }
            }
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
            [DataMember]
            public ICollection<Ders> Dersler { get; set; }
        };
        [DataContract]
        public class Duyuru
        {
            [DataMember]
            public string Baslik { get; set; }
            [DataMember]
            public string Icerik { get; set; }
        };
    };
}