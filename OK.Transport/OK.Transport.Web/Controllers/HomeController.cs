using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OK.Transport.Web.Helpers;
using OK.Transport.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OK.Transport.Web.Controllers
{
    public static class Exts
    {
        /*
        public static double DegreeBearing(double lat1, double lon1, double lat2, double lon2)
        {
            //const double R = 6371; //earth’s radius (mean radius = 6,371km)
            var dLon = ToRad(lon2 - lon1);
            var dPhi = Math.Log(
                Math.Tan(ToRad(lat2) / 2 + Math.PI / 4) / Math.Tan(ToRad(lat1) / 2 + Math.PI / 4));
            if (Math.Abs(dLon) > Math.PI)
                dLon = dLon > 0 ? -(2 * Math.PI - dLon) : (2 * Math.PI + dLon);
            return ToBearing(Math.Atan2(dLon, dPhi));
        }

        public static double ToRad(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        public static double ToDegrees(double radians)
        {
            return radians * 180 / Math.PI;
        }

        public static double ToBearing(double radians)
        {
            // convert radians to degrees (as bearing: 0...360)
            return (ToDegrees(radians) + 360) % 360;
        }*/
    }
    public class HomeController : Controller
    {
        const string API_KEY = "AIzaSyA8ZOOxJmhvlmMAZE4sFTZIjmzKsSFa0Mo";
        TransportDbEntities db = new TransportDbEntities();

        public ActionResult Index()
        {
            return View();
        }

        public string SearchPlace(string query)
        {
            string url = "https://maps.googleapis.com/maps/api/place/autocomplete/json?input=" + query + "&language=tr&components=country:tr&types=geocode&key=AIzaSyBmoh_ZJg_zNJrP9k1uv09ffIwIrNSCjHk";
            var request = HttpWebRequest.Create(url) as HttpWebRequest;
            var response = request.GetResponse();
            var stream = response.GetResponseStream();
            var reader = new StreamReader(stream);
            string json = reader.ReadToEnd();
            
            return json;
        }

        public string PlaceDetails(string place_id)
        {
            //string url = "https://maps.googleapis.com/maps/api/place/details/json?placeid=" + place_id + "&key=AIzaSyBmoh_ZJg_zNJrP9k1uv09ffIwIrNSCjHk";
            string url = "https://maps.googleapis.com/maps/api/geocode/json?place_id=" + place_id + "&key=" + API_KEY;
            var request = HttpWebRequest.Create(url) as HttpWebRequest;
            var response = request.GetResponse();
            var stream = response.GetResponseStream();
            var reader = new StreamReader(stream);
            string json = reader.ReadToEnd();

            return json;
        }

        /*public string GetDirections(string from, string to)
        {
            string url = "https://maps.googleapis.com/maps/api/directions/json?origin=" + from + "&destination=" + to + "&language=tr&key=" + API_KEY;
            var request = HttpWebRequest.Create(url) as HttpWebRequest;
            var response = request.GetResponse();
            var stream = response.GetResponseStream();
            var reader = new StreamReader(stream);
            string json = reader.ReadToEnd();

            return json;
        }*/

        public int GetDistance(string from, string to)
        {
            string url = "https://maps.googleapis.com/maps/api/directions/json?origin=place_id:" + from + "&destination=place_id:" + to + "&language=tr&key=" + API_KEY;
            var request = HttpWebRequest.Create(url) as HttpWebRequest;
            var response = request.GetResponse();
            var stream = response.GetResponseStream();
            var reader = new StreamReader(stream);
            string json = reader.ReadToEnd();
            var jobj = JObject.Parse(json);
            int dist = Convert.ToInt32(((((jobj["routes"] as JArray)[0])["legs"] as JArray)[0])["distance"]["value"]) / 1000;
            return dist;
        }

        public string GetTransporters(string from, string to)
        {
            string fromPlaceJson = PlaceDetails(from);
            string fromCityName = ((JObject.Parse(fromPlaceJson)["results"] as JArray)[0]["address_components"] as JArray).SingleOrDefault(x => x.ToString().Contains("administrative_area_level_1"))["short_name"].ToString().Split(' ')[0];

            string toPlaceJson = PlaceDetails(to);
            string toCityName = ((JObject.Parse(toPlaceJson)["results"] as JArray)[0]["address_components"] as JArray).SingleOrDefault(x => x.ToString().Contains("administrative_area_level_1"))["short_name"].ToString().Split(' ')[0];

            int dist = GetDistance(from, to);

            var araclar = db.Transporters.ToList().Where(x => 
                                                            x.IsAvailable(from, true, fromCityName) &&
                                                            x.IsAvailable(to, false, toCityName) &&
                                                            x.MinimumDistance <= dist &&
                                                            x.MaximumDistance >= dist).ToList();

            string json = "";
            foreach (var item in araclar)
            {
                json += "{ \"id\": \"" + item.Id + "\", \"distance\": \"" + dist + "\", \"plate\": \"" + item.Plate + "\", \"capacity\": \"" + item.PassengerCount + "\", \"price\": \"" + (dist * item.PricePerKilometer) + "\" },";
            }
            json = "[" + json.TrimEnd(',') + "]";
            return json;
        }

        public string GetTransporterDetails(int id)
        {
            var db = new TransportDbEntities();
            var arac = db.Transporters.Find(id);
            
            return "{ \"baggageCapacity\": \"" + arac.TotalBaggageCapacity + "\", \"plate\": \"" + arac.Plate + "\", \"brand\": \"" + arac.Brand + "\", \"model\": \"" + arac.Model + "\", \"year\": \"" + arac.Year + "\", \"capacity\": \"" + arac.PassengerCount + "\", \"workingHours\": \"" + arac.WorkingHours + "\", \"seatSchema\": \"" + arac.SeatSchema + "\" }";
        }
    }
}