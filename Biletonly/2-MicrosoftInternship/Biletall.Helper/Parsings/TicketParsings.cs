using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Biletall.Helper.Entities;
using Biletall.Helper.Enums;
using Biletall.Helper.Requests;

namespace Biletall.Helper.Parsings
{
    internal class TicketParsings
    {
        public static string GetTicket(PNR pnr)
        {
            var sb = new StringBuilder();
            sb.Append("<PnrIslem>");
            sb.Append("<PnrNo>" + pnr.Code + "</PnrNo>");
            sb.Append("<PnrIslemTip>0</PnrIslemTip>");
            sb.Append("<PnrAramaParametre>" + pnr.Parameter + "</PnrAramaParametre>");
            sb.Append("</PnrIslem>");
            return sb.ToString();
        }

        public static BaseResponse<Ticket> ParseTicket(string xml)
        {
            var response = new BaseResponse<Ticket>();

            var ticket = new Ticket();
            var departureJourney = new Journey();
            var returnJourney = new Journey() { IsReturn = true };

            var root = XElement.Parse(xml);
            if (root.Element(XName.Get("Sonuc")) != null && root.Element(XName.Get("Sonuc")).Value.ToString() == "false")
            {
                response.Status = ResponseStatus.NotFound;
                response.Message = root.Element(XName.Get("Hata")).Value;
                return response;
            }

            #region PNR
            var pnr = root.Element(XName.Get("PNR"));
            ticket.PNR = new PNR();
            ticket.PNR.Code = pnr.Element(XName.Get("PNR")).Value.ToString();
            ticket.Type = Functions.GetTicketType(pnr.Element(XName.Get("PnrTip")).Value.ToString());
            //ticket.PNR.TrackingId = pnr.Element(XName.Get("TakipNo")).Value.ToString();
            //ticket.PNR.FirstName = pnr.Element(XName.Get("Ad")).Value.ToString();
            //ticket.PNR.LastName = pnr.Element(XName.Get("Soyad")).Value.ToString();
            ticket.PNR.Parameter = pnr.Element(XName.Get("Soyad")).Value.ToString();
            ticket.PNR.PhoneNumber = pnr.Element(XName.Get("Tel")).Value.ToString();
            ticket.PNR.Email = pnr.Element(XName.Get("Email")).Value.ToString();
            ticket.PNR.ExpirationDate = Convert.ToDateTime(pnr.Element(XName.Get("OpsiyonTarih")).Value.ToString());
            if (pnr.Element(XName.Get("SabitTel")) != null)
                ticket.PNR.TelephoneNumber = pnr.Element(XName.Get("SabitTel")).Value.ToString();
            //if (!string.IsNullOrEmpty(pnr.Element(XName.Get("TCKimlikNo")).Value))
            //    ticket.PNR.TCKN = Con vert.ToInt64(pnr.Element(XName.Get("TCKimlikNo")).Value.ToString());
            //ticket.PNR.Factory = new Factory() { Id = Convert.ToInt32(pnr.Element(XName.Get("FirmaNo")).Value.ToString()) };

            /*
            if (pnrDetail != null)
                ticket.ActionType = pnrDetail.Element(XName.Get("IslemTipi")).Value.ToString() == "SATIS" ? ActionType.Buying : ActionType.Reservation;
            else
                ticket.ActionType = ActionType.Reservation;
            */
            #endregion

            #region Passengers
            var passengerNodes = root.Elements().Where(x => x.Name.LocalName == "Yolcu" && x.Element(XName.Get("Durum1")) != null).ToList();
            foreach (var pNode in passengerNodes.ToList())
            {
                var passenger = new Passenger();
                passenger.FirstName = pNode.Element(XName.Get("Ad")).Value.ToString();
                passenger.LastName = pNode.Element(XName.Get("Soyad")).Value.ToString();
                if (!string.IsNullOrEmpty(pNode.Element(XName.Get("TCKimlikNo")).Value))
                    passenger.TCKN = Convert.ToInt64(pNode.Element(XName.Get("TCKimlikNo")).Value.ToString());
                passenger.Gender = pNode.Element(XName.Get("Cinsiyet")).Value.ToString() == "1" ? Gender.Female : Gender.Male;

                if (pNode.Element(XName.Get("KoltukNo")) != null)
                    passenger.Seat = new Seat() { Number = Convert.ToInt32(pNode.Element(XName.Get("KoltukNo")).Value.ToString()) };

                passenger.Type = Functions.GetPassengerType(pNode.Element(XName.Get("Tip")).Value.ToString());

                if (pNode.Element(XName.Get("EBiletNo")) != null)
                    passenger.ETicketId = pNode.Element(XName.Get("EBiletNo")).Value;

                if (pNode.Element(XName.Get("FirmaKartNo")) != null)
                    passenger.FactoryCardId = pNode.Element(XName.Get("FirmaKartNo")).Value;

                passenger.FromServiceStop = new ServiceStop();
                passenger.ToServiceStop = new ServiceStop();

                passenger.TicketActions = new List<TicketAction>();

                for (int i = 1; i < 5; i++)
                {
                    var actionNode = pNode.Element(XName.Get("Durum" + i));
                    if (actionNode == null)
                        break;

                    passenger.TicketActions.Add(new TicketAction()
                    {
                        Id = i,
                        Type = Functions.GetActionType(pNode.Element(XName.Get("Durum" + i)).Value.ToString()),
                        Date = Convert.ToDateTime(pNode.Element(XName.Get("Durum" + i + "Tarih")).Value.ToString())
                    });
                }

                int currentActionId = Convert.ToInt32(pNode.Element(XName.Get("AktifDurum")).Value.ToString());
                passenger.LastAction = passenger.TicketActions.SingleOrDefault(x => x.Id == currentActionId);

                var price = new Price();
                price.NetPrice = Convert.ToDouble(pNode.Element(XName.Get("Fiyat")).Value.ToString());
                price.ServicePrice = Convert.ToDouble(pNode.Element(XName.Get("ServisUcret")).Value.ToString());
                price.Tax = Convert.ToDouble(pNode.Element(XName.Get("Vergi")).Value.ToString());
                price.TotalPrice = price.NetPrice + price.ServicePrice + price.Tax;
                passenger.Price = price;

                ticket.Passengers.Add(passenger);
            }
            #endregion

            #region Segments
            int segmentIndex = 1;
            var segmentNodes = root.Elements().Where(x => x.Name.LocalName == "Segment");
            int departureCount = segmentNodes.Count(x => x.Element(XName.Get("DonusMu")).Value != "1");
            foreach (var sNode in segmentNodes.ToList())
            {
                var segment = new Segment();
                bool isReturn = sNode.Element(XName.Get("DonusMu")).Value == "1";
                segment.Id = sNode.Element(XName.Get("SeferNo")).Value.ToString();
                if (segmentNodes.Count() > 1 || isReturn)
                {
                    if (segmentIndex == departureCount)
                        segmentIndex = 1;
                    if (isReturn)
                        segment.Name = "Dönüş" + (segmentNodes.Count() - departureCount == 1 ? "" : " (" + segmentIndex++ + ". uçuş)");
                    else
                        segment.Name = "Gidiş" + (departureCount == 1 ? "" : " (" + segmentIndex++ + ". uçuş)");
                }
                segment.From = new Station() { Name = sNode.Element(XName.Get("Kalkis")).Value.ToString() };
                segment.To = new Station() { Name = sNode.Element(XName.Get("Varis")).Value.ToString() };

                
                if (ticket.Type != TicketType.BusJourney && sNode.Element(XName.Get("KalkisKod")) != null && !string.IsNullOrEmpty(sNode.Element(XName.Get("KalkisKod")).Value.ToString()))
                {
                    if (Global.Stations.Any())
                    {
                        segment.From = Global.Stations.SingleOrDefault(x => x.Code == sNode.Element(XName.Get("KalkisKod")).Value.ToString());
                        segment.To = Global.Stations.SingleOrDefault(x => x.Code == sNode.Element(XName.Get("VarisKod")).Value.ToString());
                    }
                    else
                    {
                        segment.From = new Station() { Code = sNode.Element(XName.Get("KalkisKod")).Value.ToString() };
                        segment.To = new Station() { Code = sNode.Element(XName.Get("VarisKod")).Value.ToString() };
                    }
                }

                segment.Factory = new Factory() { Id = Convert.ToInt32(sNode.Element(XName.Get("TasiyiciFirma")).Value.ToString()), Code = sNode.Element(XName.Get("FirmaKod")).Value.ToString(), Name = sNode.Element(XName.Get("FirmaAd")).Value.ToString() };
                segment.DepartureDate = Convert.ToDateTime(sNode.Element(XName.Get("KalkisTarih")).Value.ToString());
                segment.ArrivalDate = Convert.ToDateTime(sNode.Element(XName.Get("VarisTarih")).Value.ToString());

                if (sNode.Element(XName.Get("SeferSure")) != null)
                    segment.Duration = Convert.ToDateTime(sNode.Element(XName.Get("SeferSure")).Value.ToString());

                if (sNode.Element(XName.Get("BosSaat")) != null && sNode.Element(XName.Get("BosTarih")) != null)
                {
                    var date = Convert.ToDateTime(sNode.Element(XName.Get("BosTarih")).Value);
                    string hour = sNode.Element(XName.Get("BosSaat")).Value.Replace("1900-01-01", date.ToString("yyyy") + "-" + date.ToString("MM") + "-" + date.ToString("dd"));
                    segment.Hour = hour;
                }

                if (ticket.Type == TicketType.BusJourney)
                {
                    segment.Bus = new Bus();

                    segment.UnitPrice = ticket.Passengers.First().Price;

                    if (sNode.Element(XName.Get("HatNo")) != null)
                        segment.LineNumber = Convert.ToInt32(sNode.Element(XName.Get("HatNo")).Value.ToString());

                    if (sNode.Element(XName.Get("SeferTip")) != null)
                        segment.Type = sNode.Element(XName.Get("SeferTip")).Value.ToString() == "MOLALI" ? SegmentType.Stop : SegmentType.Nonstop;

                    if (sNode.Element(XName.Get("AracTipi")) != null)
                        segment.Bus.Type = new BusType() { Name = sNode.Element(XName.Get("AracTipi")).Value.ToString() };

                    if (sNode.Element(XName.Get("PeronNo")) != null)
                        segment.Bus.PlatformNumber = sNode.Element(XName.Get("PeronNo")).Value.ToString();
                }

                var selectedClass = new JourneyClass();
                if (sNode.Element(XName.Get("SinifTip")) != null)
                    selectedClass.Name = sNode.Element(XName.Get("SinifTip")).Value.ToString();

                if (sNode.Element(XName.Get("Sinif")) != null)
                    selectedClass.ShortName = sNode.Element(XName.Get("Sinif")).Value.ToString();
                segment.SelectedClass = selectedClass;
                
                if (!isReturn)
                    departureJourney.Segments.Add(segment);
                else
                    returnJourney.Segments.Add(segment);
            }
            #endregion

            #region Journeys
            departureJourney.Factory = departureJourney.Segments.First().Factory;
            departureJourney.From = departureJourney.Segments.First().From;
            departureJourney.To = departureJourney.Segments.Last().To;
            departureJourney.DepartureDate = departureJourney.Segments.First().DepartureDate;
            ticket.Journeys.Add(departureJourney);

            if (returnJourney.Segments.Any())
            {
                returnJourney.Factory = returnJourney.Segments.First().Factory;
                returnJourney.From = returnJourney.Segments.First().From;
                returnJourney.To = returnJourney.Segments.Last().To;
                returnJourney.DepartureDate = returnJourney.Segments.First().DepartureDate;
                ticket.Journeys.Add(returnJourney);
            }
            #endregion

            #region Prices
            var ticketPrice = new Price();
            foreach (var item in ticket.Passengers.ToList())
            {
                ticketPrice.NetPrice += item.Price.NetPrice;
                ticketPrice.Tax += item.Price.Tax;
                ticketPrice.ServicePrice += item.Price.ServicePrice;
                ticketPrice.TotalPrice += item.Price.TotalPrice;
            }
            ticket.Price = ticketPrice;
            #endregion
            
            #region Bill
            var billNode = root.Element(XName.Get("Fatura"));
            if (billNode != null)
            {
                var bill = new Bill();
                if (billNode.Element(XName.Get("KisiAd")) != null)
                {
                    bill.Type = BillType.Person;
                    bill.PersonFirstName = billNode.Element(XName.Get("KisiAd")).Value;
                    bill.PersonLastName = billNode.Element(XName.Get("KisiSoyad")).Value;
                    bill.PersonTCKN = Convert.ToInt64(billNode.Element(XName.Get("KisiTcKimlikNo")).Value);
                    bill.Address = billNode.Element(XName.Get("KisiAdres")).Value;
                }
                else if (billNode.Element(XName.Get("FirmaAd")) != null)
                {
                    bill.Type = BillType.Factory;
                    bill.FactoryName = billNode.Element(XName.Get("FirmaAd")).Value;
                    bill.FactoryTaxId = billNode.Element(XName.Get("FirmaVergiNo")).Value;
                    bill.FactoryTaxOfficeName = billNode.Element(XName.Get("FirmaVergiDairesi")).Value;
                    bill.Address = billNode.Element(XName.Get("FirmaAdres")).Value;
                }
                ticket.Bill = bill;
            }
            #endregion
            
            #region Price Details
            if (ticket.Type != TicketType.BusJourney)
            {
                JourneyRequests.PriceDetailsRequest.OnCompleted = (priceResponse) =>
                {
                    if (priceResponse.Status == ResponseStatus.Successful)
                    {
                        var priceDetail = priceResponse.Result;
                        JourneyRequests.FillPassengerPrices(ticket, priceDetail);
                    }
                    else
                    {
                        // 3D Secure zorunluluk bilgisine erişilemediğinde
                        ticket.Is3DBuyingActivated = true;
                        ticket.Is3DBuyingRequired = false;
                        ticket.IsPassportRequired = ticket.Type == TicketType.InternationalJourney;
                    }
                };
                JourneyRequests.GetPriceDetails(ticket);
            }
            else
            {
                BusRequests.BusRequest.OnCompleted = (busResponse) =>
                {
                    if (busResponse.Status == ResponseStatus.Successful)
                    {
                        var bus = busResponse.Result;
                        ticket.Is3DBuyingActivated = bus.Is3DBuyingActivated;
                        ticket.Is3DBuyingRequired = bus.Is3DSecureRequired;
                    }
                    else
                    {
                        ticket.Is3DBuyingActivated = true;
                        ticket.Is3DBuyingRequired = false;
                    }
                };
                BusRequests.GetBus(ticket.Journeys[0].Segments[0]);
            }
            #endregion

            response.Result = ticket;
            return response;
        }
    }
}
