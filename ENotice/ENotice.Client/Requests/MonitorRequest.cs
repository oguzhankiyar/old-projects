using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ENotice.Client.Entities;
using ENotice.Client.Parsings;

namespace ENotice.Client.Requests
{
    public class MonitorRequest
    {
        public static Action<Monitor> OnDataLoaded { get; set; }
        public static bool IsDataLoaded { get; set; }

        public static void GetMonitor(int monitorId)
        {
            IsDataLoaded = false;
            var request = HttpWebRequest.Create("http://" + Settings.HostUrl + ":" + Settings.HostPort + "/api/monitorsapi/" + monitorId);
            var response = request.GetResponse();
            var streamReader = new StreamReader(response.GetResponseStream());
            string jsonResult = streamReader.ReadToEnd();
            var monitor = MonitorParsing.ParseMonitor(jsonResult);
            PopulateMonitor(monitor);
        }

        public static void PopulateMonitor(Monitor monitor)
        {
            IsDataLoaded = true;
            if (OnDataLoaded != null)
                OnDataLoaded(monitor);
        }
    }
}
