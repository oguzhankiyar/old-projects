using Newtonsoft.Json.Linq;
using OK.Transport.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace OK.Transport.Web.Helpers
{
    public static class Extensions
    {
        public static bool IsAvailable(this Transporter t, string place_id, bool isDeparture, string cityName)
        {
            string from = t.Place.GoogleId;
            string url = "https://maps.googleapis.com/maps/api/directions/json?origin=place_id:" + from + "&destination=place_id:" + place_id + "&language=tr&key=" + "AIzaSyA8ZOOxJmhvlmMAZE4sFTZIjmzKsSFa0Mo";
            var request = HttpWebRequest.Create(url) as HttpWebRequest;
            var response = request.GetResponse();
            var stream = response.GetResponseStream();
            var reader = new StreamReader(stream);
            string json = reader.ReadToEnd();
            var jobj = JObject.Parse(json);
            int dist = Convert.ToInt32(((((jobj["routes"] as JArray)[0])["legs"] as JArray)[0])["distance"]["value"]) / 1000;

            if (!t.IsActivatedUpstate && t.Place.City != cityName)
                return false;

            if (isDeparture)
                return dist <= t.DepartureDistance;
            return dist <= t.ArrivalDistance;
        }
    }
}