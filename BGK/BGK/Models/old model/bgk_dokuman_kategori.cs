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
    
    public partial class bgk_dokuman_kategori
    {
        public bgk_dokuman_kategori()
        {
            this.bgk_dokuman = new HashSet<bgk_dokuman>();
        }
    
        public int Id { get; set; }
        public string Adi { get; set; }
        public string Aciklama { get; set; }
        public bool Onay { get; set; }
        public string Seo { get; set; }
        public int Sira { get; set; }
    
        public virtual ICollection<bgk_dokuman> bgk_dokuman { get; set; }
    }
}