using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockTetris.Data;
using BlockTetris.Entities;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace BlockTetris.Panels
{
    class WordPanel : StackPanel
    {
        public static WordPanel Current { get; private set; }
        public static Storyboard Animation { get; set; }
        public static DispatcherTimer AnimationTimer { get; set; }
        public static Color CompletedColor { get; set; }

        private WordPanel() { }

        public static void Create()
        {
            if (Game.Current.Word != null)
            {
                Current = new WordPanel();
                Current.Background = new SolidColorBrush(Color.FromArgb(0x50, 0x00, 0x00, 0x00));
                CompletedColor = Color.FromArgb(0x75, 0x00, 0x64, 0x00);
                Current.Height = 40;
                Current.Width = Game.Current.Word.Chars.Count() * 40;
                Current.VerticalAlignment = VerticalAlignment.Top;
                Current.Margin = new Thickness((Database.Current.ScreenWidth - Current.Width) / 2, 0, 0, 0);
                Current.Orientation = Orientation.Horizontal;
                Canvas.SetTop(Current, -1 * Block.Size);
                GamePanel.Current.Children.Add(Current);
            }
        }

        public void AddChar(string s)
        {
            if (Animation != null)
                Animation.Stop();
            if (AnimationTimer != null)
                AnimationTimer.Stop();
            Current.Children.Add(new TextBlock() { FontSize = 30, TextAlignment = TextAlignment.Center, Text = s, Width = 40 });
            Animations.ShowWordAnimation(Current);
        }

    }
}
