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
    
    public partial class forum
    {
        public forum()
        {
            this.topics = new HashSet<topic>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageFile { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> ParentForumId { get; set; }
        public Nullable<bool> IsSubForum { get; set; }
        public Nullable<bool> IsApproval { get; set; }
        public string Seo { get; set; }
        public Nullable<int> Sort { get; set; }
        public Nullable<int> ViewsCount { get; set; }
    
        public virtual category category { get; set; }
        public virtual ICollection<topic> topics { get; set; }
    }
}
