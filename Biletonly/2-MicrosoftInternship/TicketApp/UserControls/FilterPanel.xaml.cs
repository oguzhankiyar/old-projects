using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Biletall.Helper;
using Biletall.Helper.Entities;
using Biletall.Helper.Enums;
using Biletall.Helper.Parsings;
using Biletall.Helper.Requests;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TicketApp.Models;

namespace TicketApp.UserControls
{
    public partial class FilterPanel : UserControl
    {
        bool isExpanded = false;
        int orderType;
        List<Factory> SelectedFactories = new List<Factory>();
        List<Journey> TempJourneys = new List<Journey>();
        List<Factory> AllFactories = new List<Factory>();

        private Color _firstColor = Colors.Black;
        private Color _secondColor = Color.FromArgb(0xFF, 0xF5, 0xF5, 0xF5); //Color.FromArgb(0xFF, 0xE5, 0xE5, 0xE5);

        public static DependencyProperty IsReturnProperty =
            DependencyProperty.Register("IsReturn", typeof(bool), typeof(FilterPanel), null);
        public bool IsReturn
        {
            get { return (bool)GetValue(IsReturnProperty); }
            set
            {
                SetValue(IsReturnProperty, value);
                if (value)
                {
                    StationText.Text = Database.TempData.TicketSearch.To.Name + " » " + Database.TempData.TicketSearch.From.Name;
                    tbDepartureDate.Text = Database.TempData.TicketSearch.ReturnDate.ToString("d MMM, ddd");
                }
            }
        }

        public FilterPanel()
        {
            Logger.MethodCalled("FilterPanel()");
            InitializeComponent();
            LayoutRoot.DataContext = Database.TempData.TicketSearch;
            TempJourneys = Database.TempData.Journeys;
            if (Database.TempData.Ticket.Type == TicketType.BusJourney)
                AllFactories = Global.Factories.Where(x => Database.TempData.Journeys.Where(y => y.Factory.Id == x.Id).Any()).OrderBy(x => x.Name).ToList();
            else
                AllFactories = Global.Factories.Where(x => Database.TempData.Journeys.Where(y => y.Factory.Code == x.Code).Any()).OrderBy(x => x.Name).ToList();
            CreateFactoriesGrid();
            Time_Tap(null, null);
        }
        
