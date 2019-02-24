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
    public partial class FlightPriceRow : UserControl
    {
        public static DependencyProperty PriceProperty =
            DependencyProperty.Register("Price", typeof(Price), typeof(FlightPriceRow), null);
        public Price Price
        {
            get { return (Price)GetValue(PriceProperty); }
            set
            {
                SetValue(PriceProperty, value);
                UpdateRowData();
            }
        }

        public static DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(FlightPriceRow), null);
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public FlightPriceRow()
        {
            InitializeComponent();
            this.Loaded += (s, e) =>
                {
                    UpdateRowData();
                };
        }

        public void UpdateRowData()
        {
            if (this.Price == null)
            {
                PassengerText.Text = "Yolcu";
                NetPriceText.Text = "Net";
                TaxText.Text = "Vergi";
                ServiceText.Text = "Hizmet";
                TotalText.Text = "Toplam";
                PassengerText.Style = NetPriceText.Style = TaxText.Style = ServiceText.Style = TotalText.Style = Resources["PhoneTextSmallStyle"] as Style;
            }
            else
            {
                NetPriceText.Text = "₺" + (this.Price.NetPrice * this.Price.PassengerCount).ToString();
                TaxText.Text = "₺" + (this.Price.Tax * this.Price.PassengerCount).ToString();
                ServiceText.Text = "₺" + (this.Price.ServicePrice * this.Price.PassengerCount).ToString();
                TotalText.Text = "₺" + (this.Price.TotalPrice * this.Price.PassengerCount).ToString();
            }
        }
    }
}
