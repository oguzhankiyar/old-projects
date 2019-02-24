using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BlockTetris.Data;
using BlockTetris.Entities;
using BlockTetris.Strings;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace BlockTetris.Dialogs
{
    class AboutDialog : Grid
    {
        public static AboutDialog Current { get; private set; }

        private AboutDialog() { }

        public static void Create()
        {
            Current = new AboutDialog();
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            StackPanel panel = new StackPanel();
            panel.Children.Add(new TextBlock() { Text = LocalizedStrings.Get(LocalizedString.About), Style = App.Current.Resources["TitleStyle"] as Style });
            panel.Children.Add(new TextBlock() { Text = LocalizedStrings.Get(LocalizedString.AppName), FontSize = 25 });
            panel.Children.Add(new TextBlock() { Text = LocalizedStrings.Get(LocalizedString.VersionText), FontSize = 20, Opacity = .7, Margin = new Thickness(2, 0, 0, 10) });
            panel.Children.Add(new TextBlock() { Text = LocalizedStrings.Get(LocalizedString.AboutText), LineHeight = 30, TextWrapping = TextWrapping.Wrap, FontSize = 20 });

            Grid buttonPanel = new Grid() { Margin = new Thickness(0, 0, 0, 16) };
            buttonPanel.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            buttonPanel.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            buttonPanel.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            buttonPanel.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });

            Button goTutorial = new Button() { Content = LocalizedStrings.Get(LocalizedString.GoTutorial), HorizontalAlignment = HorizontalAlignment.Stretch, Margin = new Thickness(0, 0, 0, -5) };
            goTutorial.Click += (c, r) =>
            {
                GamePage.CurrentFrame.Navigate(typeof(TutorialPage));
            };

            Button goStore = new Button() { Content = LocalizedStrings.Get(LocalizedString.GoStore), HorizontalAlignment = HorizontalAlignment.Stretch, Margin = new Thickness(0, 0, 0, -5) };
            goStore.Click += (c, r) =>
            {
                GamePage.CurrentFrame.Navigate(typeof(FeedbackPage));
            };
            Grid.SetColumn(goTutorial, 0);
            Grid.SetColumn(goStore, 1);
            Grid.SetRow(goTutorial, 0);
            Grid.SetRow(goStore, 0);

            Button feedbackButton = new Button() { Content = LocalizedStrings.Get(LocalizedString.SendFeedback), HorizontalAlignment = HorizontalAlignment.Stretch };
            feedbackButton.Click += (c, r) =>
            {
                GamePage.CurrentFrame.Navigate(typeof(FeedbackPage));
            };

            Button resetButton = new Button() { Content = LocalizedStrings.Get(LocalizedString.Reset), HorizontalAlignment = HorizontalAlignment.Stretch };
            resetButton.Click += (c, r) =>
            {
                Functions.ResetDatabase();
                Functions.DeleteMyScore();
                GamePage.CurrentFrame.Navigate(typeof(TutorialPage));
            };
            Grid.SetColumn(feedbackButton, 0);
            Grid.SetColumn(resetButton, 1);
            Grid.SetRow(feedbackButton, 1);
            Grid.SetRow(resetButton, 1);
            buttonPanel.Children.Add(goTutorial);
            buttonPanel.Children.Add(goStore);
            buttonPanel.Children.Add(feedbackButton);
            buttonPanel.Children.Add(resetButton);

            Grid.SetRow(panel, 0);
            Grid.SetRow(buttonPanel, 1);
            grid.Children.Add(panel);
            grid.Children.Add(buttonPanel);
            Current.Children.Add(grid);
        }
    }
}
