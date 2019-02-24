using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biletall.Helper.Enums;

namespace Biletall.Helper.Entities
{
    public class TicketAction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public ActionType Type { get; set; }
    }
}
