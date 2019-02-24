using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading;
using Mobisis.ServiceReference;
using System.Net.NetworkInformation;

namespace Mobisis
{
    public partial class LessonPage : PhoneApplicationPage
    {
        private int YearCode { get; set; }
        private int No { get; set; }
        public LessonPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (Data.Connection == null)
            {
                MessageBox.Show("Giriş yapmanız gerekiyor!");
                NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
            }
            else
            {
                if (NavigationContext.QueryString.ContainsKey("YearCode") && NavigationContext.QueryString.ContainsKey("No"))
                {
                    YearCode = Convert.ToInt32(NavigationContext.QueryString["YearCode"]);
                    No = Convert.ToInt32(NavigationContext.QueryString["No"]);
                    Donem period = Data.Connection.Ogrenci.Donemler.SingleOrDefault(x => x.No == No && x.OgretimYiliKodu == YearCode);
                    Title.Text = period.OgretimYili + " " + period.Adi;
                    LessonList.ItemsSource = period.Dersler;
                }
                else
                {
                    Donem period = Data.Connection.Ogrenci.Donemler.First();
                    YearCode = period.OgretimYiliKodu;
                    No = period.No;
                    LessonList.ItemsSource = period.Dersler;

                    if (NetworkInterface.GetIsNetworkAvailable())
                    {
                        ProgressIndicator indicator = new ProgressIndicator();
                        indicator.IsIndeterminate = true;
                        indicator.IsVisible = true;
                        try
                        {
                            indicator.Text = "Dersler güncelleniyor..";
                            Service1Client client = new Service1Client();
                            client.CurrentPeriodAsync(Data.Connection.OgrenciNo, Data.Connection.Sifre, "WP8");
                            client.CurrentPeriodCompleted += new EventHandler<CurrentPeriodCompletedEventArgs>(Completed);
                        }
                        catch (Exception)
                        {
                            indicator.Text = "Dersler yüklenemedi!";
                        }
                        SystemTray.SetProgressIndicator(this, indicator);
                    }
                }
            }
        }
        private void LessonList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Ders item = LessonList.SelectedItem as Ders;
            NavigationService.Navigate(new Uri("/LessonDetailsPage.xaml?YearCode=" + YearCode + "&No=" + No +"&Code=" + item.Kod, UriKind.Relative));
        }
        private void Completed(object sender, CurrentPeriodCompletedEventArgs e)
        {
            SystemTray.ProgressIndicator.IsVisible = false;
            Donem period = e.Result;
            Donem current = Data.Connection.Ogrenci.Donemler.First();
            string text = "";
            foreach (var lesson in period.Dersler)
            {
                Ders currentLesson = current.Dersler.Single(x => x.Kod == lesson.Kod);
                bool isNew = false;
                text += lesson.Adi + "\n";
                if (lesson.Vize1 != currentLesson.Vize1 && lesson.Vize1 != null)
                {
                    text += "  1. Vize: " + lesson.Vize1 + "\n";
                    isNew = true;
                }
                if (lesson.Vize2 != currentLesson.Vize2 && lesson.Vize2 != null)
                {
                    text += "  2. Vize: " + lesson.Vize2 + "\n";
                    isNew = true;
                }
                if (lesson.Vize3 != currentLesson.Vize3 && lesson.Vize3 != null)
                {
                    text += "  3. Vize: " + lesson.Vize3 + "\n";
                    isNew = true;
                }
                if (lesson.Final != currentLesson.Final && lesson.Final != null)
                {
                    text += "  Final: " + lesson.Final + "\n";
                    isNew = true;
                }
                if (lesson.Butunleme != currentLesson.Butunleme && lesson.Butunleme != null)
                {
                    text += "  Bütünleme: " + lesson.Butunleme + "\n";
                    isNew = true;
                }
                if (!isNew)
                    text = text.Replace(lesson.Adi + "\n", "");
            }
            if (text != "")
            {
                MessageBox.Show(text);
                Data.Connection.Ogrenci.Donemler.First().Dersler = period.Dersler;
                DataSaver<Baglanti> dataSaver = new DataSaver<Baglanti>();
                dataSaver.SaveMyData(Data.Connection, "Connection");
                YearCode = period.OgretimYiliKodu;
                No = period.No;
                LessonList.ItemsSource = period.Dersler;
            }
        }
    }
}