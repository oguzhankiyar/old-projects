using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ObisisService.Models;

namespace ObisisService
{
    public class Service : IService
    {
        public Connection GetStudent(int studentID, string password)
        {
            return new Connection().GetStudent(studentID, password);
        }
        public Connection GetCurrentPeriod(int studentID, string password)
        {
            return new Connection().GetCurrentPeriod(studentID, password);
        }
        public Connection GetActivities()
        {
            return new Connection().GetActivities();
        }
        public Connection GetNotices()
        {
            return new Connection().GetNotices();
        }
        public Connection GetFoodLists()
        {
            return new Connection().GetFoodLists();
        }
    }
}
