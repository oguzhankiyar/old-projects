using ObisisService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ObisisService
{
    public class ObisisMobileService : IObisisMobileService
    {
        public Connection GetStudent(int studentID, string password)
        {
            return new Connection().GetStudent(studentID, password);
        }
        public Connection GetCurrentPeriod(Period currentPeriod, int studentID, string password)
        {
            return new Connection().GetCurrentPeriod(currentPeriod, studentID, password);
        }
        public Connection GetActivities()
        {
            return new Connection().GetActivities();
        }
        public Connection GetFoodLists()
        {
            return new Connection().GetFoodLists();
        }
    }
}
