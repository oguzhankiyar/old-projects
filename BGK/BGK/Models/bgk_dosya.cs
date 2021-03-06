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
    
    public partial class bgk_dosya
    {
        public bgk_dosya()
        {
            this.bgk_butce = new HashSet<bgk_butce>();
            this.bgk_dokuman = new HashSet<bgk_dokuman>();
            this.bgk_etkinlik = new HashSet<bgk_etkinlik>();
            this.bgk_firma = new HashSet<bgk_firma>();
            this.bgk_galeri_resim = new HashSet<bgk_galeri_resim>();
            this.bgk_gorev_kategori = new HashSet<bgk_gorev_kategori>();
            this.bgk_gorev_uye = new HashSet<bgk_gorev_uye>();
        }
    
        public int Id { get; set; }
        public string Aciklama { get; set; }
        public string DosyaAdi { get; set; }
        public string DosyaTipi { get; set; }
        public int YukleyenID { get; set; }
        public System.DateTime YuklenmeTarihi { get; set; }
        public string Adres { get; set; }
    
        public virtual ICollection<bgk_butce> bgk_butce { get; set; }
        public virtual ICollection<bgk_dokuman> bgk_dokuman { get; set; }
        public virtual bgk_uye bgk_uye { get; set; }
        public virtual ICollection<bgk_etkinlik> bgk_etkinlik { get; set; }
        public virtual ICollection<bgk_firma> bgk_firma { get; set; }
        public virtual ICollection<bgk_galeri_resim> bgk_galeri_resim { get; set; }
        public virtual ICollection<bgk_gorev_kategori> bgk_gorev_kategori { get; set; }
        public virtual ICollection<bgk_gorev_uye> bgk_gorev_uye { get; set; }
    }
}
