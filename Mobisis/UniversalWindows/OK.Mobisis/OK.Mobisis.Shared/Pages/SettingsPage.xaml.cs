using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using OK.Mobisis.Api.Helper;
using OK.Mobisis.Data;
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
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
            new SwipeMenu().AddTo(this);

            Prepare();
        }

        private void Prepare()
        {
            var settings = Database.SavedData.Settings;
            NotificationState.IsOn = settings.NotificationState;
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
        }

        private async void NotificationState_Toggled(object sender, RoutedEventArgs e)
        {
            if (NotificationState.IsOn == true)
            {
                if (Database.SavedData.Student != null)
                {
                    Database.SavedData.Settings.NotificationState = true;
                    await Helper.StartTask();
                }
                else
                {
                    await new MessageDialog("Bildirimleri açmak için giriş yapmalısınız!").ShowAsync();
                    NotificationState.IsOn = false;
                }
            }
            else
            {
                Database.SavedData.Settings.NotificationState = false;
                await Helper.EndTask();
            }
            await Database.Update();
        }

        private async void MidtermFactor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(MidtermFactor.Text))
                Database.SavedData.Settings.MidtermFactor = Convert.ToInt32(MidtermFactor.Text);
            else
                Database.SavedData.Settings.MidtermFactor = 40;
            await Database.Update();
        }

        private async void FinalFactor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(FinalFactor.Text))
                Database.SavedData.Settings.FinalFactor = Convert.ToInt32(FinalFactor.Text);
            else
                Database.SavedData.Settings.FinalFactor = 60;
            await Database.Update();
        }

        private async void FD_Limit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(FD_Limit.Text))
                Database.SavedData.Settings.FD_Limit = Convert.ToInt32(FD_Limit.Text);
            else
                Database.SavedData.Settings.FD_Limit = 30;
            await Database.Update();
        }

        private async void DD_Limit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(FD_Limit.Text))
                Database.SavedData.Settings.DD_Limit = Convert.ToInt32(DD_Limit.Text);
            else
                Database.SavedData.Settings.DD_Limit = 40;
            await Database.Update();
        }

        private async void DC_Limit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(DC_Limit.Text))
                Database.SavedData.Settings.DC_Limit = Convert.ToInt32(DC_Limit.Text);
            else
                Database.SavedData.Settings.DC_Limit = 45;
            await Database.Update();
        }

        private async void CC_Limit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(CC_Limit.Text))
                Database.SavedData.Settings.CC_Limit = Convert.ToInt32(CC_Limit.Text);
            else
                Database.SavedData.Settings.CC_Limit = 50;
            await Database.Update();
        }

        private async void CB_Limit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(CB_Limit.Text))
                Database.SavedData.Settings.CB_Limit = Convert.ToInt32(CB_Limit.Text);
            else
                Database.SavedData.Settings.CB_Limit = 57;
            await Database.Update();
        }

        private async void BB_Limit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(BB_Limit.Text))
                Database.SavedData.Settings.BB_Limit = Convert.ToInt32(BB_Limit.Text);
            else
                Database.SavedData.Settings.BB_Limit = 65;
            await Database.Update();
        }

        private async void BA_Limit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(BA_Limit.Text))
                Database.SavedData.Settings.BA_Limit = Convert.ToInt32(BA_Limit.Text);
            else
                Database.SavedData.Settings.BA_Limit = 75;
            await Database.Update();
        }

        private async void AA_Limit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(AA_Limit.Text))
                Database.SavedData.Settings.AA_Limit = Convert.ToInt32(AA_Limit.Text);
            else
                Database.SavedData.Settings.AA_Limit = 85;
            await Database.Update();
        }
    }
}
