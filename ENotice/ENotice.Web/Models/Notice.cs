//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ENotice.Web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Notice
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public int WriterId { get; set; }
        public int Type { get; set; }
        public int Priority { get; set; }
        public string FacultyIds { get; set; }
        public string DepartmentIds { get; set; }
        public string MonitorIds { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public Nullable<System.DateTime> ModifiedAt { get; set; }
        public bool IsApproved { get; set; }
    }
}