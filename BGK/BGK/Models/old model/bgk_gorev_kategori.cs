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
    
    public partial class bgk_gorev_kategori
    {
        public bgk_gorev_kategori()
        {
            this.bgk_gorev = new HashSet<bgk_gorev>();
            this.bgk_gorev_kategori_uye = new HashSet<bgk_gorev_kategori_uye>();
        }
    
        public int Id { get; set; }
        public string Adi { get; set; }
        public string Aciklama { get; set; }
        public int ResimID { get; set; }
        public int OlusturanID { get; set; }
        public Nullable<int> PuanSiniri { get; set; }
        public int Sira { get; set; }
        public bool Onay { get; set; }
    
        public virtual bgk_dosya bgk_dosya { get; set; }
        public virtual ICollection<bgk_gorev> bgk_gorev { get; set; }
        public virtual bgk_uye bgk_uye { get; set; }
        public virtual ICollection<bgk_gorev_kategori_uye> bgk_gorev_kategori_uye { get; set; }
    }
}
