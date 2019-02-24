using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Biletall.Helper.Entities;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace TicketApp.UserControls
{
    public partial class TicketListItem : UserControl
    {
        public event EventHandler Clicked;
        private static event EventHandler _onAction;
        private static event EventHandler _onHide;
        public static bool IsOpenedCheckBoxes { get; set; }
        public static List<Ticket> CheckedTicketList { get; set; }

        public TicketListItem()
        {
            InitializeComponent();
            CheckedTicketList = new List<Ticket>();
            this.Loaded += TicketListItem_Loaded;
        }

        void TicketListItem_Loaded(object sender, RoutedEventArgs e)
        {
            _onAction += (c, r) =>
            {
                cbTicket.Visibility = Visibility.Visible;
                Grid.SetColumn(TicketPanel, 1);
                Grid.SetColumnSpan(TicketPanel, 1);
            };

            _onHide += (c, r) =>
            {
                cbTicket.IsChecked = false;
                cbTicket.Visibility = Visibility.Collapsed;
                Grid.SetColumn(TicketPanel, 0);
                Grid.SetColumnSpan(TicketPanel, 2);
            };
        }

        private void Ticket_Clicked(object sender, EventArgs e)
        {
            if (IsOpenedCheckBoxes)
                cbTicket.IsChecked = !cbTicket.IsChecked;
            else if (Clicked != null)
                Clicked(sender, e);
        }

        private void cbTicket_StateChanged(object sender, RoutedEventArgs e)
        {
            if (cbTicket.IsChecked == true)
                CheckedTicketList.Add(this.DataContext as Ticket);
            else
                CheckedTicketList.Remove(this.DataContext as Ticket);
        }

        public static void ShowCheckBoxes()
        {
            IsOpenedCheckBoxes = true;
            if (_onAction != null)
                _onAction(null, null);
        }

        public static void HideCheckBoxes()
        {
            IsOpenedCheckBoxes = false;
            if (_onHide != null)
                _onHide(null, null);
        }
    }
}
