using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace OK.Mobisis
{
    public class MessageControl : Grid
    {
        public MessageControl(string message)
        {
            var panel = new StackPanel();
            panel.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            panel.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;

            var textBlock = new TextBlock();
            textBlock.Text = message;
            textBlock.Foreground = new SolidColorBrush(Colors.White);
            textBlock.TextAlignment = TextAlignment.Center;
            textBlock.FontSize = 20;
            textBlock.Margin = new Thickness(10);
            panel.Children.Add(textBlock);
            
            this.Style = App.Current.Resources["PageStyle"] as Style;
            this.Children.Add(panel);
        }
    }
}
