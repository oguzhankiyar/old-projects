using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using BlockTetris.Entities;
using BlockTetris.Enums;
using Microsoft.WindowsAzure.MobileServices;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace BlockTetris.Data
{
    [DataContract]
    public class Database
    {
        [DataMember]
        public static Database Current { get; set; }

        [DataMember]
        public List<Color> ColorList { get; set; }
        [DataMember]
        public Color SelectedBackgroundColor { get; set; }
        [DataMember]
        public List<Word> Words { get; set; }
        [DataMember]
        public List<Score> Scoreboard { get; set; }
        [DataMember]
        public Player Player { get; set; }
        [DataMember]
        public double ScreenWidth { get; set; }
        [DataMember]
        public double ScreenHeight { get; set; }
        [DataMember]
        public string DeviceId { get; set; }
        [DataMember]
        public string Region { get; set; }
        [DataMember]
        public bool TutorialCompleted { get; set; }
    }
}
