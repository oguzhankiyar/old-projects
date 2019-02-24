using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OK.Mobisis.Pages;

namespace OK.Mobisis.Data
{
    public static class Database
    {
        public static SavedData SavedData { get; set; }
        public static TempData TempData { get; set; }

        static Database()
        {
            TempData = new TempData();
        }

        internal async static Task Update()
        {
            if (SavedData != null)
                await DataSaver<SavedData>.Save("SavedData", SavedData);
        }

        internal async static Task Init()
        {
            SavedData = await DataSaver<SavedData>.Load("SavedData");
            if (SavedData == null)
            {
                SavedData = new SavedData();
                SavedData.Init();
            }
            else
            {
                TempData.Student = SavedData.Student;
                TempData.RememberMe = true;
            }
            SwipeMenu.Items = Database.SavedData.MenuItems;
        }
    }
}
