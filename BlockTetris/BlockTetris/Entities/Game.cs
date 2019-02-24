using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BlockTetris.Data;
using BlockTetris.Dialogs;
using BlockTetris.Enums;
using BlockTetris.Panels;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace BlockTetris.Entities
{
    class Game
    {
        public static Game Current { get; private set; }

        private int _point;
        public int Point
        {
            get { return _point; }
            set
            {
                if (Game.Current.IsDoublePoint && value > _point)
                    _point += (value - _point) * 2;
                else
                    _point = value;
                ControlPanel.Current.Point.Text = _point.ToString();
            }
        }

        public Word Word { get; set; }
        public int WordCharIndex = 0;
        private bool _isDoublePoint;
        public bool IsDoublePoint
        {
            get
            {
                return _isDoublePoint;
            }
            set
            {
                _isDoublePoint = value;
                if (_isDoublePoint == true)
                    ControlPanel.Current.DoubleIcon.Visibility = Visibility.Visible;
                else
                    ControlPanel.Current.DoubleIcon.Visibility = Visibility.Collapsed;
            }
        }
        private DispatcherTimer gameTimer;
        private DispatcherTimer doubleTimer;
        public Block[,] BlockData { get; set; }
        public int[] ColumnData { get; set; }
        //public int[] EmptyRowData { get; set; }
        public bool IsJoker { get; set; }
        public bool IsActive { get; set; }

        private Game() { }

        public static void Init()
        {
            Current = new Game();
        }

        public void Create()
        {
            SelectWord();
            IsActive = false;
            IsDoublePoint = false;
            IsJoker = false;

            BlockData = new Block[GamePanel.Current.RowCount, GamePanel.Current.ColumnCount];
            ColumnData = new int[GamePanel.Current.ColumnCount];
            //EmptyRowData = new int[GamePanel.Current.ColumnCount];
            Array.Clear(ColumnData, 0, ColumnData.Length);
            
            /*
            for (int i = 0; i < EmptyRowData.Count(); i++)
                EmptyRowData[i] = GamePanel.Current.RowCount - 1;
            */

            gameTimer = new DispatcherTimer();
            gameTimer.Tick += createBlock_CallBack;
            gameTimer.Interval = TimeSpan.FromMilliseconds(200);

            WordPanel.Create();
        }

        public void AddWordChar(string s)
        {
            //ControlPanel.Current.Word.Text += s;
            WordPanel.Current.AddChar(s);
        }

        private void createBlock_CallBack(object sender, object e)
        {
            ControlGameLevel();
            Block block;
            //int random = new Random().Next(0, 25);
            if (new Random().Next(0, 4) % 3 == 0)
                block = new Block(BlockType.Selected);
            else if (new Random().Next(0, 15) % 4 == 0 && Word != null && !Word.IsFinished)
                block = new Block(BlockType.Alphabet);
            else if (new Random().Next(0, 20) % 6 == 0 && !IsDoublePoint)
                block = new Block(BlockType.Double);
            else if (new Random().Next(0, 25) % 7 == 0 && !IsJoker)
            {
                block = new Block(BlockType.Joker);
                IsJoker = true;
            }
            else
                block = new Block(BlockType.Classic);
            ColumnData[block.Column] += 1;

            block.Tapped += Block.block_Tapped;
            block.Opacity = 0;
            block.IsCompleted = false;
            GamePanel.Current.Children.Add(block);
            Animations.CreateAnimation(block);

            if (Functions.HasBlockOnFront(block))
            {
                DispatcherTimer tmr = new DispatcherTimer();
                tmr.Tick += (c, r) =>
                {
                    if (block.IsCompleted)
                        tmr.Stop();
                    else
                    {
                        int row = Functions.GetEmptyRow(block.Column); //EmptyRowData[block.Column]
                        double to = row * Block.Size;
                        DoubleAnimation d = block.Animation.Children[0] as DoubleAnimation;
                        d.To = to;
                        d.Duration = TimeSpan.FromMilliseconds(3 * to);
                    }
                };
                tmr.Interval = TimeSpan.FromMilliseconds(100);
                tmr.Start();
            }

            Animations.DownAnimation(block);
        }

        private void ControlGameLevel()
        {
            if (Point < 500)
                gameTimer.Interval = TimeSpan.FromMilliseconds(500);
            else if (Point < 1000)
                gameTimer.Interval = TimeSpan.FromMilliseconds(450);
            else if (Point < 2000)
                gameTimer.Interval = TimeSpan.FromMilliseconds(400);
            else if (Point < 4000)
                gameTimer.Interval = TimeSpan.FromMilliseconds(350);
            else if (Point < 8000)
                gameTimer.Interval = TimeSpan.FromMilliseconds(300);
            else if (Point < 10000)
                gameTimer.Interval = TimeSpan.FromMilliseconds(350);
            else if (Point < 15000)
                gameTimer.Interval = TimeSpan.FromMilliseconds(200);
            else if (Point < 20000)
                gameTimer.Interval = TimeSpan.FromMilliseconds(175);
            else
                gameTimer.Interval = TimeSpan.FromMilliseconds(150);
        }

        public void Start()
        {
            StopTimer();
            RemoveAllBlocks();
            StartDialog.Create();
            GamePanel.Current.ShowDialog(StartDialog.Current);
        }

        public void Pause()
        {
            StopTimer();
            PauseAllBlocks();
            PauseDialog.Create();
            GamePanel.Current.ShowDialog(PauseDialog.Current);
        }

        public void GameOver()
        {
            StopTimer();
            RemoveAllBlocks();
            CalculateWordPoint();
            if (Point > Database.Current.Player.Score.PlayerScore)
            {
                Database.Current.Player.Score.PlayerScore = Point;
                ScoreboardDialog.Update();
                NewScoreDialog.Create();
                GamePanel.Current.ShowDialog(NewScoreDialog.Current);
            }
            else
            {
                GameOverDialog.Create();
                GamePanel.Current.ShowDialog(GameOverDialog.Current);
            }
        }


        private async void RemoveAllBlocks()
        {
            foreach (var block in GamePanel.Current.Children.Where(x => x.GetType() == typeof(Block)).OrderBy(x => Guid.NewGuid()))
            {
                Animations.RemoveAnimation((Block)block);
                await Task.Delay(TimeSpan.FromMilliseconds(15));
            }
        }

        public void UseDoubleBlock()
        {
            if (doubleTimer == null)
                doubleTimer = new DispatcherTimer();
            if (doubleTimer.IsEnabled)
                doubleTimer.Stop();
            IsDoublePoint = true;
            doubleTimer.Interval = TimeSpan.FromSeconds(10);
            doubleTimer.Tick += (c, r) =>
                {
                    IsDoublePoint = false;
                    doubleTimer.Stop();
                };
            doubleTimer.Start();
        }

        public void UseJokerBlock()
        {
            int num = new Random().Next(2, 5);
            for (int x = 0; x < num; x++)
            {
                int selectedRow = GamePanel.Current.RowCount - 1;
                int selectedColumn = 0;
                int i = 0;
                for (int j=0; j < GamePanel.Current.ColumnCount; j++)
                {
                    i = Functions.GetEmptyRow(j); //EmptyRowData[j]
                    if (i < selectedRow)
                    {
                        selectedRow = i + 1;
                        selectedColumn = j;
                    }
                }

                Block block = Game.Current.BlockData[selectedRow, selectedColumn];
                if (block != null)
                {
                    //EmptyRowData[selectedColumn] = selectedRow;
                    BlockData[selectedRow, selectedColumn] = null;
                    Animations.RemoveAnimation(block);
                }
            }
            IsJoker = false;
        }
        private void SelectWord()
        {
            if (Database.Current.Words.Any())
            {
                var words = Database.Current.Words.Where(x => x.IsFinished == false);
                if (words.Count() == 0)
                {
                    Functions.ResetWordStates();
                    SelectWord();
                }
                else
                {
                    Word = words.OrderBy(x => Guid.NewGuid()).First();
                }
            }
        }

        public void RemoveBlock(Block block)
        {
            block.IsCompleted = true;
            BlockData[block.Row, block.Column] = null;
            GamePanel.Current.Children.Remove(block);
        }

        private void CalculateWordPoint()
        {
            int extra = 0;
            if (Word != null && Word.IsFinished)
                extra += Word.Chars.Count() * 200;
            Point += extra;
        }

        public void StartTimer()
        {
            gameTimer.Start();
            if (doubleTimer != null)
                doubleTimer.Start();
            IsActive = true;
        }

        public void StopTimer()
        {
            gameTimer.Stop();
            if (doubleTimer != null)
                doubleTimer.Stop();
            IsActive = false;
        }

        public void PauseAllBlocks()
        {
            foreach (var block in GamePanel.Current.Children.Where(x => x.GetType() == typeof(Block)).OrderBy(x => Guid.NewGuid()))
                (block as Block).Animation.Pause();
        }
        public void ResumeAllBlocks()
        {
            foreach (var block in GamePanel.Current.Children.Where(x => x.GetType() == typeof(Block)).OrderBy(x => Guid.NewGuid()))
                (block as Block).Animation.Resume();
        }
    }
}
