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
    
    public partial class bgk_bildirim
    {
        public int Id { get; set; }
        public string Icerik { get; set; }
        public int UyeID { get; set; }
        public System.DateTime Tarih { get; set; }
        public bool Okundu { get; set; }
    
        public virtual bgk_uye bgk_uye { get; set; }
    }
}