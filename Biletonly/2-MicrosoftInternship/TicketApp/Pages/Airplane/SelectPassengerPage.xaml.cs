using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Biletall.Helper.Entities;
using Biletall.Helper.Enums;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TicketApp.Controls.Enums;
using TicketApp.Models;

namespace TicketApp.Pages.Airplane
{
    public partial class SelectPassengerPage : PhoneApplicationPage
    {
        public SelectPassengerPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            App.SetTitle("Yolcu Seçimi");
            if (NavigationContext.QueryString.ContainsKey("internationalFlight"))
            {
                Grid.SetColumnSpan(txtChildCount, 2);
                Grid.SetColumnSpan(txtBabyCount, 2);
                txtInfantCount.Visibility = Visibility.Collapsed;
                txtStudentCount.Visibility = Visibility.Collapsed;
                txtMilitaryCount.Visibility = Visibility.Collapsed;
                txtTeenagerCount.Visibility = Visibility.Collapsed;
            }
            txtAdultCount.Value = Database.TempData.TicketSearch.Passengers.Count(x => x.Type == PassengerType.Adult);
            txtChildCount.Value = Database.TempData.TicketSearch.Passengers.Count(x => x.Type == PassengerType.Child);
            txtBabyCount.Value = Database.TempData.TicketSearch.Passengers.Count(x => x.Type == PassengerType.Baby);
            txtInfantCount.Value = Database.TempData.TicketSearch.Passengers.Count(x => x.Type == PassengerType.Infant);
            txtStudentCount.Value = Database.TempData.TicketSearch.Passengers.Count(x => x.Type == PassengerType.Student);
            txtMilitaryCount.Value = Database.TempData.TicketSearch.Passengers.Count(x => x.Type == PassengerType.Military);
            txtTeenagerCount.Value = Database.TempData.TicketSearch.Passengers.Count(x => x.Type == PassengerType.Teenager);

            App.AddBackPressedEvent(BackPressed);
        }

        private void BackPressed()
        {
            ControlValues();
        }

        private void Continue_Clicked(object sender, EventArgs e)
        {
            ControlValues();
        }

        private void ControlValues()
        {
            Database.TempData.TicketSearch.Passengers = new List<Passenger>();
            int adultCount = Convert.ToInt32(txtAdultCount.Value);
            int childCount = Convert.ToInt32(txtChildCount.Value);
            int babyCount = Convert.ToInt32(txtBabyCount.Value);
            int infantCount = Convert.ToInt32(txtInfantCount.Value);
            int studentCount = Convert.ToInt32(txtStudentCount.Value);
            int militaryCount = Convert.ToInt32(txtMilitaryCount.Value);
            int teenagerCount = Convert.ToInt32(txtTeenagerCount.Value);
            int totalCount = adultCount + childCount + babyCount + infantCount + studentCount + militaryCount + teenagerCount;

            if (totalCount == 0)
                App.ShowProgress("en az 1 yolcu seçmelisiniz", ProgressType.Warning, ProgressTime.Normal);
            else if (totalCount > 4)
                App.ShowProgress("en fazla 4 yolcu için işlem yapılabilir", ProgressType.Warning, ProgressTime.Normal);
            else
            {
                for (int i = 0; i < adultCount; i++)
                    Database.TempData.TicketSearch.Passengers.Add(new Passenger() { Type = PassengerType.Adult });

                for (int i = 0; i < childCount; i++)
                    Database.TempData.TicketSearch.Passengers.Add(new Passenger() { Type = PassengerType.Child });

                for (int i = 0; i < babyCount; i++)
                    Database.TempData.TicketSearch.Passengers.Add(new Passenger() { Type = PassengerType.Baby });

                for (int i = 0; i < infantCount; i++)
                    Database.TempData.TicketSearch.Passengers.Add(new Passenger() { Type = PassengerType.Infant });

                for (int i = 0; i < studentCount; i++)
                    Database.TempData.TicketSearch.Passengers.Add(new Passenger() { Type = PassengerType.Student });

                for (int i = 0; i < militaryCount; i++)
                    Database.TempData.TicketSearch.Passengers.Add(new Passenger() { Type = PassengerType.Military });

                for (int i = 0; i < teenagerCount; i++)
                    Database.TempData.TicketSearch.Passengers.Add(new Passenger() { Type = PassengerType.Teenager });

                App.RemoveBackPressedEvent(BackPressed);
                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();
            }
        }
    }
}