using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biletall.Helper.Entities;
using Biletall.Helper.Parsings;

namespace Biletall.Helper.Requests
{
    public static class TicketRequests
    {
        public static BaseRequest<Ticket> TicketRequest;
        
        static TicketRequests()
        {
            TicketRequest = BaseRequest<Ticket>.GetInstance();
        }

        public static void GetTicket(PNR pnr)
        {
            string xml = TicketParsings.GetTicket(pnr);
            TicketRequest.OnPopulated = (xmlResult) =>
            {
                TicketRequest.Response = TicketParsings.ParseTicket(xmlResult);
            };
            Global.OnRequestCalled("TicketRequests.GetTicket", new object[] { pnr });
            TicketRequest.GetResult(xml, ApiAction.SearchTicket);
        }
    }
}
