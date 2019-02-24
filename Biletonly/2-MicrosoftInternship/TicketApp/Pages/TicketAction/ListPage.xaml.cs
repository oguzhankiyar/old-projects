using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;
using Biletall.Helper.Entities;
using Biletall.Helper.Requests;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TicketApp.Models;
using TicketApp.Controls.Enums;
using TicketApp.UserControls;
using Biletall.Helper.Enums;

namespace TicketApp.Pages.TicketAction
{
    public partial class ListPage : PhoneApplicationPage
    {
        private static bool _isLoadedFromUri;

        public ListPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            App.SetTitle("Bilet İşlemleri");

            TicketList.ItemsSource = Database.SavedData.Tickets.ToList();
            TicketList.SelectedItem = null;

            Database.TempData.PNR = new PNR();
            SearchPanel.DataContext = Database.TempData.PNR;

            TicketList.Hold += TicketList_Hold;

            // For navigating from another apps
            if (!_isLoadedFromUri && NavigationContext.QueryString.ContainsKey("code") && NavigationContext.QueryString.ContainsKey("parameter"))
            {
                string code, parameter;
                NavigationContext.QueryString.TryGetValue("code", out code);
                NavigationContext.QueryString.TryGetValue("parameter", out parameter);
                OpenSearchPanel();
                txtPNR.Value = code;
                txtParameter.Value = parameter;
                _isLoadedFromUri = true;
                btnSearch_Clicked(this, null);
            }
        }

        void TicketList_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (TicketListItem.IsOpenedCheckBoxes)
                HideRemoveButtons();
            else
                ShowRemoveButtons();
        }

        private void ShowRemoveButtons()
        {
            App.AddBackPressedEvent(BackPressedForCheckBoxes);
            TicketListItem.ShowCheckBoxes();
            SearchButtons.Visibility = Visibility.Collapsed;
            RemoveButtons.Visibility = Visibility.Visible;
        }

        private void HideRemoveButtons()
        {
            App.RemoveBackPressedEvent(BackPressedForCheckBoxes);
            TicketListItem.HideCheckBoxes();
            RemoveButtons.Visibility = Visibility.Collapsed;
            SearchButtons.Visibility = Visibility.Visible;
        }

        private void btnRemove_Clicked(object sender, EventArgs e)
        {
            foreach (var ticket in TicketListItem.CheckedTicketList)
                Database.RemoveTicket(ticket);
            TicketList.ItemsSource = Database.SavedData.Tickets;
            HideRemoveButtons();
        }

        private void btnCancelRemove_Clicked(object sender, EventArgs e)
        {
            HideRemoveButtons();
        }

        private void btnSearch_Clicked(object sender, EventArgs e)
        {
            if (SearchPanel.Visibility == Visibility.Collapsed)
            {
                OpenSearchPanel();
            }
            else
            {
                if (string.IsNullOrEmpty(txtPNR.Value) || string.IsNullOrEmpty(txtParameter.Value))
                    App.ShowProgress("boş bıraktığınız alan(lar) var", ProgressType.Warning, ProgressTime.Normal);
                else if (!App.IsInternetAvailable)
                    App.ShowProgress("internet bağlantınızı kontrol ediniz", ProgressType.Warning, ProgressTime.Normal);
                else
                {
                    btnSearch.IsActivated = false;
                    btnCancel.IsActivated = false;
                    App.AddBackPressedEvent(BackPressedForSearch);
                    App.ShowProgress("bilet aranıyor...");

                    TicketRequests.TicketRequest.OnCompleted = (response) =>
                    {
                        var ticket = response.Result;
                        Database.TempData.Ticket = ticket;
                        App.HideProgress();
                        btnSearch.IsActivated = true;
                        btnCancel.IsActivated = true;
                        App.RemoveBackPressedEvent(BackPressedForSearchPanel);

                        if (response.Status != ResponseStatus.Successful)
                            App.ShowProgress("bu bilgilere ait bilet bulunamadı", ProgressType.Error, ProgressTime.Normal);
                        else
                        {
                            CloseSearchPanel();
                            Database.AddTicket(ticket);
                            NavigationService.Navigate(new Uri("/Pages/TicketAction/DetailsPage.xaml", UriKind.RelativeOrAbsolute));
                        }
                    };
                    TicketRequests.GetTicket(Database.TempData.PNR);
                }
            }
        }

        private void BackPressedForSearch()
        {
            App.RemoveBackPressedEvent(BackPressedForSearch);
            TicketRequests.TicketRequest.Cancel();
            App.HideProgress();
            btnSearch.IsActivated = true;
            btnCancel.IsActivated = true;
        }
        
        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            CloseSearchPanel();
        }

        private void Ticket_Clicked(object sender, EventArgs e)
        {
            var selected = TicketList.SelectedItem as Ticket;
            if (selected != null)
            {
                Functions.UpdateTicket(selected.PNR, NavigationService);
            }
        }

        private void BackPressedForSearchPanel()
        {
            if (SearchPanel.Visibility == Visibility.Visible)
                CloseSearchPanel();
        }

        private void BackPressedForCheckBoxes()
        {
            HideRemoveButtons();
        }

        private void OpenSearchPanel()
        {
            App.AddBackPressedEvent(BackPressedForSearchPanel);
            Grid.SetColumnSpan(btnSearch, 1);
            btnCancel.Visibility = Visibility.Visible;
            SearchPanel.Visibility = Visibility.Visible;
        }
        
        private void CloseSearchPanel()
        {
            App.RemoveBackPressedEvent(BackPressedForSearchPanel);
            Grid.SetColumnSpan(btnSearch, 2);
            btnCancel.Visibility = Visibility.Collapsed;
            SearchPanel.Visibility = Visibility.Collapsed;
        }
    }
}