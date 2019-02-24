using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ENotice.Client.Entities;
using ENotice.Client.Parsings;

namespace ENotice.Client.Requests
{
    public class NoticeRequest
    {
        public static bool IsDataLoaded { get; set; }
        public static Action<List<Notice>> OnDataLoaded { get; set; }

        public static void GetNotices(int monitorId)
        {
            IsDataLoaded = false;
            var request = HttpWebRequest.Create("http://" + Settings.HostUrl + ":" + Settings.HostPort + "/api/noticesapi?monitorId=" + monitorId);
            var response = request.GetResponse();
            var streamReader = new StreamReader(response.GetResponseStream());
            string jsonResult = streamReader.ReadToEnd();
            var list = NoticeParsing.ParseNotices(jsonResult);
            PopulateNotices(list);
        }

        private static void PopulateNotices(List<Notice> list)
        {
            IsDataLoaded = true;
            if (OnDataLoaded != null)
                OnDataLoaded(list);
        }
    }
}
