using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using OK.Mobisis.Api.Models;
using OK.Mobisis.Api.Version10;
using OK.Mobisis.Entities.ApiEntities;
using OK.Mobisis.Entities.Enums;
using OK.Mobisis.Entities.ModelEntities;

namespace OK.Mobisis.Api.Controllers
{
    public class ServiceApiController : ApiController
    {
#if DEBUG
        public object Get()
        {
            var student = Helper.GetStudent(new StudentObject() { Id = "1030515866", Password = "211958" });
            return student;
        }
#endif

        public object Post(ApiObject obj)
        {
            try
            {
                if (obj.Version == "1.0")
                    return OK.Mobisis.Api.Version10.Global.Execute(obj);
                else if (obj.Version == "1.1")
                    return OK.Mobisis.Api.Version11.Global.Execute(obj);
                else
                    return new ResultObject<String>() { Status = false, Message = "Invalid version" };
            }
            catch (Exception ex)
            {
                return new ResultObject<String>() { Status = false, Message = "Giriş Başarısız" };
            }
        }
    }
}
