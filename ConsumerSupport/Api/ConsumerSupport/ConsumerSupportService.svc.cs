using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ConsumerSupport.Models;

namespace ConsumerSupport
{
    public class ConsumerSupportService : IConsumerSupportService
    {
        private SupportEntities Db = new SupportEntities();
        public bool CreateSupport(Support support)
        {
            if (support == null || support.Consumer == null || support.App == null || string.IsNullOrEmpty(support.Content))
                return false;
            try
            {
                App app = Db.Apps.SingleOrDefault(x => x.Name == support.App.Name && x.Platform == support.App.Platform && x.Version == support.App.Version);
                if (app == null)
                {
                    Db.Apps.Add(support.App);
                    Db.SaveChanges();
                    app = Db.Apps.SingleOrDefault(x => x.Name == support.App.Name && x.Platform == support.App.Platform && x.Version == support.App.Version);
                }
                Consumer consumer = Db.Consumers.SingleOrDefault(x => x.Name == support.Consumer.Name && x.Email == support.Consumer.Email);
                if (consumer == null)
                {
                    Db.Consumers.Add(support.Consumer);
                    Db.SaveChanges();
                    consumer = Db.Consumers.SingleOrDefault(x => x.Name == support.Consumer.Name && x.Email == support.Consumer.Email);
                }
                support.App = app;
                support.AppId = app.Id;
                support.Consumer = consumer;
                support.ConsumerId = consumer.Id;
                Db.Supports.Add(support);
                Db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ListModel ListSupports(Admin admin)
        {
            if (admin == null || string.IsNullOrEmpty(admin.Username) || string.IsNullOrEmpty(admin.Password))
                return null;
            string password = MD5Hashing.Hash(admin.Password);
            if (Db.Admins.SingleOrDefault(x => x.Username == admin.Username && x.Password == password) != null)
            {
                Support[] arry = new Support[1];
                arry[0] = new Support() { Content = "deneme" };
                return new ListModel() { List = arry };
            }
            else
                return null;
        }

        public Support ViewSupport(Admin admin, int id)
        {
            if (admin == null || string.IsNullOrEmpty(admin.Username) || string.IsNullOrEmpty(admin.Password))
                return null;
            string password = MD5Hashing.Hash(admin.Password);
            if (Db.Admins.SingleOrDefault(x => x.Username == admin.Username && x.Password == password) != null)
            {
                Support support = Db.Supports.SingleOrDefault(x => x.Id == id);
                if (support != null)
                {
                    support.IsRead = true;
                    Db.SaveChanges();
                    return support;
                }
                else
                    return null;
            }
            else
                return null;
        }

        public bool UpdateFlagSupport(Admin admin, int id, bool state)
        {
            if (admin == null || string.IsNullOrEmpty(admin.Username) || string.IsNullOrEmpty(admin.Password))
                return false;
            string password = MD5Hashing.Hash(admin.Password);
            if (Db.Admins.SingleOrDefault(x => x.Username == admin.Username && x.Password == password) != null)
            {
                Support support = Db.Supports.SingleOrDefault(x => x.Id == id);
                if (support != null)
                {
                    support.IsFlagged = state;
                    Db.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public bool ReplySupport(Admin admin, Consumer consumer, string text)
        {
            if (admin == null || string.IsNullOrEmpty(admin.Username) || string.IsNullOrEmpty(admin.Password))
                return false;
            string password = MD5Hashing.Hash(admin.Password);
            if (Db.Admins.SingleOrDefault(x => x.Username == admin.Username && x.Password == password) != null)
            {
                try
                {
                    // Send reply to consumer
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
                return false;
        }

        public bool DeleteSupport(Admin admin, int id)
        {
            if (admin == null || string.IsNullOrEmpty(admin.Username) || string.IsNullOrEmpty(admin.Password))
                return false;
            string password = MD5Hashing.Hash(admin.Password);
            if (Db.Admins.SingleOrDefault(x => x.Username == admin.Username && x.Password == password) != null)
            {
                Support message = Db.Supports.SingleOrDefault(x => x.Id == id);
                if (message == null)
                    return false;
                else
                {
                    Db.Supports.Remove(message);
                    Db.SaveChanges();
                    return true;
                }
            }
            else
                return false;
        }
    }
}
