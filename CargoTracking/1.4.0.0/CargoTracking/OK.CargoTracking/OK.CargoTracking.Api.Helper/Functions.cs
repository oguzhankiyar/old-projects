using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OK.CargoTracking.Api.Helper
{
    public static class Functions
    {
        internal static async Task<string> GetHtmlAsync(string urlString, Encoding encoding = null, string postData = "")
        {
            var request = (HttpWebRequest)WebRequest.Create(urlString);
            return await GetHtmlAsync(request, encoding, postData);
        }

        internal static async Task<string> GetHtmlAsync(HttpWebRequest request, Encoding encoding = null, string postData = "")
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            if (string.IsNullOrEmpty(postData))
            {
                var response = await new HttpClient().GetAsync(request.RequestUri);
                string result = await response.Content.ReadAsStringAsync();
                return result;
            }
            else
            {
                var content = new StringContent(postData, encoding, "application/json");
                var response = await new HttpClient().PostAsync(request.RequestUri, content);
                string result = await response.Content.ReadAsStringAsync();
                return result;
            }
        }
    }
}
