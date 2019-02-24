using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlockTetris.Data;
using BlockTetris.Enums;
using BlockTetris.Panels;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace BlockTetris.Entities
{
    class Animations
    {
        public static void CreateAnimation(Block block)
        {
            var d = new DoubleAnimation() { From = 0, To = 1, Duration = TimeSpan.FromMilliseconds(200), EnableDependentAnimation = true };
            var dw = new DoubleAnimation() { From = 0, To = Block.InnerSize, Duration = TimeSpan.FromMilliseconds(200), EnableDependentAnimation = true };
            var dh = new DoubleAnimation() { From = 0, To = Block.InnerSize, Duration = TimeSpan.FromMilliseconds(200), EnableDependentAnimation = true };
            var s = new Storyboard();
            Storyboard.SetTarget(d, block);
            Storyboard.SetTarget(dw, block.Current);
            Storyboard.SetTarget(dh, block.Current);
            Storyboard.SetTargetProperty(d, "(Grid.Opacity)");
            Storyboard.SetTargetProperty(dw, "(Grid.Width)");
            Storyboard.SetTargetProperty(dh, "(Grid.Height)");
            s.Children.Add(d);
            s.Children.Add(dw);
            s.Children.Add(dh);
            s.Begin();
        }
        
        public static void BackgroundAnimation(Block block, Color color)
        {
            var d = new ColorAnimation() { To = color, Duration = TimeSpan.FromMilliseconds(200), EnableDependentAnimation = true };
            var s = new Storyboard();
            Storyboard.SetTarget(d, block.Current);
            Storyboard.SetTargetProperty(d, "(Grid.Background).(SolidColorBrush.Color)");
            s.Children.Add(d);
            s.Begin();
        }

        public static void DownAnimation(Block block)
        {
            int row = Functions.GetEmptyRow(block.Column); //Game.Current.EmptyRowData[block.Column];
            double to = row * Block.Size;
            var d = new DoubleAnimation() { To = to, Duration = TimeSpan.FromMilliseconds(3 * to), EnableDependentAnimation = true };
            var s = new Storyboard();
            Storyboard.SetTarget(d, block);
            Storyboard.SetTargetProperty(d, "(Canvas.Top)");
            s.Children.Add(d);
            s.Begin();
            block.Animation = s;
            s.Completed += (c, r) => {
                block.IsCompleted = true;
                block.Tapped -= Block.block_Tapped;
                if (block.Type != BlockType.Black && block.Type != BlockType.Selected)
                {
                    RemoveAnimation(block);
                    if (block.Type == BlockType.Joker)
                        Game.Current.IsJoker = false;
                }
                else
                {
                    //Game.Current.EmptyRowData[block.Column] = row - 1;
                    Game.Current.ColumnData[block.Column] -= 1;
                    Game.Current.BlockData[block.Row, block.Column] = block;
                    PointAnimation(block, -25);
                    if (block.Row == 0)
                        Game.Current.GameOver();
                }
            }; 
        }
        
        public static void RemoveAnimation(Block block)
        {
            block.Animation.Pause();
            if (!block.IsCompleted)
                Game.Current.ColumnData[block.Column] -= 1;
            /*else
                Game.Current.EmptyRowData[block.Column] = block.Row;*/
            var d = new DoubleAnimation() { To = 0, Duration = TimeSpan.FromMilliseconds(400), EnableDependentAnimation = true };
            var dw = new DoubleAnimation() { To = 0, Duration = TimeSpan.FromMilliseconds(400), EnableDependentAnimation = true };
            var dh = new DoubleAnimation() { To = 0, Duration = TimeSpan.FromMilliseconds(400), EnableDependentAnimation = true };
            var s = new Storyboard();
            Storyboard.SetTarget(d, block);
            Storyboard.SetTarget(dw, block.Current);
            Storyboard.SetTarget(dh, block.Current);
            Storyboard.SetTargetProperty(d, "(Grid.Opacity)");
            Storyboard.SetTargetProperty(dw, "(Grid.Width)");
            Storyboard.SetTargetProperty(dh, "(Grid.Height)");
            s.Children.Add(d);
            s.Children.Add(dw);
            s.Children.Add(dh);
            s.Begin();
            s.Completed += (c, r) =>
            {
                Game.Current.RemoveBlock(block);
            };
        }

        public static void PointAnimation(Block block, int point)
        {
            Game.Current.Point += point;
        }

        public static void ShowDialogAnimation(Grid grid)
        {
            var dw = new DoubleAnimation() { To = Database.Current.ScreenWidth, Duration = TimeSpan.FromMilliseconds(200), EnableDependentAnimation = true };
            var dh = new DoubleAnimation() { To = Database.Current.ScreenHeight / 2, Duration = TimeSpan.FromMilliseconds(200), EnableDependentAnimation = true };
            var s = new Storyboard();
            (grid.Children[0] as UIElement).Opacity = 0;
            Storyboard.SetTarget(dw, grid);
            Storyboard.SetTarget(dh, grid);
            Storyboard.SetTargetProperty(dw, "(Grid.Width)");
            Storyboard.SetTargetProperty(dh, "(Grid.Height)");
            s.Children.Add(dw);
            s.Children.Add(dh);
            s.Begin();
            s.Completed += (c, r) =>
            {
                (grid.Children[0] as UIElement).Opacity = 1;
            };
        }

        public static void HideDialogAnimation(Grid grid)
        {
            var dw = new DoubleAnimation() { To = 0, Duration = TimeSpan.FromMilliseconds(200), EnableDependentAnimation = true };
            var dh = new DoubleAnimation() { To = 0, Duration = TimeSpan.FromMilliseconds(200), EnableDependentAnimation = true };
            var s = new Storyboard();
            Storyboard.SetTarget(dw, grid);
            Storyboard.SetTarget(dh, grid);
            Storyboard.SetTargetProperty(dw, "(Grid.Width)");
            Storyboard.SetTargetProperty(dh, "(Grid.Height)");
            (grid.Children[0] as UIElement).Opacity = 0;
            s.Children.Add(dw);
            s.Children.Add(dh);
            s.Begin();
            s.Completed += (c, r) =>
            {
                ControlPanel.Show();
            };
        }
        public static void ShowWordAnimation(StackPanel panel)
        {
            var d = new DoubleAnimation() { To = 0, Duration = TimeSpan.FromMilliseconds(300), EnableDependentAnimation = true };
            var s = new Storyboard();
            Storyboard.SetTarget(d, panel);
            Storyboard.SetTargetProperty(d, "(Canvas.Top)");
            s.Children.Add(d);
            s.Begin();
            WordPanel.Animation = s;
            s.Completed += (c, r) =>
            {
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                WordPanel.AnimationTimer = timer;
                if (Game.Current.Word.IsFinished)
                    WordCompletedAnimation(panel);
                int i = 0;
                timer.Tick += (j, k) =>
                    {
                        if (i == 1)
                        {
                            HideWordAnimation(panel);
                            WordPanel.AnimationTimer = null;
                            timer.Stop();
                        }
                        i++;
                    };
                timer.Start();
            };
        }
        public static void HideWordAnimation(StackPanel panel)
        {
            var d = new DoubleAnimation() { To = -1 * Block.Size, Duration = TimeSpan.FromMilliseconds(300), EnableDependentAnimation = true };
            var s = new Storyboard();
            Storyboard.SetTarget(d, panel);
            Storyboard.SetTargetProperty(d, "(Canvas.Top)");
            s.Children.Add(d);
            s.Begin();
            s.Completed += (c, r) =>
                {
                    WordPanel.Animation = null;
                };
        }
        public static void WordCompletedAnimation(StackPanel panel)
        {
            var d = new ColorAnimation() { To = WordPanel.CompletedColor, Duration = TimeSpan.FromMilliseconds(200), EnableDependentAnimation = true };
            var s = new Storyboard();
            Storyboard.SetTarget(d, panel);
            Storyboard.SetTargetProperty(d, "(StackPanel.Background).(SolidColorBrush.Color)");
            s.Children.Add(d);
            s.Begin();
        }
    }
}
