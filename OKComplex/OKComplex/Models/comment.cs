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
    
    public partial class comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public Nullable<int> TopicId { get; set; }
        public Nullable<int> WriterId { get; set; }
        public string Rating { get; set; }
        public string QuoteId { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public Nullable<bool> IsApproval { get; set; }
        public bool ChangeModifyDate { get; set; }

        public virtual topic topic { get; set; }
        public virtual user user { get; set; }
    }
}