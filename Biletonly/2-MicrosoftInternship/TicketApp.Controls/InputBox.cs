using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using TicketApp.Controls.Enums;

namespace TicketApp.Controls
{
    public class InputBox : Control
    {
        #region Members
        public static DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(InputBox), null);
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(InputBox), null);
        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static DependencyProperty ScopeProperty =
            DependencyProperty.Register("Scope", typeof(InputScope), typeof(InputBox), null);
        public InputScope Scope
        {
            get { return (InputScope)GetValue(ScopeProperty); }
            set { SetValue(ScopeProperty, value); }
        }

        public static DependencyProperty MultiRowProperty =
            DependencyProperty.Register("MultiRow", typeof(bool), typeof(InputBox), null);
        public bool MultiRow
        {
            get { return (bool)GetValue(MultiRowProperty); }
            set { SetValue(MultiRowProperty, value); }
        }

        public static DependencyProperty LengthProperty =
            DependencyProperty.Register("Length", typeof(int), typeof(InputBox), null);
        public int Length
        {
            get { return (int)GetValue(LengthProperty); }
            set { SetValue(LengthProperty, value); }
        }

        public static DependencyProperty CasingModeProperty =
            DependencyProperty.Register("CasingMode", typeof(CasingMode), typeof(InputBox), null);
        public CasingMode CasingMode
        {
            get { return (CasingMode)GetValue(CasingModeProperty); }
            set { SetValue(CasingModeProperty, value); }
        }

        private TextBox _valueText;
        private Image _clearButton;
        #endregion

        #region Constructor
        public InputBox()
        {
            DefaultStyleKey = typeof(InputBox);
        }
        #endregion

        #region Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var layoutRoot = GetTemplateChild("LayoutRoot") as Grid;
            layoutRoot.Tap += layoutRoot_Tap;

            _valueText = GetTemplateChild("ValueText") as TextBox;
            _valueText.GotFocus += valueText_GotFocus;
            _valueText.LostFocus += valueText_LostFocus;
            _valueText.KeyUp += valueText_KeyUp;
            _valueText.TextWrapping = this.MultiRow ? TextWrapping.Wrap : TextWrapping.NoWrap;

            _clearButton = GetTemplateChild("ClearButton") as Image;
            _clearButton.Tap += _clearButton_Tap;
        }
        
        void layoutRoot_Tap(object sender, GestureEventArgs e)
        {
            _valueText.Focus();
        }

        void valueText_GotFocus(object sender, RoutedEventArgs e)
        {
            _valueText.Foreground = new SolidColorBrush(Colors.LightGray);
            if (!string.IsNullOrEmpty(_valueText.Text))
                _clearButton.Visibility = Visibility.Visible;
            else
                _clearButton.Visibility = Visibility.Collapsed;
        }

        void valueText_LostFocus(object sender, RoutedEventArgs e)
        {
            _valueText.Foreground = new SolidColorBrush(Colors.White);
            _clearButton.Visibility = Visibility.Collapsed;
        }

        void valueText_KeyUp(object sender, KeyEventArgs e)
        {
            var currentText = (TextBox)sender;
            int currentPosition = currentText.SelectionStart;

            switch (CasingMode)
            {
                case CasingMode.Upper:
                    currentText.Text = currentText.Text.ToUpper();
                    break;
                case CasingMode.Lower:
                    currentText.Text = currentText.Text.ToLower();
                    break;
                case CasingMode.FirstUpper:
                    if (currentText.Text.Length > 0)
                        currentText.Text = currentText.Text[0].ToString().ToUpper() + currentText.Text.Substring(1).ToLower().ToString();
                    break;
                case CasingMode.UpperAfterSpace:
                    string newText = "";
                    for (int i = 0; i < currentText.Text.Length; i++)
                    {
                        char curChar = currentText.Text[i];
                        if (i == 0 || currentText.Text[i - 1] == ' ')
                            newText += curChar.ToString().ToUpper();
                        else
                            newText += curChar.ToString().ToLower();
                    }
                    currentText.Text = newText;
                    break;
                default:
                    break;
            }
            currentText.SelectionStart = currentPosition++;

            if (!string.IsNullOrEmpty(currentText.Text))
                _clearButton.Visibility = Visibility.Visible;
            else
                _clearButton.Visibility = Visibility.Collapsed;
        }

        void _clearButton_Tap(object sender, GestureEventArgs e)
        {
            _valueText.Text = "";
            _clearButton.Visibility = Visibility.Collapsed;
        }
        #endregion
    }
}
