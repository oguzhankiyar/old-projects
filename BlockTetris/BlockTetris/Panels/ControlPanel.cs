using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Text;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI;
using BlockTetris.Entities;
using BlockTetris.Data;

namespace BlockTetris.Panels
{
    class ControlPanel : Grid
    {
        public static ControlPanel Current { get; private set; }
        public TextBlock Point { get; set; }
        public Image DoubleIcon { get; set; }
        private Image PauseImage { get; set; }

        private ControlPanel() { }

        public static void Create()
        {
            Current = new ControlPanel();
            Current.MinWidth = 50;
            Current.Margin = new Thickness(0,50,5,0);
            Current.Height = Database.Current.ScreenHeight;
            Current.HorizontalAlignment = HorizontalAlignment.Right;
            Current.VerticalAlignment = VerticalAlignment.Top;
            StackPanel stackPanel = new StackPanel();
            Current.Point = new TextBlock() { Style = App.Current.Resources["PointTextStyle"] as Style };
            Current.DoubleIcon = new Image() { Source = new BitmapImage(new Uri("ms-appx:///assets/x2_transparent_block_2.png")), Width = 40, Height = 40, Visibility = Visibility.Collapsed, HorizontalAlignment = HorizontalAlignment.Right };
            Current.PauseImage = new Image() { Source = new BitmapImage(new Uri("ms-appx:///assets/pause.png")), Width = 50, Height = 50, HorizontalAlignment = HorizontalAlignment.Right };
            Current.PauseImage.Tapped += (c, r) =>
                {
                    //Animations.ButtonAnimation(Current.PauseImage);
                    Game.Current.Pause();
                };
            stackPanel.Children.Add(Current.PauseImage);
            stackPanel.Children.Add(Current.Point);
            stackPanel.Children.Add(Current.DoubleIcon);
            Current.Children.Add(stackPanel);
        }

        public static void Show()
        {
            if (!GamePanel.Current.LayoutRoot.Children.Contains(Current))
                GamePanel.Current.LayoutRoot.Children.Add(Current);
        }

        public static void Hide()
        {
            if (GamePanel.Current.LayoutRoot.Children.Contains(Current))
                GamePanel.Current.LayoutRoot.Children.Remove(Current);
        }
    }
}
