using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OK.Mobisis.Api.Helper
{
    public class Global
    {
        public static Uri ApiUrl = new Uri("http://mobisis.ogzkyr.net/api"); //http://localhost:21432/api
        public static string ApiVersion = "1.0";
        public static string ApiUsername;
        public static string ApiPassword;

        public static Action<Action> Invoke { get; set; }
        public static Action<string, object[]> OnRequestCalled { get; set; }
        public static Action<string> OnRequestReturned { get; set; }
        public static Action<object> OnResultParsed { get; set; }
        public static Action OnRequestCompleted { get; set; }
        public static Action<Exception> OnRequestFailed { get; set; }
        public static Action OnRequestCanceled { get; set; }
        public static Action<string> OnRequestParsed { get; set; }

        static Global()
        {
            Global.OnRequestCalled = (name, parameters) =>
            {
                System.Diagnostics.Debug.WriteLine("hi");
                //Logger.RequestCalled(name, parameters);
            };

            Global.OnRequestReturned = (response) =>
            {
                System.Diagnostics.Debug.WriteLine("hi");
                //Logger.RequestReturned(response);
            };

            Global.OnResultParsed = (response) =>
            {
                System.Diagnostics.Debug.WriteLine("hi");
                //Logger.ResultParsed(response);
            };

            Global.OnRequestCompleted = () =>
            {
                System.Diagnostics.Debug.WriteLine("hi");
                //Logger.RequestCompleted();
            };

            Global.OnRequestCanceled = () =>
            {
                System.Diagnostics.Debug.WriteLine("hi");
                //Logger.RequestCanceled();
            };

            Global.OnRequestFailed = (exp) =>
            {
                System.Diagnostics.Debug.WriteLine("hi");
                //Logger.RequestFailed(exp);
            };

            Global.OnRequestParsed = (xml) =>
            {
                System.Diagnostics.Debug.WriteLine("hi");
                //Logger.RequestParsed(xml);
            };
        }
    }
}
