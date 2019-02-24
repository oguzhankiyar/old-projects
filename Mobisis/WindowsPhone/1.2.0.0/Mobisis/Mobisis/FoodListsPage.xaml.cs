using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Mobisis.Models;
using Mobisis.ObisisServiceReference;

namespace Mobisis
{
    public partial class FoodListsPage : PhoneApplicationPage
    {
        public FoodListsPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            PageCanvas = new SwipeMenu(this, PageCanvas, LayoutRoot, MoveAnimation, LeftMenu);
            Container.Height = App.Current.RootVisual.RenderSize.Height;

            DateTime currentDate = DateTime.Now;
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
                currentDate = currentDate.AddDays(2);
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                currentDate = currentDate.AddDays(1);

            if (Database.FoodLists == null || Database.FoodLists.SingleOrDefault(x => x.Date == Convert.ToDateTime(currentDate.ToString("dd.MM.yyyy"))) == null)
            {
                if (Database.FoodLists == null)
                {
                    if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType == Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None)
                        MessageBox.Show("İnternet bağlantınızı kontrol ediniz!");
                }
                else
                {
                    FoodLists.ItemsSource = Database.FoodLists;
                    FoodLists.SelectedIndex = Database.FoodLists.Count - 1;
                }
                if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType != Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None)
                {
                    ProgressIndicator indicator = new ProgressIndicator();
                    indicator.IsIndeterminate = true;
                    indicator.IsVisible = true;
                    indicator.Text = "Yemekler güncelleniyor..";
                    SystemTray.SetProgressIndicator(this, indicator);
                    ObisisMobileServiceClient client = new ObisisMobileServiceClient();
                    client.GetFoodListsAsync();
                    client.GetFoodListsCompleted += client_GetFoodListsCompleted;
                }
                else
                {
                    FoodLists.ItemsSource = Database.FoodLists;
                    FoodLists.SelectedIndex = Database.FoodLists.Count - 1;
                }
            }
            else
            {
                FoodLists.ItemsSource = Database.FoodLists;
                FoodLists.SelectedIndex = Database.FoodLists.FindIndex(x => x.Date == Convert.ToDateTime(currentDate.ToString("dd.MM.yyyy")));
            }
            base.OnNavigatedTo(e);
        }

        private void client_GetFoodListsCompleted(object sender, GetFoodListsCompletedEventArgs e)
        {
            if (SystemTray.ProgressIndicator != null)
                SystemTray.ProgressIndicator.IsVisible = false; 
            Connection con = e.Result;
            if (!con.State)
            {
                MessageBox.Show("Yemek listesine erişilemiyor..");
            }
            else
            {
                ObjectModel[] lists = ((ListModel)con.DataObject).List.ToArray();
                List<FoodList> foods = new List<FoodList>();
                foreach (var item in lists)
                    foods.Add((FoodList)item);
                Database.FoodLists = foods;
                FoodLists.ItemsSource = Database.FoodLists;

                DateTime currentDate = DateTime.Now;
                if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
                    currentDate = currentDate.AddDays(2);
                else if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                    currentDate = currentDate.AddDays(1);

                if (Database.FoodLists.SingleOrDefault(x => x.Date == Convert.ToDateTime(currentDate.ToShortDateString())) == null)
                {
                    MessageBox.Show("Bugüne ait yemek listesine erişilemedi..");
                    FoodLists.SelectedIndex = Database.FoodLists.Count - 1;
                }
                else
                {
                    FoodLists.SelectedIndex = Database.FoodLists.FindIndex(x => x.Date == Convert.ToDateTime(currentDate.ToShortDateString()));
                }
            }
        }
    }
}