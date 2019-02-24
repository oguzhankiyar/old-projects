using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace TicketApp.Controls
{
    public class Button : Control
    {
        #region Members
        public static DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(Button), null);
        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static DependencyProperty IsActivatedProperty =
            DependencyProperty.Register("IsActivated", typeof(bool?), typeof(Button), null);
        public bool? IsActivated
        {
            get { return (bool?)GetValue(IsActivatedProperty); }
            set
            {
                SetValue(IsActivatedProperty, value);
                SetActivated();
            }
        }

        public event EventHandler Clicked;

        private Grid _layoutRoot;
        #endregion

        #region Constructor
        public Button()
        {
            DefaultStyleKey = typeof(Button);
        }
        #endregion

        #region Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _layoutRoot = GetTemplateChild("LayoutRoot") as Grid;
            _layoutRoot.Tap += LayoutRoot_Tap;
            this.FontSize = this.FontSize <= 20 ? 25 : this.FontSize;
            if (!double.IsNaN(this.Height))
                _layoutRoot.Height = this.Height;
        }

        private void SetTimer()
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                VisualStateManager.GoToState(this, "Normal", true);
                if (Clicked != null)
                    Clicked(this, e);
            };
            timer.Start();
        }

        private void LayoutRoot_Tap(object sender, GestureEventArgs e)
        {
            VisualStateManager.GoToState(this, "Pressed", true);
            SetTimer();
        }

        private void SetActivated()
        {
            if (_layoutRoot != null)
            {
                if (this.IsActivated == false)
                {
                    VisualStateManager.GoToState(this, "Deactivated", true);
                    _layoutRoot.Tap -= LayoutRoot_Tap;
                }
                else
                {
                    VisualStateManager.GoToState(this, "Normal", true);
                    _layoutRoot.Tap += LayoutRoot_Tap;
                }
            }
            else
            {
                this.Loaded += (s, e) => { SetActivated(); };
            }
        }
        #endregion
    }
}
