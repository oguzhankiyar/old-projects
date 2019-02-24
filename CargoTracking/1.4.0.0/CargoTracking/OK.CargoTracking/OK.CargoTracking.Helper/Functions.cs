using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OK.CargoTracking.Helper
{
    public static class Functions
    {
        internal static async Task<string> GetHtmlSourceAsync(HttpClient client, string requestUrl, StringContent content = null, Dictionary<string, string> headers = null, Encoding readEncoding = null)
        {
            if (client == null)
                client = new HttpClient();

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            if (content == null)
                content = new StringContent("");
            var httpResponse = await client.PostAsync(requestUrl, content);
            var stream = await httpResponse.Content.ReadAsStreamAsync();
            if (readEncoding == null)
                readEncoding = Encoding.UTF8;
            var reader = new StreamReader(stream, readEncoding);
            return reader.ReadToEnd();
        }

        internal static async Task<string> GetHtmlSourceAsync(string requestUrl, StringContent content = null, Dictionary<string, string> headers = null, Encoding readEncoding = null)
        {
            return await GetHtmlSourceAsync(new HttpClient(), requestUrl, content, headers, readEncoding);
        }

        /*
        internal static async Task<string> GetHtmlAsync(string urlString, Encoding encoding = null, string postData = "")
        {
            var request = (HttpWebRequest)WebRequest.Create(urlString);
            return await GetHtmlAsync(request, encoding, postData);
        }

        internal static Task<string> GetHtmlAsync(HttpWebRequest request, Encoding encoding = null, string postData = "")
        {

#if TEST
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
                var content = new StringContent(postData, encoding, "");
                var response = await new HttpClient().PostAsync(request.RequestUri, content);
                string result = await response.Content.ReadAsStringAsync();
                return result;
            }
#else
            TaskCompletionSource<string> result = new TaskCompletionSource<string>();
            
            if (encoding == null)
                encoding = Encoding.UTF8;

            if (string.IsNullOrEmpty(postData))
            {
                request.Method = "GET";
                Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, request)
                    .ContinueWith(responseTask =>
                    {
                        try
                        {
                            using (var webResponse = responseTask.Result)
                            using (var responseStream = webResponse.GetResponseStream())
                            using (var sr = new StreamReader(responseStream, encoding))
                            {
                                string xmlResult = sr.ReadToEnd();
                                xmlResult = WebUtility.HtmlDecode(xmlResult);
                                result.SetResult(xmlResult);
                            }
                        }
                        catch (Exception e)
                        {
                            result.SetException(e);
                        }
                    }, TaskContinuationOptions.AttachedToParent);
            }
            else
            {
                request.Method = "POST";
                byte[] data = Encoding.UTF8.GetBytes(postData);
                Task.Factory.FromAsync<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream, request)
                   .ContinueWith(requestStreamTask =>
                   {
                       try
                       {
                           using (var localStream = requestStreamTask.Result)
                           {
                               localStream.Write(data, 0, data.Length);
                               localStream.Flush();
                           }

                           Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, request)
                               .ContinueWith(responseTask =>
                               {
                                   try
                                   {
                                       using (var webResponse = responseTask.Result)
                                       using (var responseStream = webResponse.GetResponseStream())
                                       using (var sr = new StreamReader(responseStream, encoding))
                                       {
                                           string xmlResult = sr.ReadToEnd();
                                           xmlResult = WebUtility.HtmlDecode(xmlResult);
                                           result.SetResult(xmlResult);
                                       }
                                   }
                                   catch (Exception e)
                                   {
                                       result.SetException(e);
                                   }
                               }, TaskContinuationOptions.AttachedToParent);
                       }
                       catch (Exception e)
                       {
                           result.SetException(e);
                       }

                   }, TaskContinuationOptions.AttachedToParent);
            }

            return result.Task;
#endif
        }
        */

        internal static string TidyText(string text)
        {
            try
            {
                text = WebUtility.HtmlDecode(text);
                text = text.Replace("&nbsp", " ");
                text = text.Trim();
                text = text.Replace("-", " - ").Replace("/", " / ").Replace("  ", "");
                if (!string.IsNullOrEmpty(text.Replace("-", "").Replace("/", "").Trim()))
                {
                    string[] split = text.Split(' ');
                    text = "";
                    for (int i = 0; i < split.Length; i++)
                    {
                        char[] chars = split[i].ToCharArray();
                        split[i] = chars[0].GetUpper().ToString();
                        for (int j = 1; j < chars.Length; j++)
                        {
                            split[i] += chars[j].GetLower().ToString();
                        }
                        text += split[i] + " ";
                    }
                    text = text.Replace("VE", "ve");
                }
                return text.Trim();
            }
            catch (Exception)
            {
                return text.Trim();
            }
        }

        private static char GetUpper(this char chr)
        {
            if (Char.IsUpper(chr))
                return chr;

            switch (chr)
            {
                case 'ı':
                    return 'I';
                case 'i':
                    return 'İ';
                default:
                    return chr.ToString().ToUpper().ToCharArray()[0];
            }
        }

        private static char GetLower(this char chr)
        {
            if (Char.IsLower(chr))
                return chr;

            switch (chr)
            {
                case 'I':
                    return 'ı';
                case 'İ':
                    return 'i';
                default:
                    return chr.ToString().ToLower().ToCharArray()[0];
            }
        }
    }
}
