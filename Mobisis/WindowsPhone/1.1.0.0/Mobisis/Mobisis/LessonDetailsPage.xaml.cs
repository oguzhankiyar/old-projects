using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Mobisis.ServiceReference;

namespace Mobisis
{
    public partial class LessonDetailsPage : PhoneApplicationPage
    {
        public LessonDetailsPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (Data.Connection == null)
            {
                MessageBox.Show("Giriş yapmanız gerekiyor!");
                NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
            }
            else
            {
                if (NavigationContext.QueryString["YearCode"] != null && NavigationContext.QueryString["No"] != null)
                {
                    int YearCode = Convert.ToInt32(NavigationContext.QueryString["YearCode"]);
                    int No = Convert.ToInt32(NavigationContext.QueryString["No"]);
                    string Code = NavigationContext.QueryString["Code"];
                    Ders lesson = Data.Connection.Ogrenci.Donemler.SingleOrDefault(x => x.No == No && x.OgretimYiliKodu == YearCode).Dersler.SingleOrDefault(x => x.Kod == Code);
                    lessonCode.Text = lesson.Kod;
                    lessonName.Text = lesson.Adi;
                    lessonKredi.Text = lesson.Kredi.ToString();
                    lessonVize1.Text = lesson.Vize1.ToString();
                    lessonVize2.Text = lesson.Vize2.ToString();
                    lessonVize3.Text = lesson.Vize3.ToString();
                    lessonFinal.Text = lesson.Final.ToString();
                    lessonButunleme.Text = lesson.Butunleme.ToString();
                    lessonOrtalama.Text = lesson.Ortalama.ToString();
                    lessonHarfNotu.Text = lesson.HarfNotu;
                    lessonDurum.Text = lesson.Durum;
                    if (lesson.Vize2 == null)
                    {
                        lessonVize2_Text.Visibility = Visibility.Collapsed;
                        lessonVize2.Visibility = Visibility.Collapsed;
                    }
                    if (lesson.Vize3 == null)
                    {
                        lessonVize3_Text.Visibility = Visibility.Collapsed;
                        lessonVize3.Visibility = Visibility.Collapsed;
                    }
                    if (lesson.Butunleme == null)
                    {
                        lessonButunleme_Text.Visibility = Visibility.Collapsed;
                        lessonButunleme.Visibility = Visibility.Collapsed;
                    }
                    if (lesson.Ortalama == null)
                    {
                        lessonOrtalama_Text.Visibility = Visibility.Collapsed;
                        lessonOrtalama.Visibility = Visibility.Collapsed;
                    }
                    if (lesson.HarfNotu == null)
                    {
                        lessonHarfNotu_Text.Visibility = Visibility.Collapsed;
                        lessonHarfNotu.Visibility = Visibility.Collapsed;
                    }
                    if (lesson.Durum == null)
                    {
                        lessonDurum_Text.Visibility = Visibility.Collapsed;
                        lessonDurum.Visibility = Visibility.Collapsed;
                    }                        
                }
            }
        }
    }
}