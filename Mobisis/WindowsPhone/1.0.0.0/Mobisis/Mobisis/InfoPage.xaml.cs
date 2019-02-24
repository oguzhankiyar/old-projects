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

namespace Mobisis
{
    public partial class InfoPage : PhoneApplicationPage
    {
        public InfoPage()
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
                Ogrenci student = Data.Connection.Ogrenci;
                StudentName.Text = student.AdSoyad;
                Faculty.Text = student.Fakulte;
                Department.Text = student.Bolum;
                Class.Text = student.Sinif.ToString();
                GANO.Text = student.GANO.ToString();
            }
        }
    }
}