using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biletall.Helper.Entities
{
    public class ActionResult
    {
        public string PNR { get; set; }
        //public bool IsSuccessful { get; set; }
        //public string Message { get; set; }
        public DateTime ExpirationDate { get; set; }
        public List<string> TicketNumbers { get; set; }
        public double OpenedPrice { get; set; }
    }
}
