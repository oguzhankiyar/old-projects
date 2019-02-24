using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Biletall.Helper.Entities;
using Biletall.Helper.Enums;
using Biletall.Helper.Parsings;
using HtmlAgilityPack;
using Microsoft.Phone.Controls;

namespace Biletall.Helper.Requests
{
    public static class BuyingRequests
    {
        public static BaseRequest<ActionResult> BuyingRequest;
        public static BaseRequest<ActionResult> CancelBuyingRequest;

        static BuyingRequests()
        {
            BuyingRequest = BaseRequest<ActionResult>.GetInstance();
            BuyingRequest.IsCancelable = false;
            CancelBuyingRequest = BaseRequest<ActionResult>.GetInstance();
            CancelBuyingRequest.IsCancelable = false;
        }

        public static void GetBuying(Ticket ticket)
        {
            PassengerRequests.OnListVerificationCompleted = (stateResponse) =>
            {
                var state = stateResponse.Result;
                if (!state && BuyingRequest.OnCompleted != null)
                {
                    var response = new BaseResponse<ActionResult>();
                    response.Status = ResponseStatus.InvalidTCKN;
                    BuyingRequest.OnCompleted(response);
                }
                else
                {
                    string xml;
                    if (ticket.Type == TicketType.BusJourney)
                        xml = BuyingParsings.GetBusBuying(ticket);
                    else
                        xml = BuyingParsings.GetAirplaneBuying(ticket);

                    BuyingRequest.OnPopulated = (xmlResult) =>
                    {
                        BuyingRequest.Response = BuyingParsings.ParseBuying(xmlResult);
                    };
                    Global.OnRequestCalled("BuyingRequests.GetBuying", new object[] { ticket });
                    if (ticket.Type == TicketType.BusJourney)
                        BuyingRequest.GetResult(xml, ApiAction.BusBuying);
                    else
                        BuyingRequest.GetResult(xml, ApiAction.AirplaneBuying);
                }
            };
                PassengerRequests.VerifyPassengers(ticket.Passengers);

        }

        public static bool Is3DSecureBuyingCompleted { get; private set; }
        public static Action<BaseResponse<ActionResult>> On3DSecureBuyingCompleted = null;
        public static Action OnPageLoaded = null;
        private static WebBrowser _webBrowser;
        private static BaseResponse<ActionResult> _response;

        public static void Get3DSecureBuying(Ticket ticket, Grid webBrowserGrid)
        {
            Global.OnRequestCalled("BuyingRequests.Get3DSecureBuying", new object[] { ticket });
            if (ticket.PNR == null || string.IsNullOrEmpty(ticket.PNR.Code))
            {
                PassengerRequests.OnListVerificationCompleted = (stateResponse) =>
                {
                    var state = stateResponse.Result;
                    if (!state && On3DSecureBuyingCompleted != null)
                    {
                        var response = new BaseResponse<ActionResult>();
                        response.Status = ResponseStatus.InvalidTCKN;
                        On3DSecureBuyingCompleted(response);
                    }
                    else
                    {
                        prepareBrowser(ticket, webBrowserGrid);
                    }
                };
                PassengerRequests.VerifyPassengers(ticket.Passengers);
            }
            else
            {
                prepareBrowser(ticket, webBrowserGrid);
            }
        }

        private static void prepareBrowser(Ticket ticket, Grid webBrowserGrid)
        {
            Is3DSecureBuyingCompleted = false;
            _webBrowser = new WebBrowser();
            webBrowserGrid.Background = new SolidColorBrush(Colors.White);
            var innerGrid = new Grid();
            innerGrid.Children.Add(_webBrowser);
            webBrowserGrid.Children.Add(innerGrid);

            string html = BuyingParsings.Get3DSecureBuying(ticket);

            _webBrowser.IsScriptEnabled = true;
            _webBrowser.NavigateToString(html);
            _webBrowser.Navigated += webBrowser_Navigated;
        }
        
