using ObisisService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ObisisService
{
    [ServiceContract]
    public interface IObisisMobileService
    {
        [OperationContract]
        Connection GetStudent(int studentID, string password);

        [OperationContract]
        Connection GetCurrentPeriod(Period currentPeriod, int studentID, string password);

        [OperationContract]
        Connection GetActivities();

        [OperationContract]
        Connection GetFoodLists();
    }
}
