using ObisisService.EruWebServiceReference;
using ObisisService.MobileServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ObisisService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public Baglanti Login(string OgrenciNo, string Sifre, string Ref)
        {
            Baglanti baglanti = new Baglanti();
            baglanti.Reference = Ref;
            baglanti.Baglan(OgrenciNo, Sifre);
            return baglanti;
        }
        public List<YemekList> GetFood(string Ref)
        {
            EruWebServisClient client = new EruWebServisClient();
            ResponseObject response = client.YemekList();
            List<YemekList> yemekList = new List<YemekList>();
            List<YemekModel> list = ((YemekModel[])response.DataObject).ToList();
            foreach (var yemek in list)
            {
                YemekList yeniYemek = new YemekList();
                yeniYemek.Tarih = yemek.date.Split(' ')[0].Replace("101", "01").Replace("2013", "2014");
                yeniYemek.Ogun = "Öğle - Akşam";
                yeniYemek.Yemekler = new List<YemekList.Yemek>();
                string[] split = yemek.YemekLunch.Replace("\n", "").Replace("<br>", "").Split('\r');
                for (int i = 0; i < split.Length; i++)
                {
                    if (split[i] != "")
                    {
                        string[] kaloriSplit = split[i].Split('(');
                        string kalori = kaloriSplit[kaloriSplit.Length - 1];
                        yeniYemek.Yemekler.Add(new YemekList.Yemek() { Adi = Function.TidyText(split[i].Replace("(" + kalori, "")), Kalori = kalori.ToLower().Replace("kcal)", "").Trim() });
                    }
                }
                yemekList.Add(yeniYemek);
            }

            return yemekList.OrderBy(x => Convert.ToDateTime(x.Tarih)).ToList();
            //return YemekList.GetList();
        }
        public Donem CurrentPeriod(string OgrenciNo, string Sifre, string Ref)
        {
            Baglanti baglanti = new Baglanti();
            baglanti.Reference = Ref;
            baglanti.Baglan(OgrenciNo, Sifre);
            return baglanti.Ogrenci.Donemler.First();
        }
    }
}
