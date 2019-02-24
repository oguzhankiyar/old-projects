using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mobisis.ObisisServiceReference;

namespace Mobisis.Models
{
    class Database
    {
        public static Student Student { get; set; }
        public static List<LessonPlanItem> LessonPlan { get; set; }
        public static List<FoodList> FoodLists { get; set; }
        public static Settings Settings { get; set; }

        public static void UpdateAll()
        {
            UpdateStudent();
            UpdateLessonPlan();
            UpdateFoodLists();
            UpdateSettings();
        }
        public static void Fill()
        {
            DataSaver<Student> StudentData = new DataSaver<Student>();
            Student = StudentData.LoadMyData("Student");

            DataSaver<List<LessonPlanItem>> LessonPlanData = new DataSaver<List<LessonPlanItem>>();
            LessonPlan = LessonPlanData.LoadMyData("LessonPlan") == null ? new List<LessonPlanItem>() : LessonPlanData.LoadMyData("LessonPlan");

            DataSaver<List<FoodList>> FoodListsData = new DataSaver<List<FoodList>>();
            FoodLists = FoodListsData.LoadMyData("FoodLists");

            DataSaver<Settings> SettingsData = new DataSaver<Settings>();
            Settings = SettingsData.LoadMyData("Settings") == null ? new Settings() : SettingsData.LoadMyData("Settings");
        }
        public static void UpdateStudent()
        {
            DataSaver<Student> StudentData = new DataSaver<Student>();
            StudentData.SaveMyData(Student, "Student");
        }
        public static void UpdateLessonPlan()
        {
            DataSaver<List<LessonPlanItem>> LessonPlanData = new DataSaver<List<LessonPlanItem>>();
            LessonPlanData.SaveMyData(LessonPlan, "LessonPlan");
        }
        public static void UpdateFoodLists()
        {
            DataSaver<List<FoodList>> FoodListsData = new DataSaver<List<FoodList>>();
            FoodListsData.SaveMyData(FoodLists, "FoodLists");
        }
        public static void UpdateSettings()
        {
            DataSaver<Settings> SettingsData = new DataSaver<Settings>();
            SettingsData.SaveMyData(Settings, "Settings");
        }
    }
}
