using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TicketApp.Enums;

namespace TicketApp.UserControls
{
    public partial class Header : UserControl
    {
        private static Header _current;
        public static Header Current
        {
            get
            {
                return _current;
            }
            set
            {
                _current = value;
                if (value != null)
                {
                    IsHeaderLoaded = true;
                    if (OnHeaderLoaded != null)
                        OnHeaderLoaded();
                }
            }
        }
        public static bool IsHeaderLoaded { get; set; }
        public static Action OnHeaderLoaded { get; set; }

        // back işleminde title karışıklığını önlemek için
        public static bool IsProgressCompleted = true;
        public static Action OnProgressCompleted { get; set; }

        public Header()
        {
            InitializeComponent();
            IsHeaderLoaded = false;
            this.Loaded += Header_Loaded;
        }

        void Header_Loaded(object sender, RoutedEventArgs e)
        {
            Current = this;
        }

        public void SetTitle(string title)
        {
            OnHeaderLoaded = null;
            if (IsProgressCompleted)
            {
                TitleText.Text = title;
            }
            else
            {
                OnProgressCompleted = () =>
                {
                    TitleText.Text = title;
                };
            }
        }
        
        public void ShowProgress(string text, ProgressType type = ProgressType.Loading, ProgressTime time = ProgressTime.Infinite)
        {
            IsProgressCompleted = false;
            ProgressText.Text = text;
            if (!string.IsNullOrEmpty(text))
                LogoGrid.Visibility = Visibility.Collapsed;
            ProgressGrid.Visibility = Visibility.Visible;

            ProgressRing.Visibility = Visibility.Collapsed;
            ProgressImage.Visibility = Visibility.Collapsed;

            switch (type)
            {
                case ProgressType.Loading:
                    ProgressRing.Visibility = Visibility.Visible;
                    break;
                case ProgressType.Success:
                    ProgressImage.Visibility = Visibility.Visible;
                    ProgressImage.Source = new BitmapImage(new Uri("/Assets/success.png", UriKind.RelativeOrAbsolute));
                    break;
                case ProgressType.Warning:
                    ProgressImage.Visibility = Visibility.Visible;
                    ProgressImage.Source = new BitmapImage(new Uri("/Assets/warning.png", UriKind.RelativeOrAbsolute));
                    break;
                case ProgressType.Error:
                    ProgressImage.Visibility = Visibility.Visible;
                    ProgressImage.Source = new BitmapImage(new Uri("/Assets/error.png", UriKind.RelativeOrAbsolute));
                    break;
                default:
                    break;
            }

            if (time != ProgressTime.Infinite)
            {
                var timer = new DispatcherTimer();
                if (time == ProgressTime.Short)
                    timer.Interval = TimeSpan.FromMilliseconds(1000);
                else if (time == ProgressTime.Normal)
                    timer.Interval = TimeSpan.FromMilliseconds(1750);
                else
                    timer.Interval = TimeSpan.FromMilliseconds(2500);
                timer.Tick += (c, r) =>
                {
                    IsProgressCompleted = true;
                    HideProgress();
                    timer.Stop();
                };
                timer.Start();
            }
            else
                IsProgressCompleted = true;
        }

        public void HideProgress()
        {
            if (IsProgressCompleted)
            {
                if (OnProgressCompleted != null)
                    OnProgressCompleted();
                ProgressText.Text = "";
                ProgressGrid.Visibility = Visibility.Collapsed;
                LogoGrid.Visibility = Visibility.Visible;
            }
        }

        private void GoBack_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var frame = Application.Current.RootVisual as PhoneApplicationFrame;
            if (frame.BackStack.Count() > 0)
                frame.GoBack();
        }
    }
}
