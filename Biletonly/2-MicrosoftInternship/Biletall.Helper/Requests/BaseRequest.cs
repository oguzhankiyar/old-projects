using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Biletall.Helper.Enums;
using Biletall.Helper.Parsings;
using Newtonsoft.Json.Linq;

namespace Biletall.Helper.Requests
{
    public sealed class BaseRequest<T>
    {
        public bool IsCompleted { get; private set; }
        public Action<BaseResponse<T>> OnCompleted = null;
        public bool IsCancelable { get; internal set; }

        private bool _isCanceled;

        internal BaseResponse<T> Response;
        internal Action<string> OnPopulated = null;

        private BaseRequest() { }

        internal static BaseRequest<T> GetInstance()
        {
            return new BaseRequest<T>() { IsCompleted = true, IsCancelable = true };
        }

        byte[] _byteArray;
        internal void GetResult(string xml, int type = 0)
        {
            IsCompleted = false;

            try
            {
                GetResultWithPost(xml, type);
            }
            catch (Exception exp)
            {
                Global.Invoke(() =>
                {
                    IsCompleted = true;
                    if (OnCompleted != null)
                    {
                        OnCompleted(new BaseResponse<T>() { Status = ResponseStatus.Undefined });
                        Global.OnRequestFailed(exp);
                    }
                });
            }
        }
        
        private void GetResultWithPost(string xml, int type)
        {
            try
            {
                Global.OnRequestParsed(xml);
                var obj = new JObject();
                obj.Add("version", "1.0");
                obj.Add("username", Global.ApiUsername);
                obj.Add("password", Global.ApiPassword);
                var jsonRequest = new JObject();
                jsonRequest.Add("type", type);
                jsonRequest.Add("xml", WebUtility.HtmlEncode(xml));
                obj.Add("request", jsonRequest);

                _byteArray = Encoding.UTF8.GetBytes(obj.ToString());

                var request = HttpWebRequest.Create(ApiHelper.ApiUrl);
                request.ContentType = "application/json; charset=UTF-8";

                request.Method = "POST";
                request.BeginGetRequestStream(SendDataCallBack, request);
            }
            catch (Exception exp)
            {
                OnError(exp);
            }
        }

        private void SendDataCallBack(IAsyncResult result)
        {
            try
            {
                var request = result.AsyncState as HttpWebRequest;
                if (_isCanceled)
                {
                    _isCanceled = false;
                    request.Abort();
                    return;
                }
                if (request != null)
                {
                    using (var stream = request.EndGetRequestStream(result))
                    {
                        stream.Write(_byteArray, 0, _byteArray.Length);
                        stream.Close();
                    }
                    request.BeginGetResponse(GetResponseCallBack, request);
                }
            }
            catch (Exception exp)
            {
                OnError(exp);
            }
        }

        private void GetResponseCallBack(IAsyncResult result)
        {
            try
            {
                var request = result.AsyncState as HttpWebRequest;
                if (_isCanceled)
                {
                    _isCanceled = false;
                    request.Abort();
                    return;
                }

                if (request != null)
                {
                    var response = request.EndGetResponse(result);
                    var reader = new StreamReader(response.GetResponseStream());
                    var xmlResult = reader.ReadToEnd();

                    Global.Invoke(() =>
                    {
                        try
                        {
                            var obj = JObject.Parse(xmlResult);
                            if (obj["status"].ToString() == "True")
                            {
                                var jsonResponse = obj["response"] as JObject;
                                xmlResult = WebUtility.HtmlDecode(jsonResponse["xml"].ToString());
                                Populate(xmlResult);
                            }
                            else
                            {
                                OnError(null);
                            }
                        }
                        catch (Exception exp)
                        {
                            OnError(exp);
                        }
                    });
                }
            }
            catch (Exception exp)
            {
                OnError(exp);
            }
        }

        private void Populate(string xmlResult)
        {
            if (_isCanceled)
            {
                _isCanceled = false;
                return;
            }

            Global.OnRequestReturned(xmlResult);
            try
            {
                if (OnPopulated != null)
                    OnPopulated(xmlResult);

                Global.OnResultParsed(Response);
                IsCompleted = true;
                if (OnCompleted != null)
                    OnCompleted(Response);
                Global.OnRequestCompleted();
            }
            catch (Exception)
            {
                Global.Invoke(() =>
                {
                    IsCompleted = true;
                    if (OnCompleted != null)
                    {
                        Response = new BaseResponse<T>();
                        Response.Status = ResponseStatus.Undefined;
                        if (XElement.Parse(xmlResult).Element(XName.Get("Hata")) != null)
                            Response.Message = XElement.Parse(xmlResult).Element(XName.Get("Hata")).Value;

                        Global.OnResultParsed(Response);
                        OnCompleted(Response);
                        Global.OnRequestCompleted();
                    }
                });
            }

        }

        public void Cancel()
        {
            if (IsCancelable && !IsCompleted)
            {
                Global.OnRequestCanceled();
                IsCompleted = true;
                _isCanceled = true;
            }
        }

        private void OnError(Exception exp)
        {
            Global.Invoke(() =>
            {
                IsCompleted = true;
                if (OnCompleted != null)
                {
                    OnCompleted(new BaseResponse<T>() { Status = ResponseStatus.Undefined });
                    Global.OnRequestFailed(exp);
                }
            });
        }
    }
}