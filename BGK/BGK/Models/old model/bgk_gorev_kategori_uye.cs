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
    
    public partial class bgk_gorev_kategori_uye
    {
        public int Id { get; set; }
        public int UyeID { get; set; }
        public int KategoriID { get; set; }
        public System.DateTime BaslamaTarihi { get; set; }
    
        public virtual bgk_gorev_kategori bgk_gorev_kategori { get; set; }
        public virtual bgk_uye bgk_uye { get; set; }
    }
}
