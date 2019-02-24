using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Biletall.Helper.Entities;
using Biletall.Helper.Enums;
using HtmlAgilityPack;

namespace Biletall.Helper.Parsings
{
    internal class BuyingParsings
    {
        internal static string GetBusBuying(Ticket ticket)
        {
            var sb = new StringBuilder();
            var segment = ticket.Journeys[0].Segments[0];
            sb.Append("<IslemSatis>");
            sb.Append("<FirmaNo>" + segment.Factory.Id + "</FirmaNo>");
            sb.Append("<KalkisAdi>" + segment.From.Name + "</KalkisAdi>");
            sb.Append("<VarisAdi>" + segment.To.Name + "</VarisAdi>");
            sb.Append("<Tarih>" + segment.DepartureDate.ToString("yyyy-MM-dd") + "</Tarih>");
            sb.Append("<Saat>" + segment.Hour + "</Saat>");
            sb.Append("<HatNo>" + segment.LineNumber + "</HatNo>");
            sb.Append("<SeferNo>" + segment.Id + "</SeferNo>");

            int i = 1;
            foreach (var p in ticket.Passengers.ToList())
            {
                sb.Append("<KoltukNo" + i + ">" + p.Seat.Number + "</KoltukNo" + i + ">");
                sb.Append("<Adi" + i + ">" + Functions.TidyName(p.FirstName) + "</Adi" + i + ">");
                sb.Append("<Soyadi" + i + ">" + Functions.TidyName(p.LastName) + "</Soyadi" + i + ">");
                if (p.TCKN != null)
                    sb.Append("<TcKimlikNo" + i + ">" + p.TCKN.ToString() + "</TcKimlikNo" + i + ">");
                /*if (p.WhereCatch != null)
                    sb.Append("<BinecegiYer" + i + ">" + p.WhereCatch + "</BinecegiYer" + i + ">");*/
                if (p.FromServiceStop != null && p.FromServiceStop.Location != "Servis istemiyorum")
                    sb.Append("<ServisYeriKalkis" + i + ">" + p.FromServiceStop.Location.ToString() + "</ServisYeriKalkis" + i + ">");
                if (p.ToServiceStop != null && p.ToServiceStop.Location != "Servis istemiyorum")
                    sb.Append("<ServisYeriVaris" + i + ">" + p.ToServiceStop.Location.ToString() + "</ServisYeriVaris" + i + ">");
                i++;
            }
            sb.Append("<TelefonNo>" + ticket.PNR.PhoneNumber + "</TelefonNo>");
            sb.Append("<Cinsiyet>" + (ticket.Passengers[0].Gender == Gender.Female ? 1 : 2) + "</Cinsiyet>");
            sb.Append("<ToplamBiletFiyati>" + ticket.Price.TotalPrice + "</ToplamBiletFiyati>");
            sb.Append("<YolcuSayisi>" + (i - 1) + "</YolcuSayisi>");
            sb.Append("<BiletSeriNo>1</BiletSeriNo>");
            sb.Append("<OdemeSekli>0</OdemeSekli>");
            sb.Append("<FirmaAciklama></FirmaAciklama>");
            sb.Append("<HatirlaticiNot></HatirlaticiNot>");
            sb.Append("<SeyahatTipi>0</SeyahatTipi>");
            sb.Append("<WebYolcu>");
            sb.Append("<WebUyeNo>0</WebUyeNo>");
            sb.Append("<Ip>" + Functions.GetIPAddress().ToString() + "</Ip>");
            sb.Append("<Email>" + ticket.PNR.Email + "</Email>");
            sb.Append("<KrediKartNo>" + ticket.CreditCard.Number + "</KrediKartNo>");
            sb.Append("<KrediKartSahip>" + ticket.CreditCard.OwnerName + "</KrediKartSahip>");
            sb.Append("<KrediKartGecerlilikTarihi>" + ticket.CreditCard.ExpirationMonth + "." + ticket.CreditCard.ExpirationYear + "</KrediKartGecerlilikTarihi>");
            sb.Append("<KrediKartCCV2>" + ticket.CreditCard.CVC + "</KrediKartCCV2>");
            if (!string.IsNullOrEmpty(ticket.PNR.Code))
                sb.Append("<RezervePnrNo>" + ticket.PNR.Code + "</RezervePnrNo>");
            /*
            sb.Append("<OnOdemeKullan>1</OnOdemeKullan>");
            sb.Append("<OnOdemeTutar>35</OnOdemeTutar>");
            */
            sb.Append("</WebYolcu>");
            sb.Append("</IslemSatis>");

            return ApiHelper.Encrypt(sb.ToString());
        }

