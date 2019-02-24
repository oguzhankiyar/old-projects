using System.IO;
using Newtonsoft.Json;

namespace OK.PhoneBook.Models
{
    public class Database
    {
        static string path = "data.json";
        public static SavedData SavedData { get; set; }

        static Database()
        {
            if (!File.Exists(path))
                Save();
            Load();
        }

        public static void Load()
        {
            string json = File.ReadAllText(path);
            SavedData = JsonConvert.DeserializeObject<SavedData>(json);
        }

        public static void Save()
        {
            if (Database.SavedData == null)
                Database.SavedData = new SavedData();
            string json = JsonConvert.SerializeObject(Database.SavedData);
            File.WriteAllText(path, json);
        }
    }
}
