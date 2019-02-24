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

namespace BlockTetris.Dialogs
{
    class OptionsDialog : Grid
    {
        public static OptionsDialog Current { get; private set; }

        private OptionsDialog() { }

        public static void Create()
        {
            Current = new OptionsDialog();
            StackPanel panel = new StackPanel();
            panel.Children.Add(new TextBlock() { Text = LocalizedStrings.Get(LocalizedString.Options), Style = App.Current.Resources["TitleStyle"] as Style, Margin = new Thickness(0,0,0,5) });
            Grid selectColorPanel = new Grid();
            selectColorPanel.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            selectColorPanel.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            selectColorPanel.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
            selectColorPanel.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
            selectColorPanel.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
            selectColorPanel.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
            selectColorPanel.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
            selectColorPanel.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
            Border border = new Border() { BorderThickness = new Thickness(2), BorderBrush = new SolidColorBrush(Colors.White) };
            Grid currentGrid = new Grid();
            int index = 0;
            foreach (var color in Database.Current.ColorList)
            {
                Grid colorGrid = new Grid() { Background = new SolidColorBrush(color), Width = 45, Height = 45, Margin = new Thickness(5) };
                if (color == Database.Current.SelectedBackgroundColor)
                {
                    currentGrid = colorGrid;
                    currentGrid.Children.Add(border);
                }
                colorGrid.Tapped += (c, r) =>
                    {
                        currentGrid.Children.Remove(border);
                        Database.Current.SelectedBackgroundColor = color;
                        currentGrid = colorGrid;
                        colorGrid.Children.Add(border);
                        Functions.UpdateDatabase();
                    };
                Grid.SetRow(colorGrid, index / 6);
                Grid.SetColumn(colorGrid, index % 6);
                selectColorPanel.Children.Add(colorGrid);
                index++;
            }
            panel.Children.Add(new TextBlock() { Text = LocalizedStrings.Get(LocalizedString.SelectColor), Opacity = .7, FontSize = 20, Foreground = new SolidColorBrush(Colors.White), Margin = new Thickness(0, 10, 0, 10) });
            panel.Children.Add(selectColorPanel);
            panel.Children.Add(new TextBlock() { Text = LocalizedStrings.Get(LocalizedString.WriteName), Opacity = .7, FontSize = 20, Foreground = new SolidColorBrush(Colors.White), Margin = new Thickness(0, 10, 0, 10) });
            TextBox tbName = new TextBox() { Text = Database.Current.Player.Name };
            tbName.TextChanged += (c, r) =>
                {
                    Database.Current.Player.Name = tbName.Text;
                    Functions.UpdateDatabase();
                };
            panel.Children.Add(tbName);
            Current.Children.Add(panel);
        }

    }
}
