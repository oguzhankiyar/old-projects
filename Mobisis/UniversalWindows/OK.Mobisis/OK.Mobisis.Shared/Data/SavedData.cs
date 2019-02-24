using System;
using System.Collections.Generic;
using System.Text;
using OK.Mobisis.Entities.ModelEntities;
using OK.Mobisis.Pages;

namespace OK.Mobisis.Data
{
    public class SavedData
    {
        public Student Student { get; set; }
        public List<FoodList> FoodLists { get; set; }
        public List<SwipeMenuItem> MenuItems { get; set; }
        public Settings Settings { get; set; }

        internal void Init()
        {
            Settings = new Data.Settings();

            var list = new List<SwipeMenuItem>();
            list.Add(new SwipeMenuItem("Giriş", typeof(LoginPage)));
            list.Add(new SwipeMenuItem("Bilgiler", typeof(StudentPage)));
            list.Add(new SwipeMenuItem("Dönemler", typeof(PeriodListPage)));
            list.Add(new SwipeMenuItem("Dersler", typeof(LessonListPage)));
            list.Add(new SwipeMenuItem("Gano Hesapla", typeof(CalculateGanoPage)));
            list.Add(new SwipeMenuItem("Yemek Listesi", typeof(FoodListsPage)));
            list.Add(new SwipeMenuItem("Ayarlar", typeof(SettingsPage)));
            list.Add(new SwipeMenuItem("Hakkında", typeof(AboutPage)));
            this.MenuItems = list;
        }
    }
}
