using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biletall.Helper.BiletallService;
using Biletall.Helper.Entities;
using Biletall.Helper.Parsings;

namespace Biletall.Helper.Requests
{
    public class TicketRequest
    {
        public static bool IsTicketCompleted { get; private set; }
        public static Action<Ticket> OnTicketCompleted = null;

        public static void GetTicket(PNR pnr)
        {
            IsTicketCompleted = false;
            string xml = TicketParsing.GetTicket(pnr);
            var client = new ServiceSoapClient();
            client.StrIsletAsync(xml, Global.Authorization);
            client.StrIsletCompleted += (c, e) =>
            {
                string xmlResult = e.Result;
                PopulateTicket(xmlResult);
            };
        }

        private static void PopulateTicket(string xmlResult)
        {
            var ticket = TicketParsing.ParseTicket(xmlResult);
            IsTicketCompleted = true;
            if (OnTicketCompleted != null)
                OnTicketCompleted(ticket);
        }
    }
}
