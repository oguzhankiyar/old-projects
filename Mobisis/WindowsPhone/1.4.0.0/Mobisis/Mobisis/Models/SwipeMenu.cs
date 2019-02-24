using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace Mobisis.Models
{
    public class SwipeMenu : Canvas
    {
        
        Canvas pageCanvas;
        Canvas layoutRoot;
        Storyboard moveAnimation;
        Grid leftMenu;
        IApplicationBar applicationBar;
        double initialPosition;
        bool viewMoved = false;
        PhoneApplicationPage currentPage;

        public SwipeMenu(PhoneApplicationPage page, Canvas pageCanvas, Canvas layoutRoot, Storyboard moveAnimation, Grid leftMenu)
        {
            this.pageCanvas = pageCanvas;
            this.layoutRoot = layoutRoot;
            this.moveAnimation = moveAnimation;
            this.leftMenu = leftMenu;
            this.applicationBar = page.ApplicationBar;
            this.currentPage = page;
            NavigationService navigationService = page.NavigationService;

            pageCanvas.ManipulationCompleted += pageCanvas_ManipulationCompleted;
            pageCanvas.ManipulationDelta += pageCanvas_ManipulationDelta;
            pageCanvas.ManipulationStarted += pageCanvas_ManipulationStarted;

            pageCanvas.Height = leftMenu.Height = App.Current.RootVisual.RenderSize.Height;

            StackPanel panel = new StackPanel();
            Thickness margin = panel.Margin;
            margin.Top = 50;
            panel.Margin = margin;
            SwipeMenuItem.navigationService = navigationService;
            panel.Children.Add(new SwipeMenuItem("Giriş", "/Assets/MenuItem.png", "/LoginPage.xaml?" + DateTime.Now));
            panel.Children.Add(new SwipeMenuItem("Bilgiler", "/Assets/MenuItem.png", "/StudentPage.xaml?" + DateTime.Now));
            panel.Children.Add(new SwipeMenuItem("Dönemler", "/Assets/MenuItem.png", "/PeriodsPage.xaml?" + DateTime.Now));
            panel.Children.Add(new SwipeMenuItem("Dersler", "/Assets/MenuItem.png", "/PeriodDetailsPage.xaml?" + DateTime.Now));
            panel.Children.Add(new SwipeMenuItem("Ders programı", "/Assets/MenuItem.png", "/LessonPlanPage.xaml?" + DateTime.Now));
            panel.Children.Add(new SwipeMenuItem("Gano hesapla", "/Assets/MenuItem.png", "/CalculateGanoPage.xaml?" + DateTime.Now));
            panel.Children.Add(new SwipeMenuItem("Yemek listesi", "/Assets/MenuItem.png", "/FoodListsPage.xaml?" + DateTime.Now));
            panel.Children.Add(new SwipeMenuItem("Ayarlar", "/Assets/MenuItem.png", "/SettingsPage.xaml?" + DateTime.Now));
            panel.Children.Add(new SwipeMenuItem("Hakkında", "/Assets/MenuItem.png", "/AboutPage.xaml?" + DateTime.Now));
            leftMenu.Children.Add(panel);

            MoveViewWindow(-350);
        }
        void MoveViewWindow(double left)
        {
            viewMoved = true;
            if (applicationBar != null)
            {
                if (left == -350)
                    applicationBar.IsVisible = true;
                else
                    applicationBar.IsVisible = false;
            }
            currentPage.Focus();
            moveAnimation.SkipToFill();
            ((DoubleAnimation)moveAnimation.Children[0]).To = left;
            moveAnimation.Begin();
        }
        private void pageCanvas_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (e.DeltaManipulation.Translation.X != 0)
                Canvas.SetLeft(layoutRoot, Math.Min(Math.Max(-350, Canvas.GetLeft(layoutRoot) + e.DeltaManipulation.Translation.X), 0));
        }
        private void pageCanvas_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            viewMoved = false;
            initialPosition = Canvas.GetLeft(layoutRoot);
        }
        private void pageCanvas_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            double left = Canvas.GetLeft(layoutRoot);
            if (!viewMoved)
            {
                if (Math.Abs(initialPosition - left) < 100)
                    MoveViewWindow(initialPosition);
                else if (initialPosition - left > 0 && initialPosition > -350)
                    MoveViewWindow(-350);
                else if (initialPosition >= -350)
                    MoveViewWindow(0);
            }
        }
    }
}
