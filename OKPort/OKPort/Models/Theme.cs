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
    
    public partial class Theme
    {
        public Theme()
        {
            this.Pages = new HashSet<Page>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Styles { get; set; }
        public int OwnerID { get; set; }
        public bool IsApproval { get; set; }
    
        public virtual ICollection<Page> Pages { get; set; }
        public virtual User User { get; set; }
    }
}
