using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using OK.CargoTracking.Model;
using System.Globalization;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Xaml.Controls;
using OK.CargoTracking.Windows.Data;
using Windows.UI.Xaml.Media.Animation;

namespace OK.CargoTracking.Windows
{
    public partial class TrackingPage : Page
    {
        private const string LOADING_TEXT = "girdiğiniz takip koduna ait bilgiler aranıyor...";
        private const string TIPS_TEXT = "bir kargo şirketi seçin,\ntakip kodunu yazın,\ndetayları görün.";
        private bool isRequestCompleted = true;

        public TrackingPage()
        {
            this.InitializeComponent();

            this.Loaded += (sender, args) =>
            {
                if (Database.SavedData.Messages.Any())
                    showMessage(Database.SavedData.Messages.First());

                txtInfo.Text = TIPS_TEXT;

                txtCode.GotFocus += (s, e) => { txtCode.Background = new SolidColorBrush(Colors.Transparent); };

                lsFactories.ItemsSource = Database.SavedData.Factories.OrderBy(x => x.Sort).ToList();

                setHistoryList();
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            NavigationService.RemoveBackEntry();
        }

        #region Message Panel
        private void showMessage(Message message)
        {
            pnlMessageContent.Children.Clear();
            if (message.Type == "xaml" && message.Content != null)
            {
                var content = XamlReader.Load(message.Content) as UIElement;
                pnlMessageContent.Children.Add(content);
                message.Content = null;
            }
            pnlMessage.DataContext = message;
            createButtons(message);
            openMessagePanel();
        }

        private void createButtons(Message message)
        {
            pnlButtons.ColumnDefinitions.Clear();
            pnlButtons.Children.Clear();
            int i = 0;
            foreach (var button in message.Buttons)
            {
                pnlButtons.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                var buttonGrid = new Grid();
                buttonGrid.DataContext = button;
                buttonGrid.Tap += btnMessage_Tap;
                buttonGrid.Tag = message;
                buttonGrid.Margin = new Thickness(0, 5, 15, 0);
                buttonGrid.Children.Add(new Rectangle() { Fill = new SolidColorBrush(Colors.Black), Opacity = .5 });
                var textBlock = new TextBlock();
                textBlock.Text = button.Content;
                textBlock.Padding = new Thickness(0, 10, 0, 10);
                textBlock.TextAlignment = TextAlignment.Center;
                textBlock.VerticalAlignment = VerticalAlignment.Center;
                textBlock.FontFamily = new FontFamily("Segoe WP");
                textBlock.FontWeight = FontWeights.ExtraLight;
                textBlock.FontSize = 25;
                buttonGrid.Children.Add(textBlock);
                buttonGrid.SetValue(Grid.ColumnProperty, i);
                pnlButtons.Children.Add(buttonGrid);
                i++;
            }
        }

        private void btnMessage_Tap(object sender, GestureEventArgs e)
        {
            var buttonGrid = sender as Grid;
            if (buttonGrid != null)
            {
                var message = buttonGrid.Tag as Message;
                var button = buttonGrid.DataContext as MessageButton;
                string actions = button.Action;
                if (actions.Contains(';'))
                {
                    foreach (var item in actions.Split(';'))
                    {
                        doAction(item);
                    }
                    Database.RemoveMessage(message);
                    closeMessagePanel();
                }
                else
                {
                    doAction(actions);
                    Database.RemoveMessage(message);
                    closeMessagePanel();
                }
            }
        }

        private void openMessagePanel()
        {
            pnlBack.Visibility = Visibility.Visible;
            var sb = new Storyboard();
            var da = new DoubleAnimation() { To = 0, Duration = TimeSpan.FromMilliseconds(500) };
            Storyboard.SetTarget(da, pnlMessageTranslate);
            Storyboard.SetTargetProperty(da, new PropertyPath("TranslateY"));
            sb.Children.Add(da);
            sb.Begin();
        }

        private void closeMessagePanel()
        {
            pnlBack.Visibility = Visibility.Visible;
            var sb = new Storyboard();
            var da = new DoubleAnimation() { From = 0, To = -500, Duration = TimeSpan.FromMilliseconds(500) };
            Storyboard.SetTarget(da, pnlMessageTranslate);
            Storyboard.SetTargetProperty(da, new PropertyPath("TranslateY"));
            sb.Children.Add(da);
            sb.Completed += (s, e) =>
            {
                if (!Database.SavedData.Messages.Any())
                    pnlBack.Visibility = Visibility.Collapsed;
                else
                    showMessage(Database.SavedData.Messages.First());
            };
            sb.Begin();
        }

        private void doAction(string action)
        {
            string actionName = action.Split('(')[0];
            List<string> parameters = new List<string>();
            string parameter = action.Split('(')[1].Replace(")", "");
            if (parameter.Contains(','))
            {
                foreach (var item in parameter.Split(','))
                {
                    if (!string.IsNullOrEmpty(item))
                        parameters.Add(item);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(parameter))
                    parameters.Add(parameter);
            }
            doAction(actionName, parameters);
        }

        private async void doAction(string actionName, List<string> parameters)
        {
            if(actionName == "launcher")
            {
                string url = parameters[0];
                await Launcher.LaunchUriAsync(new Uri(url, UriKind.RelativeOrAbsolute));
            }
            else if (actionName == "track")
            {
                var factory = Database.SavedData.Factories.FirstOrDefault(x => x.Id == Convert.ToInt32(parameters[0]));
                lsFactories.SelectedItem = factory;
                TrackingPivot.SelectedIndex = 0;
                if (parameters.Count() != 1)
                {
                    txtCode.Text = parameters[1];
                    Search();
                }
            }
            else if (actionName == "updateFactories")
            {
                updateFactories();
            }
            else if (actionName == "updateFactory")
            {
                if (parameters.Any())
                    updateFactories(Convert.ToInt32(parameters[0]));
            }
            else if (actionName == "resetDevice")
            {
                Database.SavedData.IsDeviceRegistered = false;
                Database.Update();
            }
            else if (actionName == "resetData")
            {
                Database.SavedData = new SavedData();
                Database.Update();
            }
            else if (actionName == "resetFactories")
            {
                Database.SavedData.Factories = new List<Factory>();
                Database.Update();
            }
            else if (actionName == "resetMessages")
            {
                Database.SavedData.Messages = new List<Message>();
                Database.Update();
            }
            else if (actionName == "resetHistory")
            {
                Database.SavedData.History = new List<Tracking>();
                Database.Update();
            }
            else if (actionName == "registerDevice")
            {
                Global.Token = CryptoHelper.CreateToken(DeviceHelper.Id);
                var registerResponse = await DeviceRequests.RegisterDeviceAsync();
                if (registerResponse.Status)
                {
                    Database.SavedData.IsDeviceRegistered = true;
                    Database.Update();
                }
            }
        }
        #endregion

        #region Tracking Panel
        private void imgFactory_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (lsFactories.Visibility == Visibility.Collapsed)
                lsFactories.Visibility = Visibility.Visible;
            else
                lsFactories.Visibility = Visibility.Collapsed;
        }

