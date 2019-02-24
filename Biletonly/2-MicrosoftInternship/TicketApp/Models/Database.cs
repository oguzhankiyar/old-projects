using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Biletall.Helper.Entities;
using Microsoft.Phone.Controls;

namespace TicketApp.Models
{
    public static class Database
    {
        public static SavedData SavedData { get; set; }
        public static TempData TempData { get; set; }

        static Database()
        {
            Logger.MethodCalled("Database()");
            TempData = new TempData();
        }

        public static void Init()
        {
            Logger.MethodCalled("Database.Init()");
            var SavedDataManager = new DataSaver<SavedData>();
            SavedData = SavedDataManager.LoadMyData("SavedData") == null ? new SavedData() : SavedDataManager.LoadMyData("SavedData");
        }

        public static void Update()
        {
            Logger.MethodCalled("Database.Update()");
            var SavedDataManager = new DataSaver<SavedData>();
            SavedDataManager.SaveMyData(SavedData, "SavedData");
        }

        public static void AddTicket(Ticket ticket)
        {
            Logger.MethodCalled("Database.AddTicket(Ticket)", new object[] { ticket });
            RemoveTicket(ticket);
            Database.SavedData.Tickets.Insert(0, ticket);
        }

        public static void UpdateTicket(Ticket ticket)
        {
            Logger.MethodCalled("Database.UpdateTicket(Ticket)", new object[] { ticket });
            AddTicket(ticket);
        }

        public static void RemoveTicket(Ticket ticket)
        {
            Logger.MethodCalled("Database.RemoveTicket(Ticket)", new object[] { ticket });
            var oldTicket = Database.SavedData.Tickets.SingleOrDefault(x => x.PNR.Code.ToUpper() == ticket.PNR.Code.ToUpper());
            if (oldTicket != null)
                Database.SavedData.Tickets.Remove(oldTicket);
        }

        public static void AddPassenger(Passenger passenger)
        {
            Logger.MethodCalled("Database.AddPassenger(Passenger)", new object[] { passenger });
            RemovePassenger(passenger);
            Database.SavedData.FavoritePassengers.Add(passenger);
        }

        public static void AddPassenger(List<Passenger> passengers)
        {
            Logger.MethodCalled("Database.AddPassenger(List<Passenger>)", new object[] { passengers });
            foreach (var passenger in passengers)
                AddPassenger(passenger);
        }

        public static void RemovePassenger(Passenger passenger)
        {
            Logger.MethodCalled("Database.RemovePassenger(Passenger)", new object[] { passenger });
            Database.SavedData.FavoritePassengers.RemoveAll(x => x.FirstName == passenger.FirstName && x.LastName == passenger.LastName);
        }

        internal static void FillSavedPNR()
        {
            Logger.MethodCalled("Database.FillSavedPNR()");
            var pnr = Database.TempData.Ticket.PNR;
            if (pnr == null)
                return;
            if (!string.IsNullOrEmpty(pnr.Email) || !string.IsNullOrEmpty(pnr.TelephoneNumber) || !string.IsNullOrEmpty(pnr.PhoneNumber))
            {
                Database.SavedData.PNR = new PNR();
                Database.SavedData.PNR.Email = pnr.Email;
                Database.SavedData.PNR.TelephoneNumber = pnr.TelephoneNumber;
                Database.SavedData.PNR.PhoneNumber = pnr.PhoneNumber;
            }
        }
    }
}
