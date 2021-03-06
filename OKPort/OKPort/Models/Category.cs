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
    
    public partial class Category
    {
        public Category()
        {
            this.Posts = new HashSet<Post>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortURL { get; set; }
        public Nullable<int> ParentID { get; set; }
        public int SectionID { get; set; }
        public int Sort { get; set; }
        public bool IsApproval { get; set; }
    
        public virtual Section Section { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
