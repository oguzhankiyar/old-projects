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
    
    public partial class club_postcomment
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public Nullable<int> PostId { get; set; }
        public Nullable<int> MemberId { get; set; }
        public string WriterName { get; set; }
        public Nullable<System.DateTime> WritingDate { get; set; }
        public bool IsApproval { get; set; }
    
        public virtual club_member club_member { get; set; }
        public virtual club_post club_post { get; set; }
    }
}
