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
using TicketApp.Controls;
using TicketApp.Controls.Enums;
using TicketApp.Models;

namespace TicketApp.Pages.Setting
{
    public partial class AddPassengerPage : PhoneApplicationPage
    {
        private Passenger _currentPassenger;

        public AddPassengerPage()
        {
            InitializeComponent();
            _currentPassenger = new Passenger();
            LayoutRoot.DataContext = _currentPassenger;
            txtGender.Items = new List<string>() { "Bay", "Bayan" };
            txtGender.Values = new List<object>() { Gender.Male, Gender.Female };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            App.SetTitle("Favori Yolcu Ekle");
            if (NavigationContext.QueryString.ContainsKey("name"))
            {
                string name = NavigationContext.QueryString["name"];
                _currentPassenger = Database.SavedData.FavoritePassengers.SingleOrDefault(x => x.FullName == name);
                LayoutRoot.DataContext = _currentPassenger;
                btnRemove.Visibility = Visibility.Visible;
                Grid.SetColumnSpan(btnSave, 1);
            }
        }

        private void btnSave_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFirstName.Value) || string.IsNullOrEmpty(txtLastName.Value))
                App.ShowProgress("yolcu adı ve soyadı girmelisiniz", ProgressType.Warning, ProgressTime.Normal);
            else if (txtTCKN.Value.Length < 11)
                App.ShowProgress("tc kimlik numaranız 11 haneden oluşmalıdır", ProgressType.Warning, ProgressTime.Normal);
            else if (txtGender == null)
                App.ShowProgress("cinsiyeti boş bırakamazsınız", ProgressType.Warning, ProgressTime.Normal);
            else if (dbBirthday == null)
                App.ShowProgress("doğum tarihini boş bırakamazsınız", ProgressType.Warning, ProgressTime.Normal);
            else
            {
                Database.AddPassenger(_currentPassenger);
                App.BackPressed();
            }
        }

        private void btnRemove_Clicked(object sender, EventArgs e)
        {
            App.ShowProgress("yolcuyu silmek istediğinizden emin misiniz?", ProgressType.Plain, ProgressTime.Infinite, new List<string> { "Evet", "Vazgeç" });
            Header.HeaderConfirmed = (buttonId) =>
            {
                App.HideProgress();
                if (buttonId == 0)
                {
                    Database.RemovePassenger(_currentPassenger);
                    App.BackPressed();
                }
            };
        }
    }
}