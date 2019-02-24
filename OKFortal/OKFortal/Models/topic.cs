//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OKFortal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    
    public partial class topic
    {
        public topic()
        {
            this.comments = new HashSet<comment>();
            this.forum1 = new HashSet<forum>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Bo� ge�ilemez")]
        [Display(Name = "Ba�l�k: ", Description = "Bu konu ne ile alakal�? Uygun bir ba�l�k yaz�n..")]
        public string Title { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "Konunuz en az 100 karakter olmal�..")]
        //[RegularExpression(@"^.{100,}$", ErrorMessage = "Konunuz en az 100 karakter olmal�.. Tekrar deneyin..")]
        [Display(Name = "��erik: ", Description = "En az 100 karakterden olu�an bir�eyler yaz�n..")]
        public string Content { get; set; }

        public string ImageFile { get; set; }
        public Nullable<int> ForumId { get; set; }
        public Nullable<int> WriterId { get; set; }
        public string Seo { get; set; }
        public string Rating { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public Nullable<bool> IsSticky { get; set; }
        public Nullable<bool> IsClosed { get; set; }
        public Nullable<bool> IsApproval { get; set; }
        public Nullable<int> ViewsCount { get; set; }

        [Display(Name = "D�zenlenme Tarihi De�i�tirilsin mi?")]
        public bool ChangeModifyDate { get; set; }
    
        public virtual ICollection<comment> comments { get; set; }
        public virtual forum forum { get; set; }
        public virtual ICollection<forum> forum1 { get; set; }
        public virtual user user { get; set; }
    }
}
