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
    
    public partial class bgk_yorum
    {
        public int Id { get; set; }
        public string Yorum { get; set; }
        public int YaziID { get; set; }
        public Nullable<int> UyeID { get; set; }
        public string Yazan { get; set; }
        public System.DateTime YazilmaTarihi { get; set; }
        public bool Onay { get; set; }
    
        public virtual bgk_uye bgk_uye { get; set; }
        public virtual bgk_yazi bgk_yazi { get; set; }
    }
}
