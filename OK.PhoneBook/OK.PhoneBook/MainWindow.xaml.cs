using OK.PhoneBook.Models;
using OK.PhoneBook.Views;
using System.Diagnostics;
using System.DirectoryServices;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows;
using System;
using Microsoft.Win32;
using System.Windows.Controls;

namespace OK.PhoneBook
{
    public partial class MainWindow : Window
    {
        private static MainWindow Current { get; set; }
        public static bool IsAdmin { get; set; }

        public static bool IsLogin = false;

        public MainWindow()
        {
            InitializeComponent();

            Current = this;

            SetContent(new HomePanel());
        }

        private void btnHome_Clicked(object sender, EventArgs e)
        {
            SetContent(new HomePanel());
        }

        private void btnPeople_Clicked(object sender, EventArgs e)
        {
            SetContent(new RecordsPanel());
        }

        private void btnAppointments_Clicked(object sender, EventArgs e)
        {
            SetContent(new AppointmentsPanel());
        }

        private void btnFavorites_Clicked(object sender, EventArgs e)
        {
            SetContent(new FavoritesPanel());
        }

        private void btnSettings_Clicked(object sender, EventArgs e)
        {
            SetContent(new SettingsPanel());
        }        

        private void btnQuit_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

        public static void SetContent(ContentControl content)
        {
            Current.btnHome.IsActive = Current.btnPeople.IsActive = Current.btnAppointments.IsActive = Current.btnFavorites.IsActive = Current.btnSettings.IsActive = Current.btnQuit.IsActive = false;
            if (!IsLogin)
                Current.pnlContent.Content = new LoginPanel();
            else
            {
                var type = content.GetType();
                if (type == typeof(HomePanel))
                    Current.btnHome.IsActive = true;
                else if (type == typeof(RecordsPanel) || type == typeof(CreateRecordPanel))
                    Current.btnPeople.IsActive = true;
                else if (type == typeof(AppointmentsPanel) || type == typeof(CreateAppointmentPanel))
                    Current.btnAppointments.IsActive = true;
                else if (type == typeof(FavoritesPanel))
                    Current.btnFavorites.IsActive = true;
                else if (type == typeof(SettingsPanel))
                    Current.btnSettings.IsActive = true;
                Current.pnlContent.Content = content;
            }
        }
    }
}
