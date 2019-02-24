using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Phone.Controls;

namespace CargoTracking.Models
{
    public class Database
    {
        public static Dispatcher Dispatcher { get; set; }
        public static Pivot TrackingDetails { get; set; }
        public static TextBlock WarningText { get; set; }
        public static string TrackingCode { get; set; }
        public static Factory SelectedFactory { get; set; }

        public static List<Factory> Factories { get; set; }
        public static List<Tracking> History { get; set; }

        public static void Fill()
        {
            Factories = new List<Factory>
            {
                ArasCargo.Factory,
                DhlCargo.Factory,
                MngCargo.Factory,
                PttCargo.Factory,
                SuratCargo.Factory,
                UpsCargo.Factory,
                YurticiCargo.Factory
            };
            DataSaver<List<Tracking>> HistoryData = new DataSaver<List<Tracking>>();
            History = HistoryData.LoadMyData("History") ?? new List<Tracking>();
        }

        public static void Update()
        {
            DataSaver<List<Tracking>> HistoryData = new DataSaver<List<Tracking>>();
            HistoryData.SaveMyData(History, "History");
        }

        public static void AddToHistory(Tracking tracking)
        {
            if (History.SingleOrDefault(x => x.Code == tracking.Code) == null)
                History.Add(tracking);
            Update();
        }

        public static void RemoveFromHistory(Tracking tracking)
        {
            History.Remove(tracking);
            Update();
        }
    }
}
