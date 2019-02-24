using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Biletall.Helper.Entities;
using Biletall.Helper.Enums;

namespace Biletall.Helper.Parsings
{
    internal class JourneyParsings
    {
        #region BusJourney
        public static string GetJourneys(TicketSearch search)
        {
            var sb = new StringBuilder();
            sb.Append("<Sefer>");
            sb.Append("<FirmaNo>" + search.Factory.Id + "</FirmaNo>");
            sb.Append("<KalkisAdi>" + search.From.Name + "</KalkisAdi>");
            sb.Append("<VarisAdi>" + search.To.Name + "</VarisAdi>");
            sb.Append("<Tarih>" + search.DepartureDate.ToString("yyyy-MM-dd") + "</Tarih>");
            sb.Append("<AraNoktaGelsin>" + 1 + "</AraNoktaGelsin>");
            sb.Append("<IslemTipi>" + (search.ActionType == ActionType.Buying ? 0 : 1) + "</IslemTipi>"); //
            sb.Append("<YolcuSayisi>" + search.Passengers.Count() + "</YolcuSayisi>");
            sb.Append("</Sefer>");
            return sb.ToString();
        }

        public static BaseResponse<List<Journey>> ParseJourneys(string xml)
        {
            var response = new BaseResponse<List<Journey>>();

            var journeys = new List<Journey>();
            var doc = XDocument.Parse(xml);

            XElement root;
            IEnumerable<XElement> elements;
            if (xml.Contains("<Hata>Aradığınız Kriterlere Uygun Sefer Bulunamamıştır! Ancak alternatif seferler gönderilmiştir!</Hata>"))
            {
                response.Status = ResponseStatus.AlternativeJourneys;
                root = doc.Element(XName.Get("IslemSonuc")).Element(XName.Get("AlternatifSeferler"));
                elements = root.Elements().Where(x => x.Name.LocalName == "Sefer");
            }
            else if (xml.Contains("<Sonuc>false</Sonuc>"))
            {
                response.Status = ResponseStatus.Undefined;
                return response;
            }
            else
            {
                root = doc.Element(XName.Get("NewDataSet"));
                elements = root.Elements().Where(x => x.Name.LocalName == "Table");
            }

            Global.BusProperties = BusParsings.ParseAllProperties(root.Elements().Where(x => x.Name.LocalName == "OTipOzellik")).Result;
            Global.Factories = new List<Factory>();

            foreach (var item in elements)
            {
                var journey = new Journey();
                journey.Id = item.Element(XName.Get("ID")).Value.ToString();

                var factory = new Factory()
                {
                    Id = Convert.ToInt32(item.Element(XName.Get("FirmaNo")).Value.ToString()),
                    Name = item.Element(XName.Get("FirmaAdi")).Value.ToString()
                };
                if (Global.Factories.Where(x => x.Id == factory.Id).Count() == 0)
                    Global.Factories.Add(factory);

                var segment = new Segment();

                journey.Factory = segment.Factory = factory;
                segment.Time = Functions.GetTime(item.Element(XName.Get("Vakit")).Value.ToString());
                journey.DepartureDate = segment.DepartureDate = Convert.ToDateTime(item.Element(XName.Get("YerelInternetSaat")).Value.ToString());
                segment.Duration = Convert.ToDateTime(item.Element(XName.Get("SeyahatSuresi")).Value.ToString());
                segment.Hour = item.Element(XName.Get("Saat")).Value.ToString();
                segment.ArrivalDate = journey.DepartureDate.AddHours(segment.Duration.Hour).AddMinutes(segment.Duration.Minute);
                segment.LineNumber = Convert.ToInt32(item.Element(XName.Get("HatNo")).Value.ToString());

                journey.From = segment.From = new Station(item.Element(XName.Get("KalkisYeri")).Value.ToString());
                journey.To = segment.To = new Station(item.Element(XName.Get("VarisYeri")).Value.ToString());
                var bus = new Bus();
                bus.Type = new BusType();
                if (item.Element(XName.Get("OtobusTipi")) != null)
                    bus.Type.Name = item.Element(XName.Get("OtobusTipi")).Value.ToString();
                if (item.Element(XName.Get("OTipAciklamasi")) != null)
                    bus.Type.Desription = item.Element(XName.Get("OTipAciklamasi")).Value.ToString();
                if (item.Element(XName.Get("OtobusKoltukYerlesimTipi")) != null)
                    bus.Type.Column = item.Element(XName.Get("OtobusKoltukYerlesimTipi")).Value.ToString();
                if (item.Element(XName.Get("OtobusPlaka")) != null)
                    bus.Plate = item.Element(XName.Get("OtobusPlaka")).Value.ToString();
                if (item.Element(XName.Get("OtobusTelefonu")) != null)
                    bus.PhoneNumber = item.Element(XName.Get("OtobusTelefonu")).Value.ToString();
                bus.Properties = BusParsings.ParseProperties(item.Element(XName.Get("OTipOzellik")).Value.ToString()).Result;
                segment.Bus = bus;
                segment.Type = item.Element(XName.Get("SeferTipiAciklamasi")).Value.ToString() == "MOLALI" ? SegmentType.Stop : SegmentType.Nonstop;
                segment.Id = item.Element(XName.Get("SeferTakipNo")).Value.ToString();
                segment.EmptySeatCount = Convert.ToInt32(item.Element(XName.Get("SeferBosKoltukSayisi")).Value.ToString());
                segment.Route = item.Element(XName.Get("Guzergah")).Value.ToString().Replace("->", "»").Split('»').ToList();
                journey.MaxReservationDate = Convert.ToDateTime(item.Element(XName.Get("MaximumRezerveTarihiSaati")).Value.ToString());
                journey.IsFull = item.Element(XName.Get("DoluSeferMi")).Value.ToString() == "1";
                journey.Price = segment.UnitPrice = new Price() { TotalPrice = Convert.ToDouble(item.Element(XName.Get("BiletFiyatiInternet")).Value.ToString()) };

                journey.Segments.Add(segment);
                journeys.Add(journey);
            }

            response.Result = journeys;
            return response;
        }
        #endregion

