//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OKComplex.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class club_menulink
    {
        public int Id { get; set; }

        [Display(Name = "Men� Ad�")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Javascript Adresi")]
        [Required]
        public string OnClick { get; set; }

        [Display(Name = "�st Men� / Alt Men�")]
        public Nullable<int> Type { get; set; }

        [Display(Name = "S�ra")]
        public Nullable<int> Sort { get; set; }

        [Display(Name = "Onay Durumu")]
        public Nullable<bool> IsApproval { get; set; }
    }
}