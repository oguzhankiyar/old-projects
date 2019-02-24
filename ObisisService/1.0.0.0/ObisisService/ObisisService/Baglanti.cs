using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using ObisisService.MobileServiceReference;
using System.IO;
using System.Text;

namespace ObisisService
{
    [DataContract]
    public class Baglanti
    {
        [DataMember]
        public string OgrenciNo;
        [DataMember]
        public string Sifre;
        [DataMember]
        public bool Durum;
        [DataMember]
        public string Mesaj;
        [DataMember]
        public string Reference;
        [DataMember]
        public Ogrenci Ogrenci;

        public void Baglan(string OgrenciNo, string Sifre)
        {
            OgrenciResult ogrResult = null;
            try
            {
                ObisisMobileServiceClient client = new ObisisMobileServiceClient();
                ogrResult = client.OgrenciBilgiGetir(OgrenciNo, Sifre, 0, 0);
                this.OgrenciNo = OgrenciNo;
                this.Sifre = Sifre;
                this.Durum = ogrResult.Message == "Başarılı";

                this.Ogrenci = new Ogrenci(OgrenciNo, Sifre);
                this.Mesaj = this.Reference == "WP8" ? "Mobisis+ 1.1 sürümü Windows Phone Store'da.." : null;

                try
                {
                    var path = System.Web.Hosting.HostingEnvironment.MapPath("~/LoginLog.txt");
                    TextWriter tw = new StreamWriter(path, true, Encoding.UTF8);
                    tw.WriteLine(DateTime.Now + " - " + this.Reference + " - " + this.Ogrenci.AdSoyad + " - " + this.Ogrenci.Fakulte + " " + this.Ogrenci.Bolum + " " + this.Ogrenci.Sinif);
                    tw.Close();
                }
                catch (Exception) { }
            }
            catch (Exception)
            {
                this.Durum = false;
                this.Mesaj = ogrResult == null ? "Üniversite kaynaklı bir sorun oluştu!\n\nEn kısa sürede düzeltilecektir.." : null;
            }
        }
    }
}