        #region AirplaneJourney
        private static bool isRoundtrip = false;
        private static bool isInternational = false;

        public static string GetFlights(TicketSearch search)
        {            
            var sb = new StringBuilder();
            sb.Append("<Sefer>");
            sb.Append("<FirmaNo>" + (search.Type == TicketType.InternationalJourney ? 1100 : 1000) + "</FirmaNo>");
            sb.Append("<KalkisAdi>" + search.From.Code + "</KalkisAdi>");
            sb.Append("<VarisAdi>" + search.To.Code + "</VarisAdi>");
            sb.Append("<Tarih>" + search.DepartureDate.ToString("yyyy-MM-dd") + "</Tarih>");
            if (search.JourneyType == JourneyType.RoundTrip)
            {
                sb.Append("<DonusTarih>" + search.ReturnDate.ToString("yyyy-MM-dd") + "</DonusTarih>");
                isRoundtrip = true;
            }

            if (search.Type == TicketType.InternationalJourney)
                isInternational = true;
            else
                isInternational = false;

            sb.Append("<SeyahatTipi>" + (search.JourneyType == JourneyType.OneWay ? 1 : 2) + "</SeyahatTipi>");
            sb.Append("<IslemTipi>" + (search.ActionType == ActionType.Reservation ? 1 : 0) + "</IslemTipi>");
            sb.Append("<YetiskinSayi>" + search.Passengers.Where(x => x.Type == PassengerType.Adult).Count() + "</YetiskinSayi>");
            sb.Append("<CocukSayi>" + search.Passengers.Where(x => x.Type == PassengerType.Child).Count() + "</CocukSayi>");
            sb.Append("<BebekSayi>" + search.Passengers.Where(x => x.Type == PassengerType.Baby).Count() + "</BebekSayi>");
            sb.Append("<OgrenciSayi>" + search.Passengers.Where(x => x.Type == PassengerType.Student).Count() + "</OgrenciSayi>");
            sb.Append("<YasliSayi>" + search.Passengers.Where(x => x.Type == PassengerType.Infant).Count() + "</YasliSayi>");
            sb.Append("<AskerSayi>" + search.Passengers.Where(x => x.Type == PassengerType.Military).Count() + "</AskerSayi>");
            sb.Append("<GencSayi>" + search.Passengers.Where(x => x.Type == PassengerType.Teenager).Count() + "</GencSayi>");
            sb.Append("</Sefer>");
            return sb.ToString();
        }

