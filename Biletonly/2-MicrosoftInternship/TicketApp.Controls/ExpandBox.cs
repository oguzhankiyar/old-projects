using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using TicketApp.Controls.Enums;

namespace TicketApp.Controls
{
    public class ExpandBox : Control
    {
        #region Members
        public static DependencyProperty MinimizedPanelProperty =
            DependencyProperty.Register("MinimizedPanel", typeof(UIElement), typeof(ExpandBox), null);
        public UIElement MinimizedPanel
        {
            get { return (UIElement)GetValue(MinimizedPanelProperty); }
            set { SetValue(MinimizedPanelProperty, value); }
        }

        public static DependencyProperty MaximizedPanelProperty =
            DependencyProperty.Register("MaximizedPanel", typeof(UIElement), typeof(ExpandBox), null);
        public UIElement MaximizedPanel
        {
            get { return (UIElement)GetValue(MaximizedPanelProperty); }
            set { SetValue(MaximizedPanelProperty, value); }
        }

        public static DependencyProperty IsOpenedProperty =
            DependencyProperty.Register("IsOpened", typeof(bool), typeof(ExpandBox), null);
        public bool IsOpened
        {
            get { return (bool)GetValue(IsOpenedProperty); }
            set { SetValue(IsOpenedProperty, value); }
        }

        public static DependencyProperty ModeProperty =
            DependencyProperty.Register("Mode", typeof(ExpandBoxMode), typeof(ExpandBox), null);
        public ExpandBoxMode Mode
        {
            get { return (ExpandBoxMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        public event Action<bool> OnStateChanged;

        private DetailBox _layoutRoot;
        private StackPanel _contentPanel;
        private Image _toggleImage;
        private double _minimizedHeight;
        private double _maximizedHeight;
        #endregion

        #region Constructor
        public ExpandBox()
        {
            this.DefaultStyleKey = typeof(ExpandBox);
            this.Loaded += (s, e) =>
            {
                _minimizedHeight = MinimizedPanel.DesiredSize.Height;
                _maximizedHeight = _contentPanel.ActualHeight;
                _contentPanel.Height = _minimizedHeight;
                _toggleImage.Margin = new Thickness(12, (_contentPanel.Height - _toggleImage.DesiredSize.Height) / 2 + 5, 12, 0);
            };
        }
        #endregion

        #region Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _layoutRoot = GetTemplateChild("LayoutRoot") as DetailBox;
            _contentPanel = GetTemplateChild("ContentPanel") as StackPanel;
            _contentPanel.Height = MinimizedPanel.DesiredSize.Height;
            _contentPanel.Children.Add(MinimizedPanel);
            _contentPanel.Children.Add(MaximizedPanel);

            _toggleImage = GetTemplateChild("ToggleImage") as Image;

            if (Mode == ExpandBoxMode.Image)
            {
                _toggleImage.Visibility = Visibility.Visible;
                _toggleImage.Tap += (s, e) =>
                {
                    Toggle();
                };
                MinimizedPanel.Tap += (s, e) =>
                {
                    Toggle();
                };
            }
            else
            {
                _toggleImage.Visibility = Visibility.Collapsed;
                _layoutRoot.Tap += (s, e) =>
                {
                    Toggle();
                };
            }
        }
        
        public void Open()
        {
            var d = new DoubleAnimation() { From = _minimizedHeight, To = _maximizedHeight, Duration = TimeSpan.FromMilliseconds(200) };
            var s = new Storyboard();
            Storyboard.SetTarget(d, _contentPanel);
            Storyboard.SetTargetProperty(d, new PropertyPath("Height"));
            s.Children.Add(d);
            s.Begin();
            s.Completed += (c, r) =>
            {
                IsOpened = true;
                _toggleImage.Source = new BitmapImage(new Uri("/Assets/up_light.png", UriKind.RelativeOrAbsolute));
                if (OnStateChanged != null)
                    OnStateChanged(true);
            };
        }

        public void Close()
        {
            var d = new DoubleAnimation() { From = _maximizedHeight, To = _minimizedHeight, Duration = TimeSpan.FromMilliseconds(200) };
            var s = new Storyboard();
            Storyboard.SetTarget(d, _contentPanel);
            Storyboard.SetTargetProperty(d, new PropertyPath("Height"));
            s.Children.Add(d);
            s.Begin();
            s.Completed += (c, r) =>
            {
                IsOpened = false;
                _toggleImage.Source = new BitmapImage(new Uri("/Assets/down_light.png", UriKind.RelativeOrAbsolute));
                if (OnStateChanged != null)
                    OnStateChanged(false);
            };
        }

        public void Toggle()
        {
            if (IsOpened)
                Close();
            else
                Open();
        }
        #endregion
    }
}
