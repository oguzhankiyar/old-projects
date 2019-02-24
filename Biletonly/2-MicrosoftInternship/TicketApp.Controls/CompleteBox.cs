using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using Microsoft.Phone.Controls;

namespace TicketApp.Controls
{
    public class CompleteBox : Control
    {
        #region Members
        public static DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(CompleteBox), null);
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(CompleteBox), null);
        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set
            {
                SetValue(ValueProperty, value);
                if (_valueText != null)
                    _valueText.SelectedItem = this.Value;
            }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(CompleteBox), null);
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set
            {
                SetValue(ItemsSourceProperty, value);
                if (_valueText != null)
                    _valueText.ItemsSource = this.ItemsSource;
            }
        }

        public static DependencyProperty MemberPathProperty =
            DependencyProperty.Register("MemberPath", typeof(string), typeof(CompleteBox), null);
        public string MemberPath
        {
            get { return (string)GetValue(MemberPathProperty); }
            set { SetValue(MemberPathProperty, value); }
        }

        public EventHandler ValueChanged;

        private AutoCompleteBox _valueText;
        private Image _clearButton;
        #endregion

        #region Constructor
        public CompleteBox()
        {
            DefaultStyleKey = typeof(CompleteBox);
        }
        #endregion

        #region Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var layoutRoot = GetTemplateChild("LayoutRoot") as Grid;
            _valueText = GetTemplateChild("ValueText") as AutoCompleteBox;
            layoutRoot.Tap += layoutRoot_Tap;

            _valueText.ValueMemberPath = this.MemberPath;

            var builder = new StringBuilder();
            builder.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">");
            builder.Append("<TextBlock Padding=\"15\" Text=\"{Binding Path=" + this.MemberPath + "}\" />");
            builder.Append("</DataTemplate>");
            
            var template = (DataTemplate)XamlReader.Load(builder.ToString());
            _valueText.ItemTemplate = template;
            _valueText.ItemsSource = this.ItemsSource;
            _valueText.SelectedItem = this.Value;
            _valueText.SelectionChanged += _valueText_SelectionChanged;
            _valueText.GotFocus += _valueText_GotFocus;
            _valueText.LostFocus += _valueText_LostFocus;
            _valueText.KeyUp += _valueText_KeyUp;

            _clearButton = GetTemplateChild("ClearButton") as Image;
            _clearButton.Tap += _clearButton_Tap;
        }

        void _valueText_GotFocus(object sender, RoutedEventArgs e)
        {
            _valueText.Foreground = new SolidColorBrush(Colors.LightGray);
            if (!string.IsNullOrEmpty(_valueText.Text))
                _clearButton.Visibility = Visibility.Visible;
            else
                _clearButton.Visibility = Visibility.Collapsed;
        }

        void _valueText_LostFocus(object sender, RoutedEventArgs e)
        {
            _valueText.Foreground = new SolidColorBrush(Colors.White);
            _clearButton.Visibility = Visibility.Collapsed;
        }

        void _valueText_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(_valueText.Text))
                _clearButton.Visibility = Visibility.Visible;
            else
                _clearButton.Visibility = Visibility.Collapsed;
        }
        
        void _clearButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            _valueText.Text = "";
            _clearButton.Visibility = Visibility.Collapsed;
        }

        void layoutRoot_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            _valueText.Focus();
        }

        void _valueText_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Value = _valueText.SelectedItem;
            if (ValueChanged != null)
                ValueChanged(this, e);
        }
        #endregion
    }
}