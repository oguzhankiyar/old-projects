using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Color = System.Windows.Media.Color;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace TicketApp.Models
{
    public class Function
    {
        public static List<Sefer> XmlToSefer(string xmlString)
        {
            XDocument doc = XDocument.Parse(xmlString);
            XElement el = doc.Element("NewDataSet");
            List<XElement> list = el.Elements().Where(x => x.Name.LocalName == "Table").ToList();
            List<Sefer> seferler = new List<Sefer>();
            foreach (var element in list)
            {
                seferler.Add(new Sefer()
                {
                    No = Convert.ToInt32((element.Elements().SingleOrDefault(x => x.Name.LocalName == "ID").Value)),
                    Firma = new Firma()
                    {
                        No =
                            Convert.ToInt32(
                                (element.Elements().SingleOrDefault(x => x.Name.LocalName == "FirmaNo").Value)),
                        Adi = element.Elements().SingleOrDefault(x => x.Name.LocalName == "FirmaAdi").Value
                    },
                    KalkisYeri = element.Elements().SingleOrDefault(x => x.Name.LocalName == "KalkisYeri").Value,
                    VarisYeri = element.Elements().SingleOrDefault(x => x.Name.LocalName == "VarisYeri").Value,
                    //IlkKalkisYeri = element.Elements().SingleOrDefault(x => x.Name.LocalName == "IlkKalkisYeri").Value,
                    //SonVarisYeri = element.Elements().SingleOrDefault(x => x.Name.LocalName == "SonVarisYeri").Value,
                    HatNo = Convert.ToInt32(element.Elements().SingleOrDefault(x => x.Name.LocalName == "HatNo").Value),
                    Saat = element.Elements().SingleOrDefault(x => x.Name.LocalName == "Saat").Value,
                    HareketSaati =
                        Convert.ToDateTime(element.Elements().SingleOrDefault(x => x.Name.LocalName == "YerelInternetSaat").Value),
                    SeyahatSuresi =
                        Convert.ToDateTime(
                            element.Elements().SingleOrDefault(x => x.Name.LocalName == "SeyahatSuresi").Value),
                    Otobus = new Otobus()
                    {
                        Tipi = element.Elements().SingleOrDefault(x => x.Name.LocalName == "OtobusTipi").Value
                        //TipAciklama = element.Elements().SingleOrDefault(x => x.Name.LocalName == "OtobusTipiAciklama").Value
                    },
                    SeferTipi = element.Elements().SingleOrDefault(x => x.Name.LocalName == "SeferTipiAciklamasi").Value,
                    Guzergah = element.Elements().SingleOrDefault(x => x.Name.LocalName == "Guzergah").Value,
                    DoluMu = element.Elements().SingleOrDefault(x => x.Name.LocalName == "DoluSeferMi").Value == "1",
                    TakipNo = Convert.ToInt32(element.Elements().SingleOrDefault(x => x.Name.LocalName == "SeferTakipNo").Value),
                    Fiyat =
                        Convert.ToDouble(
                            element.Elements().SingleOrDefault(x => x.Name.LocalName == "BiletFiyatiInternet").Value)
                });
            }
            return seferler;
        }

        public static List<Sehir> XmlToSehir(string xmlString)
        {
            XDocument doc = XDocument.Parse(xmlString);
            XElement el = doc.Element("NewDataSet");
            List<XElement> list = el.Elements().Where(x => x.Name.LocalName == "Table").ToList();
            List<Sehir> sehirler = new List<Sehir>();
            foreach (var element in list)
            {
                sehirler.Add(new Sehir()
                {
                    Adi = element.Elements().SingleOrDefault((x => x.Name.LocalName == "Kalkis_Yeri")).Value
                });
            }
            return sehirler;
        }

        public static List<Koltuk> XmlToKoltuk(string xmlString)
        {
            XDocument doc = XDocument.Parse(xmlString);
            XElement el = doc.Element("Otobus");
            List<XElement> list = el.Elements().Where(x => x.Name.LocalName == "Koltuk").ToList();
            List<Koltuk> koltuklar = new List<Koltuk>();
            foreach (var element in list)
            {
                koltuklar.Add(new Koltuk()
                {
                    No = Convert.ToInt32(element.Elements().SingleOrDefault((x => x.Name.LocalName == "KoltukNo")).Value),
                    Str = element.Elements().SingleOrDefault((x => x.Name.LocalName == "KoltukStr")).Value,
                    Durum = Convert.ToInt32(element.Elements().SingleOrDefault((x => x.Name.LocalName == "Durum")).Value),
                    DurumYan = Convert.ToInt32(element.Elements().SingleOrDefault((x => x.Name.LocalName == "DurumYan")).Value),
                    Fiyat = Convert.ToDouble(element.Elements().SingleOrDefault((x => x.Name.LocalName == "KoltukFiyatiInternet")).Value)
                });
            }
            return koltuklar;
        }

        public static Rezervasyon XmlToRezervasyon(string xmlString)
        {
            try
            {
                XDocument doc = XDocument.Parse(xmlString);
                XElement el = doc.Element("IslemSonuc");
                List<XElement> list = el.Elements().ToList();
                Rezervasyon rezervasyon = new Rezervasyon();
                rezervasyon.Sonuc = list.SingleOrDefault(x => x.Name.LocalName == "Sonuc").Value == "true";
                rezervasyon.PNR = list.SingleOrDefault(x => x.Name.LocalName == "PNR").Value;
                rezervasyon.Opsiyon = Convert.ToDateTime(list.SingleOrDefault(x => x.Name.LocalName == "RezervasyonOpsiyon").Value);
                rezervasyon.Mesaj = list.SingleOrDefault(x => x.Name.LocalName == "Mesaj").Value;
                rezervasyon.SeferTarih = list.SingleOrDefault(x => x.Name.LocalName == "SeferInternetTarihSaat").Value;
                return rezervasyon;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static void CreateSeats(Grid grid)
        {
            Database.SecilenKoltuklar = new List<Koltuk>();
            int i = 0;
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            foreach (var koltuk in Database.Koltuklar)
            {
                if (i % 5 == 0)
                    grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                if (koltuk.Str != "KO" && koltuk.Str != "KA" && koltuk.Str != "PI")
                {
                    Grid seat = new Grid();
                    TextBlock tb = new TextBlock() {Text = koltuk.Str, TextAlignment = TextAlignment.Center};
                    Thickness tck = tb.Margin;
                    tck.Top = 15;
                    tck.Bottom = 15;
                    tb.Margin = tck;
                    seat.Children.Add(tb);
                    seat.Tap += seat_Tap;
                    Rectangle rect = new Rectangle();
                    if (koltuk.Durum != 0)
                        rect.Fill = new SolidColorBrush(Color.FromArgb(0x25, 0x00, 0x00, 0x00));
                    else if (koltuk.DurumYan == 1)
                        rect.Fill = new SolidColorBrush(Color.FromArgb(0x75, 0xFF, 0x69, 0xB4));
                    else if (koltuk.DurumYan == 2)
                        rect.Fill = new SolidColorBrush(Color.FromArgb(0x75, 0x33, 0x99, 0xFF));
                    else
                        rect.Fill = new SolidColorBrush(Color.FromArgb(0x75, 0xFF, 0xC2, 0x00));
                    seat.Children.Add(rect);
                    Grid.SetRow(seat, i / 5);
                    Grid.SetColumn(seat, i % 5);
                    grid.Children.Add(seat);
                }
                i++;
            }
        }

        static void seat_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Grid grid = sender as Grid;
            TextBlock tb = grid.Children[0] as TextBlock;
            string koltukNo = tb.Text;
            Koltuk koltuk = Database.Koltuklar.SingleOrDefault(x => x.Str == koltukNo);
            if (koltuk.Str != "PR")
            {
                if (Database.YolcuSayisi != 0)
                {
                    if (!Database.SecilenKoltuklar.Contains(koltuk) && koltuk.Durum == 0)
                    {
                        if (koltuk.DurumYan == 2 && Database.Cinsiyet == Cinsiyet.Bay)
                        {
                            Database.SecilenKoltuklar.Add(koltuk);
                            Database.YolcuSayisi--;
                            Rectangle rect = grid.Children[1] as Rectangle;
                            rect.Fill = new SolidColorBrush(Colors.Transparent);
                            grid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xC2, 0x00));
                        }
                        else if (koltuk.DurumYan == 2 && Database.Cinsiyet == Cinsiyet.Bayan)
                        {
                            MessageBox.Show("Bir bayın yanına oturamazsınız!");
                        }
                        else if (koltuk.DurumYan == 1 && Database.Cinsiyet == Cinsiyet.Bayan)
                        {
                            Database.SecilenKoltuklar.Add(koltuk);
                            Database.YolcuSayisi--;
                            Rectangle rect = grid.Children[1] as Rectangle;
                            rect.Fill = new SolidColorBrush(Colors.Transparent);
                            grid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xC2, 0x00));
                        }
                        else if (koltuk.DurumYan == 1 && Database.Cinsiyet == Cinsiyet.Bay)
                        {
                            MessageBox.Show("Bir bayanın yanına oturamazsınız!");
                        }
                        else
                        {
                            Database.SecilenKoltuklar.Add(koltuk);
                            Database.YolcuSayisi--;
                            Rectangle rect = grid.Children[1] as Rectangle;
                            rect.Fill = new SolidColorBrush(Colors.Transparent);
                            grid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xC2, 0x00));
                        }
                    }
                    else
                    {
                        MessageBox.Show("Seçtiğiniz koltuk daha önceden alınmış!");
                    }
                }
                else if (Database.SecilenKoltuklar.Contains(koltuk))
                {
                    Database.YolcuSayisi++;
                    Database.SecilenKoltuklar.Remove(koltuk);
                    Rectangle rect = grid.Children[1] as Rectangle;
                    grid.Background = new SolidColorBrush(Colors.Transparent);
                    if (koltuk.Durum != 0)
                        rect.Fill = new SolidColorBrush(Colors.Transparent);
                    if (koltuk.DurumYan == 1)
                        rect.Fill = new SolidColorBrush(Color.FromArgb(0x75, 0xFF, 0x69, 0xB4));
                    else if (koltuk.DurumYan == 2)
                        rect.Fill = new SolidColorBrush(Color.FromArgb(0x75, 0x33, 0x99, 0xFF));
                    else
                        rect.Fill = new SolidColorBrush(Color.FromArgb(0x75, 0xFF, 0xC2, 0x00));
                }
                else
                {
                    MessageBox.Show("Tüm yolcular için koltuk seçtiniz!\nDaha fazla yolcu için yeniden arama yapınız..");
                }
            }
        }
    }
}
