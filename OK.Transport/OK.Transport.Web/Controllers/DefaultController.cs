using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OK.Transport.Web.Helpers;
using OK.Transport.Web.Models;
using OK.Transport.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OK.Transport.Web.Controllers
{
    public class DefaultController : Controller
    {
        const string API_KEY = "AIzaSyA8ZOOxJmhvlmMAZE4sFTZIjmzKsSFa0Mo";
        const string SECOND_API_KEY = "AIzaSyBmoh_ZJg_zNJrP9k1uv09ffIwIrNSCjHk";
        TransportDbEntities db = new TransportDbEntities();

        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(SearchViewModel model)
        {
            return RedirectToAction("Transporters", new { from = model.From, to = model.To });
        }

        public string AutoComplete(string query)
        {
            string url = "https://maps.googleapis.com/maps/api/place/autocomplete/json?input=" + query + "&language=tr&components=country:tr&types=geocode&key=" + API_KEY;
            var request = HttpWebRequest.Create(url) as HttpWebRequest;
            var response = request.GetResponse();
            var stream = response.GetResponseStream();
            var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public string PlaceDetails(string place_id)
        {
            //string url = "https://maps.googleapis.com/maps/api/place/details/json?placeid=" + place_id + "&key=AIzaSyBmoh_ZJg_zNJrP9k1uv09ffIwIrNSCjHk";
            string url = "https://maps.googleapis.com/maps/api/geocode/json?place_id=" + place_id + "&language=tr&key=" + API_KEY;
            var request = HttpWebRequest.Create(url) as HttpWebRequest;
            var response = request.GetResponse();
            var stream = response.GetResponseStream();
            var reader = new StreamReader(stream);
            string json = reader.ReadToEnd();

            return json;
        }

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

        public ActionResult Transporters(string from, string to)
        {
            string fromPlaceJson = PlaceDetails(from);
            string fromCityName = ((JObject.Parse(fromPlaceJson)["results"] as JArray)[0]["address_components"] as JArray).SingleOrDefault(x => x.ToString().Contains("administrative_area_level_1"))["short_name"].ToString().Split(' ')[0];
            string fromFullName = ((JObject.Parse(fromPlaceJson)["results"] as JArray)[0])["formatted_address"].ToString();
            string fromLat = (JObject.Parse(fromPlaceJson)["results"] as JArray)[0]["geometry"]["location"]["lat"].ToString();
            string fromLng = (JObject.Parse(fromPlaceJson)["results"] as JArray)[0]["geometry"]["location"]["lng"].ToString();

            string toPlaceJson = PlaceDetails(to);
            string toCityName = ((JObject.Parse(toPlaceJson)["results"] as JArray)[0]["address_components"] as JArray).SingleOrDefault(x => x.ToString().Contains("administrative_area_level_1"))["short_name"].ToString().Split(' ')[0];
            string toFullName = ((JObject.Parse(toPlaceJson)["results"] as JArray)[0])["formatted_address"].ToString();
            string toLat = (JObject.Parse(toPlaceJson)["results"] as JArray)[0]["geometry"]["location"]["lat"].ToString();
            string toLng = (JObject.Parse(toPlaceJson)["results"] as JArray)[0]["geometry"]["location"]["lng"].ToString();


            int dist = GetDistance(from, to);

            var transporters = db.Transporters.ToList().Where(x =>
                                                            x.IsAvailable(from, true, fromCityName) &&
                                                            x.IsAvailable(to, false, toCityName) &&
                                                            x.MinimumDistance <= dist &&
                                                            x.MaximumDistance >= dist).ToList();

            var result = new SearchResultViewModel();
            result.Distance = dist;
            result.From = new PlaceViewModel() { Id = from, Name = fromFullName, Lat = fromLat, Lng = fromLng };
            result.To = new PlaceViewModel() { Id = to, Name = toFullName, Lat = toLat, Lng = toLng };

            foreach (var item in transporters)
            {
                var transporter = new TransporterViewModel();
                transporter.Id = item.Id;
                transporter.Plate = item.Plate;
                transporter.PassengerCapacity = item.PassengerCount;
                transporter.Price = dist * item.PricePerKilometer;
                result.Transporters.Add(transporter);
            }
            return View(result);
        }

        public ActionResult TransporterDetails(int id, string from, string to, string from_name, string to_name, string from_lat, string from_lng, string to_lat, string to_lng)
        {
            ViewBag.From = from;
            ViewBag.To = to;
            ViewBag.FromName = from_name;
            ViewBag.ToName = to_name;
            ViewBag.FromLat = from_lat;
            ViewBag.FromLng = from_lng;
            ViewBag.ToLat = to_lat;
            ViewBag.ToLng = to_lng;

            var item = db.Transporters.Find(id);
            var transporter = new TransporterViewModel();
            transporter.Plate = item.Plate;
            transporter.Brand = item.Brand;
            transporter.Model = item.Model;
            transporter.WorkingHours = item.WorkingHours;
            transporter.BaggageCapacity = item.TotalBaggageCapacity;
            transporter.PassengerCapacity = item.PassengerCount;
            transporter.SeatSchema = item.SeatSchema;
            return PartialView(transporter);
        }

        [HttpPost]
        public ActionResult TransporterDetails()
        {
            var form = Request.Form;
            TempData["from"] = new PlaceViewModel() { Id = form["From"], Name = form["FromName"], Lat = form["FromLat"], Lng = form["FromLng"] };
            TempData["to"] = new PlaceViewModel() { Id = form["To"], Name = form["ToName"], Lat = form["ToLat"], Lng = form["ToLng"] };
            return RedirectToAction("ReserveTransporter", new { id = form["Id"] });
        }

        public ActionResult ReserveTransporter(int id)
        {
            ViewBag.fromId = (TempData["from"] as PlaceViewModel).Id;
            ViewBag.toId = (TempData["to"] as PlaceViewModel).Id;
            ViewBag.fromName = (TempData["from"] as PlaceViewModel).Name;
            ViewBag.toName = (TempData["to"] as PlaceViewModel).Name;
            ViewBag.FromLat = (TempData["from"] as PlaceViewModel).Lat;
            ViewBag.FromLng = (TempData["from"] as PlaceViewModel).Lng;
            ViewBag.ToLat = (TempData["to"] as PlaceViewModel).Lat;
            ViewBag.ToLng = (TempData["to"] as PlaceViewModel).Lng;

            var item = db.Transporters.Find(id);
            var transporter = new TransporterViewModel();
            transporter.Plate = item.Plate;
            transporter.Brand = item.Brand;
            transporter.Model = item.Model;
            transporter.WorkingHours = item.WorkingHours;
            transporter.BaggageCapacity = item.TotalBaggageCapacity;
            transporter.PassengerCapacity = item.PassengerCount;
            transporter.SeatSchema = item.SeatSchema;
            return View(transporter);
        }

        [HttpPost]
        public ActionResult ReserveTransporter()
        {
            var form = Request.Form;
            var model = new TransporterReservationViewModel();
            model.Id = Convert.ToInt32(form["transporterId"]);
            model.From = new PlaceViewModel() { Id = form["fromId"], Name = WebUtility.HtmlDecode(WebUtility.HtmlDecode(form["fromName"])), Lat = form["fromLat"], Lng = form["fromLng"] };
            model.To = new PlaceViewModel() { Id = form["toId"], Name = WebUtility.HtmlDecode(WebUtility.HtmlDecode(form["toName"])), Lat = form["toLat"], Lng = form["toLng"] };
            string json = form["Stops"];
            if (!string.IsNullOrEmpty(json))
            {
                var jarr = JArray.Parse(json);
                for (int i = 0; i < jarr.Count; i++)
                {
                    var jobj = jarr[i] as JObject;
                    var stop = new StopViewModel()
                    {
                        Sort = Convert.ToInt32(jobj["stopSort"].ToString()),
                        Place = new PlaceViewModel() { Id = jobj["placeId"].ToString(), Name = jobj["placeName"].ToString() },
                        PassengerCountGetOn = Convert.ToInt32(jobj["passengerCountGetOn"].ToString()),
                        PassengerCountGetOff = Convert.ToInt32(jobj["passengerCountGetOff"].ToString())
                    };
                    model.Stops.Add(stop);
                }
            }
            TempData["model"] = model;

            return RedirectToAction("PurchaseBill");
        }

        public ActionResult PurchaseBill()
        {
            var model = TempData["model"] as TransporterReservationViewModel;
            ViewBag.ModelJson = JsonConvert.SerializeObject(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult PurchaseComplete()
        {
            var form = Request.Form;
            string json = form["ModelJson"];
            var model = JsonConvert.DeserializeObject<TransporterReservationViewModel>(json);


            var depPlace = new Place() { GoogleId = model.From.Id, Name = model.From.Name, City = model.From.Name, Country = model.From.Name, Latitude = 35, Longitude = 35 };
            var arrPlace = new Place() { GoogleId = model.To.Id, Name = model.To.Name, City = model.To.Name, Country = model.To.Name, Latitude = 35, Longitude = 35 };
            db.Places.Add(depPlace);
            db.Places.Add(arrPlace);
            db.SaveChanges();


            var journey = new Journey();
            journey.DeparturePlaceId = depPlace.Id;
            journey.ArrivalPlaceId = arrPlace.Id;
            journey.DepartureDate = DateTime.Now;
            journey.ArrivalDate = DateTime.Now;
            journey.Title = model.From.Name + " mekanından " + model.To.Name + " mekanına yolculuk";
            journey.Transporter = db.Transporters.Find(model.Id);
            db.Journeys.Add(journey);
            db.SaveChanges();

            journey.JourneyStops = new List<JourneyStop>();
            foreach (var item in model.Stops)
            {
                var stop = new JourneyStop();
                stop.EstimatedDate = DateTime.Now;
                stop.PassengerCountGetOn = item.PassengerCountGetOn;
                stop.PassengerCountGetOff = item.PassengerCountGetOff;
                stop.Place = new Place() { GoogleId = item.Place.Id, Name = item.Place.Name, City = item.Place.Name, Country = item.Place.Name, Latitude = 35, Longitude = 35 };
                stop.Sort = item.Sort;
                stop.JourneyId = journey.Id;
                db.JourneyStops.Add(stop);
            }
            db.SaveChanges();

            var payment = new Payment();
            payment.Provision = "provision 1";
            payment.Type = 1;
            db.Payments.Add(payment);
            db.SaveChanges();

            var reservation = new Reservation();
            reservation.Code = CreateCode();
            reservation.Payment = payment;
            reservation.JourneyId = journey.Id;
            db.Reservations.Add(reservation);
            db.SaveChanges();

            return RedirectToAction("ReservationDetails", new { code = reservation.Code });
        }

        public string CreateCode()
        {
            string code;
            do
            {
                code = Guid.NewGuid().ToString().Split('-')[0].ToUpper(CultureInfo.GetCultureInfo("en-US"));
            } while (db.Reservations.Any(x => x.Code == code));
            return code;
        }

        public ActionResult ReservationSearch()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReservationSearchPost()
        {
            return RedirectToAction("ReservationDetails", new { code = Request.Form["code"] });
        }

        public ActionResult ReservationDetails(string code)
        {
            var reservation = db.Reservations.SingleOrDefault(x => x.Code == code);
            return View(reservation);
        }

        public ActionResult PopularJourneys()
        {
            var popularFrom = db.Places.Where(x => x.DepartureJourneys.Any()).OrderByDescending(x => x.DepartureJourneys.Count()).Take(5);
            var popularTo = db.Places.Where(x => x.DepartureJourneys.Any()).OrderByDescending(x => x.DepartureJourneys.Count()).Take(5);

            var list = new List<Journey>();

            foreach (var item in popularFrom)
            {
                foreach (var journey in item.DepartureJourneys)
                {
                    if (list.SingleOrDefault(x => x.DeparturePlace.GoogleId == journey.DeparturePlace.GoogleId && x.ArrivalPlace.GoogleId == journey.ArrivalPlace.GoogleId) == null)
                        list.Add(journey);
                }
            }

            foreach (var item in popularFrom)
            {
                foreach (var journey in item.ArrivalJourneys)
                {
                    if (list.SingleOrDefault(x => x.DeparturePlace.GoogleId == journey.DeparturePlace.GoogleId && x.ArrivalPlace.GoogleId == journey.ArrivalPlace.GoogleId) == null)
                        list.Add(journey);
                }
            }

            return View(list);
        }

        public ActionResult Campaigns()
        {
            return View();
        }

        public ActionResult RouteMap()
        {
            return View();
        }
    }
}