        public static BaseResponse<List<Journey>> ParseFlights(string xml)
        {
            var response = new BaseResponse<List<Journey>>();

            Global.Factories = new List<Factory>();
            var root = XElement.Parse(xml);

            var departureJourneys = new List<Journey>();
            var options = root.Elements().Where(x => x.Name.LocalName == "Secenekler");
            var segments = root.Elements().Where(x => x.Name.LocalName == "Segmentler");
            if (options.Count() != 0 && segments.Count() != 0)
                if (isInternational)
                    departureJourneys = ParseInternationalOptions(options, segments, false);
                else
                    departureJourneys = ParseOptions(options, segments, false);

            var returnJourneys = new List<Journey>();
            var returnOptions = root.Elements().Where(x => x.Name.LocalName == "DonusSecenekler").Count() == 0 ? options : root.Elements().Where(x => x.Name.LocalName == "DonusSecenekler");
            var returnSegments = root.Elements().Where(x => x.Name.LocalName == "DonusSegmentler");
            if (returnOptions.Count() != 0 && returnSegments.Count() != 0)
                if (isInternational)
                    returnJourneys = ParseInternationalOptions(returnOptions, returnSegments, true);
                else
                    returnJourneys = ParseOptions(returnOptions, returnSegments, true);

            response.Result = departureJourneys.Concat(returnJourneys).ToList();
            return response;
        }

        private static List<Journey> ParseOptions(IEnumerable<XElement> options, IEnumerable<XElement> segments, bool isRound)
        {
            var journeys = new List<Journey>();
            foreach (var option in options.ToList())
            {
                var optionSegments = segments.Where(x => x.Element(XName.Get("SecenekID")).Value == option.Element(XName.Get("ID")).Value);
                var journey = new Journey();
                journey.Id = option.Element(XName.Get("A")).Value.ToString();
                journey.Factory = new Factory() { Id = Convert.ToInt32(option.Element(XName.Get("FirmaNo")).Value.ToString()) };

                if (Convert.ToDouble(option.Element(XName.Get("FiyatB")).Value.ToString()) != 0)
                    journey.Price = new Price() { TotalPrice = Convert.ToDouble(option.Element(XName.Get("FiyatB")).Value.ToString()) };
                if (Convert.ToDouble(option.Element(XName.Get("FiyatE")).Value.ToString()) != 0)
                    journey.Price = new Price() { TotalPrice = Convert.ToDouble(option.Element(XName.Get("FiyatE")).Value.ToString()) };
                if (Convert.ToDouble(option.Element(XName.Get("FiyatP")).Value.ToString()) != 0)
                    journey.Price = new Price() { TotalPrice = Convert.ToDouble(option.Element(XName.Get("FiyatP")).Value.ToString()) };

                if (journey.Price == null)
                    journey.IsFull = true;

                //journey.Time = Functions.GetTime(option.Element(XName.Get("Vakit")).Value.ToString());
                journey.Segments = new List<Segment>();
                int i = 0;
                foreach (var item in optionSegments.ToList())
                {
                    var segment = new Segment();
                    segment.Id = item.Element(XName.Get("SeferNo")).Value.ToString();
                    var factory = new Factory()
                    {
                        Id = journey.Factory.Id,
                        Code = item.Element(XName.Get("Firma")).Value.ToString(),
                        Name = item.Element(XName.Get("FirmaAd")).Value.ToString()
                    };
                    journey.Factory = segment.Factory = factory;
                    if (Global.Factories.Where(x => x.Code == factory.Code).Count() == 0)
                        Global.Factories.Add(factory);

                    string fromCode = item.Element(XName.Get("Kalkis")).Value.ToString();
                    string toCode = item.Element(XName.Get("Varis")).Value.ToString();
                    if (optionSegments.Count() > 1 || isRoundtrip)
                    {
                        if (isRound)
                            segment.Name = "Dönüş" + (optionSegments.Count() == 1 ? "" : " (" + ++i + ". uçuş)");
                        else
                            segment.Name = "Gidiş" + (optionSegments.Count() == 1 ? "" : " (" + ++i + ". uçuş)");
                    }
                    segment.From = Global.Stations.FirstOrDefault(x => x.Code == fromCode);
                    segment.To = Global.Stations.FirstOrDefault(x => x.Code == toCode);
                    segment.DepartureDate = Convert.ToDateTime(item.Element(XName.Get("KalkisTarih")).Value.ToString());
                    segment.ArrivalDate = Convert.ToDateTime(item.Element(XName.Get("VarisTarih")).Value.ToString());
                    journey.IsReturn = isRound;
                    segment.Classes.Add(new JourneyClass()
                    {
                        Name = "Promosyon",
                        ShortName = item.Element(XName.Get("SinifP")).Value.ToString(),
                        SeatCount = Convert.ToInt32(item.Element(XName.Get("KoltukP")).Value.ToString())
                    });
                    segment.Classes.Add(new JourneyClass()
                    {
                        Name = "Ekonomi",
                        ShortName = item.Element(XName.Get("SinifE")).Value.ToString(),
                        SeatCount = Convert.ToInt32(item.Element(XName.Get("KoltukE")).Value.ToString())
                    });
                    segment.Classes.Add(new JourneyClass()
                    {
                        Name = "Business",
                        ShortName = item.Element(XName.Get("SinifB")).Value.ToString(),
                        SeatCount = Convert.ToInt32(item.Element(XName.Get("KoltukB")).Value.ToString())
                    });
                    journey.Segments.Add(segment);
                }
                journey.DepartureDate = journey.Segments.First().DepartureDate;
                journey.From = journey.Segments.First().From;
                journey.To = journey.Segments.Last().To;
                journeys.Add(journey);
            }
            return journeys;
        }

