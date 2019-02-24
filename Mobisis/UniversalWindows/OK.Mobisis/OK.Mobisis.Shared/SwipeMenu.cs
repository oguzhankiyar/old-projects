using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace OK.Mobisis
{
    public class SwipeMenu
    {
        ScrollViewer menuScroll;
        StackPanel panel;
        Storyboard moveAnimation;
        DoubleAnimation animation;
        double initialPosition;
        double width = 275;
        Grid layoutRoot;
        StackPanel menuPanel;

        public static int SwipableMaxWidth { get; set; }
        public static List<SwipeMenuItem> Items { get; set; }

        bool isSwipable { get { return (SwipableMaxWidth >= Window.Current.Bounds.Width || SwipableMaxWidth == 0); } }

        static SwipeMenu()
        {
            Items = new List<SwipeMenuItem>();
        }

        public void AddTo(Page page)
        {
            layoutRoot = page.FindName("LayoutRoot") as Grid;
            layoutRoot.Name = "LayoutRoot";
            
            page.Content = null;

            menuScroll = new ScrollViewer();
            menuScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            menuScroll.VerticalScrollMode = ScrollMode.Auto;

            menuPanel = new StackPanel();
            menuScroll.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x21, 0x20, 0x21));
            menuScroll.Width = width;
            menuPanel.Children.Add(new Border() { Background = new SolidColorBrush(Colors.Transparent), Height = 30 });

            if (Items != null)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    var item = Items[i];

                    var textBlock = new TextBlock();
                    textBlock.Text = item.Text;
                    textBlock.FontSize = 25;
                    textBlock.Padding = new Thickness(30, 7.5, 0, 7.5);
                    textBlock.Tapped += (a, r) =>
                    {
                        page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { page.Frame.Navigate(item.AddressType); });
                    };
                    menuPanel.Children.Add(textBlock);
                    if (i != Items.Count - 1)
                        menuPanel.Children.Add(new Border() { Background = new SolidColorBrush(Color.FromArgb(0x20, 0x00, 0x00, 0x00)), Height = 5 });
                }
            }
            menuScroll.Content = menuPanel;
            var layoutCanvas = new Canvas();
            layoutCanvas.Children.Add(layoutRoot);

            panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Children.Add(menuScroll);
            panel.Children.Add(layoutCanvas);
            if (isSwipable)
            {
                panel.ManipulationMode = ManipulationModes.All;
                panel.ManipulationStarted += layoutRoot_ManipulationStarted;
                panel.ManipulationDelta += layoutRoot_ManipulationDelta;
                panel.ManipulationCompleted += layoutRoot_ManipulationCompleted;
            }

            moveAnimation = new Storyboard();
            animation = new DoubleAnimation();
            animation.Duration = TimeSpan.FromMilliseconds(100);
            Storyboard.SetTargetProperty(animation, "(Canvas.Left)");
            Storyboard.SetTarget(animation, panel);
            moveAnimation.Children.Add(animation);

            var canvas = new Canvas();
            canvas.Children.Add(panel);

            if (isSwipable)
                Canvas.SetLeft(panel, -1 * width);
            else
                Canvas.SetLeft(panel, 0);

            page.SizeChanged += page_SizeChanged;

            page.Content = canvas;
        }

        void page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            menuScroll.Height = Window.Current.Bounds.Height;
            layoutRoot.Height = Window.Current.Bounds.Height;

            if (isSwipable)
            {
                layoutRoot.Width = Window.Current.Bounds.Width;
                panel.ManipulationMode = ManipulationModes.All;
                panel.ManipulationStarted += layoutRoot_ManipulationStarted;
                panel.ManipulationDelta += layoutRoot_ManipulationDelta;
                panel.ManipulationCompleted += layoutRoot_ManipulationCompleted;
                Canvas.SetLeft(panel, -1 * width);
            }
            else
            {
                layoutRoot.Width = Window.Current.Bounds.Width - width;
                panel.ManipulationMode = ManipulationModes.None;
                panel.ManipulationStarted -= layoutRoot_ManipulationStarted;
                panel.ManipulationDelta -= layoutRoot_ManipulationDelta;
                panel.ManipulationCompleted -= layoutRoot_ManipulationCompleted;
                Canvas.SetLeft(panel, 0);
            }
        }

        private void MoveViewWindow(double left)
        {
            moveAnimation.Stop();
            moveAnimation.SkipToFill();
            animation.To = left;
            moveAnimation.Begin();
        }

        private void layoutRoot_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            e.Handled = true;
            if (e.Delta.Translation.X != 0)
            {
                double left = Math.Min(Math.Max(-1 * width, Canvas.GetLeft(panel) + e.Delta.Translation.X), 0);
                Canvas.SetLeft(panel, left);
            }
        }

        private void layoutRoot_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            e.Handled = true;
            initialPosition = Canvas.GetLeft(panel);
        }

        private void layoutRoot_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            e.Handled = true;
            double left = Canvas.GetLeft(panel);
            if (Math.Abs(initialPosition - left) < 50)
                MoveViewWindow(initialPosition);
            else if (initialPosition - left > 0 && initialPosition > -1 * width / 2)
                MoveViewWindow(-1 * width);
            else if (initialPosition >= -1 * width)
                MoveViewWindow(0);
        }
    }
}
