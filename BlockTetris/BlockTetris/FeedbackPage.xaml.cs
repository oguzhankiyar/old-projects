using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using BlockTetris.Data;
using BlockTetris.Entities;
using BlockTetris.Strings;
using Microsoft.WindowsAzure.MobileServices;
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

namespace BlockTetris
{
    public sealed partial class FeedbackPage : Page
    {
        public FeedbackPage()
        {
            this.InitializeComponent();
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
                e.Handled = true;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            tbFeedbackTitle.Text = LocalizedStrings.Get(LocalizedString.FeedbackTitle);
            tbFeedbackText.Text = LocalizedStrings.Get(LocalizedString.FeedbackText);
            tbName.Text = LocalizedStrings.Get(LocalizedString.YourName);
            tbEmail.Text = LocalizedStrings.Get(LocalizedString.YourEmail);
            tbMessage.Text = LocalizedStrings.Get(LocalizedString.YourMessage);
            btnSend.Content = LocalizedStrings.Get(LocalizedString.SendButton);
            btnCancel.Content = LocalizedStrings.Get(LocalizedString.CancelButton);
        }

        private async void Send_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtName.Text))
                    await new MessageDialog(LocalizedStrings.Get(LocalizedString.NameError)).ShowAsync();
                else if (txtMessage.Text.Length < 20)
                    await new MessageDialog(LocalizedStrings.Get(LocalizedString.MessageError)).ShowAsync();
                else
                {
                    IMobileServiceTable<Feedback> feedbackTable = App.MobileService.GetTable<Feedback>();
                    Feedback fb = new Feedback();
                    fb.Name = txtName.Text;
                    fb.Email = txtEmail.Text;
                    fb.Message = txtMessage.Text;
                    fb.DeviceId = Database.Current.DeviceId;
                    fb.Region = System.Globalization.RegionInfo.CurrentRegion.EnglishName;
                    fb.Culture = System.Globalization.CultureInfo.CurrentCulture.EnglishName;
                    txtName.IsEnabled = false;
                    txtEmail.IsEnabled = false;
                    txtMessage.IsEnabled = false;
                    btnSend.IsEnabled = false;
                    btnCancel.IsEnabled = false;
                    await feedbackTable.InsertAsync(fb);
                    await new MessageDialog(LocalizedStrings.Get(LocalizedString.FeedbackSuccessful)).ShowAsync();
                    this.Frame.Navigate(typeof(GamePage));
                }
            }
            catch (Exception)
            {
                new MessageDialog(LocalizedStrings.Get(LocalizedString.FeedbackError)).ShowAsync();
                txtName.IsEnabled = true;
                txtEmail.IsEnabled = true;
                txtMessage.IsEnabled = true;
                btnSend.IsEnabled = true;
                btnCancel.IsEnabled = true;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GamePage));
        }
    }
}
