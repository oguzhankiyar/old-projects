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

namespace Mobisis
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            PageCanvas = new SwipeMenu(this, PageCanvas, LayoutRoot, MoveAnimation, LeftMenu);
            Container.Height = App.Current.RootVisual.RenderSize.Height;

            Settings settings = Database.Settings;
            NotificationState.IsChecked = settings.NotificationState;
            MidtermFactor.Text = settings.MidtermFactor.ToString();
            FinalFactor.Text = settings.FinalFactor.ToString();
            FD_Limit.Text = settings.FD_Limit.ToString();
            DD_Limit.Text = settings.DD_Limit.ToString();
            DC_Limit.Text = settings.DC_Limit.ToString();
            CC_Limit.Text = settings.CC_Limit.ToString();
            CB_Limit.Text = settings.CB_Limit.ToString();
            BB_Limit.Text = settings.BB_Limit.ToString();
            BA_Limit.Text = settings.BA_Limit.ToString();
            AA_Limit.Text = settings.AA_Limit.ToString();
            base.OnNavigatedTo(e);
        }

        private void NotificationState_CheckedChange(object sender, RoutedEventArgs e)
        {
            if (NotificationState.IsChecked == true)
            {
                Database.Settings.NotificationState = true;
                Function.StartTask();
            }
            else
            {
                Database.Settings.NotificationState = false;
                Function.EndTask();
            }
        }

        private void MidtermFactor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(MidtermFactor.Text))
                Database.Settings.MidtermFactor = Convert.ToInt32(MidtermFactor.Text);
            else
                Database.Settings.MidtermFactor = 40;
        }

        private void FinalFactor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(FinalFactor.Text))
                Database.Settings.FinalFactor = Convert.ToInt32(FinalFactor.Text);
            else
                Database.Settings.FinalFactor = 60;
        }

        private void FD_Limit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(FD_Limit.Text))
                Database.Settings.FD_Limit = Convert.ToInt32(FD_Limit.Text);
            else
                Database.Settings.FD_Limit = 30;
        }

        private void DD_Limit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(FD_Limit.Text))
                Database.Settings.DD_Limit = Convert.ToInt32(DD_Limit.Text);
            else
                Database.Settings.DD_Limit = 40;
        }

        private void DC_Limit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(DC_Limit.Text))
                Database.Settings.DC_Limit = Convert.ToInt32(DC_Limit.Text);
            else
                Database.Settings.DC_Limit = 45;
        }

        private void CC_Limit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(CC_Limit.Text))
                Database.Settings.CC_Limit = Convert.ToInt32(CC_Limit.Text);
            else
                Database.Settings.CC_Limit = 50;
        }

        private void CB_Limit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(CB_Limit.Text))
                Database.Settings.CB_Limit = Convert.ToInt32(CB_Limit.Text);
            else
                Database.Settings.CB_Limit = 57;
        }

        private void BB_Limit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(BB_Limit.Text))
                Database.Settings.BB_Limit = Convert.ToInt32(BB_Limit.Text);
            else
                Database.Settings.BB_Limit = 65;
        }

        private void BA_Limit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(BA_Limit.Text))
                Database.Settings.BA_Limit = Convert.ToInt32(BA_Limit.Text);
            else
                Database.Settings.BA_Limit = 75;
        }

        private void AA_Limit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(AA_Limit.Text))
                Database.Settings.AA_Limit = Convert.ToInt32(AA_Limit.Text);
            else
                Database.Settings.AA_Limit = 85;
        }
    }
}