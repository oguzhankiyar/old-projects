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
using TicketApp.Models;

namespace TicketApp.UserControls
{
    public partial class TicketPriceSummaryPanel : UserControl
    {
        public TicketPriceSummaryPanel()
        {
            InitializeComponent();

            this.Loaded += TicketPriceSummaryPanel_Loaded;
        }

        void TicketPriceSummaryPanel_Loaded(object sender, RoutedEventArgs e)
        {
            var ticket = this.DataContext as Ticket;
            if (ticket != null)
                txtPassengers.Text = Functions.FillPassengersText(ticket.Passengers);
        }
    }
}
