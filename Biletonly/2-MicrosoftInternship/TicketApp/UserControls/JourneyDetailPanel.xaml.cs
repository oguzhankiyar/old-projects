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
using TicketApp.Pages.Airplane;

namespace TicketApp.UserControls
{
    public partial class JourneyDetailPanel : UserControl
    {
        public JourneyDetailPanel()
        {
            InitializeComponent();

            this.Loaded += (s, e) =>
            {
                var segment = this.DataContext as Segment;
                if (segment.Bus != null && !segment.Bus.Properties.Any())
                    BusProperties.Visibility = Visibility.Collapsed;
            };
        }
    }
}
