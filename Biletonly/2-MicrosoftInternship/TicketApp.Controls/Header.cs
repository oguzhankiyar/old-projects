using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using TicketApp.Controls.Enums;

namespace TicketApp.Controls
{
    public class Header : Control
    {
        #region Members
        private Grid _logoGrid;
        private Grid _progressGrid;
        private Grid _backGrid;
        private TextBlock _titleText;
        private TextBlock _progressText;
        private ProgressRing _progressRing;
        private Image _progressImage;
        private Grid _progressButtons;
        private Grid _layoutRoot;
        private static Stack<Action> _progressStack = new Stack<Action>();
        private static ProgressTime _lastTime;

        private static Header Current { get; set; }

        private static bool _isHeaderLoaded = true;
        private static event EventHandler _onHeaderLoaded;

        private static bool _isProgressCompleted = true;

        public static event EventHandler BackPressed;
        public static Action<int> HeaderConfirmed = null;
        public static Action OnProgressCompleted;
        #endregion

        #region Constructor
        public Header()
        {
            DefaultStyleKey = typeof(Header);
            _isHeaderLoaded = false;
            this.Loaded += Header_Loaded;
        }
        #endregion

        #region Methods
        private void Header_Loaded(object sender, RoutedEventArgs e)
        {
            Current = this;
            _isHeaderLoaded = true;
            if (_onHeaderLoaded != null)
                _onHeaderLoaded(null, null);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _layoutRoot = GetTemplateChild("LayoutRoot") as Grid;
            _logoGrid = GetTemplateChild("LogoGrid") as Grid;
            _progressGrid = GetTemplateChild("ProgressGrid") as Grid;
            _backGrid = GetTemplateChild("BackGrid") as Grid;
            _titleText = GetTemplateChild("TitleText") as TextBlock;
            _progressText = GetTemplateChild("ProgressText") as TextBlock;
            _progressRing = GetTemplateChild("ProgressRing") as ProgressRing;
            _progressImage = GetTemplateChild("ProgressImage") as Image;
            _progressButtons = GetTemplateChild("ProgressButtons") as Grid;

            _backGrid.Tap += (sender, e) =>
            {
                if (BackPressed != null)
                    BackPressed(null, null);
            };
        }


        public static void SetTitle(string title)
        {
            if (_isHeaderLoaded)
            {
                if (_isProgressCompleted)
                {
                    if (Current != null)
                        Current._SetTitle(title);
                }
                else
                {
                    _progressStack.Push(() =>
                    {
                        if (Current != null)
                            Current._SetTitle(title);
                    });
                }
            }
            else
            {
                _onHeaderLoaded += (sender, e) =>
                {
                    if (_isProgressCompleted)
                    {
                        if (Current != null)
                            Current._SetTitle(title);
                    }
                    else
                    {
                        _progressStack.Push(() =>
                        {
                            if (Current != null)
                                Current._SetTitle(title);
                        });
                    }
                };
            }
        }

        private void _SetTitle(string title)
        {
            _onHeaderLoaded = null;
            _titleText.Text = title;
        }

        public static void ShowProgress(string text, ProgressType type = ProgressType.Loading, ProgressTime time = ProgressTime.Infinite, List<string> buttons = null)
        {
            if (_isHeaderLoaded)
            {
                if (_isProgressCompleted || _lastTime == ProgressTime.Infinite)
                {
                    if (Current != null)
                        Current._ShowProgress(text, type, time, buttons);
                }
                else
                {
                    _progressStack.Push(() =>
                    {
                        if (Current != null)
                            Current._ShowProgress(text, type, time, buttons);
                    });
                }
            }
            else
            {
                _onHeaderLoaded += (s, e) =>
                {
                    if (_isProgressCompleted || _lastTime == ProgressTime.Infinite)
                    {
                        if (Current != null)
                            Current._ShowProgress(text, type, time, buttons);
                    }
                    else
                    {
                        _progressStack.Push(() =>
                        {
                            if (Current != null)
                                Current._ShowProgress(text, type, time, buttons);
                        });
                    }
                };
            }
        }

