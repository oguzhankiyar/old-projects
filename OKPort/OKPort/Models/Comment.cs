//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OKPort.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Comment
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public int PostID { get; set; }
        public int WriterID { get; set; }
        public System.DateTime WritingDate { get; set; }
        public bool IsApproval { get; set; }
    
        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
