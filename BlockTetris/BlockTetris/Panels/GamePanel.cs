using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlockTetris.Data;
using BlockTetris.Dialogs;
using BlockTetris.Entities;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace BlockTetris.Panels
{
    class GamePanel : Canvas
    {
        public static GamePanel Current { get; private set; }
        public int RowCount { get; private set; }
        public int ColumnCount { get; private set; }
        public Grid LayoutRoot { get; private set; }
        public static Grid Dialog { get; private set; }
        public static Pivot DialogTabs { get; private set; }

        private GamePanel() { }

        public static void Create(Grid layoutRoot)
        {
            Current = new GamePanel();
            Current.LayoutRoot = layoutRoot;
            layoutRoot.Children.Add(Current);

            Block.Size = Convert.ToInt32((Database.Current.ScreenWidth - 7 * 4) / 6);
            Current.RowCount = Convert.ToInt32(Database.Current.ScreenHeight / Block.Size) - 1;
            Current.ColumnCount = 6;
            
            ScoreboardDialog.Create();
            OptionsDialog.Create();
            AboutDialog.Create();
        }

        public static void Refresh()
        {
            Current.LayoutRoot.Children.Remove(Current);
            GamePanel.Create(Current.LayoutRoot);
        }

        public void CreateDialog()
        {
            Dialog = new Grid();
            Dialog.Width = 0;
            Dialog.Height = 0;
            Dialog.VerticalAlignment = VerticalAlignment.Center;
            Dialog.HorizontalAlignment = HorizontalAlignment.Stretch;
            Dialog.Background = new SolidColorBrush(Colors.Orange);

            DialogTabs = new Pivot() { Margin = new Thickness(0, -25, 0, -25) };
            PivotItem scorePivot = new PivotItem() { Content = ScoreboardDialog.Current, Margin = new Thickness(12, 0, 12, 12) };
            DialogTabs.Items.Add(new PivotItem() { Content = null, Margin = new Thickness(12, 0, 12, 12) });
            DialogTabs.Items.Add(scorePivot);
            DialogTabs.Items.Add(new PivotItem() { Content = OptionsDialog.Current, Margin = new Thickness(12, 0, 12, 12) });
            DialogTabs.Items.Add(new PivotItem() { Content = AboutDialog.Current, Margin = new Thickness(12, 0, 12, 12) });
            DialogTabs.SelectionChanged += (c, r) =>
            {
                if (DialogTabs.SelectedIndex == 1)
                {
                    ScoreboardDialog.Update();
                    scorePivot.Content = ScoreboardDialog.Current;
                }
            };
            Dialog.Children.Add(DialogTabs);
            Canvas.SetZIndex(Dialog, 99999);
            LayoutRoot.Children.Add(Dialog);
        }

        public void ShowDialog(Grid innerGrid)
        {
            ControlPanel.Hide();
            innerGrid.VerticalAlignment = VerticalAlignment.Center;
            innerGrid.HorizontalAlignment = HorizontalAlignment.Center;
           
            if (Dialog == null)
                CreateDialog();
            DialogTabs.Items.RemoveAt(0);
            DialogTabs.Items.Insert(0, new PivotItem() { Content = innerGrid, Margin = new Thickness(12, 0, 12, 12) });
            DialogTabs.SelectedIndex = 0;
            Animations.ShowDialogAnimation(Dialog);

            AdsPanel.Show();
        }

        public void HideDialog()
        {
            AdsPanel.Hide();
            Animations.HideDialogAnimation(Dialog);
        }
    }
}