        private void _ShowProgress(string text, ProgressType type = ProgressType.Loading, ProgressTime time = ProgressTime.Infinite, List<string> buttons = null)
        {
            HideProgress();
            _lastTime = time;
            _isProgressCompleted = false;
            _progressText.Text = text;
            
            if (!string.IsNullOrEmpty(text))
                _logoGrid.Visibility = Visibility.Collapsed;
            _progressGrid.Visibility = Visibility.Visible;

            _progressRing.Visibility = Visibility.Collapsed;
            _progressImage.Visibility = Visibility.Collapsed;
            _progressText.TextWrapping = TextWrapping.NoWrap;
            _progressButtons.Visibility = Visibility.Collapsed;

            switch (type)
            {
                case ProgressType.Loading:
                    _progressRing.Visibility = Visibility.Visible;
                    break;
                case ProgressType.Success:
                    BackgroundAnimation(Colors.Green);
                    _progressImage.Visibility = Visibility.Visible;
                    _progressImage.Source = new BitmapImage(new Uri("/Assets/success.png", UriKind.RelativeOrAbsolute));
                    break;
                case ProgressType.Warning:
                    BackgroundAnimation(Color.FromArgb(0xFF, 0xFF, 0x72, 0x56));
                    _progressImage.Visibility = Visibility.Visible;
                    _progressImage.Source = new BitmapImage(new Uri("/Assets/warning.png", UriKind.RelativeOrAbsolute));
                    break;
                case ProgressType.Error:
                    BackgroundAnimation(Colors.Red);
                    _progressImage.Visibility = Visibility.Visible;
                    _progressImage.Source = new BitmapImage(new Uri("/Assets/error.png", UriKind.RelativeOrAbsolute));
                    break;
                default:
                    break;
            }

            if (buttons != null && buttons.Count() != 0)
            {
                int i = 0;
                foreach (var buttonText in buttons)
                {
                    var button = new Button() { Value = buttonText };
                    _progressButtons.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                    Grid.SetColumn(button, i);
                    button.Clicked += (s, e) =>
                    {
                        if (HeaderConfirmed != null)
                            HeaderConfirmed(Grid.GetColumn(s as Button));
                        HideProgress();
                    };
                    _progressButtons.Children.Add(button);
                    i++;
                }
                _progressButtons.Visibility = Visibility.Visible;
            }

            HeightControl();

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
                    _isProgressCompleted = true;
                    HideProgress();
                    timer.Stop();
                };
                timer.Start();
            }
            else
                _isProgressCompleted = false;
        }

        private void HeightControl()
        {
            _progressText.TextWrapping = TextWrapping.NoWrap;
            double availableWidth = _layoutRoot.ActualWidth - (_progressImage.Visibility == Visibility.Collapsed ? 45 : 77);
            if (_progressText.ActualWidth > availableWidth || _progressButtons.Visibility == Visibility.Visible)
            {
                if (availableWidth >= 0)
                {
                    double newHeight = (_progressText.ActualWidth / availableWidth) * 35 + 50;
                    _progressText.TextWrapping = TextWrapping.Wrap;
                    if (newHeight > 425)
                        newHeight = 425;
                    if (_progressButtons.Visibility == Visibility.Visible)
                        newHeight += 75;
                    _progressText.Width = availableWidth;
                    HeightAnimation(newHeight);
                }
            }
        }
        
        public static void HideProgress()
        {
            if (_isHeaderLoaded)
            {
                if (Current != null)
                    Current._HideProgress();
            }
            else
            {
                _onHeaderLoaded = (sender, e) =>
                {
                    if (Current != null)
                        Current._HideProgress();
                };
            }
        }

        private void _HideProgress()
        {
            if (OnProgressCompleted != null)
                OnProgressCompleted();

            BackgroundAnimation(Colors.Orange);
            HeightAnimation(65);

            _isProgressCompleted = true;
            _progressText.Text = "";
            _progressGrid.Visibility = Visibility.Collapsed;
            _logoGrid.Visibility = Visibility.Visible;

            //HeaderConfirmed = null;
            _progressButtons.Children.Clear();
            _progressButtons.ColumnDefinitions.Clear();

            if (_progressStack.Any())
            {
                var action = _progressStack.Pop();
                action();
            }
        }

        private void BackgroundAnimation(Color color)
        {
            Dispatcher.BeginInvoke(() =>
            {
                var d = new ColorAnimation() { To = color, Duration = TimeSpan.FromMilliseconds(500) };
                var s = new Storyboard();
                Storyboard.SetTarget(d, _layoutRoot);
                Storyboard.SetTargetProperty(d, new PropertyPath("(Grid.Background).(SolidColorBrush.Color)"));
                s.Children.Add(d);
                s.Begin();
            });
        }

        private void HeightAnimation(double toHeight)
        {
            Dispatcher.BeginInvoke(() =>
            {
                var sb = new Storyboard();
                var da = new DoubleAnimation() { From = _layoutRoot.ActualHeight, To = toHeight, Duration = TimeSpan.FromMilliseconds(350) };
                Storyboard.SetTarget(da, _layoutRoot);
                Storyboard.SetTargetProperty(da, new PropertyPath("(Grid.Height)"));
                sb.Children.Add(da);
                sb.Begin();
            });
        }
        #endregion
    }
}
