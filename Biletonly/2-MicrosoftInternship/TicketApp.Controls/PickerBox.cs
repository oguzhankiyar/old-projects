using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.Phone.Controls;

namespace TicketApp.Controls
{
    public class PickerBox : Control
    {
        #region Members
        public static DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(PickerBox), null);
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(PickerBox), null);
        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set
            {
                SetValue(ValueProperty, value);
                if (_valueText != null)
                    _valueText.SelectedItem = value;
            }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(PickerBox), null);
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
            DependencyProperty.Register("MemberPath", typeof(string), typeof(PickerBox), null);
        public string MemberPath
        {
            get { return (string)GetValue(MemberPathProperty); }
            set { SetValue(MemberPathProperty, value); }
        }

        public event EventHandler ValueChanged;

        private ListPicker _valueText;
        #endregion

        #region Constructor
        public PickerBox()
        {
            DefaultStyleKey = typeof(PickerBox);
        }
        #endregion
        
        #region Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var layoutRoot = GetTemplateChild("LayoutRoot") as Grid;
            _valueText = GetTemplateChild("ValueText") as ListPicker;
            layoutRoot.Tap += (s, e) =>
            {
                _valueText.Focus();
            };
                        
            var sbItem = new StringBuilder();
            sbItem.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">");
            sbItem.Append("<TextBlock Text=\"{Binding " + this.MemberPath + "}\" FontSize=\"25\" />");
            sbItem.Append("</DataTemplate>");

            var itemTemplate = (DataTemplate)XamlReader.Load(sbItem.ToString());
            _valueText.ItemTemplate = itemTemplate;
            
            var sbFullItem = new StringBuilder();
            sbFullItem.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">");
            sbFullItem.Append("<TextBlock Text=\"{Binding " + this.MemberPath + "}\" FontSize=\"25\" />");
            sbFullItem.Append("</DataTemplate>");
            
            var fullItemTemplate = (DataTemplate)XamlReader.Load(sbFullItem.ToString());
            _valueText.FullModeItemTemplate = fullItemTemplate;

            _valueText.ItemsSource = this.ItemsSource;
            _valueText.SelectedItem = this.Value;
            _valueText.SelectionChanged += (s, e) =>
            {
                Value = _valueText.SelectedItem;
                if (ValueChanged != null)
                    ValueChanged(this, e);
            };
        }

        public void Open()
        {
            if (_valueText != null)
                _valueText.Open();
        }
        #endregion
    }
}
