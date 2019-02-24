using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Biletall.Helper.Entities;
using Biletall.Helper.Enums;
using Biletall.Helper.Requests;
using Microsoft.Phone.Controls;
using TicketApp.Controls.Enums;
using TicketApp.UserControls;

namespace TicketApp.Models
{
    public static class Functions
    {
        private static List<Seat> _seatList;
        public static void CreateSeats(Grid grid, List<Seat> seatList)
        {
            Logger.MethodCalled("Functions.CreateSeats(Grid, List<Seat>)", new object[] {  seatList });
            _seatList = seatList.ToList();
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            int i = 0;
            foreach (var seat in _seatList)
            {
                if (i % 5 == 0)
                    grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                if (seat.Name != "KO" && seat.Name != "KA" && seat.Name != "PI")
                {
                    var seatGrid = new Grid();
                    Grid.SetRow(seatGrid, i / 5);
                    Grid.SetColumn(seatGrid, i % 5);
                    var tb = new TextBlock() { Text = seat.Name, TextAlignment = TextAlignment.Center };
                    var tck = tb.Margin;
                    tck.Top = 20;
                    tck.Bottom = 20;
                    tb.Margin = tck;
                    Rectangle rect = new Rectangle();
                    if (seat.State != SeatState.Empty || seat.Name == "MA" || seat.Name == "SA")
                        rect.Fill = new SolidColorBrush(Color.FromArgb(0x25, 0x00, 0x00, 0x00));
                    else if (seat.NextState == SeatState.Male)
                        rect.Fill = new SolidColorBrush(Color.FromArgb(0x90, 0xFF, 0x69, 0xB4));
                    else if (seat.NextState == SeatState.Female)
                        rect.Fill = new SolidColorBrush(Color.FromArgb(0x90, 0x33, 0x99, 0xFF));
                    else
                        rect.Fill = new SolidColorBrush(Color.FromArgb(0x90, 0xFF, 0xC2, 0x00));
                    rect.Margin = new Thickness(2);
                    seatGrid.Children.Add(rect);
                    seatGrid.Children.Add(tb);
                    seatGrid.Tap += (s, e) => { SelectSeat(seatGrid); };
                    if (Database.TempData.Ticket.Passengers.SingleOrDefault(x => x.Seat.Name == seat.Name) != null)
                    {
                        Database.TempData.Ticket.Passengers.RemoveAll(x => x.Seat.Name == seat.Name);
                        SelectSeat(seatGrid);
                    }
                    grid.Children.Add(seatGrid);
                }
                i++;
            }
        }

