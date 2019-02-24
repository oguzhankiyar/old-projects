using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using BlockTetris.Data;
using BlockTetris.Entities;
using BlockTetris.Strings;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace BlockTetris
{
    public sealed partial class TutorialPage : Page
    {
        public TutorialPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            btnStart.Content = LocalizedStrings.Get(LocalizedString.StartButton);
        }
        
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            Database.Current.TutorialCompleted = true;
            Functions.UpdateDatabase();
            this.Frame.Navigate(typeof(GamePage));
        }
    }
}
