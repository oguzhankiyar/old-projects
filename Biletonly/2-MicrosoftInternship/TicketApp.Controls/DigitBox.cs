using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TicketApp.Controls
{
    public class DigitBox : Control
    {
        #region Members
        public static DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(DigitBox), null);
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int?), typeof(DigitBox), null);
        public int? Value
        {
            get { return (int?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static DependencyProperty MinProperty =
            DependencyProperty.Register("Min", typeof(int?), typeof(DigitBox), null);
        public int? Min
        {
            get { return (int?)GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }

        public static DependencyProperty MaxProperty =
            DependencyProperty.Register("Max", typeof(int?), typeof(DigitBox), null);
        public int? Max
        {
            get { return (int?)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }
        #endregion

        #region Constructor
        public DigitBox()
        {
            DefaultStyleKey = typeof(DigitBox);
        }
        #endregion

        #region Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var layoutRoot = GetTemplateChild("LayoutRoot") as Grid;
            layoutRoot.ManipulationStarted += layoutRoot_ManipulationStarted;
            layoutRoot.ManipulationDelta += layoutRoot_ManipulationDelta;

            var incrementGrid = GetTemplateChild("IncrementGrid") as Grid;
            incrementGrid.Tap += (s, e) =>
            {
                IncrementValue();
            };

            var decrementGrid = GetTemplateChild("DecrementGrid") as Grid;
            decrementGrid.Tap += (s, e) =>
            {
                DecrementValue();
            };
        }

        private Point _initialPoint;
        private void layoutRoot_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            _initialPoint = e.ManipulationOrigin;
        }

        private void layoutRoot_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            var currentpoint = e.ManipulationOrigin;
            if (currentpoint.X - _initialPoint.X >= 100)
            {
                IncrementValue();
                e.Complete();
            }
            else if (_initialPoint.X - currentpoint.X >= 100)
            {
                DecrementValue();
                e.Complete();
            }
        }

        private void IncrementValue()
        {
            if (this.Value == null)
                this.Value = this.Min == null ? 0 : this.Min;
            else if (this.Max == null || this.Value != this.Max)
                this.Value += 1;
        }

        private void DecrementValue()
        {
            if (this.Value == null)
                this.Value = this.Min == null ? 0 : this.Min;
            else if (this.Min == null || this.Value != this.Min)
                this.Value -= 1;
        }
        #endregion
    }
}
