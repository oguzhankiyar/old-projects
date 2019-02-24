using OK.CargoTracking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OK.CargoTracking.Web.Models
{
    public static class RequestHelper
    {
        public static bool VerifyRequest(BaseRequest request, string page)
        {
            if (request == null)
                return false;

            if (page == "device")
                return true;

            if (!Repository.IsDeviceExist(request.Device.Id))
                return false;

            if (VerifyToken(request.Token, request.Device.Id))
                return true;

            return false;
        }

        public static bool VerifyToken(string token, string deviceId)
        {
            var date = DateTime.UtcNow;
            string dateString = CryptoHelper.DecryptToken(token, deviceId);
            var tokenDate = Convert.ToDateTime(dateString);
            if (tokenDate <= date.AddMinutes(5))
                return true;
            return false;
        }
    }
}