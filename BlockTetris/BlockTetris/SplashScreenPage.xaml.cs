using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using BlockTetris.Data;
using BlockTetris.Entities;
using BlockTetris.Enums;
using BlockTetris.Panels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace BlockTetris
{
    public sealed partial class SplashScreenPage : Page
    {
        public SplashScreenPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            List<int> showed = new List<int>();
            GamePanel.Create(LayoutRoot);
            await Task.Delay(TimeSpan.FromMilliseconds(500));
            for (int i = 0; i < 9; i++)
            {
                int index;
                do
                {
                    index = new Random().Next(0, 9);
                }
                while (showed.Contains(index));
                var block = new Block(BlockType.Classic) { Width = 50, Height = 50, Opacity = 0 };
                block.Current.Background = new SolidColorBrush(Database.Current.ColorList[i % Database.Current.ColorList.Count()]);
                Logo.Children.Add(block);
                Grid.SetColumn(block, Convert.ToInt32(index % 3));
                Grid.SetRow(block, Convert.ToInt32(index / 3));
                Animations.CreateAnimation(block);
                await Task.Delay(TimeSpan.FromMilliseconds(40));
                showed.Add(index);
            }

            var s = new Storyboard();
            var d = new DoubleAnimation() { To = 1, Duration = TimeSpan.FromMilliseconds(500) };
            Storyboard.SetTarget(d, AppName);
            Storyboard.SetTargetProperty(d, "(Grid.Opacity)");
            s.Children.Add(d);
            s.Begin();

            await Task.Delay(TimeSpan.FromMilliseconds(1000));

            if (Database.Current.TutorialCompleted)
            {
                if (!this.Frame.Navigate(typeof(GamePage)))
                    throw new Exception("Failed to create initial page");
            }
            else
            {
                if (!this.Frame.Navigate(typeof(TutorialPage)))
                    throw new Exception("Failed to create initial page");
            }
        }
    }
}
