using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Biletall.Helper.Entities;
using Biletall.Helper.Enums;
using TicketApp.UserControls;

namespace TicketApp.Selectors
{
    public class JourneyDetailsTemplateSelector : ContentControl
    {
        public JourneyDetailsTemplateSelector()
        {
            this.Loaded += (s, e) => { SelectTemplate(); };
        }

        private void SelectTemplate()
        {
            this.HorizontalContentAlignment = HorizontalAlignment.Stretch;

            var segment = this.DataContext as Segment;
            if (segment.From.Type == StationType.Bus)
                Content = new JourneyDetailPanel() { DataContext = segment };
            else
                Content = new FlightDetailPanel() { DataContext = segment };
        }
    }
}
