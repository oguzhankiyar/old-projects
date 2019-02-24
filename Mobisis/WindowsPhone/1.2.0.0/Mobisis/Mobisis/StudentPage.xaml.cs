using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media;
using Mobisis.Models;

namespace Mobisis
{
    public partial class StudentPage : PhoneApplicationPage
    {
        public StudentPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var backStack = NavigationService.BackStack.FirstOrDefault();
            if (backStack != null)
                if (backStack.Source.OriginalString == "/SplashScreenPage.xaml")
                    NavigationService.RemoveBackEntry();

            PageCanvas = new SwipeMenu(this, PageCanvas, LayoutRoot, MoveAnimation, LeftMenu);
            Container.Height = App.Current.RootVisual.RenderSize.Height;

            if (Database.Student == null)
            {
                MessageBox.Show("Giriş yapmanız gerekiyor..");
                NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
            }
            else
            {
                Container.DataContext = Database.Student;
            }
            base.OnNavigatedTo(e);
        }
    }
}