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
    
    public partial class bgk_grup_uye
    {
        public int Id { get; set; }
        public string Aciklama { get; set; }
        public int UyeID { get; set; }
        public int GrupID { get; set; }
        public System.DateTime BaslangicTarihi { get; set; }
        public Nullable<System.DateTime> BitisTarihi { get; set; }
        public int Tip { get; set; }
        public bool Aktif { get; set; }
    
        public virtual bgk_grup bgk_grup { get; set; }
        public virtual bgk_uye bgk_uye { get; set; }
    }
}
