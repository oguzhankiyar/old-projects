using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OK.Mobisis.Entities.ApiEntities;
using OK.Mobisis.Api.Helper.Parsings;
using OK.Mobisis.Api.Helper.Enums;

namespace OK.Mobisis.Api.Helper.Requests
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
        internal void GetResult(RequestObject requestObj)
        {
            IsCompleted = false;

            try
            {
                GetResultWithPost(requestObj);
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

        private void GetResultWithPost(RequestObject requestObj)
        {
            try
            {
                var obj = new ApiObject();
                obj.Version = Global.ApiVersion;
                obj.Username = Global.ApiUsername;
                obj.Password = Global.ApiPassword;
                obj.Request = requestObj;

                string requestJson = JsonConvert.SerializeObject(obj);
                Global.OnRequestParsed(requestJson);
                _byteArray = Encoding.UTF8.GetBytes(requestJson);

                var request = HttpWebRequest.Create(Global.ApiUrl);
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
                        stream.Flush();
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
                    var jsonResult = reader.ReadToEnd();

                    Global.Invoke(() =>
                    {
                        try
                        {
                            Populate(jsonResult);
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
