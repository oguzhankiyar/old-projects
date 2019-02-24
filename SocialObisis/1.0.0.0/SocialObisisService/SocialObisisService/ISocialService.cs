using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SocialObisisService
{
    [ServiceContract]
    public interface ISocialService
    {
        [OperationContract]
        Obisis.Ogrenci GetStudent(string ogrenciNo, string sifre);

        [OperationContract]
        Obisis.Duyuru GetNotices();
    }
}
