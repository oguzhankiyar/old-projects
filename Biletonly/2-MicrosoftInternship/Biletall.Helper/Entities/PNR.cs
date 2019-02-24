using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biletall.Helper.Entities
{
    public class PNR
    {
        public string Code { get; set; }
        public string Parameter { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string TelephoneNumber { get; set; }
    }
}
