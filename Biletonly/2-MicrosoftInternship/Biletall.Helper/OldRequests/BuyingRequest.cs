using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml.Linq;
using Biletall.Helper.BiletallService;
using Biletall.Helper.Entities;
using Biletall.Helper.Enums;
using Biletall.Helper.Parsings;
using Microsoft.Phone.Controls;

namespace Biletall.Helper.Requests
{
    public class BuyingRequest
    {
        public static bool IsBuyingCompleted { get; private set; }
        public static Action<ActionResult> OnBuyingCompleted = null;

        public static void GetBuying(Ticket ticket)
        {
            IsBuyingCompleted = false;
            string xml;
            if (ticket.Type == TicketType.BusJourney)
                xml = BuyingParsing.GetBusBuying(ticket);
            else
                xml = BuyingParsing.GetAirplaneBuying(ticket);
            var client = new ServiceSoapClient();
            client.StrIsletAsync(xml, Global.Authorization);
            client.StrIsletCompleted += (c, e) =>
            {
                string xmlResult = e.Result;
                PopulateBuying(xmlResult);
            };
        }

        private static void PopulateBuying(string xmlResult)
        {
            var buying = BuyingParsing.ParseBuying(xmlResult);
            IsBuyingCompleted = true;
            if (OnBuyingCompleted != null)
                OnBuyingCompleted(buying);
        }

        public static bool Is3DSecureBuyingCompleted { get; private set; }
        public static Action<ActionResult> On3DSecureBuyingCompleted = null;
        private static WebBrowser _webBrowser;
        private const string _secureServiceURL = "http://www.biletall.com/WS/3DSecure/SecurePay2.aspx";
        private const string _secureServiceResultURL = "http://www.biletall.com/WS/3DSecure/Secure3DSonuc.aspx";
        private static ActionResult _actionResult;

        public static void Get3DSecureBuying(Ticket ticket, Grid webBrowserGrid)
        {
            Is3DSecureBuyingCompleted = false;
            _webBrowser = new WebBrowser();
            _actionResult = new ActionResult();
            webBrowserGrid.Children.Add(_webBrowser);

            string html = BuyingParsing.Get3DSecureBuying(ticket);

            _webBrowser.IsScriptEnabled = true;
            _webBrowser.NavigateToString(html);
            _webBrowser.Navigated += webBrowser_Navigated;
        }

        private static void webBrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            var uri = e.Uri;
            string result = _webBrowser.SaveToString();
            if (!string.IsNullOrEmpty(uri.OriginalString))
            {
                if (uri.OriginalString == _secureServiceURL || uri.OriginalString == _secureServiceResultURL)
                {
                    _actionResult = BuyingParsing.Parse3DSecureBuying(result);
                }
                if (uri.LocalPath == "/success")
                {
                    Populate3DSecureBuying(_actionResult);
                }
                else if (uri.LocalPath == "/fail")
                {
                    _webBrowser.Navigated -= webBrowser_Navigated;
                    Populate3DSecureBuying(_actionResult);
                }
            }
        }

        private static void Populate3DSecureBuying(ActionResult actionResult)
        {
            Is3DSecureBuyingCompleted = true;
            if (On3DSecureBuyingCompleted != null)
                On3DSecureBuyingCompleted(actionResult);
        }
    }
}
