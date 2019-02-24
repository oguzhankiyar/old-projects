using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OK.CargoTracking.Model;

namespace OK.CargoTracking.Web.Models
{
    public static class Repository
    {
        private static CargoTrackingEntities db = new CargoTrackingEntities();

        public static List<Model.Factory> GetAllFactories()
        {
            var list = new List<Model.Factory>();
            foreach (var factory in db.Factories.OrderBy(x => x.Sort))
            {
                list.Add(new Model.Factory()
                {
                    Id = factory.Id,
                    Name = factory.Name,
                    IconSource = factory.IconSource,
                    Sort = factory.Sort
                });
            }
            return list;
        }

        internal static bool IsDeviceExist(string id)
        {
            var device = db.Devices.SingleOrDefault(x => x.Code == id);
            return device != null;
        }

        internal static List<Model.Message> GetDeviceMessages(string id)
        {
            var messages = new List<Model.Message>();
            AddDeviceMessages(id);
            var device = db.Devices.SingleOrDefault(x => x.Code == id);
            var deviceMessages = db.DeviceMessages.Where(x => x.DeviceId == device.Id && x.IsSent == false);
            foreach (var item in deviceMessages)
            {
                var message = item.Message;
                item.IsSent = true;
                item.SentAt = DateTime.Now;
                messages.Add(new Model.Message()
                {
                    Title = message.Title,
                    Content = message.Content,
                    Type = message.Type,
                    Buttons = GetMessageButtons(message)
                });
            }
            db.SaveChanges();
            return messages;
        }

        private static List<Model.MessageButton> GetMessageButtons(Message message)
        {
            var list = new List<Model.MessageButton>();
            var buttons = message.MessageButtons;
            foreach (var item in buttons)
            {
                var button = item.Button;
                list.Add(new Model.MessageButton(button.Content, button.Action));
            }
            return list;
        }

        internal static void AddDeviceMessages(string id)
        {
            var device = db.Devices.SingleOrDefault(x => x.Code == id);
            int registerHours = (DateTime.Now - device.RegisterDate).Hours;
            var messages = from message in db.Messages
                           where
                           (message.DeviceName == null || message.DeviceName == device.Name) &&
                           (message.PlatformName == null || message.PlatformName == device.Platform) &&
                           (message.AppVersion == null || message.AppVersion == device.AppVersion) &&
                           (message.HoursAfterRegister == null || message.HoursAfterRegister <= registerHours)
                           select message;
            foreach (var message in messages)
            {
                var deviceMessage = new DeviceMessage();
                deviceMessage.Device = device;
                deviceMessage.Message = message;
                deviceMessage.IsSent = false;
                if (!db.DeviceMessages.Where(x => x.MessageId == message.Id).Any())
                    db.DeviceMessages.Add(deviceMessage);
            }
            db.SaveChanges();
        }

        public static Model.Factory GetFactory(int factoryId)
        {
            var factory = db.Factories.SingleOrDefault(x => x.Id == factoryId);
            return new Model.Factory()
            {
                Id = factory.Id,
                Name = factory.Name,
                IconSource = factory.IconSource,
                Sort = factory.Sort
            };
        }

        public static Action AddAction(string code, int factoryId, bool isSuccessful, string deviceCode)
        {
            var device = db.Devices.SingleOrDefault(x => x.Code == deviceCode);
            var action = new Action()
            {
                Code = code,
                Factory = db.Factories.SingleOrDefault(x => x.Id == factoryId),
                IsSuccessful = isSuccessful,
                Date = DateTime.Now,
                DeviceId = device.Id
            };
            db.Actions.Add(action);
            db.SaveChanges();
            return action;
        }
        
        public static Device AddDevice(string code, string name, string platform, string appVersion)
        {
            bool isNewRecord = false;
            var device = db.Devices.SingleOrDefault(x => x.Code == code);
            if (device == null)
            {
                isNewRecord = true;
                device = new Device();
            }

            device.Code = code;
            device.Name = name;
            device.Platform = platform;
            device.AppVersion = appVersion;
            device.RegisterDate = DateTime.Now;

            if (isNewRecord)
                db.Devices.Add(device);
            db.SaveChanges();
            return device;
        }
    }
}