using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace TicketApp.Controls
{
    public class DateBox : Control
    {
        #region Members
        public static DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(DateBox), null);
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(DateTime?), typeof(DateBox), null);
        public DateTime? Value
        {
            get { return (DateTime?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        #endregion

        #region Constructor
        public DateBox()
        {
            DefaultStyleKey = typeof(DateBox);
        }
        #endregion

        #region Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var layoutRoot = GetTemplateChild("LayoutRoot") as Grid;
            var datePicker = GetTemplateChild("ValueText") as DatePicker;
            var txtValue = GetTemplateChild("txtValue") as TextBlock;

            if (datePicker.Value != null)
                txtValue.Text = Convert.ToDateTime(datePicker.Value).ToString("d MMMMMMM yyyy");
            datePicker.ValueChanged += (s, e) =>
            {
                txtValue.Text = Convert.ToDateTime(datePicker.Value).ToString("d MMMMMMM yyyy");
            };
            layoutRoot.Tap += (s, e) =>
            {
                datePicker.Focus();
            };
            layoutRoot.ManipulationStarted += layoutRoot_ManipulationStarted;
            layoutRoot.ManipulationDelta += layoutRoot_ManipulationDelta;
        }

        private Point _initialPoint;
        void layoutRoot_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            _initialPoint = e.ManipulationOrigin;
        }

        void layoutRoot_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            var currentpoint = e.ManipulationOrigin;
            if (currentpoint.X - _initialPoint.X >= 150)
            {
                if (this.Value != null)
                    this.Value = Convert.ToDateTime(this.Value).AddDays(-1);
                e.Complete();
            }
            else if (_initialPoint.X - currentpoint.X >= 150)
            {
                if (this.Value != null)
                    this.Value = Convert.ToDateTime(this.Value).AddDays(+1);
                e.Complete();
            }
        }
        #endregion
    }
}
