//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BGK.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class bgk_anket
    {
        public bgk_anket()
        {
            this.bgk_anket_secenek = new HashSet<bgk_anket_secenek>();
        }
    
        public int Id { get; set; }
        public string Soru { get; set; }
        public bool CokluSecim { get; set; }
        public bool SadeceUye { get; set; }
        public System.DateTime BaslangicTarihi { get; set; }
        public System.DateTime BitisTarihi { get; set; }
        public int Sira { get; set; }
        public bool Onay { get; set; }
    
        public virtual ICollection<bgk_anket_secenek> bgk_anket_secenek { get; set; }
    }
}
