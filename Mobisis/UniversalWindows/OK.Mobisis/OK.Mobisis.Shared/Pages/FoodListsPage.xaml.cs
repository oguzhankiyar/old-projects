using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using OK.Mobisis.Api.Helper.Enums;
using OK.Mobisis.Api.Helper.Parsings;
using OK.Mobisis.Api.Helper.Requests;
using OK.Mobisis.Data;
using OK.Mobisis.Entities.ModelEntities;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OK.Mobisis.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FoodListsPage : Page
    {
        private DateTime currentDate;

        public FoodListsPage()
        {
            this.InitializeComponent();
            new SwipeMenu().AddTo(this);
            
            Prepare();
        }

        private async void Prepare()
        {
            currentDate = DateTime.Now;
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    currentDate = DateTime.Now.AddDays(2);
                    break;
                case DayOfWeek.Sunday:
                    currentDate = DateTime.Now.AddDays(1);
                    break;
            }

            if (Database.SavedData.FoodLists == null || Database.SavedData.FoodLists.FirstOrDefault(x => x.Date.ToString("d.M.yyyy") == currentDate.ToString("d.M.yyyy")) == null)
            {
                if (Database.SavedData.FoodLists == null)
                {
                    if (!NetworkInterface.GetIsNetworkAvailable())
                        await new MessageDialog("İnternet bağlantınızı kontrol ediniz").ShowAsync();
                }
                else
                {
                    FillFoodListHub(Database.SavedData.FoodLists, Database.SavedData.FoodLists.Last().Date);
                }

                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    this.LayoutRoot.ShowStatus("yemek listesi güncelleniyor..");
                    FoodRequests.FoodRequest.OnCompleted += GetFoodLists_Completed;
                    FoodRequests.GetFoodLists();
                }
            }
            else
            {
                FillFoodListHub(Database.SavedData.FoodLists, currentDate);
            }
        }

        private void FillFoodListHub(List<FoodList> foodLists, DateTime selectedDay)
        {
            var contentTemplate = this.Resources["FoodListTemplate"] as DataTemplate;
            int index = 0;
            for (int i = 0; i < foodLists.Count; i++)
            {
                var item = foodLists[i];
                var section = new HubSection() { ContentTemplate = contentTemplate };
                section.DataContext = item;
                this.FoodListHub.Sections.Add(section);
                if (item.Date.ToString("d.M.yyyy") == selectedDay.ToString("d.M.yyyy"))
                    index = i;
            }
            this.FoodListHub.DefaultSectionIndex = index;
        }

        private async void GetFoodLists_Completed(BaseResponse<List<Entities.ModelEntities.FoodList>> obj)
        {
            this.LayoutRoot.HideStatus();
            if (obj.Status != ResponseStatus.Successful)
                await new MessageDialog(obj.Message).ShowAsync();
            else
            {
                var foodLists = obj.Result as List<FoodList>;
                Database.SavedData.FoodLists = foodLists;
                await Database.Update();
                FillFoodListHub(foodLists, currentDate);
                if (foodLists.FirstOrDefault(x => x.Date.ToString("d.M.yyyy") == currentDate.ToString("d.M.yyyy")) == null)
                    await new MessageDialog("güncel liste bulunmuyor..").ShowAsync();
            }
        }
    }
}