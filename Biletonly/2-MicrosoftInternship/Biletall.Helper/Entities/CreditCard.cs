using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biletall.Helper.Entities
{
    public class CreditCard
    {
        public string OwnerName { get; set; }
        public string Number { get; set; }
        public string CVC { get; set; }
        public int? ExpirationMonth { get; set; }
        public int? ExpirationYear { get; set; }
    }
}
