using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace OK.Mobisis
{
    public class SwipeMenuItem
    {
        public string Text { get; set; }
        public Type AddressType { get; set; }
        public Uri IconSource { get; set; }

        public SwipeMenuItem(string text, Type addressType, Uri iconSource = null)
        {
            this.Text = text;
            this.AddressType = addressType;
            this.IconSource = iconSource;
        }
    }
}
