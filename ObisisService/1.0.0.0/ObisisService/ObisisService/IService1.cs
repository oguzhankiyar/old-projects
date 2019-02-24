using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ObisisService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        //[WebInvoke(UriTemplate = "/Login/?n={OgrenciNo}&p={Sifre}&r={Ref}", ResponseFormat = WebMessageFormat.Json)]
        Baglanti Login(string OgrenciNo, string Sifre, string Ref);

        [OperationContract]
        //[WebInvoke(UriTemplate = "/GetFood/?r={Ref}", ResponseFormat = WebMessageFormat.Json)]
        List<YemekList> GetFood(string Ref);

        [OperationContract]
        //[WebInvoke(UriTemplate = "/CurrentPeriod/?n={OgrenciNo}&p={Sifre}&r={Ref}", ResponseFormat = WebMessageFormat.Json)]
        Donem CurrentPeriod(string OgrenciNo, string Sifre, string Ref);
    }
}
