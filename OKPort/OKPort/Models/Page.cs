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
    
    public partial class Page
    {
        public Page()
        {
            this.Links = new HashSet<Link>();
            this.Sections = new HashSet<Section>();
            this.Subscriptions = new HashSet<Subscription>();
            this.PageWidgets = new HashSet<PageWidget>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public int OwnerID { get; set; }
        public string ShortURL { get; set; }
        public int ThemeID { get; set; }
        public System.DateTime CreationDate { get; set; }
        public bool IsApproval { get; set; }
        public string IndexTemplate { get; set; }
        public string LayoutTemplate { get; set; }
    
        public virtual ICollection<Link> Links { get; set; }
        public virtual Theme Theme { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Section> Sections { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
        public virtual ICollection<PageWidget> PageWidgets { get; set; }
    }
}
