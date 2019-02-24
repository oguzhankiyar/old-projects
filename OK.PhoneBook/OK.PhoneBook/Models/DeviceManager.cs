using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace OK.PhoneBook.Models
{
    public static class DeviceManager
    {
        public static List<Device> GetDevices()
        {
            var devices = new List<Device>();
            string ipBase = "192.168.1.";
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 1; i < 255; i++)
            {
                string ip = ipBase + i.ToString();

                Ping p = new Ping();
                var reply = p.Send(ip, 100);
                if (reply.Status == IPStatus.Success)
                {
                    Debug.WriteLine(ip);
                    string name = "";
                    try
                    {
                        name = Dns.GetHostEntry(ip).HostName;
                    }
                    catch (Exception) { }
                    devices.Add(new Device() { Name = name, Address = ip });
                }

                if (sw.ElapsedMilliseconds > 30000)
                {
                    Debug.WriteLine("ABORTED");
                    i = 255;
                }
            }
            Debug.WriteLine("{0} hosts active.", devices.Count.ToString());
            return devices;
        }
    }
}