        private static List<Journey> ParseInternationalOptions(IEnumerable<XElement> options, IEnumerable<XElement> segments, bool isRound)
        {
            var journeys = new List<Journey>();
            var flights = segments.GroupBy(x => Convert.ToInt32(x.Element(XName.Get("UcusID")).Value)).ToList();

            foreach (var flightSegments in flights)
            {
                var journey = new Journey();
                journey.Segments = new List<Segment>();

                var option = options.SingleOrDefault(x => x.Element(XName.Get("ID")).Value == flightSegments.First().Element(XName.Get("SecenekID")).Value);
                if (option != null)
                {
                    journey.Factory = new Factory() { Id = Convert.ToInt32(option.Element(XName.Get("FirmaNo")).Value.ToString()) };
                    journey.Price = new Price() { TotalPrice = Convert.ToDouble(option.Element(XName.Get("VFiyat")).Value.ToString()) };

                    int i = 0;
                    foreach (var item in flightSegments.ToList())
                    {
                        var segment = new Segment();
                        segment.Id = item.Element(XName.Get("SeferNo")).Value.ToString();
                        segment.Code = item.Element(XName.Get("SeferKod")).Value.ToString();
                        var factory = new Factory()
                        {
                            Id = journey.Factory.Id,
                            Code = item.Element(XName.Get("HavaYoluKod")).Value.ToString(),
                            Name = item.Element(XName.Get("HavaYolu")).Value.ToString()
                        };
                        journey.Factory = segment.Factory = factory;
                        if (Global.Factories.Where(x => x.Code == factory.Code).Count() == 0)
                            Global.Factories.Add(factory);

                        string fromCode = item.Element(XName.Get("KalkisKod")).Value.ToString();
                        string toCode = item.Element(XName.Get("VarisKod")).Value.ToString();
                        if (flightSegments.Count() > 1 || isRoundtrip)
                        {
                            if (isRound)
                                segment.Name = "Dönüş" + (flightSegments.Count() == 1 ? "" : " (" + ++i + ". uçuş)");
                            else
                                segment.Name = "Gidiş" + (flightSegments.Count() == 1 ? "" : " (" + ++i + ". uçuş)");
                        }
                        segment.From = Global.Stations.FirstOrDefault(x => x.Code == fromCode);
                        segment.To = Global.Stations.FirstOrDefault(x => x.Code == toCode);
                        segment.DepartureDate = Convert.ToDateTime(item.Element(XName.Get("KalkisTarih")).Value.ToString());
                        segment.ArrivalDate = Convert.ToDateTime(item.Element(XName.Get("VarisTarih")).Value.ToString());
                        journey.IsReturn = isRound;

                        //segment.Duration = Convert.ToDateTime("1900-01-01").AddMinutes(Convert.ToInt32(item.Element(XName.Get("UcusSuresi")).Value.ToString()));
                        segment.Time = Functions.GetTime(item.Element(XName.Get("Vakit")).Value.ToString());
                        switch (item.Element(XName.Get("SinifTip")).Value)
	                    {
                            case "Promosyon":
                                segment.Classes.Add(new JourneyClass()
                                {
                                    Name = "Promosyon",
                                    ShortName = item.Element(XName.Get("Sinif")).Value,
                                    SeatCount = 1
                                });
                                segment.Classes.Add(new JourneyClass() { Name = "Ekonomi" });
                                segment.Classes.Add(new JourneyClass() { Name = "Business" });
                                break;
                            case "Ekonomi":
                                segment.Classes.Add(new JourneyClass() { Name = "Promosyon" });
                                segment.Classes.Add(new JourneyClass()
                                {
                                    Name = "Ekonomi",
                                    ShortName = item.Element(XName.Get("Sinif")).Value,
                                    SeatCount = 1
                                });
                                segment.Classes.Add(new JourneyClass() { Name = "Business" });
                                break;
                            case "Business":
                                segment.Classes.Add(new JourneyClass() { Name = "Promosyon" });
                                segment.Classes.Add(new JourneyClass() { Name = "Ekonomi" });
                                segment.Classes.Add(new JourneyClass()
                                {
                                    Name = "Business",
                                    ShortName = item.Element(XName.Get("Sinif")).Value,
                                    SeatCount = 1
                                });
                                break;
                            default:
                                segment.Classes.Add(new JourneyClass() { Name = "Ekonomi" });
                                segment.Classes.Add(new JourneyClass() { Name = "Business" });
                                segment.Classes.Add(new JourneyClass()
                                {
                                    Name = item.Element(XName.Get("SinifTip")).Value,
                                    ShortName = item.Element(XName.Get("Sinif")).Value,
                                    SeatCount = 1
                                });
                                break;
	                    }
                        journey.Segments.Add(segment);
                    }
                    journey.DepartureDate = journey.Segments.First().DepartureDate;
                    journey.From = journey.Segments.First().From;
                    journey.To = journey.Segments.Last().To;
                    journeys.Add(journey);
                }
            }
            return journeys;
        }

