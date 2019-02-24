//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OKBlog.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Comment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public bool Approval { get; set; }
        public int PostId { get; set; }
        public string UserIP { get; set; }
        public Nullable<int> AuthorId { get; set; }
    
        public virtual Author Author { get; set; }
        public virtual Post Post { get; set; }
    }
}
