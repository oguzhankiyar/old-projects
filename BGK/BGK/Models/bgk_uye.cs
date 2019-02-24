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
    
    public partial class bgk_uye
    {
        public bgk_uye()
        {
            this.bgk_anket_secim = new HashSet<bgk_anket_secim>();
            this.bgk_bildirim = new HashSet<bgk_bildirim>();
            this.bgk_dokuman = new HashSet<bgk_dokuman>();
            this.bgk_dosya = new HashSet<bgk_dosya>();
            this.bgk_etkinlik_gorevli = new HashSet<bgk_etkinlik_gorevli>();
            this.bgk_firma = new HashSet<bgk_firma>();
            this.bgk_gorev = new HashSet<bgk_gorev>();
            this.bgk_gorev_kategori = new HashSet<bgk_gorev_kategori>();
            this.bgk_gorev_kategori_uye = new HashSet<bgk_gorev_kategori_uye>();
            this.bgk_gorev_uye = new HashSet<bgk_gorev_uye>();
            this.bgk_grup_uye = new HashSet<bgk_grup_uye>();
            this.bgk_not = new HashSet<bgk_not>();
            this.bgk_oylama = new HashSet<bgk_oylama>();
            this.bgk_yazi = new HashSet<bgk_yazi>();
            this.bgk_yorum = new HashSet<bgk_yorum>();
        }
    
        public int Id { get; set; }
        public string AdSoyad { get; set; }
        public string Email { get; set; }
        public string Sifre { get; set; }
        public Nullable<int> Telefon { get; set; }
        public int OgrenciNo { get; set; }
        public string Fakulte { get; set; }
        public string Bolum { get; set; }
        public int GirisYili { get; set; }
        public int Puan { get; set; }
        public int CezaPuani { get; set; }
        public int Yetki { get; set; }
        public System.DateTime SonGiris { get; set; }
        public System.DateTime KayitTarihi { get; set; }
        public bool Onay { get; set; }
    
        public virtual ICollection<bgk_anket_secim> bgk_anket_secim { get; set; }
        public virtual ICollection<bgk_bildirim> bgk_bildirim { get; set; }
        public virtual ICollection<bgk_dokuman> bgk_dokuman { get; set; }
        public virtual ICollection<bgk_dosya> bgk_dosya { get; set; }
        public virtual ICollection<bgk_etkinlik_gorevli> bgk_etkinlik_gorevli { get; set; }
        public virtual ICollection<bgk_firma> bgk_firma { get; set; }
        public virtual ICollection<bgk_gorev> bgk_gorev { get; set; }
        public virtual ICollection<bgk_gorev_kategori> bgk_gorev_kategori { get; set; }
        public virtual ICollection<bgk_gorev_kategori_uye> bgk_gorev_kategori_uye { get; set; }
        public virtual ICollection<bgk_gorev_uye> bgk_gorev_uye { get; set; }
        public virtual ICollection<bgk_grup_uye> bgk_grup_uye { get; set; }
        public virtual ICollection<bgk_not> bgk_not { get; set; }
        public virtual ICollection<bgk_oylama> bgk_oylama { get; set; }
        public virtual ICollection<bgk_yazi> bgk_yazi { get; set; }
        public virtual ICollection<bgk_yorum> bgk_yorum { get; set; }
    }
}