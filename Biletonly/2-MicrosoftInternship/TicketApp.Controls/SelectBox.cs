using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TicketApp.Controls
{
    public class SelectBox : Control
    {
        #region Members
        public static DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(SelectBox), null);
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(SelectBox), null);
        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set
            {
                SetValue(ValueProperty, value);
                SetText();
            }
        }

        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register("Items", typeof(List<string>), typeof(SelectBox), null);
        public List<string> Items
        {
            get { return (List<string>)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        public static readonly DependencyProperty ValuesProperty =
            DependencyProperty.Register("Values", typeof(List<object>), typeof(SelectBox), null);
        public List<object> Values
        {
            get { return (List<object>)GetValue(ValuesProperty); }
            set
            { SetValue(ValuesProperty, value); }
        }

        private TextBlock _valueText;
        private static int _index = 0;
        #endregion

        #region Constructor
        public SelectBox()
        {
            DefaultStyleKey = typeof(SelectBox);
        }
        #endregion

        #region Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var layoutRoot = GetTemplateChild("LayoutRoot") as Grid;
            _valueText = GetTemplateChild("ValueText") as TextBlock;
            layoutRoot.Tap += layoutRoot_Tap;
            SetText();
        }

        private void layoutRoot_Tap(object sender, GestureEventArgs e)
        {
            if (this.Items != null && this.Items.Any())
            {
                if (_index == Items.Count() - 1 || this.Value == null)
                    _index = 0;
                else
                    _index += 1;
                this.Value = Values[_index];
            }
        }

        private void SetText()
        {
            if (this.Value != null)
            {
                _index = Values.IndexOf(this.Value);
                _valueText.Text = Items[_index].ToString();
            }
        }
        #endregion
    }
}
