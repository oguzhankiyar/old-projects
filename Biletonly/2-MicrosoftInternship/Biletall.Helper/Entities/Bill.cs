using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biletall.Helper.Enums;

namespace Biletall.Helper.Entities
{
    public class Bill
    {
        public BillType Type { get; set; }
        public long? PersonTCKN { get; set; }
        public string PersonFirstName { get; set; }
        public string PersonLastName { get; set; }

        public string FactoryName { get; set; }
        public string FactoryTaxOfficeName { get; set; }
        public string FactoryTaxId { get; set; }

        public string Address { get; set; }
    }
}