        private void lsFactories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = lsFactories.SelectedItem as Factory;
            if (selected != null)
            {
                imgFactory.Source = Database.GetFactoryImage(selected);
                Database.TempData.Factory = selected;
                lsFactories.Visibility = Visibility.Collapsed;
                lsFactories.SelectedItem = null;

                pnlTrackingDetails.DataContext = null;
                pnlTrackingDetails.Visibility = Visibility.Collapsed;
            }
        }

        private void btnSearch_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Search();
        }
        
        private async void Search()
        {
            if (!isRequestCompleted)
                return;

            if (NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.None)
            {
                var message = new Message();
                message.Content = "İnternet bağlantınızı kontrol ediniz.";
                message.Buttons.Add(new MessageButton("Tamam", "closeMessage()"));
                showMessage(message);
            }
            else if (Database.TempData.Factory == null)
            {
                var message = new Message();
                message.Content = "Arama yapmak için bir kargo şirketi seçmelisiniz.";
                message.Buttons.Add(new MessageButton("Tamam", "closeMessage()"));
                showMessage(message);
            }
            else if (string.IsNullOrEmpty(txtCode.Text))
            {
                var message = new Message();
                message.Content = "Arama yapmak için takip kodunu yazmalısınız.";
                message.Buttons.Add(new MessageButton("Tamam", "closeMessage()"));
                showMessage(message);
            }
            else
            {
                isRequestCompleted = false;
                txtInfo.Text = LOADING_TEXT;
                pnlTrackingDetails.DataContext = null;
                pnlTrackingDetails.Visibility = Visibility.Collapsed;
                lsFactories.Visibility = Visibility.Collapsed;
                
                Database.TempData.Code = txtCode.Text;
                Global.Token = CryptoHelper.CreateToken(DeviceHelper.Id);
                var response = await TrackingRequests.GetDetailsAsync(Database.TempData.Factory, Database.TempData.Code);
                isRequestCompleted = true;
                if (response.Status == true)
                {
                    var tracking = response.Result as Tracking;
                    pnlTrackingDetails.Visibility = Visibility.Visible;
                    tracking.Factory = Database.TempData.Factory;
                    pnlTrackingDetails.DataContext = response.Result;

                    Database.AddTracking(tracking);
                    setHistoryList();
                }
                else if (response.Message != null)
                {
                    showMessage(response.Message);
                }
                else
                {
                    var message = new Message();
                    message.Content = "Aradığınız koda ait kayıt bulunamadı!";
                    message.Buttons.Add(new MessageButton("Tamam", "closeMessage()"));
                    showMessage(message);
                }
            }
        }

        private void txtCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            int selectionStart = txtCode.SelectionStart;
            txtCode.Text = txtCode.Text.ToUpper(new CultureInfo("en-US"));
            txtCode.SelectionStart = selectionStart;

            pnlTrackingDetails.DataContext = null;
            pnlTrackingDetails.Visibility = Visibility.Collapsed;
            txtInfo.Text = TIPS_TEXT;
        }
        #endregion

        #region History Panel
        private void lsHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = lsHistory.SelectedItem as Tracking;
            if (selected != null)
            {
                lsFactories.SelectedItem = selected.Factory;
                txtCode.Text = selected.Code;
                TrackingPivot.SelectedIndex = 0;
                Search();
            }
        }

        private void imgDelete_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var img = sender as Image;
            if (img != null)
            {
                var tracking = img.Tag as Tracking;
                if (tracking != null)
                    Database.RemoveTracking(tracking);
                setHistoryList();
            }
        }

        private void setHistoryList()
        {
            if (Database.SavedData.History.Any())
            {
                txtHistoryEmpty.Visibility = Visibility.Collapsed;
                lsHistory.ItemsSource = null;
                lsHistory.ItemsSource = Database.SavedData.History;
            }
            else
            {
                txtHistoryEmpty.Visibility = Visibility.Visible;
                lsHistory.ItemsSource = null;
            }
        }
        #endregion

        #region About Panel
        private async void updateFactories(int? id = null)
        {
            if (NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.None)
            {
                var message = new Message();
                message.Content = "İnternet bağlantınızı kontrol ediniz.";
                message.Buttons.Add(new MessageButton("Tamam", "closeMessage()"));
                showMessage(message);
            }
            else
            {

                if (id == null)
                {
                    Global.Token = CryptoHelper.CreateToken(DeviceHelper.Id);
                    var factoriesResponse = await FactoryRequests.GetFactoriesAsync();
                    if (factoriesResponse.Status)
                    {
                        var factories = factoriesResponse.Result as List<Factory>;
                        if (factories != null)
                            Database.UpdateFactories(factories);
                        lsFactories.ItemsSource = null;
                        lsFactories.ItemsSource = Database.SavedData.Factories.OrderBy(x => x.Sort).ToList();
                        lsHistory.ItemsSource = null;
                        lsHistory.ItemsSource = Database.SavedData.History;
                    }
                    else if (factoriesResponse.Message != null)
                    {
                        showMessage(factoriesResponse.Message);
                    }
                }
                else
                {
                    Global.Token = CryptoHelper.CreateToken(DeviceHelper.Id);
                    var factoriesResponse = await FactoryRequests.GetFactoryDetailsAsync((int)id);
                    if (factoriesResponse.Status)
                    {
                        var factory = factoriesResponse.Result as Factory;
                        if (factory != null)
                            Database.AddFactory(factory);
                        lsFactories.ItemsSource = null;
                        lsFactories.ItemsSource = Database.SavedData.Factories.OrderBy(x => x.Sort).ToList();
                        lsHistory.ItemsSource = null;
                        lsHistory.ItemsSource = Database.SavedData.History;
                    }
                    else if (factoriesResponse.Message != null)
                    {
                        showMessage(factoriesResponse.Message);
                    }
                }
            }
        }

        private void btnClearHistory_Tap(object sender, GestureEventArgs e)
        {
            Database.ClearHistory();
            lsHistory.ItemsSource = null;
            lsHistory.ItemsSource = Database.SavedData.History;
            setHistoryList();
        }

        private void btnUpdateFactories_Tap(object sender, GestureEventArgs e)
        {
            updateFactories();
        }

        private void btnRateAndReview_Tap(object sender, GestureEventArgs e)
        {
            Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp", UriKind.RelativeOrAbsolute));
        }
        
        private void btnDeveloperApps_Tap(object sender, GestureEventArgs e)
        {
            Launcher.LaunchUriAsync(new Uri("ms-windows-store:search?publisher=Oğuzhan Kiyar", UriKind.RelativeOrAbsolute));
        }
        #endregion
    }
}