        private static void SelectSeat(Grid grid)
        {
            Logger.Clicked("Functions.SelectSeat(Grid)");
            if (Database.TempData.SelectedGender == null)
            {
                App.ShowProgress("önce cinsiyet seçmelisiniz", ProgressType.Warning, ProgressTime.Normal);
                return;
            }
            var rect = grid.Children[0] as Rectangle;
            var tb = grid.Children[1] as TextBlock;
            var seat = _seatList.FirstOrDefault(x => x.Name == tb.Text);

            if (seat.Name != "PR" && seat.Name != "MA" && seat.Name != "SA")
            {
                if (Database.TempData.Ticket.Passengers.SingleOrDefault(x => x.Seat == seat) == null)
                {
                    if (Database.TempData.Ticket.Passengers.Count() < 4)
                    {
                        if (seat.State == SeatState.Empty)
                        {
                            if (seat.NextState == SeatState.Female && Database.TempData.SelectedGender == Gender.Male)
                            {
                                Database.TempData.Ticket.Passengers.Add(new Passenger() { Seat = seat });
                                grid.Background = new SolidColorBrush(Colors.Transparent);
                                rect.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xC2, 0x00));
                            }
                            else if (seat.NextState == SeatState.Female && Database.TempData.SelectedGender == Gender.Female)
                            {
                                App.ShowProgress("seçtiğiniz koltuk bayan yolcularımıza uygun değildir", ProgressType.Warning, ProgressTime.Normal);
                            }
                            else if (seat.NextState == SeatState.Male && Database.TempData.SelectedGender == Gender.Female)
                            {
                                Database.TempData.Ticket.Passengers.Add(new Passenger() { Seat = seat });
                                grid.Background = new SolidColorBrush(Colors.Transparent);
                                rect.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xC2, 0x00));
                            }
                            else if (seat.NextState == SeatState.Male && Database.TempData.SelectedGender == Gender.Male)
                            {
                                App.ShowProgress("seçtiğiniz koltuk bay yolcularımız için uygun değildir", ProgressType.Warning, ProgressTime.Normal);
                            }
                            else
                            {
                                Database.TempData.Ticket.Passengers.Add(new Passenger() { Seat = seat });
                                grid.Background = new SolidColorBrush(Colors.Transparent);
                                rect.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xC2, 0x00));
                            }
                        }
                        else
                        {
                            App.ShowProgress("seçtiğiniz koltuk daha önceden alınmış", ProgressType.Warning, ProgressTime.Normal);
                        }
                    }
                    else
                    {
                        App.ShowProgress("tek seferde en fazla 4 yolcu için işlem yapabilirsiniz", ProgressType.Warning, ProgressTime.Normal);
                    }
                }
                else
                {
                    Database.TempData.Ticket.Passengers.Remove(Database.TempData.Ticket.Passengers.SingleOrDefault(x => x.Seat == seat));
                    grid.Background = new SolidColorBrush(Colors.Transparent);
                    if (seat.NextState == SeatState.Male)
                        rect.Fill = new SolidColorBrush(Color.FromArgb(0x90, 0xFF, 0x69, 0xB4));
                    else if (seat.NextState == SeatState.Female)
                        rect.Fill = new SolidColorBrush(Color.FromArgb(0x90, 0x33, 0x99, 0xFF));
                    else
                        rect.Fill = new SolidColorBrush(Color.FromArgb(0x90, 0xFF, 0xC2, 0x00));
                }
            }
        }

        public static void SetPassengerGender(Gender? gender)
        {
            Logger.MethodCalled("Functions.SetPassengerGender(Gender)", new object[] { gender });
            if (gender == null)
                return;
            foreach (var passenger in Database.TempData.Ticket.Passengers.ToList())
            {
                passenger.Gender = gender;
            }
        }

        public static void ScrollTo(ScrollViewer scrollViewer, Grid layoutRoot, double offset)
        {
            Logger.MethodCalled("Functions.ScrollTo(ScrollViewer, Grid, double)");
            double y = scrollViewer.VerticalOffset;
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += (s, e) =>
            {
                scrollViewer.ScrollToVerticalOffset(y);
                if (offset <= y || layoutRoot.Height <= y)
                    timer.Stop();
                y += 25;
            };
            timer.Start();
        }

        public static void UpdateTicket(PNR pnr, NavigationService navigation)
        {
            Logger.MethodCalled("Functions.UpdateTicket(PNR, NavigationService)", new object[] { pnr });
            if (!App.IsInternetAvailable)
            {
                var savedTicket = Database.SavedData.Tickets.SingleOrDefault(x => x.PNR.Code == pnr.Code);
                if (savedTicket == null)
                {
                    App.ShowProgress("internet bağlantınızı kontrol ediniz", ProgressType.Warning, ProgressTime.Normal);
                }
                else
                {
                    App.ShowProgress("güncel bilgiler yüklenemedi", ProgressType.Error, ProgressTime.Normal);
                    App.ShowProgress("kayıtlı bilgiler yükleniyor", ProgressType.Warning, ProgressTime.Normal);
                    Database.TempData.Ticket = savedTicket;
                    var timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromMilliseconds(2000);
                    timer.Tick += (c, r) =>
                    {
                        timer.Stop();
                        App.HideProgress();
                        navigation.Navigate(new Uri("/Pages/TicketAction/DetailsPage.xaml", UriKind.RelativeOrAbsolute));
                    };
                    timer.Start();
                }
            }
            else
            {
                App.ShowProgress("bilet güncelleniyor...");
                App.AddBackPressedEvent(_ticketUpdateBackPressed);
                TicketRequests.TicketRequest.OnCompleted = (response) =>
                {
                    var ticket = response.Result;
                    Database.TempData.Ticket = ticket;
                    App.RemoveBackPressedEvent(_ticketUpdateBackPressed);
                    App.HideProgress();
                    if (ticket == null)
                    {
                        App.ShowProgress("bilet bulunamadı", ProgressType.Error, ProgressTime.Normal);
                    }
                    else
                    {
                        Database.UpdateTicket(ticket);
                        navigation.Navigate(new Uri("/Pages/TicketAction/DetailsPage.xaml", UriKind.RelativeOrAbsolute));
                    }
                };
                TicketRequests.GetTicket(pnr);
            }
        }

        private static void _ticketUpdateBackPressed()
        {
            App.HideProgress();
            TicketRequests.TicketRequest.Cancel();
        }


        public static List<Passenger> ClonePassengers(List<Passenger> list)
        {
            Logger.MethodCalled("Functions.ClonePassengers(List<Passenger>)", new object[] { list });
            var cloneList = new List<Passenger>();
            foreach (var item in list)
                cloneList.Add(new Passenger() { Type = item.Type });
            return cloneList;
        }

        internal static string FillPassengersText(List<Passenger> passengers)
        {
            Logger.MethodCalled("Functions.FillPassengersText(List<Passenger>)", new object[] { passengers });
            int adultCount = passengers.Where(x => x.Type == PassengerType.Adult).Count();
            int childCount = passengers.Where(x => x.Type == PassengerType.Child).Count();
            int babyCount = passengers.Where(x => x.Type == PassengerType.Baby).Count();
            int infantCount = passengers.Where(x => x.Type == PassengerType.Infant).Count();
            int studentCount = passengers.Where(x => x.Type == PassengerType.Student).Count();
            int militaryCount = passengers.Where(x => x.Type == PassengerType.Military).Count();
            int teenagerCount = passengers.Where(x => x.Type == PassengerType.Teenager).Count();

            var sb = new StringBuilder();

            if (adultCount != 0)
                sb.Append(adultCount + " yetişkin, ");
            if (childCount != 0)
                sb.Append(childCount + " çocuk, ");
            if (babyCount != 0)
                sb.Append(babyCount + " bebek, ");
            if (infantCount != 0)
                sb.Append(infantCount + " yaşlı, ");
            if (studentCount != 0)
                sb.Append(studentCount + " öğrenci, ");
            if (militaryCount != 0)
                sb.Append(militaryCount + " asker, ");
            if (teenagerCount != 0)
                sb.Append(teenagerCount + " genç");

            return sb.ToString().Trim().TrimEnd(',');
        }

        internal static void UpdateStations()
        {
            Logger.MethodCalled("Functions.UpdateStations()");
            if (App.IsInternetAvailable)
            {
                // Eğer istasyonların son yükleme tarihinden x saat geçmişse güncelle
                var timeSpan = (TimeSpan)(DateTime.Now - Database.SavedData.StationsUpdatedDate);
                if (timeSpan.TotalHours >= Constraints.StationsMinimumUpdateHour && App.IsInternetAvailable)
                {
                    StationRequests.FromStationsRequest.OnCompleted = (response) =>
                    {
                        if (response.Status == ResponseStatus.Successful)
                        {
                            App.HideProgress();
                            var stations = response.Result;
                            Database.SavedData.StationsUpdatedDate = DateTime.Now;
                            Database.SavedData.Stations = stations;
                        }
                    };
                    StationRequests.GetFromStations();

                    StationRequests.AirportsRequest.OnCompleted = (response) =>
                    {
                        if (response.Status == ResponseStatus.Successful)
                        {
                            App.HideProgress();
                            var stations = response.Result;
                            Database.SavedData.StationsUpdatedDate = DateTime.Now;
                            Database.SavedData.Airports = stations;
                        }
                    };
                    StationRequests.GetAirports();
                }
                else
                {
                    Biletall.Helper.Global.LoadStations(Database.SavedData.Airports);
                }
            }
            else
                App.ShowProgress("İnternet bağlantınzı kontrol ediniz", ProgressType.Error, ProgressTime.Normal);
        }
    }
}
