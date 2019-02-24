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
using System.IO.IsolatedStorage;
using System.Net.NetworkInformation;

namespace Mobisis
{
    public partial class FoodPage : PhoneApplicationPage
    {
        public FoodPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            DataSaver<List<YemekList>> dataSaver = new DataSaver<List<YemekList>>();
            Data.FoodList = dataSaver.LoadMyData("FoodList");

            DateTime currentDate = DateTime.Now;
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
                currentDate = currentDate.AddDays(2);
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                currentDate = currentDate.AddDays(1);

            if (Data.FoodList == null || Data.FoodList.SingleOrDefault(x => x.Tarih == currentDate.ToString("dd.MM.yyyy")) == null)
            {
                if (Data.FoodList == null)
                {
                    if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType == Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None)
                        MessageBox.Show("İnternet bağlantınızı kontrol ediniz!");
                }
                if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType != Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None)
                {
                    ProgressIndicator indicator = new ProgressIndicator();
                    indicator.IsIndeterminate = true;
                    indicator.IsVisible = true;
                    indicator.Text = "Yemekler yükleniyor..";
                    SystemTray.SetProgressIndicator(this, indicator);
                    Service1Client client = new Service1Client();
                    client.GetFoodAsync(Data.Version);
                    client.GetFoodCompleted += new EventHandler<GetFoodCompletedEventArgs>(Completed);
                }
                else
                {
                    FoodList.ItemsSource = Data.FoodList;
                    FoodList.SelectedIndex = Data.FoodList.Count - 1;
                }
            }
            else
            {
                FoodList.ItemsSource = Data.FoodList;
                FoodList.SelectedIndex = Data.FoodList.FindIndex(x => x.Tarih == currentDate.ToString("dd.MM.yyyy"));
            }
        }
        private void Completed(object sender, GetFoodCompletedEventArgs e)
        {
            SystemTray.ProgressIndicator.IsVisible = false;
            Data.FoodList = e.Result.ToList();
            if (Data.FoodList == null)
            {
                MessageBox.Show("Yemek listesine erişilemiyor..");
            }
            else
            {
                DataSaver<List<YemekList>> dataSaver = new DataSaver<List<YemekList>>();
                dataSaver.SaveMyData(Data.FoodList, "FoodList");
                FoodList.ItemsSource = Data.FoodList;

                DateTime currentDate = DateTime.Now;
                if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
                    currentDate = currentDate.AddDays(2);
                else if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                    currentDate = currentDate.AddDays(1);

                if (Data.FoodList.SingleOrDefault(x => x.Tarih == currentDate.ToString("dd.MM.yyyy")) == null)
                {
                    MessageBox.Show("Bugüne ait yemek listesine erişilemedi..");
                    FoodList.SelectedIndex = Data.FoodList.Count - 1;
                }
                else
                {
                    FoodList.SelectedIndex = Data.FoodList.FindIndex(x => x.Tarih == currentDate.ToString("dd.MM.yyyy"));
                }
            }
        }
    }
}