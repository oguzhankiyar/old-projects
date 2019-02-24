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
using Biletall.Helper.Requests;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TicketApp.Models;

namespace TicketApp.UserControls
{
    public partial class TicketPassengerItem : UserControl
    {
        private static event EventHandler _onBuying;
        private static event EventHandler _onCancel;
        private static event EventHandler _onHide;
        public static List<Passenger> CheckedPassengerList { get; set; }
        public static List<Passenger> UncheckedPassengerList { get; set; }

        public TicketPassengerItem()
        {
            InitializeComponent();
            CheckedPassengerList = new List<Passenger>();
            UncheckedPassengerList = new List<Passenger>();

            if (Database.TempData.Ticket.Type == TicketType.BusJourney)
            {
                PriceDetailsPanel.Visibility = Visibility.Collapsed;
                BusPricePanel.Visibility = Visibility.Visible;
            }

            this.Loaded += (s, e) =>
            {
                _onBuying += (c, r) =>
                {
                    cbPassenger.Visibility = Visibility.Visible;
                    Grid.SetColumn(PassengerPanel, 1);
                    Grid.SetColumnSpan(PassengerPanel, 1);
                    var passenger = this.DataContext as Passenger;
                    if (passenger != null)
                    {
                        if (passenger.LastAction.Type != ActionType.Reservation)
                            cbPassenger.IsEnabled = false;
                    }
                };

                _onCancel += (c, r) =>
                {
                    cbPassenger.Visibility = Visibility.Visible;
                    Grid.SetColumn(PassengerPanel, 1);
                    Grid.SetColumnSpan(PassengerPanel, 1);
                    var passenger = this.DataContext as Passenger;
                    if (passenger != null)
                    {
                        if (passenger.LastAction.Type != ActionType.Buying && passenger.LastAction.Type != ActionType.Reservation)
                            cbPassenger.IsEnabled = false;
                    }
                };

                _onHide += (c, r) =>
                {
                    cbPassenger.IsChecked = false;
                    cbPassenger.Visibility = Visibility.Collapsed;
                    Grid.SetColumn(PassengerPanel, 0);
                    Grid.SetColumnSpan(PassengerPanel, 2);
                };
            };
        }

        public static void CheckBoxesForBuy()
        {
            if (_onBuying != null)
                _onBuying(null, null);
        }

        public static void CheckBoxesForCancel()
        {
            if (_onCancel != null)
                _onCancel(null, null);
        }

        public static void HideCheckBoxes()
        {
            if (_onHide != null)
                _onHide(null, null);
        }

        private void cbPassenger_StateChanged(object sender, RoutedEventArgs e)
        {
            var passenger = this.DataContext as Passenger;
            if (cbPassenger.IsChecked == true)
            {
                CheckedPassengerList.Add(passenger);
                UncheckedPassengerList.Remove(passenger);
            }
            else
            {
                UncheckedPassengerList.Add(passenger);
                CheckedPassengerList.Remove(passenger);
            }
        }
    }
}
