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
    
    public partial class bgk_anket_secenek
    {
        public bgk_anket_secenek()
        {
            this.bgk_anket_secim = new HashSet<bgk_anket_secim>();
        }
    
        public int Id { get; set; }
        public string Adi { get; set; }
        public int AnketID { get; set; }
        public int Sira { get; set; }
    
        public virtual bgk_anket bgk_anket { get; set; }
        public virtual ICollection<bgk_anket_secim> bgk_anket_secim { get; set; }
    }
}