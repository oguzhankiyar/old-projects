using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace OK.CargoTracking.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Trackings",
                routeTemplate: "api/v1/getTracking",
                defaults: new { controller = "TrackingApi", action = "GetSearch" }
            );

            config.Routes.MapHttpRoute(
                name: "Tracking",
                routeTemplate: "api/v1/tracking",
                defaults: new { controller = "TrackingApi", action = "PostSearch" }
            );

            config.Routes.MapHttpRoute(
                name: "Messages",
                routeTemplate: "api/v1/messages",
                defaults: new { controller = "MessageApi", action = "PostCheck" }
            );

            config.Routes.MapHttpRoute(
                name: "Device",
                routeTemplate: "api/v1/device",
                defaults: new { controller = "DeviceApi", action = "PostRegister" }
            );

            config.Routes.MapHttpRoute(
                name: "Factory",
                routeTemplate: "api/v1/factories",
                defaults: new { controller = "FactoryApi", action = "PostFactories" }
            );
        }
    }
}
