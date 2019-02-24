using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biletall.Helper.Entities;

namespace Biletall.Helper
{
    public class Global
    {
        internal static string ApiUsername { get; private set; }
        internal static string ApiPassword { get; private set; }
        internal static List<Station> Stations { get; set; }
        public static List<Factory> Factories { get; internal set; }
        internal static List<BusProperty> BusProperties { get; set; }
        public static Action<Action> Invoke { get; set; }

        public static Action<string, object[]> OnRequestCalled { get; set; }
        public static Action<string> OnRequestReturned { get; set; }
        public static Action<object> OnResultParsed { get; set; }
        public static Action OnRequestCompleted { get; set; }
        public static Action<Exception> OnRequestFailed { get; set; }
        public static Action OnRequestCanceled { get; set; }
        public static Action<string> OnRequestParsed { get; set; }


        static Global()
        {
            Stations = new List<Station>();
            Factories = new List<Factory>();
            BusProperties = new List<BusProperty>();
        }

        public static void Authorize(string username, string password)
        {
            ApiUsername = username;
            ApiPassword = password;
        }

        public static void LoadStations(List<Station> stations)
        {
            Global.Stations = stations;
        }
    }
}
