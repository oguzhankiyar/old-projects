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
    public partial class FlightListItem : UserControl
    {
        public static Action OnCompleted = null;

        public FlightListItem()
        {
            InitializeComponent();
            this.Loaded += (s, e) =>
            {
                if (this.DataContext != null && (this.DataContext as Journey).IsFull)
                {
                    PriceText.FontSize = 25;
                    PriceText.Text = "DOLU";
                }
            };
        }
        
        private void LayoutRoot_Tap(object sender, EventArgs e)
        {
            if (this.DataContext != null && !(this.DataContext as Journey).IsFull)
                VisualStateManager.GoToState(this, "Pressed", true);
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", true);
            if (OnCompleted != null)
                OnCompleted();
        }

    }
}