        internal static string GetAirplaneBuying(Ticket ticket)
        {
            var sb = new StringBuilder();
            sb.Append("<IslemUcak_2>");
            sb.Append("<IslemTip>0</IslemTip>");
            sb.Append("<FirmaNo>" + (ticket.Type == TicketType.InternationalJourney ? 1100 : 1000) + "</FirmaNo>");
            sb.Append("<TelefonNo>" + ticket.PNR.TelephoneNumber.ToString() + "</TelefonNo>");
            sb.Append("<CepTelefonNo>" + ticket.PNR.PhoneNumber.ToString() + "</CepTelefonNo>");
            sb.Append("<Email>" + ticket.PNR.Email.ToString() + "</Email>");
            sb.Append("<HatirlaticiNot></HatirlaticiNot>");

            int s = 1;
            foreach (var journey in ticket.Journeys.ToList())
            {
                foreach (var segment in journey.Segments.ToList())
                {
                    sb.Append("<Segment" + s + ">");
                    sb.Append("<Kalkis>" + segment.From.Code + "</Kalkis>");
                    sb.Append("<Varis>" + segment.To.Code + "</Varis>");
                    sb.Append("<KalkisTarih>" + segment.DepartureDate.ToString("s") + "</KalkisTarih>");
                    sb.Append("<VarisTarih>" + segment.ArrivalDate.ToString("s") + "</VarisTarih>");
                    sb.Append("<UcusNo>" + segment.Id + "</UcusNo>");
                    sb.Append("<FirmaKod>" + segment.Factory.Code + "</FirmaKod>");
                    sb.Append("<Sinif>" + segment.SelectedClass.ShortName.ToString() + "</Sinif>");
                    sb.Append("<DonusMu>" + (journey.IsReturn ? 1 : 0) + "</DonusMu>");
                    sb.Append("<SeferKod></SeferKod>");
                    sb.Append("</Segment" + s + ">");
                    s++;
                }
            }

            int p = 1;
            foreach (var passenger in ticket.Passengers.ToList())
            {
                sb.Append("<Yolcu" + p + ">");
                sb.Append("<Ad>" + Functions.TidyName(passenger.FirstName) + "</Ad>");
                sb.Append("<Soyad>" + Functions.TidyName(passenger.LastName) + "</Soyad>");
                sb.Append("<Cinsiyet>" + (passenger.Gender == Gender.Male ? 2 : 1) + "</Cinsiyet>");
                sb.Append("<YolcuTip>" + Functions.GetPassengerType(passenger.Type) + "</YolcuTip>");
                sb.Append("<TCKimlikNo>" + passenger.TCKN + "</TCKimlikNo>");
                if (ticket.IsBirthdayRequired)
                    sb.Append("<DogumTarih>" + ((DateTime)passenger.Birthday).ToString("yyyy-MM-dd") + "</DogumTarih>");
                if (ticket.IsPassportRequired)
                    sb.Append("<PasaportNo>" + passenger.PassaportId.ToString() + "</PasaportNo>");
                if (!string.IsNullOrEmpty(passenger.FactoryCardId))
                    sb.Append("<MilNo>" + passenger.FactoryCardId + "</MilNo>");
                else
                    sb.Append("<MilNo></MilNo>");
                sb.Append("<NetFiyat>" + passenger.Price.NetPrice + "</NetFiyat>");
                sb.Append("<Vergi>" + passenger.Price.Tax + "</Vergi>");
                sb.Append("<ServisUcret>" + passenger.Price.ServicePrice + "</ServisUcret>");
                sb.Append("</Yolcu" + p + ">");
                p++;
            }
            
            sb.Append("<Fatura>");
            if (ticket.Bill.Type == BillType.Person)
            {
                sb.Append("<FaturaTip>0</FaturaTip>");
                sb.Append("<KisiAd>" + ticket.Bill.PersonFirstName + "</KisiAd>");
                sb.Append("<KisiSoyad>" + ticket.Bill.PersonLastName + "</KisiSoyad>");
                sb.Append("<KisiTCKimlikNo>" + ticket.Bill.PersonTCKN + "</KisiTCKimlikNo>");
                if (!string.IsNullOrEmpty(ticket.Bill.Address))
                    sb.Append("<KisiAdres>" + ticket.Bill.Address + "</KisiAdres>");
                else
                    sb.Append("<KisiAdres>Adres Belirtilmedi</KisiAdres>");
                sb.Append("<FirmaAd></FirmaAd>");
                sb.Append("<FirmaVergiNo></FirmaVergiNo>");
                sb.Append("<FirmaVergiDairesi></FirmaVergiDairesi>");
                sb.Append("<FirmaAdres></FirmaAdres>");
            }
            else
            {
                sb.Append("<FaturaTip>1</FaturaTip>");
                sb.Append("<KisiAd></KisiAd>");
                sb.Append("<KisiSoyad></KisiSoyad>");
                sb.Append("<KisiTCKimlikNo></KisiTCKimlikNo>");
                sb.Append("<KisiAdres></KisiAdres>");
                sb.Append("<FirmaAd>" + ticket.Bill.FactoryName + "</FirmaAd>");
                sb.Append("<FirmaVergiNo>" + ticket.Bill.FactoryTaxId + "</FirmaVergiNo>");
                sb.Append("<FirmaVergiDairesi>" + ticket.Bill.FactoryTaxOfficeName + "</FirmaVergiDairesi>");
                if (!string.IsNullOrEmpty(ticket.Bill.Address))
                    sb.Append("<FirmaAdres>" + ticket.Bill.Address + "</FirmaAdres>");
                else
                    sb.Append("<FirmaAdres>Adres Belirtilmedi</FirmaAdres>");
            }
            sb.Append("</Fatura>");

            sb.Append("<WebYolcu>");
            sb.Append("<KrediKartNo>" + ticket.CreditCard.Number + "</KrediKartNo>");
            sb.Append("<KrediKartSahip>" + ticket.CreditCard.OwnerName + "</KrediKartSahip>");
            sb.Append("<KrediKartGecerlilikTarihi>" + ticket.CreditCard.ExpirationMonth + "." + ticket.CreditCard.ExpirationYear + "</KrediKartGecerlilikTarihi>");
            sb.Append("<KrediKartCCV2>"+ ticket.CreditCard.CVC + "</KrediKartCCV2>");

            if (ticket.PNR != null && !string.IsNullOrEmpty(ticket.PNR.Code))
                sb.Append("<RezervePnrNo>" + ticket.PNR.Code + "</RezervePnrNo>");
            sb.Append("</WebYolcu>");

            sb.Append("</IslemUcak_2>");
            return ApiHelper.Encrypt(sb.ToString());
        }

