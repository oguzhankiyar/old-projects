using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using BlockTetris.Data;
using BlockTetris.Dialogs;
using BlockTetris.Panels;
using BlockTetris.Strings;
using Microsoft.WindowsAzure.MobileServices;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.System.Profile;
using Windows.UI;
using Windows.UI.Xaml;

namespace BlockTetris.Entities
{
    class Functions
    {
        public static bool HasBlockOnFront(Block block)
        {
            return (Game.Current.ColumnData[block.Column] > 1 || Game.Current.IsJoker);
        }
        public static int GetEmptyRow(int column)
        {
            for (int i = GamePanel.Current.RowCount - 1; i > 0; i--)
            {
                if (Game.Current.BlockData[i, column] == null)
                {
                    if (Game.Current.BlockData[i - 1, column] == null)
                        return i;
                    else
                        Animations.RemoveAnimation(Game.Current.BlockData[i - 1, column]);
                }
            }
            return 0;
        }

        public static string GetDeviceId()
        {
            HardwareToken token = HardwareIdentification.GetPackageSpecificToken(null);
            IBuffer hardwareId = token.Id;

            HashAlgorithmProvider hasher = HashAlgorithmProvider.OpenAlgorithm("MD5");
            IBuffer hashed = hasher.HashData(hardwareId);

            return (CryptographicBuffer.EncodeToHexString(hashed));
        }
        public static List<Color> GetColors()
        {
            List<Color> colorList = new List<Color>();
            colorList.Add(Color.FromArgb(0xFF, 0x00, 0xA4, 0xEF));
            colorList.Add(Color.FromArgb(0xFF, 0xF2, 0x50, 0x22));
            colorList.Add(Color.FromArgb(0xFF, 0x7F, 0xBA, 0x00));
            colorList.Add(Color.FromArgb(0xFF, 0xFF, 0xB9, 0x00));
            colorList.Add(Color.FromArgb(0xFF, 0xFF, 0x69, 0xB4));
            colorList.Add(Colors.DarkCyan);
            colorList.Add(Colors.Purple);
            colorList.Add(Color.FromArgb(0xFF, 0xCB, 0x29, 0x6F));
            colorList.Add(Color.FromArgb(0xFF, 0x04, 0x7A, 0x18));
            return colorList;
        }
        public static Player GetNewPlayer()
        {
            Player newPlayer = new Player();
            newPlayer.Score = new Score()
            {
                Region = RegionInfo.CurrentRegion.EnglishName,
                DeviceId = Functions.GetDeviceId(),
                PlayerName = "Anonymous",
                PlayerScore = 0
            };
            newPlayer.Name = "Anonymous";
            return newPlayer;
        }
        public static List<Word> GetWords()
        {
            List<Word> wordList = new List<Word>();
            List<string> words = LocalizedStrings.Get(LocalizedString.Words).Split(',').ToList();
            foreach (var word in words)
                wordList.Add(CreateWord(word.Trim()));
            return wordList;
        }
        public static Word CreateWord(string text)
        {
            return new Word() { Text = text, Chars = text.ToList(), IsFinished = false };
        }
        public async static void ResetDatabase()
        {
            await DataSaver<Database>.RemoveData("Database");
            Database.Current = new Database()
            {
                Region = RegionInfo.CurrentRegion.EnglishName,
                DeviceId = Functions.GetDeviceId(),
                ScreenWidth = Window.Current.Bounds.Width,
                ScreenHeight = Window.Current.Bounds.Height,
                ColorList = Functions.GetColors(),
                Player = Functions.GetNewPlayer(),
                Scoreboard = new List<Score>(),
                SelectedBackgroundColor = Color.FromArgb(0xFF, 0xF2, 0x50, 0x22),
                Words = Functions.GetWords(),
                TutorialCompleted = false
            };
            await DataSaver<Database>.SaveData("Database", Database.Current);
        }

        public async static Task FillDatabase()
        {
            Database newDatabase = await DataSaver<Database>.LoadData("Database");
            Database.Current = newDatabase;
            if (Database.Current == null)
                Functions.ResetDatabase();
        }

        public async static void UpdateDatabase()
        {
            await DataSaver<Database>.SaveData("Database", Database.Current);
        }


        public async static Task UpdateScores()
        {
            if (!ScoreboardDialog.IsUpdating)
            {
                ScoreboardDialog.IsUpdating = true;
                try
                {
                    IMobileServiceTable<Score> scoreTable = App.MobileService.GetTable<Score>();
                    MobileServiceCollection<Score, Score> scores = await scoreTable.ToCollectionAsync();
                    Database.Current.Scoreboard = scores.OrderByDescending(x => x.PlayerScore).Take(20).ToList();

                    var myScore = Database.Current.Scoreboard.SingleOrDefault(x => x.DeviceId.ToString() == Database.Current.DeviceId.ToString());
                    if (myScore != null)
                        if (myScore.PlayerScore > Database.Current.Player.Score.PlayerScore)
                            Database.Current.Player.Score = myScore;
                }
                catch (Exception)
                {
                }
                ScoreboardDialog.IsUpdating = false;
            }
        }
        public async static Task UpdateMyScore()
        {
            if (!Functions.IsUpdatingScore)
            {
                Functions.IsUpdatingScore = true;
                try
                {
                    IMobileServiceTable<Score> scoreTable = App.MobileService.GetTable<Score>();
                    var myScore = Database.Current.Scoreboard.SingleOrDefault(x => x.DeviceId.ToString() == Database.Current.DeviceId.ToString());
                    if (myScore == null)
                    {
                        Score newScore = new Score()
                        {
                            Region = Database.Current.Region,
                            PlayerName = Database.Current.Player.Name,
                            PlayerScore = Database.Current.Player.Score.PlayerScore,
                            DeviceId = Database.Current.DeviceId
                        };
                        if (newScore.PlayerScore > 0)
                            await scoreTable.InsertAsync(newScore);
                    }
                    else
                    {
                        if (myScore.PlayerScore < Database.Current.Player.Score.PlayerScore)
                            myScore.PlayerScore = Database.Current.Player.Score.PlayerScore;
                        myScore.Region = Database.Current.Region;
                        myScore.PlayerName = Database.Current.Player.Name;
                        await scoreTable.UpdateAsync(myScore);
                    }
                }
                catch (Exception)
                {
                }
                Functions.IsUpdatingScore = false;
                Functions.UpdateDatabase();
            }
        }

        public static void ResetWordStates()
        {
            foreach (var item in Database.Current.Words)
                item.IsFinished = false;
            UpdateDatabase();
        }

        public static bool IsUpdatingScore { get; set; }

        public async static void DeleteMyScore()
        {
            try
            {
                IMobileServiceTable<Score> scoreTable = App.MobileService.GetTable<Score>();
                MobileServiceCollection<Score, Score> scores = await scoreTable.ToCollectionAsync();
                await scoreTable.DeleteAsync(scores.SingleOrDefault(x => x.DeviceId.ToString() == Database.Current.DeviceId.ToString()));
            }
            catch (Exception)
            {
            }
        }
    }
}
