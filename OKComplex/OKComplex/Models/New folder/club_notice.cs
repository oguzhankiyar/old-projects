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
    
    public partial class club_notice
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public Nullable<System.DateTime> WritingDate { get; set; }
        public string Seo { get; set; }
        public Nullable<int> Sort { get; set; }
        public bool IsApproval { get; set; }
    }
}
