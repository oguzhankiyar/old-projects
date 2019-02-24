using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockTetris.Data;
using BlockTetris.Enums;
using BlockTetris.Panels;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace BlockTetris.Entities
{
    class Block : Grid
    {
        public Grid Current { get; set; }
        private string _text;
        public string Text {
            get
            {
                return _text;
            }
            private set
            {
                if (!string.IsNullOrEmpty(value))
                    Current.Children.Add(new TextBlock() { Text = value, FontSize = 35, Foreground = new SolidColorBrush(Colors.White), Margin = new Thickness(0, 7, 0, 0), TextAlignment = TextAlignment.Center });
                _text = value;
            }
        }
        public BlockType Type { get; set; }
        public bool IsCompleted { get; set; }

        public Storyboard Animation;
        //private static Color _lastBackgroundColor;
        private static int _lastColumn;

        public static double Size { get; set; }
        public static double InnerSize { get { return Size - 2; } }
        public static double OuterSize { get { return Size + 30; } }
        public int Row { get { return Convert.ToInt32(Canvas.GetTop(this) / Block.Size); } }
        public int Column { get { return Convert.ToInt32(Canvas.GetLeft(this) / Block.Size); } }
        
        public Block(BlockType type = BlockType.Classic)
        {
            Current = new Grid();
            if (type == BlockType.Alphabet && Game.Current.Word.Chars.Count() != Game.Current.WordCharIndex)
                this.Text = Game.Current.Word.Chars[Game.Current.WordCharIndex].ToString();
            this.Width = Block.OuterSize;
            this.Height = Block.OuterSize;
            this.Type = type;
            SetLocation();
            SetBackground();
            Current.Margin = new Thickness(1);
            Current.Width = Block.InnerSize;
            Current.Height = Block.InnerSize;
            this.Children.Add(Current);
        }

        private void SetBackground()
        {
            switch (this.Type)
            {
                case BlockType.Black:
                    Current.Background = new SolidColorBrush(Colors.Black);
                    break;
                case BlockType.Selected:
                    Current.Background = new SolidColorBrush(Database.Current.SelectedBackgroundColor);
                    break;
                case BlockType.Joker:
                    ImageBrush brush1 = new ImageBrush();
                    brush1.ImageSource = new BitmapImage(new Uri("ms-appx:///assets/MS_Block.png"));
                    Current.Background = brush1;
                    break;
                case BlockType.Double:
                    ImageBrush brush2 = new ImageBrush();
                    brush2.ImageSource = new BitmapImage(new Uri("ms-appx:///assets/x2_transparent_block.png"));
                    Current.Children.Add(new Grid() { Background = brush2 });
                    Current.Background = new SolidColorBrush(GetRandomColor());
                    break;
                default:
                    Current.Background = new SolidColorBrush(GetRandomColor());
                    break;
            }
        }

        private Color GetRandomColor()
        {
            Color newColor;
            /*do
            {
                newColor = Database.ColorList[new Random().Next(0, Database.ColorList.Count - 1)];
            } while (_lastBackgroundColor == newColor);*/
            newColor = Database.Current.ColorList[new Random().Next(0, Database.Current.ColorList.Count - 1)];
            int random = new Random().Next(0, 5) % 3;
            if (random == 0)
                newColor = Database.Current.SelectedBackgroundColor;
            if (newColor == Database.Current.SelectedBackgroundColor && (this.Type == BlockType.Alphabet || this.Type == BlockType.Classic))
                return Colors.DarkCyan; // return GetRandomColor();
            return newColor;
        }
        
        private void SetLocation()
        {
            Canvas.SetTop(this, -1 * Block.Size);
            int newColumn;
            do
            {
                newColumn = new Random().Next(0, GamePanel.Current.ColumnCount);
            } while (_lastColumn == newColumn);
            _lastColumn = newColumn;
            double left = newColumn * Block.Size;
            Canvas.SetLeft(this, left);
        }

        internal static void block_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Block block = sender as Block;
            block.Tapped -= Block.block_Tapped;
            if (block.Type == BlockType.Selected)
            {
                Animations.RemoveAnimation(block);
                Animations.PointAnimation(block, +50);
            }
            else if (block.Type == BlockType.Classic)
            {
                block.Type = BlockType.Black;
                Animations.BackgroundAnimation(block, Colors.Black);
            }
            else if (block.Type == BlockType.Alphabet)
            {
                Animations.RemoveAnimation(block);
                Animations.PointAnimation(block, +50);
                if (Game.Current.WordCharIndex != Game.Current.Word.Chars.Count() && Game.Current.Word.Chars[Game.Current.WordCharIndex].ToString() == block.Text)
                {
                    Game.Current.AddWordChar(block.Text);
                    if (Game.Current.WordCharIndex == Game.Current.Word.Chars.Count() - 1)
                        Game.Current.Word.IsFinished = true;
                    else
                        Game.Current.WordCharIndex++;
                }
            }
            else if (block.Type == BlockType.Joker)
            {
                Animations.RemoveAnimation(block);
                Animations.PointAnimation(block, +50);
                Game.Current.UseJokerBlock();
            }
            else if (block.Type == BlockType.Double)
            {
                Animations.RemoveAnimation(block);
                Animations.PointAnimation(block, +50);
                Game.Current.UseDoubleBlock();
            }
        }
    }
}
