//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OK.CargoTracking.Web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Action
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int FactoryId { get; set; }
        public System.DateTime Date { get; set; }
        public bool IsSuccessful { get; set; }
        public int DeviceId { get; set; }
    
        public virtual Factory Factory { get; set; }
        public virtual Device Device { get; set; }
    }
}
