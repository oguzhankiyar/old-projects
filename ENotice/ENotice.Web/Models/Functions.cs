using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ENotice.Web.Models
{
    public class Functions
    {
        public static int _monitorId;
        public static List<Client> Clients;
        private static ENoticeEntities db = new ENoticeEntities();

        static Functions()
        {
            Clients = new List<Client>();
        }

        public static void SendMessage(int monitorId, string message)
        {
            foreach (var item in Clients.Where(x => x.MonitorId == monitorId))
            {
                item.Socket.Send(message);
            }
        }

        public static void SendServerMessage(string message)
        {
            foreach (var item in Clients.Where(x => x.MonitorId == 1000))
            {
                item.Socket.Send(message);
            }
        }

        public static void ClientOpened(SocketHandler socketHandler)
        {
            if (socketHandler.WebSocketContext.QueryString["monitorId"] != null)
            {
                _monitorId = Convert.ToInt32(socketHandler.WebSocketContext.QueryString["monitorId"].ToString());
            }
            var client = new Client(_monitorId, socketHandler);
            Clients.Add(client);
            Functions.SendServerMessage(_monitorId + " is joined");
        }

        public static void ClientMessaged(SocketHandler socketHandler, string message)
        {
            Functions.SendServerMessage(_monitorId + ": " + message);
        }

        public static void ClientClosed(SocketHandler socketHandler)
        {
            Clients.RemoveAll(x => x.Socket == socketHandler);
            Functions.SendServerMessage(_monitorId + " is left");
        }


        public static void NotifyMonitors(List<string> facultyIds, List<string> departmentIds, List<string> monitorIds, string message)
        {
            var monitorList = new List<Monitor>();

            foreach (var facultyId in facultyIds)
            {
                int id = Convert.ToInt32(facultyId);
                var faculty = db.Faculties.SingleOrDefault(x => x.Id == id);
                if (faculty != null)
                {
                    var departments = db.Departments.Where(x => x.FacultyId == faculty.Id);
                    foreach (var department in departments)
                    {
                        var monitors = db.Monitors.Where(x => x.DepartmentId == department.Id);
                        foreach (var monitor in monitors)
                        {
                            if (!monitorList.Contains(monitor))
                                monitorList.Add(monitor);
                        }
                    }
                }
            }

            foreach (var departmentId in departmentIds)
            {
                int id = Convert.ToInt32(departmentId);
                var department = db.Departments.SingleOrDefault(x => x.Id == id);
                if (department != null)
                {
                    var monitors = db.Monitors.Where(x => x.DepartmentId == department.Id);
                    foreach (var monitor in monitors)
                    {
                        if (!monitorList.Contains(monitor))
                            monitorList.Add(monitor);
                    }
                }
            }
            
            foreach (var monitorId in monitorIds)
            {
                int id = Convert.ToInt32(monitorId);
                var monitor = db.Monitors.SingleOrDefault(x => x.Id == id);
                if (monitor != null)
                {
                    if (!monitorList.Contains(monitor))
                        monitorList.Add(monitor);
                }
            }

            foreach (var monitor in monitorList)
            {
                Functions.SendMessage(monitor.Id, message);
            }
        }

        public static bool IsConnected(int monitorId)
        {
            var client = Clients.SingleOrDefault(x => x.MonitorId == monitorId);
            if (client != null)
                return true;
            return false;
        }
    }
}