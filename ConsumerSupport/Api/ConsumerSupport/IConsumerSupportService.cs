using ConsumerSupport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ConsumerSupport
{
    [ServiceContract]
    public interface IConsumerSupportService
    {
        [OperationContract]
        bool CreateSupport(Support support);

        [OperationContract]
        ListModel ListSupports(Admin admin);

        [OperationContract]
        Support ViewSupport(Admin admin, int id);

        [OperationContract]
        bool UpdateFlagSupport(Admin admin, int id, bool state);

        [OperationContract]
        bool ReplySupport(Admin admin, Consumer consumer, string text);

        [OperationContract]
        bool DeleteSupport(Admin admin, int id);
    }
}
