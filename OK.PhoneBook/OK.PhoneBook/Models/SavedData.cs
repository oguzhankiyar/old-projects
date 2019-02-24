using System;
using System.Collections.Generic;

namespace OK.PhoneBook.Models
{
    public class SavedData
    {
        public List<People> Records { get; set; }
        public List<Appointment> Appointments { get; set; }
        public List<Guid> Favorites { get; set; }
        public Settings Settings { get; set; }

        public SavedData()
        {
            Records = new List<People>();
            Appointments = new List<Appointment>();
            Favorites = new List<Guid>();
            Settings = new Settings();

            Settings.Username = "sksd";
            Settings.Password = "sksd";
            Settings.AdminPassword = "sksd";
        }
    }
}
