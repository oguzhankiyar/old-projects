using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biletall.Helper.Entities
{
    public class PriceDetail
    {
        public Price TotalPrice { get; set; }
        public Price AdultPrice { get; set; }
        public Price ChildPrice { get; set; }
        public Price TeenagerPrice { get; set; }
        public Price InfantPrice { get; set; }
        public Price StudentPrice { get; set; }
        public Price BabyPrice { get; set; }
        public Price MilitaryPrice { get; set; }
        public bool IsActivated3D { get; set; }
        public bool IsRequired3D { get; set; }
        //public double PaypalMaxLimit { get; set; }
        public bool IsRequiredBirthday { get; set; }
        public bool IsRequiredPassport { get; set; }
        //public double BAServicePrice { get; set; }
        //public double KAServicePrice { get; set; }

        public PriceDetail()
        {
            TotalPrice = new Price();
            AdultPrice = new Price();
            ChildPrice = new Price();
            TeenagerPrice = new Price();
            InfantPrice = new Price();
            StudentPrice = new Price();
            BabyPrice = new Price();
            MilitaryPrice = new Price();
        }
    }
}
