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
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        Connection GetStudent(int studentID, string password);
        [OperationContract]
        Connection GetCurrentPeriod(int studentID, string password);
        [OperationContract]
        Connection GetActivities();
        [OperationContract]
        Connection GetNotices();
        [OperationContract]
        Connection GetFoodLists();
    }
}
