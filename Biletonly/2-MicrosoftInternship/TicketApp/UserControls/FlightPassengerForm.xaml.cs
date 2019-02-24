using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace TicketApp.UserControls
{
    public partial class FlightPassengerForm : UserControl
    {
        public FlightPassengerForm()
        {
            InitializeComponent();

            txtGender.Items = new List<string>() { "Bay", "Bayan" };
            txtGender.Values = new List<object>() { Gender.Male, Gender.Female };

            if (!Database.TempData.Ticket.Journeys.Any(x => x.Segments.Any(y => y.Factory.Code.Contains("TK"))))
                txtFactoryCardId.Visibility = Visibility.Collapsed;
            else
                txtFactoryCardId.Value = "TK";

            /*if (!Database.TempData.Ticket.IsBirthdayRequired)
            {
                Grid.SetColumnSpan(txtGender, 2);
                txtBirthday.Visibility = Visibility.Collapsed;
            }*/
            if (!Database.TempData.Ticket.IsPassportRequired)
                txtPassportId.Visibility = Visibility.Collapsed;
        }

        private void FavoritePassengers_ValueChanged(object sender, EventArgs e)
        {
            Logger.ValueChanged("FlightPassengerForm.FavoritePassengers", FavoritePassengerListPicker.Value);
            var passenger = FavoritePassengerListPicker.Value as Passenger;
            if (passenger != null)
            {
                txtGender.Value = passenger.Gender;
                txtFirstName.Value = passenger.FirstName;
                txtLastName.Value = passenger.LastName;
                txtTCKN.Value = passenger.TCKN.ToString();
                txtBirthday.Value = passenger.Birthday;
                txtFactoryCardId.Value = passenger.FactoryCardId;
                txtPassportId.Value = passenger.PassaportId.ToString();
            }
        }

        private void OpenFavorites_Tap(object sender, EventArgs e)
        {
            Logger.Clicked("FlightPassengerForm.OpenFavorites");
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
