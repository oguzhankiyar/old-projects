using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENotice.Client.Entities;
using Newtonsoft.Json.Linq;

namespace ENotice.Client.Parsings
{
    public class MonitorParsing
    {
        public static Monitor ParseMonitor(string json)
        {
            var monitor = new Monitor();
            var obj = JObject.Parse(json);
            monitor.Id = Convert.ToInt32(obj["Id"].ToString());
            monitor.Name = obj["Name"].ToString();
            return monitor;
        }
    }
}
