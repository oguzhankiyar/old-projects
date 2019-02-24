using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Phone.Info;
using OK.CargoTracking.Model;

namespace OK.CargoTracking.WindowsPhone.Data
{
    public static class DeviceHelper
    {
        public static string Id
        {
            get
            {
                byte[] id = (byte[])DeviceExtendedProperties.GetValue("DeviceUniqueId");
                return Convert.ToBase64String(id);
            }
        }

        public static string Name
        {
            get
            {
                return DeviceStatus.DeviceManufacturer + " " + DeviceStatus.DeviceName;
            }
        }

        public static string Platform
        {
            get
            {
                return Environment.OSVersion.ToString();
            }
        }

        public static string AppVersion
        {
            get
            {
                var xmlReaderSettings = new XmlReaderSettings
                {
                    XmlResolver = new XmlXapResolver()
                };

                using (var xmlReader = XmlReader.Create("WMAppManifest.xml", xmlReaderSettings))
                {
                    xmlReader.ReadToDescendant("App");
                    return xmlReader.GetAttribute("Version");
                }
            }
        }

        public static Device GetDevice()
        {
            var device = new Device();
            device.Id = Id;
            device.Platform = Platform;
            device.Name = Name;
            device.AppVersion = AppVersion;
            return device;
        }
    }
}
