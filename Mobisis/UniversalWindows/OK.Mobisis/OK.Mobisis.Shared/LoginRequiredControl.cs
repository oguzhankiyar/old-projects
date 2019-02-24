using System;
using System.Collections.Generic;
using System.Text;
using OK.Mobisis.Api.Helper;
using OK.Mobisis.Pages;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace OK.Mobisis
{
    public class LoginRequiredControl : Grid
    {
        public LoginRequiredControl()
        {
            var panel = new StackPanel();
            panel.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            panel.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;

            var textBlock = new TextBlock();
            textBlock.Text = "Giriş yapmanız gerekiyor..";
            textBlock.Foreground = new SolidColorBrush(Colors.White);
            textBlock.TextAlignment = TextAlignment.Center;
            textBlock.FontSize = 20;
            textBlock.Margin = new Thickness(10);
            panel.Children.Add(textBlock);

            var button = new Button();
            button.Content = "Giriş Yap";
            button.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            button.Click += button_Click;
            panel.Children.Add(button);

            this.Style = App.Current.Resources["PageStyle"] as Style;
            this.Children.Add(panel);
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            var frame = Window.Current.Content as Frame;
            
            await frame.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                frame.Navigate(typeof(LoginPage));
            });
        }
    }
}
