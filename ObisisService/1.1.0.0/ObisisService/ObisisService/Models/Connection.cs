using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using ObisisService.ObisisMobileService;
using ObisisService.ErciyesMobileService;

namespace ObisisService.Models
{
    [DataContract]
    public class Connection
    {
        [DataMember]
        public bool State { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public object DataObject { get; set; }

        public Connection GetStudent(int studentID, string password)
        {
            this.State = true;
            this.Message = "Merhaba " + studentID;
            this.DataObject = new Student();
            return this;
        }
        public Connection GetCurrentPeriod(int studentID, string password)
        {
            this.DataObject = new Period();
            return this;
        }
        public Connection GetFoodLists()
        {
            this.DataObject = new List<Food>();
            return this;
        }
        public Connection GetActivities()
        {
            this.DataObject = new List<Activity>();
            return this;
        }
        public Connection GetNotices()
        {
            try
            {
                EruWebServisClient client = new EruWebServisClient();
                DuyurularModel model = client.DuyurularList(0, 0).DataObject as DuyurularModel;
                this.DataObject = new List<Notice>();
            }
            catch (Exception)
            {
                this.DataObject = null;
                this.State = false;
                this.Message = "Şuanda duyurulara erişilemiyor..";
            }
            return this;
        }
    }
}