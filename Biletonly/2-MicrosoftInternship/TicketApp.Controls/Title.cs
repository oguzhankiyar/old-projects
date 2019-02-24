using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TicketApp.Controls
{
    public class Title : Control
    {
        #region Members
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(Title), null);
        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        #endregion
        
        #region Constructor
        public Title()
        {
            DefaultStyleKey = typeof(Title);
        }
        #endregion
    }
}
