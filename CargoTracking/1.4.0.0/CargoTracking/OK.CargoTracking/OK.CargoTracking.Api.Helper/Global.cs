#define TEST
using OK.CargoTracking.Model;
using System;

namespace OK.CargoTracking.Api.Helper
{
    public static class Global
    {
#if TESTs
        public const string API_URL = "http://localhost:51012/api/v1";
#else
        public const string API_URL = "http://kargotakip.ogzkyr.net/api/v1";
#endif

        public static Device Device { get; set; }
        public static string Token { get; set; }
    }
}
