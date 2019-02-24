using OK.CargoTracking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace OK.CargoTracking.Windows.Data
{
    public class SavedData
    {
        public List<Tracking> History { get; set; }
        public List<Factory> Factories { get; internal set; }
        public List<Message> Messages { get; set; }
        public bool IsDeviceRegistered { get; set; }

        public SavedData()
        {
            this.IsDeviceRegistered = false;
            this.History = new List<Tracking>();
            this.Factories = new List<Factory>();
            this.Messages = new List<Message>();
        }
    }
}