        public static BaseResponse<ActionResult> ParseBuying(string xml)
        {
            var response = new BaseResponse<ActionResult>();

            var root = XElement.Parse(xml);
            var actionResult = new ActionResult();
            if (root.Element(XName.Get("Sonuc")).Value == "true")
            {
                actionResult.PNR = root.Element(XName.Get("PNR")).Value;
                actionResult.TicketNumbers = new List<string>();
                foreach (var ticketNode in root.Elements().Where(x => x.Name.LocalName.Contains("EBilet")).ToList())
                {
                    actionResult.TicketNumbers.Add(ticketNode.Value.ToString());
                }
            }
            else
            {
                response.Status = ResponseStatus.Undefined;
                response.Message = root.Element(XName.Get("Hata")).Value;
            }

            response.Result = actionResult;
            return response;
        }

        public static string Get3DSecureBuying(Ticket ticket)
        {
            string secureServiceURL = "http://biletonly.com/api/SecurePay";

            var sb = new StringBuilder();

            string buyingXml = ticket.Type == TicketType.BusJourney ? BuyingParsings.GetBusBuying(ticket) : BuyingParsings.GetAirplaneBuying(ticket);

            string satisXml = buyingXml;
            
            sb.Append("<form id=\"SatisForm\" method=\"post\" action=\"" + secureServiceURL + "\">");
            sb.Append("<input id=\"buyingXml\" name=\"buyingXml\" value=\"" + satisXml + "\" />");
            sb.Append("</form>");
            sb.Append("<script language=\"javascript\" type=\"text/javascript\">");
            sb.Append("document.forms[0].submit();");
            sb.Append("</script>");
            
            return sb.ToString();
        }

