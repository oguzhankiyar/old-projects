using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockTetris.Entities;
using BlockTetris.Panels;
using BlockTetris.Strings;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace BlockTetris.Dialogs
{
    class PauseDialog : Grid
    {
        public static PauseDialog Current { get; private set; }

        private PauseDialog() { }

        public static void Create()
        {
            Current = new PauseDialog();
            StackPanel sp = new StackPanel() { VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Stretch };
            sp.Background = new SolidColorBrush(Colors.Orange);
            sp.Children.Add(new TextBlock() { Text = LocalizedStrings.Get(LocalizedString.Paused), Foreground = new SolidColorBrush(Colors.White), FontSize = 45, TextAlignment = TextAlignment.Center });
            sp.Children.Add(new TextBlock() { Text = Game.Current.Point + " " + LocalizedStrings.Get(LocalizedString.Point), Foreground = new SolidColorBrush(Colors.White), FontSize = 30, TextAlignment = TextAlignment.Center });
            Image imgPlayAgain = new Image() { Source = new BitmapImage(new Uri("ms-appx:///assets/play.png")), Width = 76, Height = 76 };
            TextBlock playText = new TextBlock() { Foreground = new SolidColorBrush(Colors.White), FontSize = 20, TextAlignment = TextAlignment.Center };
            imgPlayAgain.Tapped += (c, r) =>
            {
                //Animations.ButtonAnimation(imgPlayAgain);
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                int to = 0;
                playText.Text = string.Format(LocalizedStrings.Get(LocalizedString.PlayTimerText), 3 - to);
                timer.Tick += (t, k) =>
                {
                    imgPlayAgain.IsTapEnabled = false;
                    if (to == 3)
                    {
                        timer.Stop();
                        GamePanel.Current.HideDialog();
                        Game.Current.ResumeAllBlocks();
                        Game.Current.StartTimer();
                    }
                    else
                    {
                        to++;
                        playText.Text = string.Format(LocalizedStrings.Get(LocalizedString.PlayTimerText), 3 - to);
                    }
                };
                timer.Start();
            };
            sp.Children.Add(imgPlayAgain);
            sp.Children.Add(playText);
            Current.Children.Add(sp);
        }

    }
}
