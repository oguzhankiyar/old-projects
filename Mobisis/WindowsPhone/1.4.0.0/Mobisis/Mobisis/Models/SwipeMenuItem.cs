using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Windows.UI;

namespace Mobisis.Models
{
    class SwipeMenuItem : Grid
    {
        string url;
        public static NavigationService navigationService;
        public SwipeMenuItem(string text, string imageSource, string clickUrl)
        {
            Thickness margin = this.Margin;
            margin.Top = 5;
            margin.Bottom = 5;
            this.Margin = margin;

            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(imageSource, UriKind.Relative));
            image.Width = 32;
            image.Height = 32;
            Thickness imageMargin = image.Margin;
            imageMargin.Left = 15;
            imageMargin.Right = 15;
            image.Margin = imageMargin;
            TextBlock label = new TextBlock();
            Thickness labelMargin = label.Margin;
            labelMargin.Top = 5;
            labelMargin.Bottom = 5;
            label.Margin = labelMargin;
            label.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            label.FontSize = 30;
            label.Text = text;
            panel.Children.Add(image);
            panel.Children.Add(label);
            this.url = clickUrl;
            this.Children.Add(panel);
            this.Tap += LeftMenuItem_Tap;
        }

        private void LeftMenuItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            navigationService.Navigate(new Uri(url, UriKind.Relative));
        }
    }
}
