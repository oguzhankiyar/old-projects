using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace TicketApp.Pages.Help
{
    public partial class SelectPage : PhoneApplicationPage
    {
        public SelectPage()
        {
            InitializeComponent();
            App.SetTitle("Uçak+Otobüs Hakkında");
        }
    }
}