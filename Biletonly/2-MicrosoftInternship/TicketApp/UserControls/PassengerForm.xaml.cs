using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Biletall.Helper.Entities;
using Biletall.Helper.Requests;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TicketApp.Controls.Enums;
using TicketApp.Models;

namespace TicketApp.UserControls
{
    public partial class PassengerForm : UserControl
    {
        public PassengerForm()
        {
            InitializeComponent();

            this.Loaded += (s, e) =>
            {
                if (Database.TempData.FromServices.Any())
                    pbFromServiceStop.ItemsSource = Database.TempData.FromServices.ToList();
                else
                    pbFromServiceStop.Visibility = Visibility.Collapsed;

                if (Database.TempData.ToServices.Any())
                    pbToServiceStop.ItemsSource = Database.TempData.ToServices.ToList();
                else
                    pbToServiceStop.Visibility = Visibility.Collapsed;
            };
        }
        
        private void FavoritePassengers_ValueChanged(object sender, EventArgs e)
        {
            Logger.ValueChanged("PassengerForm.FavoritePassengers", FavoritePassengerListPicker.Value);
            var passenger = FavoritePassengerListPicker.Value as Passenger;
            if (passenger != null)
            {
                txtTCKN.Value = passenger.TCKN.ToString();
                txtFirstName.Value = passenger.FirstName;
                txtLastName.Value = passenger.LastName;
                dbBirthday.Value = passenger.Birthday;
            }
        }

        private void OpenFavorites_Tap(object sender, EventArgs e)
        {
            Logger.Clicked("PassengerForm.OpenFavorites");
            var passengers = Database.SavedData.FavoritePassengers.Where(x => (txtFirstName.Value == x.FirstName && txtLastName.Value == x.LastName) || Database.TempData.Ticket.Passengers.Where(y => y.FirstName == x.FirstName && y.LastName == x.LastName).Count() == 0).OrderBy(x => x.FirstName).ToList();
            if (passengers.Count() > 0)
            {
                FavoritePassengerListPicker.ItemsSource = passengers;
                FavoritePassengerListPicker.Open();
            }
            else
            {
                App.ShowProgress("henüz kayıtlı yolcu yok, ayarlardan ekleyebilirsiniz", ProgressType.Error, ProgressTime.Normal);
            }
        }

    }
}
