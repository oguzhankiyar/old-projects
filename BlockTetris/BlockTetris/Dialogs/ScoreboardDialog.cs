using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockTetris.Data;
using BlockTetris.Entities;
using BlockTetris.Strings;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace BlockTetris.Dialogs
{
    class ScoreboardDialog : Grid
    {
        public static ScoreboardDialog Current { get; private set; }
        public static bool IsUpdating { get; set; }

        private ScoreboardDialog() { }

        public static void Create()
        {
            Current = new ScoreboardDialog();

            TextBlock title = new TextBlock() { Text = LocalizedStrings.Get(LocalizedString.Scoreboard), Style = App.Current.Resources["TitleStyle"] as Style };
            StackPanel panel = new StackPanel();
            StackPanel Scores = new StackPanel();

            int i = 1;
            foreach (var item in Database.Current.Scoreboard)
                Scores.Children.Add(GetScoresGrid(item, i++));

            ScrollViewer scroll = new ScrollViewer() { Height = 250 }; 
            scroll.Content = Scores;
            panel.Children.Add(title);
            panel.Children.Add(scroll);

            Current.Children.Add(panel);
        }

        private static Grid GetScoresGrid(Score score, int sort)
        {
            Grid outer = new Grid();
            outer.Margin = new Thickness(0, 0, 0, 10);
            outer.Background = new SolidColorBrush(Color.FromArgb(0x50, 0xFF, 0xFF, 0xFF));
            Grid inner = new Grid();
            inner.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(50) });
            inner.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            inner.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
            inner.Margin = new Thickness(0, 0, 10, 0);
            inner.Height = 50;
            TextBlock sortText = new TextBlock() { Text = sort.ToString(), FontSize = 25, HorizontalAlignment = HorizontalAlignment.Center };
            TextBlock playerName = new TextBlock() { Text = score.PlayerName, FontSize = 25, Margin = new Thickness(10, 0, 0, 0) };
            TextBlock playerScore = new TextBlock() { Text = score.PlayerScore.ToString(), FontSize = 30 };
            Rectangle sortRect = new Rectangle() { Fill = new SolidColorBrush(Colors.OrangeRed), VerticalAlignment = VerticalAlignment.Stretch, HorizontalAlignment = HorizontalAlignment.Stretch };
            Grid.SetColumn(sortRect, 0);
            Grid.SetColumn(sortText, 0);
            Grid.SetColumn(playerName, 1);
            Grid.SetColumn(playerScore, 2);
            inner.Children.Add(sortRect);
            inner.Children.Add(sortText);
            inner.Children.Add(playerName);
            inner.Children.Add(playerScore);
            outer.Children.Add(inner);
            return outer;
        }

        public async static void Update()
        {
            await Functions.UpdateMyScore();
            await Functions.UpdateScores();
            Create();
        }
    }
}
