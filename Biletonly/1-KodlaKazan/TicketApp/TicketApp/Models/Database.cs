using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketApp.Models
{
    public enum Cinsiyet
    {
        Bay,
        Bayan
    };

    public class Database
    {
        public static string Admin = "<Kullanici><Adi>kodlakazancom</Adi><Sifre>184ws62501</Sifre></Kullanici>";
        public static string SeferArama;
        public static Sefer SeferDetaylari;
        public static int YolcuSayisi;
        public static List<Koltuk> Koltuklar;
        public static List<Koltuk> SecilenKoltuklar;
        public static Cinsiyet Cinsiyet;
        public static List<Yolcu> Yolcular;
        public static List<Rezervasyon> Rezervasyonlar;
        public static string PhoneNumber;
        public static string Email;
        public static string Nereden;
        public static string Nereye;
        public static DateTime Tarih;
        public static List<Yolcu> FavoriKisiler; 
        
        public static void Fill()
        {
            DataSaver<List<Rezervasyon>> RezervasyonData = new DataSaver<List<Rezervasyon>>();
            Rezervasyonlar = RezervasyonData.LoadMyData("Rezervasyonlar") == null ? new List<Rezervasyon>() : RezervasyonData.LoadMyData("Rezervasyonlar");
            
            DataSaver<string> NeredenData = new DataSaver<string>();
            Nereden = NeredenData.LoadMyData("Nereden");
            
            DataSaver<string> NereyeData = new DataSaver<string>();
            Nereye = NereyeData.LoadMyData("Nereye");

            DataSaver<DateTime> TarihData = new DataSaver<DateTime>();
            Tarih = TarihData.LoadMyData("Tarih");

            DataSaver<List<Yolcu>> KisilerData = new DataSaver<List<Yolcu>>();
            FavoriKisiler = KisilerData.LoadMyData("Kisiler") == null ? new List<Yolcu>() : KisilerData.LoadMyData("Kisiler");
        }

        public static void Update()
        {
            DataSaver<List<Rezervasyon>> RezervasyonData = new DataSaver<List<Rezervasyon>>();
            RezervasyonData.SaveMyData(Rezervasyonlar, "Rezervasyonlar");

            DataSaver<string> NeredenData = new DataSaver<string>();
            NeredenData.SaveMyData(Nereden, "Nereden");

            DataSaver<string> NereyeData = new DataSaver<string>();
            NereyeData.SaveMyData(Nereye, "Nereye");

            DataSaver<DateTime> TarihData = new DataSaver<DateTime>();
            TarihData.SaveMyData(Tarih, "Tarih");

            DataSaver<List<Yolcu>> KisilerData = new DataSaver<List<Yolcu>>();
            KisilerData.SaveMyData(FavoriKisiler, "Kisiler");
        }
    }
}