        public static BaseResponse<ActionResult> Parse3DSecureBuying(string html)
        {
            var response = new BaseResponse<ActionResult>() { Status = ResponseStatus.Undefined };
            var actionResult = new ActionResult();

            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var root = doc.DocumentNode;
            var inputs = root.Descendants("input");
            var errorInput = inputs.Where(x => x.Attributes["name"] != null && x.Attributes["name"].Value == "Hata").FirstOrDefault();
            if (errorInput != null)
                response.Message = errorInput.Attributes["value"].Value;

            var resultInput = inputs.Where(x => x.Attributes["name"] != null && x.Attributes["name"].Value == "Sonuc").FirstOrDefault();
            if (resultInput != null && resultInput.Attributes["value"].Value.ToString() == "true")
                response.Status = ResponseStatus.Successful;

            var pnrInput = inputs.Where(x => x.Attributes["name"] != null && x.Attributes["name"].Value == "PNR").FirstOrDefault();
            if (pnrInput != null && !string.IsNullOrEmpty(pnrInput.Attributes["value"].Value.ToString()))
                actionResult.PNR = pnrInput.Attributes["value"].Value.ToString();

            if (response.Status != ResponseStatus.Successful && string.IsNullOrEmpty(response.Message))
                response.Message = "bir sorun oluştu! daha sonra tekrar deneyiniz";

            response.Result = actionResult;
            return response;
        }

        public static string GetCancelBuying(Ticket ticket, Passenger passenger)
        {
            var sb = new StringBuilder();
            sb.Append("<PnrIslem>");
            sb.Append("<PnrNo>" + ticket.PNR.Code + "</PnrNo>");
            if (ticket.Type == TicketType.BusJourney)
            {
                if (passenger == null)
                    sb.Append("<PnrKoltukNo>0</PnrKoltukNo>");
                else
                    sb.Append("<PnrKoltukNo>" + passenger.Seat.Number + "</PnrKoltukNo>");
            }
            sb.Append("<WebUyeNo>0</WebUyeNo>");
            sb.Append("<PnrIslemTip>8</PnrIslemTip>");
            double totalPrice;
            if (passenger == null)
                totalPrice = ticket.Price.TotalPrice;
            else
                totalPrice = passenger.Price.TotalPrice;
            
            sb.Append("<PnrSatisIptalTutar>" + string.Format("{0:0.0000}", totalPrice) + "</PnrSatisIptalTutar>");
            sb.Append("<AcikParaIade>1</AcikParaIade>");
            sb.Append("</PnrIslem>");
            return sb.ToString();
        }

        public static BaseResponse<ActionResult> ParseCancelBuying(string xmlResult)
        {
            var response = new BaseResponse<ActionResult>();

            var root = XElement.Parse(xmlResult);
            root = root.Element(XName.Get("IslemSonuc"));
            string result = root.Element(XName.Get("Sonuc")).Value.ToString();
            var actionResult = new ActionResult();
            if (root.Element(XName.Get("Sonuc")).Value != "true")
                response.Status = ResponseStatus.Undefined;
            actionResult.OpenedPrice = Convert.ToDouble(root.Element(XName.Get("Tutar")).Value);

            response.Result = actionResult;
            return response;
        }
    }
}
