using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace BlockTetris.Panels
{
    class AdsPanel : Grid
    {
        public static AdsPanel Current { get; private set; }

        private AdsPanel() { }

        public static void Show()
        {
            if (Current == null)
                Create();
            GamePanel.Current.LayoutRoot.Children.Add(Current);
        }

        private static void Create()
        {
            Current = new AdsPanel()
            {
                Background = new SolidColorBrush(Colors.Black),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Bottom,
                Height = 50
            };
            Current.Children.Add(new TextBlock() { Text = "Advertisement", FontSize = 30, TextAlignment = TextAlignment.Center, VerticalAlignment = VerticalAlignment.Center });
        }

        public static void Hide()
        {
            GamePanel.Current.LayoutRoot.Children.Remove(Current);
        }
    }
}
