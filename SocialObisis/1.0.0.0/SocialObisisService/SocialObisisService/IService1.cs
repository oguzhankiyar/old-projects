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
    public interface IService1
    {
        [OperationContract]
        [WebGet(UriTemplate = "GetStudent/?ogrenciNo={ogrenciNo}&sifre={sifre}")]
        Obisis.Ogrenci GetStudent(string ogrenciNo, string sifre);

        [OperationContract]
        [WebGet(UriTemplate = "GetStudentName/?ogrenciNo={ogrenciNo}&sifre={sifre}")]
        string GetStudentName(string ogrenciNo, string sifre);

        [OperationContract]
        Obisis.Duyuru GetNotices();

        [WebInvoke(UriTemplate = "/players.json", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        List<Person> GetPlayers();
    }
}