        public static string GetPriceDetails(Ticket ticket)
        {
            var sb = new StringBuilder();
            sb.Append("<UcusFiyat>");
            sb.Append("<FirmaNo>" + (ticket.Type == TicketType.InternationalJourney ? 1100 : 1000) + "</FirmaNo>");

            int segmentIndex = 1;

            foreach (var journey in ticket.Journeys.ToList())
            {
                foreach (var segment in journey.Segments.ToList())
                {
                    sb.Append("<Segment" + segmentIndex + ">");
                    sb.Append("<Kalkis>" + segment.From.Code + "</Kalkis>");
                    sb.Append("<Varis>" + segment.To.Code + "</Varis>");
                    sb.Append("<KalkisTarih>" + segment.DepartureDate.ToString("s") + "</KalkisTarih>");
                    sb.Append("<VarisTarih>" + segment.ArrivalDate.ToString("s") + "</VarisTarih>");
                    sb.Append("<UcusNo>" + segment.Id + "</UcusNo>");
                    sb.Append("<FirmaKod>" + segment.Factory.Code + "</FirmaKod>");
                    sb.Append("<Sinif>" + segment.SelectedClass.ShortName.ToString() + "</Sinif>");
                    sb.Append("<DonusMu>" + (journey.IsReturn ? 1 : 0) + "</DonusMu>");
                    if (string.IsNullOrEmpty(segment.Code))
                        sb.Append("<SeferKod></SeferKod>");
                    else
                        sb.Append("<SeferKod>" + segment.Code + "</SeferKod>");
                    sb.Append("</Segment" + segmentIndex + ">");
                    segmentIndex++;
                }
            }

            sb.Append("<YetiskinSayi>" + ticket.Passengers.Where(x => x.Type == PassengerType.Adult).Count() + "</YetiskinSayi>");
            sb.Append("<CocukSayi>" + ticket.Passengers.Where(x => x.Type == PassengerType.Child).Count() + "</CocukSayi>");
            sb.Append("<BebekSayi>" + ticket.Passengers.Where(x => x.Type == PassengerType.Baby).Count() + "</BebekSayi>");
            sb.Append("<OgrenciSayi>" + ticket.Passengers.Where(x => x.Type == PassengerType.Student).Count() + "</OgrenciSayi>");
            sb.Append("<YasliSayi>" + ticket.Passengers.Where(x => x.Type == PassengerType.Infant).Count() + "</YasliSayi>");
            sb.Append("<AskerSayi>" + ticket.Passengers.Where(x => x.Type == PassengerType.Military).Count() + "</AskerSayi>");
            sb.Append("<GencSayi>" + ticket.Passengers.Where(x => x.Type == PassengerType.Teenager).Count() + "</GencSayi>");
            sb.Append("</UcusFiyat>");
            return sb.ToString();
        }

