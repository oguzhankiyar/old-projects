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
    
    public partial class Log
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public Nullable<int> ItemId { get; set; }
        public Nullable<int> ItemType { get; set; }
        public System.DateTime ActionDate { get; set; }
    }
}
