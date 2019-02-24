using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Biletall.Helper.Enums;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TicketApp.Models;

namespace TicketApp.UserControls
{
    public partial class SeatPanel : UserControl
    {
        public SeatPanel()
        {
            InitializeComponent();
            if (Database.TempData.SelectedGender == Gender.Male)
            {
                MaleGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x33, 0x99, 0xFF));
                MaleText.Foreground = new SolidColorBrush(Colors.White);
            }
            else if (Database.TempData.SelectedGender == Gender.Female)
            {
                FamaleGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x69, 0xB4));
                FamaleText.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        private void Male_Tap(object sender, EventArgs e)
        {
            Logger.Clicked("SeatPanel.Male");
            MaleGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x33, 0x99, 0xFF));
            MaleText.Foreground = new SolidColorBrush(Colors.White);
            FamaleGrid.Background = new SolidColorBrush(Colors.White);
            FamaleText.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x69, 0xB4));
            Database.TempData.SelectedGender = Gender.Male;
            Functions.SetPassengerGender(Database.TempData.SelectedGender);
        }

        private void Famale_Tap(object sender, EventArgs e)
        {
            Logger.Clicked("SeatPanel.Female");
            FamaleGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x69, 0xB4));
            FamaleText.Foreground = new SolidColorBrush(Colors.White);
            MaleGrid.Background = new SolidColorBrush(Colors.White);
            MaleText.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x33, 0x99, 0xFF));
            Database.TempData.SelectedGender = Gender.Female;
            Functions.SetPassengerGender(Database.TempData.SelectedGender);
        }
    }
}