        private static void webBrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                var uri = e.Uri;
                string result = _webBrowser.SaveToString();
                Global.OnRequestReturned(uri.OriginalString + "=> " + result);
                if (uri.OriginalString.Contains("www.biletall.com/WS/3DSecure/SecurePay2.aspx"))
                {
                    var doc = new HtmlDocument();
                    doc.LoadHtml(result);
                    var root = doc.DocumentNode;
                    var inputs = root.Descendants("input");
                    if (getInputValue(inputs, "Sonuc") == "false")
                    {
                        _webBrowser.Navigated -= webBrowser_Navigated;
                        _response = BuyingParsings.Parse3DSecureBuying(result);
                        Global.OnResultParsed(_response);
                        //_webBrowser.Navigate(new Uri("about:blank", UriKind.RelativeOrAbsolute));
                        populate3DSecureBuying();
                        Global.OnRequestCompleted();
                    }
                    else
                        postToUrl("https://sanalposprov.garanti.com.tr/servlet/gt3dengine", getPostData(result));
                }
                else if (uri.OriginalString.Contains("bkm.com.tr"))
                {
                    _webBrowser.RenderTransformOrigin = new Point(0.5, _webBrowser.RenderTransformOrigin.Y);
                    _webBrowser.RenderTransform = new ScaleTransform() { ScaleX = 2, ScaleY = 2 };
                    _webBrowser.Margin = new Thickness(0, 50, 0, 0);
                    if (OnPageLoaded != null)
                        OnPageLoaded();
                }
                else if (uri.OriginalString.Contains("www.biletall.com/WS/3DSecure/Secure3DSonuc.aspx"))
                {
                    _webBrowser.Navigated -= webBrowser_Navigated;
                    _response = BuyingParsings.Parse3DSecureBuying(result);
                    Global.OnResultParsed(_response);
                    //_webBrowser.Navigate(new Uri("about:blank", UriKind.RelativeOrAbsolute));
                    populate3DSecureBuying();
                    Global.OnRequestCompleted();
                }
            }
            catch (Exception exp)
            {
                Global.Invoke(() =>
                {
                    Is3DSecureBuyingCompleted = true;
                    if (On3DSecureBuyingCompleted != null)
                    {
                        On3DSecureBuyingCompleted(new BaseResponse<ActionResult>() { Status = ResponseStatus.Undefined });
                        Global.OnRequestFailed(exp);
                    }
                });
            }
        }
        
        private static string getPostData(string content)
        {
            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(content);
                var root = doc.DocumentNode;
                var inputs = root.Descendants("input");

                string cardnumber = getInputValue(inputs, "cardnumber");
                string cardexpiredatemonth = getInputValue(inputs, "cardexpiredatemonth");
                string cardexpiredateyear = getInputValue(inputs, "cardexpiredateyear");
                string cardcvv2 = getInputValue(inputs, "cardcvv2");
                string mode = getInputValue(inputs, "mode");
                string secure3dsecuritylevel = getInputValue(inputs, "secure3dsecuritylevel");
                string apiversion = getInputValue(inputs, "apiversion");
                string terminalprovuserid = getInputValue(inputs, "terminalprovuserid");
                string terminaluserid = getInputValue(inputs, "terminaluserid");
                string terminalmerchantid = getInputValue(inputs, "terminalmerchantid");
                string txntype = getInputValue(inputs, "txntype");
                string txnamount = getInputValue(inputs, "txnamount");
                string txncurrencycode = getInputValue(inputs, "txncurrencycode");
                string txninstallmentcount = getInputValue(inputs, "txninstallmentcount");
                string orderid = getInputValue(inputs, "orderid");
                string terminalid = getInputValue(inputs, "terminalid");
                string successurl = getInputValue(inputs, "successurl");
                string errorurl = getInputValue(inputs, "errorurl");
                string customeremailaddress = getInputValue(inputs, "customeremailaddress");
                string customeripaddress = getInputValue(inputs, "customeripaddress");
                string secure3dhash = getInputValue(inputs, "secure3dhash");

                return @"cardnumber=" + cardnumber +
                            "&cardexpiredatemonth=" + cardexpiredatemonth +
                            "&cardexpiredateyear=" + cardexpiredateyear +
                            "&cardcvv2=" + cardcvv2 +
                            "&mode=" + mode +
                            "&secure3dsecuritylevel=" + secure3dsecuritylevel +
                            "&apiversion=" + apiversion +
                            "&terminalprovuserid=" + terminalprovuserid +
                            "&terminaluserid=" + terminaluserid +
                            "&terminalmerchantid=" + terminalmerchantid +
                            "&txntype=" + txntype +
                            "&txnamount=" + txnamount +
                            "&txncurrencycode=" + txncurrencycode +
                            "&txninstallmentcount=" + txninstallmentcount +
                            "&orderid=" + orderid +
                            "&terminalid=" + terminalid +
                            "&successurl=" + successurl +
                            "&errorurl=" + errorurl +
                            "&customeremailaddress=" + customeremailaddress +
                            "&customeripaddress=" + customeripaddress +
                            "&secure3dhash=" + secure3dhash;
            }
            catch (Exception exp)
            {
                return "";
            }
        }

        private static string getInputValue(IEnumerable<HtmlNode> nodes, string name)
        {
            var input = nodes.SingleOrDefault(x => x.Attributes["name"] != null && x.Attributes["name"].Value == name);
            if (input == null)
                return null;
            return input.Attributes["value"].Value;
        }

        private static void postToUrl(string url, string postData)
        {
            try
            {
                var _byteArray = Encoding.UTF8.GetBytes(postData);
                string headers = @"Accept: */*" + Environment.NewLine +
                                    "Accept-Encoding: gzip, deflate" + Environment.NewLine +
                                    "Content-Length: " + _byteArray.Length + Environment.NewLine +
                                    "Content-Type: application/x-www-form-urlencoded" + Environment.NewLine +
                                    "User-Agent: runscope/0.1";
                _webBrowser.Navigate(new Uri("https://sanalposprov.garanti.com.tr/servlet/gt3dengine", UriKind.RelativeOrAbsolute), _byteArray, headers);
            }
            catch (Exception exp)
            {
                Global.Invoke(() =>
                {
                    Is3DSecureBuyingCompleted = true;
                    if (On3DSecureBuyingCompleted != null)
                    {
                        On3DSecureBuyingCompleted(new BaseResponse<ActionResult>() { Status = ResponseStatus.Undefined });
                        Global.OnRequestFailed(exp);
                    }
                });
            }
        }

        private static void populate3DSecureBuying()
        {
            try
            {
                Is3DSecureBuyingCompleted = true;
                if (On3DSecureBuyingCompleted != null)
                    On3DSecureBuyingCompleted(_response);
            }
            catch (Exception exp)
            {
                Global.Invoke(() =>
                {
                    Is3DSecureBuyingCompleted = true;
                    if (On3DSecureBuyingCompleted != null)
                    {
                        On3DSecureBuyingCompleted(new BaseResponse<ActionResult>() { Status = ResponseStatus.Undefined });
                        Global.OnRequestFailed(exp);
                    }
                });
            }
        }

        public static void GetCancelBuying(Ticket ticket, Passenger passenger = null)
        {
            string xml = BuyingParsings.GetCancelBuying(ticket, passenger);
            CancelBuyingRequest.OnPopulated = (xmlResult) =>
            {
                CancelBuyingRequest.Response = BuyingParsings.ParseCancelBuying(xmlResult);
            };
            Global.OnRequestCalled("BuyingRequests.GetCancelBuying", new object[] { ticket, passenger });
            CancelBuyingRequest.GetResult(xml, ApiAction.CancelBuying);
        }
    }
}
