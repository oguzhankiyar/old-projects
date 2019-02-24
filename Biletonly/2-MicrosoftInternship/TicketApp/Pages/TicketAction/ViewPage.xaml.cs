using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.Media;
using TicketApp.Controls.Enums;
using TicketApp.Models;
using Microsoft.Xna.Framework.Media.PhoneExtensions;
using System.Reflection;
using Biletall.Helper.Enums;
using TicketApp.Controls;
using Biletall.Helper.Entities;
using System.Text;

namespace TicketApp.Pages.TicketAction
{
    public partial class ViewPage : PhoneApplicationPage
    {
        public ViewPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            App.SetTitle("Bilet Görüntüleme");
            App.ShowProgress("e-bilet oluşturuluyor...");
            var webBrowser = new WebBrowser();
            webBrowser.NavigateToString(GetHTML(Database.TempData.Ticket));
            webBrowser.LoadCompleted += (c, r) =>
            {
                _bitmap = GetImage(ContentPanel);
                App.HideProgress();
            };
            ContentPanel.Children.Add(webBrowser);
        }

        private string GetHTML(Ticket ticket)
        {
            string factoryImage, logo, expirationDate, documentTitle, actionType;
            logo = Windows.ApplicationModel.Package.Current.InstalledLocation.Path + "/Assets/TicketApp.MediumLarge.png";

            if (ticket.Type == TicketType.BusJourney)
            {
                factoryImage = "http://eticket.ipektr.com/wsbos3/LogoVer.aspx?fnum=" + ticket.Journeys.First().Segments.First().Factory.Id;
            }
            else
            {
                factoryImage = "http://www.biletall.com/Portals/biletallcom/resim/AirlineLogos/" + ticket.Journeys.First().Segments.First().Factory.Code + ".jpg";
            }

            expirationDate = ticket.PNR.ExpirationDate.ToString("d MMMMMMM yyyy, ddd HH:mm");
            if (ticket.Passengers.Count(x => x.LastAction.Type == ActionType.Reservation) == 0 &&
                ticket.Passengers.Count(x => x.LastAction.Type == ActionType.CanceledReservation) == 0)
            {
                expirationDate = "";
            }
            else if (ticket.PNR.ExpirationDate < DateTime.Now)
            {
                expirationDate = ticket.PNR.ExpirationDate.ToString("d MMMMMMM yyyy, ddd HH:mm") + " (Geçmiş)";
            }
            string action = ticket.ActionType.GetDescription();
            actionType = action;

            if (action.Contains("Satış") && !action.Contains("İptal"))
            {
                documentTitle = "SEYEHAT BELGESİ";
            }
            else if (action.Contains("Rezervasyon") && !action.Contains("İptal"))
            {
                documentTitle = "REZERVASYON BELGESİ";
            }
            else
            {
                documentTitle = "İŞLEM DETAYLARI";
            }

            bool isBusJourney = ticket.Type == TicketType.BusJourney;

            string html = @"<!DOCTYPE html>
                            <html>
                            <head>
                                <title>Bilet Görüntüle | Biletonly</title>
                                <meta charset='utf-8' />
                                <style type='text/css'>
                                    body {
                                        font-family: 'Segoe UI';
                                        font-size:  12px;
                                    }

                                    table {
                                        display: table;
                                        width: 100%;
                                    }

                                    #page {
                                        padding: 15px;
                                        margin: auto;
                                    }