        public static BaseResponse<PriceDetail> ParsePriceDetails(string xml)
        {
            var response = new BaseResponse<PriceDetail>();

            var root = XElement.Parse(xml);
            if (root.Element(XName.Get("Sonuc")) != null && root.Element(XName.Get("Sonuc")).Value.ToString() == "false")
            {
                if (root.Element(XName.Get("Hata")) != null && root.Element(XName.Get("Hata")).Value == "Gidiş Dönüş işlemlerinde farklı servis sağlayıcıları(Thy,Galileo,Sun Ex.,Borajet,Atlasjet,Onur Air,Pegasus) birlikte seçilemez!")
                    response.Status = ResponseStatus.DifferentFactories;
                else
                    response.Status = ResponseStatus.Undefined;
                return response;
            }

            var element = root.Element(XName.Get("FiyatListesi"));
            var detail = new PriceDetail();

            detail.TotalPrice.PassengerCount = Convert.ToInt32(element.Element(XName.Get("ToplamYolcuSayisi")).Value.ToString());
            detail.TotalPrice.TotalPrice = Convert.ToDouble(element.Element(XName.Get("ToplamBiletFiyati")).Value.ToString());
            detail.TotalPrice.NetPrice = Convert.ToDouble(element.Element(XName.Get("ToplamNetBiletFiyati")).Value.ToString());
            detail.TotalPrice.Tax = Convert.ToDouble(element.Element(XName.Get("ToplamVergi")).Value.ToString());
            detail.TotalPrice.ServicePrice = Convert.ToDouble(element.Element(XName.Get("ToplamServisUcret")).Value.ToString());
            
            detail.AdultPrice.PassengerCount = Convert.ToInt32(element.Element(XName.Get("YetiskinYolcuSayisi")).Value.ToString());
            detail.AdultPrice.NetPrice = Convert.ToDouble(element.Element(XName.Get("YetiskinNetFiyat")).Value.ToString());
            detail.AdultPrice.Tax = Convert.ToDouble(element.Element(XName.Get("YetiskinVergi")).Value.ToString());
            detail.AdultPrice.ServicePrice = Convert.ToDouble(element.Element(XName.Get("YetiskinServisUcret")).Value.ToString());
            detail.AdultPrice.TotalPrice = detail.AdultPrice.NetPrice + detail.AdultPrice.Tax + detail.AdultPrice.ServicePrice;

            detail.ChildPrice.PassengerCount = Convert.ToInt32(element.Element(XName.Get("CocukYolcuSayisi")).Value.ToString());
            detail.ChildPrice.NetPrice = Convert.ToDouble(element.Element(XName.Get("CocukNetFiyat")).Value.ToString());
            detail.ChildPrice.Tax = Convert.ToDouble(element.Element(XName.Get("CocukVergi")).Value.ToString());
            detail.ChildPrice.ServicePrice = Convert.ToDouble(element.Element(XName.Get("CocukServisUcret")).Value.ToString());
            detail.ChildPrice.TotalPrice = detail.ChildPrice.NetPrice + detail.ChildPrice.Tax + detail.ChildPrice.ServicePrice;
            
            detail.BabyPrice.PassengerCount = Convert.ToInt32(element.Element(XName.Get("BebekYolcuSayisi")).Value.ToString());
            detail.BabyPrice.NetPrice = Convert.ToDouble(element.Element(XName.Get("BebekNetFiyat")).Value.ToString());
            detail.BabyPrice.Tax = Convert.ToDouble(element.Element(XName.Get("BebekVergi")).Value.ToString());
            detail.BabyPrice.ServicePrice = Convert.ToDouble(element.Element(XName.Get("BebekServisUcret")).Value.ToString());
            detail.BabyPrice.TotalPrice = detail.BabyPrice.NetPrice + detail.BabyPrice.Tax + detail.BabyPrice.ServicePrice;
            
            detail.InfantPrice.PassengerCount = Convert.ToInt32(element.Element(XName.Get("YasliYolcuSayisi")).Value.ToString());
            detail.InfantPrice.NetPrice = Convert.ToDouble(element.Element(XName.Get("YasliNetFiyat")).Value.ToString());
            detail.InfantPrice.Tax = Convert.ToDouble(element.Element(XName.Get("YasliVergi")).Value.ToString());
            detail.InfantPrice.ServicePrice = Convert.ToDouble(element.Element(XName.Get("YasliServisUcret")).Value.ToString());
            detail.InfantPrice.TotalPrice = detail.InfantPrice.NetPrice + detail.InfantPrice.Tax + detail.InfantPrice.ServicePrice;
            
            detail.StudentPrice.PassengerCount = Convert.ToInt32(element.Element(XName.Get("OgrenciYolcuSayisi")).Value.ToString());
            detail.StudentPrice.NetPrice = Convert.ToDouble(element.Element(XName.Get("OgrenciNetFiyat")).Value.ToString());
            detail.StudentPrice.Tax = Convert.ToDouble(element.Element(XName.Get("OgrenciVergi")).Value.ToString());
            detail.StudentPrice.ServicePrice = Convert.ToDouble(element.Element(XName.Get("OgrenciServisUcret")).Value.ToString());
            detail.StudentPrice.TotalPrice = detail.StudentPrice.NetPrice + detail.StudentPrice.Tax + detail.StudentPrice.ServicePrice;
            
            detail.MilitaryPrice.PassengerCount = Convert.ToInt32(element.Element(XName.Get("AskerYolcuSayisi")).Value.ToString());
            detail.MilitaryPrice.NetPrice = Convert.ToDouble(element.Element(XName.Get("AskerNetFiyat")).Value.ToString());
            detail.MilitaryPrice.Tax = Convert.ToDouble(element.Element(XName.Get("AskerVergi")).Value.ToString());
            detail.MilitaryPrice.ServicePrice = Convert.ToDouble(element.Element(XName.Get("AskerServisUcret")).Value.ToString());
            detail.MilitaryPrice.TotalPrice = detail.MilitaryPrice.NetPrice + detail.MilitaryPrice.Tax + detail.MilitaryPrice.ServicePrice;
            
            detail.TeenagerPrice.PassengerCount = Convert.ToInt32(element.Element(XName.Get("GencYolcuSayisi")).Value.ToString());
            detail.TeenagerPrice.NetPrice = Convert.ToDouble(element.Element(XName.Get("GencNetFiyat")).Value.ToString());
            detail.TeenagerPrice.Tax = Convert.ToDouble(element.Element(XName.Get("GencVergi")).Value.ToString());
            detail.TeenagerPrice.ServicePrice = Convert.ToDouble(element.Element(XName.Get("GencServisUcret")).Value.ToString());
            detail.TeenagerPrice.TotalPrice = detail.TeenagerPrice.NetPrice + detail.TeenagerPrice.Tax + detail.TeenagerPrice.ServicePrice;

            detail.IsActivated3D = element.Element(XName.Get("Odeme3DSecureAktifMi")).Value.ToString() == "1";
            detail.IsRequired3D = element.Element(XName.Get("Odeme3DSecureZorunluMu")).Value.ToString() == "1";
            //detail.PaypalMaxLimit = Convert.ToDouble(element.Element(XName.Get("PaypalUstLimit")).Value.ToString());
            detail.IsRequiredBirthday = element.Element(XName.Get("YolcuDogumTarihiZorunluMu")).Value.ToString() == "1";
            detail.IsRequiredPassport = element.Element(XName.Get("YolcuPasaportNoZorunluMu")).Value.ToString() == "1";
            //detail.BAServicePrice = Convert.ToDouble(element.Element(XName.Get("BAServisUcret")).Value.ToString());
            //detail.KAServicePrice = Convert.ToDouble(element.Element(XName.Get("KAServisUcret")).Value.ToString());

            response.Result = detail;
            return response;
        }
        #endregion
    }
}