        public void CreateFactoriesGrid()
        {
            Logger.MethodCalled("FilterPanel.CreateFactoriesGrid()");
            if (FactoriesGrid.Children.Count() > 0)
                FactoriesGrid.Children.RemoveAt(0);
            SelectedFactories = new List<Factory>();
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            int i = 0;
            foreach (var item in AllFactories)
            {
                SelectedFactories.Add(item);
                var factoryGrid = new Grid();
                if (i % 4 == 0)
                    grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                Grid.SetRow(factoryGrid, i / 4);
                Grid.SetColumn(factoryGrid, i % 4);

                int factoryCount = AllFactories.Count();
                if (factoryCount % 4 == 1 && i + 1 == factoryCount)
                    Grid.SetColumnSpan(factoryGrid, 4);
                else if (factoryCount % 4 == 2 && i + 2 == factoryCount)
                    Grid.SetColumnSpan(factoryGrid, 2);
                else if (factoryCount % 4 == 2 && i + 1 == factoryCount)
                {
                    Grid.SetColumnSpan(factoryGrid, 2);
                    Grid.SetColumn(factoryGrid, 2);
                }
                else if (factoryCount % 4 == 3 && i + 2 == factoryCount)
                    Grid.SetColumnSpan(factoryGrid, 2);
                else if (factoryCount % 4 == 3 && i + 1 == factoryCount)
                    Grid.SetColumn(factoryGrid, 3);

                factoryGrid.Height = 60;
                factoryGrid.Margin = new Thickness(3, 0, 3, 5);
                factoryGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xEE, 0xEE, 0xEE));
                var innerText = new TextBlock() { Text = item.Name, FontSize = 15, TextAlignment = TextAlignment.Center, VerticalAlignment = VerticalAlignment.Center, TextWrapping = TextWrapping.Wrap, Padding = new Thickness(2, 15, 2, 15) };
                innerText.Foreground = new SolidColorBrush(Colors.Black);
                factoryGrid.Tap += (c, r) =>
                    {
                        Logger.SelectionChanged("factoryGrid", item);
                        if (SelectedFactories.Contains(item))
                        {
                            factoryGrid.Background = new SolidColorBrush(Colors.Transparent);
                            innerText.Foreground = new SolidColorBrush(Colors.DarkGray);
                            SelectedFactories.Remove(item);
                        }
                        else
                        {
                            factoryGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xEE, 0xEE, 0xEE));
                            innerText.Foreground = new SolidColorBrush(Colors.Black);
                            SelectedFactories.Add(item);
                        }
                        if (Database.TempData.Ticket.Type == TicketType.BusJourney)
                            Database.TempData.Journeys = TempJourneys.Where(x => SelectedFactories.Where(y => y.Id == x.Factory.Id).Count() > 0).ToList();
                        else
                            Database.TempData.Journeys = TempJourneys.Where(x => SelectedFactories.Where(y => y.Code == x.Factory.Code).Count() > 0).ToList();
                        orderRefresh();
                        if (JourneyRequests.JourneysRequest.OnCompleted != null)
                            JourneyRequests.JourneysRequest.OnCompleted(new BaseResponse<List<Journey>>() { Result = Database.TempData.Journeys });
                    };
                factoryGrid.Children.Add(innerText);
                grid.Children.Add(factoryGrid);
                i++;
            }
            FactoriesGrid.Children.Add(grid);
        }

        private void Down_Tap(object sender, EventArgs e)
        {
            Logger.Clicked("FilterPanel.Down");
            TogglePanel();
        }

        private void BackPressedForToggle()
        {
            TogglePanel();
        }

        private void TogglePanel()
        {
            var sb = new Storyboard();
            var da = new DoubleAnimation() { To = 50, Duration = TimeSpan.FromMilliseconds(200) };
            if (!isExpanded)
                da.To = 195 + (int)((int)(AllFactories.Count() / 4) + (AllFactories.Count() % 4 != 0 ? 1 : 0)) * 65;
            Storyboard.SetTarget(da, LayoutRoot);
            Storyboard.SetTargetProperty(da, new PropertyPath("(Grid.Height)"));
            sb.Children.Add(da);
            sb.Completed += (c, r) =>
            {
                isExpanded = !isExpanded;

                if (isExpanded)
                    App.AddBackPressedEvent(BackPressedForToggle);
                else
                    App.RemoveBackPressedEvent(BackPressedForToggle);
            };
            RotateControlImage();
            sb.Begin();
        }

        private void RotateControlImage()
        {
            if (isExpanded)
                ControlImage.Source = new BitmapImage(new Uri("/Assets/down.png", UriKind.RelativeOrAbsolute));
            else
                ControlImage.Source = new BitmapImage(new Uri("/Assets/up.png", UriKind.RelativeOrAbsolute));
        }

        private void PrevDay_Tap(object sender, EventArgs e)
        {
            Logger.Clicked("PrevDay");
            App.AddBackPressedEvent(BackPressedForDayChanging);

            if (JourneyRequests.JourneysRequest.IsCompleted)
            {
                App.ShowProgress("önceki gün yükleniyor...");
                if (IsReturn)
                    Database.TempData.TicketSearch.ReturnDate = Database.TempData.TicketSearch.ReturnDate.AddDays(-1);
                else
                    Database.TempData.TicketSearch.DepartureDate = Database.TempData.TicketSearch.DepartureDate.AddDays(-1);
                JourneyRequests.JourneysRequest.OnCompleted += Journeys_Completed;
                JourneyRequests.GetJourneys(Database.TempData.TicketSearch);
            }
        }

        private void NextDay_Tap(object sender, EventArgs e)
        {
            Logger.Clicked("NextDay");
            App.AddBackPressedEvent(BackPressedForDayChanging);

            if (JourneyRequests.JourneysRequest.IsCompleted)
            {
                App.ShowProgress("sonraki gün yükleniyor...");
                if (IsReturn)
                    Database.TempData.TicketSearch.ReturnDate = Database.TempData.TicketSearch.ReturnDate.AddDays(+1);
                else
                    Database.TempData.TicketSearch.DepartureDate = Database.TempData.TicketSearch.DepartureDate.AddDays(+1);
                JourneyRequests.JourneysRequest.OnCompleted += Journeys_Completed;
                JourneyRequests.GetJourneys(Database.TempData.TicketSearch);
            }
        }
        
        private void BackPressedForDayChanging()
        {
            App.RemoveBackPressedEvent(BackPressedForDayChanging);
            JourneyRequests.JourneysRequest.Cancel();
            App.HideProgress();
        }

        private void Journeys_Completed(BaseResponse<List<Journey>> response)
        {
            var journeys = response.Result;
            App.RemoveBackPressedEvent(BackPressedForDayChanging);
            App.HideProgress();
            JourneyRequests.JourneysRequest.OnCompleted -= Journeys_Completed;
            TempJourneys = journeys;
            if (Database.TempData.Ticket.Type == TicketType.BusJourney)
                AllFactories = Global.Factories.Where(x => Database.TempData.Journeys.Where(y => y.Factory.Id == x.Id).Count() > 0).ToList();
            else
                AllFactories = Global.Factories.Where(x => Database.TempData.Journeys.Where(y => y.Factory.Code == x.Code).Count() > 0).ToList();

            if (IsReturn)
                tbDepartureDate.Text = Database.TempData.TicketSearch.ReturnDate.ToString("d MMM, ddd");
            else
                tbDepartureDate.Text = Database.TempData.TicketSearch.DepartureDate.ToString("d MMM, ddd");
            CreateFactoriesGrid();
            orderRefresh();
        }

        private void orderRefresh()
        {
            Logger.MethodCalled("FilterPanel.orderRefresh()");
            switch (orderType)
            {
                case 1:
                    orderType = 2;
                    Price_Tap(null, null);
                    break;
                case 2:
                    orderType = 1;
                    Price_Tap(null, null);
                    break;
                case 4:
                    orderType = 3;
                    Time_Tap(null, null);
                    break;
                default:
                    orderType = 4;
                    Time_Tap(null, null);
                    break;
            }
        }

        private void Price_Tap(object sender, EventArgs e)
        {
            Logger.Clicked("FilterPanel.Price");
            if (orderType == 1)
            {
                TimeGrid.Background = new SolidColorBrush(Colors.Transparent);
                TimeText.Foreground = new SolidColorBrush(_firstColor);
                PriceGrid.Background = new SolidColorBrush(_secondColor);
                PriceText.Foreground = new SolidColorBrush(_firstColor);
                PriceImage.Source = new BitmapImage(new Uri("/Assets/up.png", UriKind.RelativeOrAbsolute));
                orderType = 2;

                Database.TempData.Journeys = Database.TempData.Journeys.OrderByDescending(x => x.Price.TotalPrice).ToList();
            }
            else
            {
                TimeGrid.Background = new SolidColorBrush(Colors.Transparent);
                TimeText.Foreground = new SolidColorBrush(_firstColor);
                PriceGrid.Background = new SolidColorBrush(_secondColor);
                PriceText.Foreground = new SolidColorBrush(_firstColor);
                TimeImage.Source = new BitmapImage(new Uri("/Assets/down.png", UriKind.RelativeOrAbsolute));
                PriceImage.Source = new BitmapImage(new Uri("/Assets/down.png", UriKind.RelativeOrAbsolute));
                orderType = 1;

                Database.TempData.Journeys = Database.TempData.Journeys.OrderBy(x => x.Price.TotalPrice).ToList();
            }
            if (JourneyRequests.JourneysRequest.OnCompleted != null)
                JourneyRequests.JourneysRequest.OnCompleted(new BaseResponse<List<Journey>>() { Result = Database.TempData.Journeys });
        }

        private void Time_Tap(object sender, EventArgs e)
        {
            Logger.Clicked("FilterPanel.Time");
            if (orderType == 3)
            {
                PriceGrid.Background = new SolidColorBrush(Colors.Transparent);
                PriceText.Foreground = new SolidColorBrush(_firstColor);
                TimeGrid.Background = new SolidColorBrush(_secondColor);
                TimeText.Foreground = new SolidColorBrush(_firstColor);
                TimeImage.Source = new BitmapImage(new Uri("/Assets/up.png", UriKind.RelativeOrAbsolute));
                orderType = 4;

                Database.TempData.Journeys = Database.TempData.Journeys.OrderByDescending(x => x.Segments.First().DepartureDate).ToList();
            }
            else
            {
                PriceGrid.Background = new SolidColorBrush(Colors.Transparent);
                PriceText.Foreground = new SolidColorBrush(_firstColor);
                TimeGrid.Background = new SolidColorBrush(_secondColor);
                TimeText.Foreground = new SolidColorBrush(_firstColor);
                PriceImage.Source = new BitmapImage(new Uri("/Assets/down.png", UriKind.RelativeOrAbsolute));
                TimeImage.Source = new BitmapImage(new Uri("/Assets/down.png", UriKind.RelativeOrAbsolute));
                orderType = 3;

                Database.TempData.Journeys = Database.TempData.Journeys.OrderBy(x => x.Segments.First().DepartureDate).ToList();
            }
            if (JourneyRequests.JourneysRequest.OnCompleted != null)
                JourneyRequests.JourneysRequest.OnCompleted(new BaseResponse<List<Journey>>() { Result = Database.TempData.Journeys });
        }
        
        private void SelectAll_Clicked(object sender, EventArgs e)
        {
            Logger.Clicked("FilterPanel.SelectAll");
            SelectedFactories = AllFactories;
            Database.TempData.Journeys = TempJourneys;
            if (JourneyRequests.JourneysRequest.OnCompleted != null)
                JourneyRequests.JourneysRequest.OnCompleted(new BaseResponse<List<Journey>>() { Result = Database.TempData.Journeys });

            var factories = (FactoriesGrid.Children[0] as Grid).Children.ToList();
            foreach (Grid item in factories)
            {
                var tb = item.Children[0] as TextBlock;
                item.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xEE, 0xEE, 0xEE));
                tb.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void UnselectAll_Clicked(object sender, EventArgs e)
        {
            Logger.Clicked("FilterPanel.Unselectall");
            SelectedFactories = new List<Factory>();
            Database.TempData.Journeys = new List<Journey>();
            if (JourneyRequests.JourneysRequest.OnCompleted != null)
                JourneyRequests.JourneysRequest.OnCompleted(new BaseResponse<List<Journey>>() { Result = Database.TempData.Journeys });

            var factories = (FactoriesGrid.Children[0] as Grid).Children.ToList();
            foreach (Grid item in factories)
            {
                var tb = item.Children[0] as TextBlock;
                item.Background = new SolidColorBrush(Colors.Transparent);
                tb.Foreground = new SolidColorBrush(Colors.DarkGray);
            }
        }
    }
}
