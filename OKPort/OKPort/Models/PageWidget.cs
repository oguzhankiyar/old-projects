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
    
    public partial class PageWidget
    {
        public int ID { get; set; }
        public Nullable<int> SectionID { get; set; }
        public int WidgetID { get; set; }
        public int Position { get; set; }
        public string Region { get; set; }
        public int Sort { get; set; }
        public int PageID { get; set; }
    
        public virtual Section Section { get; set; }
        public virtual Widget Widget { get; set; }
        public virtual Page Page { get; set; }
    }
}