                                    #header {
                                        padding: 50px 0;
                                        width: 100%;
                                    }

                                        #header #logo {
                                            float: left;
                                            width: 24%;
                                        }

                                        #header #doc-title {
                                            float: left;
                                            width: 50%;
                                            font-weight: bold;
                                            font-size: 22px;
                                            height: 50px;
                                            padding-top: 20px;
                                            text-align: center;
                                        }

                                        #header #factory-logo {
                                            float: left;
                                            width: 24%;
                                        }

                                    .title {
                                        background: #e5e5e5;
                                        font-size: 15px;
                                        font-weight: bold;
                                        border: 1px solid #ccc;
                                        padding: 5px 15px;
                                        margin: 2px;
                                        width: 99.5%;
                                    }
                                    table.property-row tr td {
                                        background: #f5f5f5;
                                        border: 1px solid #ccc;
                                        margin-top: 4px;
                                        margin-right: 4px;
                                        margin-bottom: 4px;
                                        vertical-align: middle;
                                        font-weight: bold;
                                        height: 50px;
                                        top: 50%;
                                        text-align: center;
                                    }
                                    table.property-row tr td:last-child {
                                        margin-right: 0;
                                    }
                                    table.value-row tr td {
                                        background: #f5f5f5;
                                        border: 1px solid #ccc;
                                        margin-top: 4px;
                                        margin-right: 4px;
                                        margin-bottom: 4px;
                                        vertical-align: middle;
                                        height: 50px;
                                        top: 50%;
                                        text-align: center;
                                    }
                                    table.value-row tr td:last-child {
                                        margin-right: 0;
                                    }
                                </style>
                            </head>
                            <body>
                                <div id='page'>";
            var sb = new StringBuilder();
            sb.Append("<table id='header'><tr>");
            sb.Append("<td id='logo'><div style='background: orange; width: 150px;'><img src='" + logo + "'  width='150' /></div></td>");

            sb.Append("<td id='doc-title'>" + documentTitle + "</td>");
            sb.Append("<td id='factory-logo'><img src='" + factoryImage + "' width='150' /></td>");
            sb.Append("</tr></table>");
            sb.Append("<table id='ticket-details'>");

            sb.Append("<table id='general-infos'>");
            sb.Append("<table class='title'><tr><td>GENEL BİLGİLER</td></tr></table>");
            sb.Append("<table class='property-row'><tr>");
            sb.Append("<td style='width: 24%;'>PNR Kodu</td>");
            sb.Append("<td style='width: 12%;'>Bilet Tutarı</td>");
            sb.Append("<td style='width: 35%;'>Opsiyon Tarihi</td>");
            sb.Append("<td style='width: 25%;'>İşlem Tipi</td>");
            sb.Append("</tr></table>");

            sb.Append("<table class='value-row'><tr>");
            sb.Append("<td style='width: 24%;'>" + ticket.PNR.Code + "</td>");
            sb.Append("<td style='width: 12%;'>₺" + ticket.Price.TotalPrice.ToString("0.##") + "</td>");
            sb.Append("<td style='width: 35%;'>" + expirationDate + "</td>");
            sb.Append("<td style='width: 25%;'>" + actionType + "</td>");
            sb.Append("</tr></table>");

            sb.Append("</table>");
            if (isBusJourney)
            {
                sb.Append("<table id='journey-infos'>");
                sb.Append("<table class='title'><tr><td>SEFER BİLGİLERİ</td></tr></table>");
                sb.Append("<table class='property-row'><tr>");
                sb.Append("<td style='width: 23.6%;'>Kalkış</td>");
                sb.Append("<td style='width: 23.7%;'>Varış</td>");
                sb.Append("<td style='width: 16%;'>Taşıyıcı Firma</td>");
                sb.Append("<td style='width: 6%;'>Peron</td>");
                sb.Append("<td style='width: 9%;'>Sefer Tipi</td>");
                sb.Append("<td style='width: 16%;'>Sefer Süresi</td>");
                sb.Append("</tr></table>");
                foreach (var journey in ticket.Journeys)
                {
                    foreach (var segment in journey.Segments)
                    {
                        sb.Append("<table class='value-row'><tr>");

                        sb.Append("<td style='width: 23.6%;'>" + segment.From.FullName + "<br />" + segment.DepartureDate.ToString("d MMMMMMM yyyy, ddd HH:mm") + "</td>");
                        sb.Append("<td style='width: 23.7%;'>" + segment.To.FullName + "<br />" + segment.ArrivalDate.ToString("d MMMMMMM yyyy, ddd HH:mm") + "</td>");
                        sb.Append("<td style='width: 16%;'>" + segment.Factory.Name + "</td>");
                        sb.Append("<td style='width: 6%;'>" + (segment.Bus.PlatformNumber == "0" ? "" : segment.Bus.PlatformNumber) + "</td>");
                        sb.Append("<td style='width: 9%;'>" + (segment.Type == SegmentType.Stop ? "Molalı" : "Molasız") + "</td>");
                        sb.Append("<td style='width: 16%;'>" + segment.Duration.GetDescription() + "</td>");
                        sb.Append("</tr></table>");
                    }
                }
                sb.Append("</table><table id='passenger-infos'>");
                sb.Append("<table class='title'><tr><td>YOLCU BİLGİLERİ</td></tr></table>");
                sb.Append("<table class='property-row'><tr>");
                sb.Append("<td style='width: 19.5%;'>Adı Soyadı</td>");
                sb.Append("<td style='width: 12%;'>E-Bilet No</td>");
                sb.Append("<td style='width: 6%;'>Koltuk</td>");
                sb.Append("<td style='width: 17.5%;'>Servis (Kalkış)</td>");
                sb.Append("<td style='width: 17.5%;'>Servis (Varış)</td>");
                sb.Append("<td style='width: 15%;'>Son Durum</td>");
                sb.Append("<td style='width: 6%;'>Fiyat</td>");
                sb.Append("</tr></table>");
                foreach (var passenger in ticket.Passengers)
                {
                    sb.Append("<table class='value-row'><tr>");
                    sb.Append("<td style='width: 19.5%;'>" + passenger.FullName + "</td>");
                    sb.Append("<td style='width: 12%;'>" + passenger.ETicketId + "</td>");
                    sb.Append("<td style='width: 6%;'>" + passenger.Seat.Number + "</td>");
                    sb.Append("<td style='width: 17.5%;'>" + passenger.FromServiceStop.Location + "</td>");
                    sb.Append("<td style='width: 17.5%;'>" + passenger.ToServiceStop.Location + "</td>");
                    sb.Append("<td style='width: 15%;'>" + passenger.LastAction.Type.GetDescription() + "</td>");
                    sb.Append("<td style='width: 6%;'>₺" + passenger.Price.TotalPrice.ToString("0.##") + "</td>");
                    sb.Append("</tr></table>");
                }
                sb.Append("</table>");
            }
            else
            {
                sb.Append("<table id='journey-infos'>");
                sb.Append("<table class='title'><tr><td>SEFER BİLGİLERİ</td></tr></table>");
                sb.Append("<table class='property-row'><tr>");
                sb.Append("<td style='width: 32%;'>Kalkış</td>");
                sb.Append("<td style='width: 32%;'>Varış</td>");
                sb.Append("<td style='width: 16%;'>Taşıyıcı Firma</td>");
                sb.Append("<td style='width: 16%;'>Sınıf</td>");
                sb.Append("</tr></table>");
                foreach (var journey in ticket.Journeys)
                {
                    foreach (var segment in journey.Segments)
                    {
                        sb.Append("<table class='value-row'><tr>");
                        sb.Append("<td style='width: 32%;'>" + segment.From.FullName + "<br />" + segment.DepartureDate.ToString("d MMMMMMM yyyy, ddd HH:mm") + "</td>");
                        sb.Append("<td style='width: 32%;'>" + segment.To.FullName + "<br />" + segment.ArrivalDate.ToString("d MMMMMMM yyyy, ddd HH:mm") + "</td>");
                        sb.Append("<td style='width: 16%;'>" + segment.Factory.Name + "</td>");
                        sb.Append("<td style='width: 16%;'>" + segment.SelectedClass.Name + " (" + segment.SelectedClass.ShortName + ")</td>");
                        sb.Append("</tr></table>");
                    }
                }
                sb.Append("</table><table id='passenger-infos'>");
                sb.Append("<table class='title'><tr><td>YOLCU BİLGİLERİ</td></tr></table>");
                sb.Append("<table class='property-row'><tr>");
                sb.Append("<td style='width: 19%;'>Adı Soyadı</td>");
                sb.Append("<td style='width: 13%;'>E-Bilet No</td>");
                sb.Append("<td style='width: 13%;'>Miles & Smiles</td>");
                sb.Append("<td style='width: 10%;'>Yolcu Tipi</td>");
                sb.Append("<td style='width: 7.5%;'>Net</td>");
                sb.Append("<td style='width: 7.5%;'>Vergi</td>");
                sb.Append("<td style='width: 7.5%;'>Hizmet</td>");
                sb.Append("<td style='width: 7.5%;'>Toplam</td>");
                sb.Append("<td style='width: 15%;'>Son Durum</td>");
                sb.Append("</tr></table>");
                foreach (var passenger in ticket.Passengers)
                {
                    sb.Append("<table class='value-row'><tr>");
                    sb.Append("<td style='width: 19%;'>" + passenger.FullName + "</td>");
                    sb.Append("<td style='width: 13%;'>" + passenger.ETicketId + "</td>");
                    sb.Append("<td style='width: 13%;'>" + passenger.FactoryCardId + "</td>");
                    sb.Append("<td style='width: 10%;'>" + passenger.Type.GetDescription() + "</td>");
                    sb.Append("<td style='width: 7.5%;'>₺" + passenger.Price.NetPrice.ToString("0.##") + "</td>");
                    sb.Append("<td style='width: 7.5%;'>₺" + passenger.Price.Tax.ToString("0.##") + "</td>");
                    sb.Append("<td style='width: 7.5%;'>₺" + passenger.Price.ServicePrice.ToString("0.##") + "</td>");
                    sb.Append("<td style='width: 7.5%;'>₺" + passenger.Price.TotalPrice.ToString("0.##") + "</td>");
                    sb.Append("<td style='width: 15%;'>" + passenger.LastAction.Type.GetDescription() + "</td>");
                    sb.Append("</tr></table>");
                }
                sb.Append("</table>");
            }
            sb.Append("</table><table id='laws'>");
            /*
            sb.Append("<table class='title'><tr><td>HİZMET SÖZLEŞMESİ</td></tr></table>");
            sb.Append("<table>");
            sb.Append(""); //laws
            sb.Append("</table>");
            sb.Append("</table>");
            sb.Append("<table id='footer'>");
            sb.Append("footer");
            sb.Append("</table>");
            */
            sb.Append("</table></body></html>");
            html += sb.ToString();
            return html;
        }
        
        /*protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            App.SetTitle("Bilet Görüntüleme");

            DetailsGrid.DataContext = Database.TempData.Ticket;

            if (Database.TempData.Ticket.Type == TicketType.BusJourney)
            {
                AirplaneJourneyDetails.Visibility = Visibility.Collapsed;
                AirplanePassengerDetails.Visibility = Visibility.Collapsed;
                FactoryImage.Source = new BitmapImage(new Uri("http://eticket.ipektr.com/wsbos3/LogoVer.aspx?fnum=" + Database.TempData.Ticket.Journeys.First().Factory.Id, UriKind.RelativeOrAbsolute));
            }
            else
            {
                BusJourneyDetails.Visibility = Visibility.Collapsed;
                BusPassengerDetails.Visibility = Visibility.Collapsed;
                FactoryImage.Source = new BitmapImage(new Uri("http://www.biletall.com/Portals/biletallcom/resim/AirlineLogos/" + Database.TempData.Ticket.Journeys.First().Factory.Code + ".jpg", UriKind.RelativeOrAbsolute));
            }

            string ticketId = Database.TempData.Ticket.Passengers.First().ETicketId;
            BarCodeText.Text = ticketId;
            BarCodeImage.Source = new BitmapImage(new Uri("https://www.biletall.com/Portals/biletallcom/resim/Barkod.aspx?b=" + ticketId, UriKind.RelativeOrAbsolute));
            QRCodeImage.Source = new BitmapImage(new Uri("https://api.qrserver.com/v1/create-qr-code/?data=" + Windows.ApplicationModel.Store.CurrentApp.LinkUri.OriginalString, UriKind.RelativeOrAbsolute));
            
            if (Database.TempData.Ticket.Passengers.Count(x => x.LastAction.Type == ActionType.Reservation) == 0 &&
                Database.TempData.Ticket.Passengers.Count(x => x.LastAction.Type == ActionType.CanceledReservation) == 0)
                ReservationOptionText.Visibility = Visibility.Collapsed;
            else if (Database.TempData.Ticket.PNR.ExpirationDate < DateTime.Now)
                ReservationOptionText.Text = Database.TempData.Ticket.PNR.ExpirationDate.ToString("d MMMMMMM yyyy, ddd HH:mm") + " (Geçmiş)";
            LastStateText.Text = Database.TempData.Ticket.GetLastState();

            if (LastStateText.Text.Contains("Satış") && !LastStateText.Text.Contains("İptal"))
                TitleText.Text = "SEYEHAT BELGESİ";
            else if (LastStateText.Text.Contains("Rezervasyon") && !LastStateText.Text.Contains("İptal"))
                TitleText.Text = "REZERVASYON BELGESİ";
            else
                TitleText.Text = "BİLET DETAYLARI";

            Loaded += ViewPage_Loaded;
            BarCodeImage.ImageOpened += BarCodeImage_ImageOpened;
            BarCodeImage.ImageFailed += BarCodeImage_ImageOpened;
            QRCodeImage.ImageOpened += QRCodeImage_ImageOpened;
            QRCodeImage.ImageFailed += QRCodeImage_ImageOpened;
            FactoryImage.ImageOpened += FactoryImage_ImageOpened;
            FactoryImage.ImageFailed += FactoryImage_ImageOpened;
            App.ShowProgress("e-bilet oluşturuluyor...");
        }

        bool _isLoaded;
        bool _isFactoryOpened;
        bool _isQROpened;
        bool _isBarcodeOpened;
        WriteableBitmap _bitmap;
        ImageBox _image;
        void ViewPage_Loaded(object sender, RoutedEventArgs e)
        {
            ScaleGrid();
            _isLoaded = true;
            if (_isBarcodeOpened && _isFactoryOpened && _isQROpened)
                CreateImage();
        }
        void FactoryImage_ImageOpened(object sender, RoutedEventArgs e)
        {
            _isFactoryOpened = true;
            if (_isBarcodeOpened && _isQROpened && _isLoaded)
                CreateImage();
        }

        void QRCodeImage_ImageOpened(object sender, RoutedEventArgs e)
        {
            _isQROpened = true;
            if (_isBarcodeOpened && _isFactoryOpened && _isLoaded)
                CreateImage();
        }

        void BarCodeImage_ImageOpened(object sender, RoutedEventArgs e)
        {
            _isBarcodeOpened = true;
            if (_isFactoryOpened && _isQROpened && _isLoaded)
                CreateImage();
        }

        private void CreateImage()
        {
            _bitmap = GetImage(DetailsGrid);
            ContentPanel.Children.Clear();
            _image = new ImageBox() { Source = _bitmap };
            ContentPanel.Children.Add(_image);
            App.HideProgress();
        }

        private void ScaleGrid()
        {
            double currentWidth = DetailsGrid.Width;
            double currentHeight = DetailsGrid.Height;
            double panelWidth = ContentPanel.ActualWidth;
            double panelHeight = ContentPanel.ActualHeight;
            DetailsGrid.RenderTransform = new ScaleTransform() { ScaleX = panelWidth / currentWidth, ScaleY = panelHeight / currentHeight };
            Viewport.Bounds = new Rect(0, 0, DetailsGrid.ActualWidth, DetailsGrid.ActualHeight);
            Viewport.ManipulationLockMode = System.Windows.Controls.Primitives.ManipulationLockMode.Horizontal & System.Windows.Controls.Primitives.ManipulationLockMode.Vertical;
        }*/

        WriteableBitmap _bitmap;

        private void SaveAsImage_Clicked(object sender, EventArgs e)
        {
            SaveToMediaLibrary(_bitmap);
            App.ShowProgress("resim galeriye kaydedildi", ProgressType.Success, ProgressTime.Normal);
        }
        
        private WriteableBitmap GetImage(Grid view)
        {
            Size size = new Size(view.ActualWidth, view.ActualHeight);
            if (size.IsEmpty)
                return null;
            var bitmap = new WriteableBitmap((int)size.Width, (int)size.Height);

            bitmap.Render(view, new TranslateTransform()
            {
                X = 1,
                Y = 1
            });

            bitmap.Invalidate();
            return bitmap;
        }

        public void SaveToMediaLibrary(WriteableBitmap wb)
        {
            var store = IsolatedStorageFile.GetUserStoreForApplication();
            // If a file with this name already exists, delete it.

            var name = "ticket-" + Database.TempData.Ticket.PNR.Code;

            using (var fileStream = store.CreateFile(name))
            {
                // Save the WriteableBitmap into isolated storage as JPEG.
                System.Windows.Media.Imaging.Extensions.SaveJpeg(wb, fileStream, wb.PixelWidth, wb.PixelHeight, 0, 70);
            }
            using (var fileStream = store.OpenFile(name, FileMode.Open, FileAccess.Read))
            {
                // Now, add the JPEG image to the photos library.
                var library = new MediaLibrary();
                var pic = library.SavePicture(name, fileStream);
            }
        }
    }
}