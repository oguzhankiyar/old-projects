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
    
    public partial class club_groupmember
    {
        public int Id { get; set; }
        public Nullable<int> MemberId { get; set; }
        public Nullable<int> GroupId { get; set; }
        public Nullable<System.DateTime> RegistrationDate { get; set; }
        public Nullable<int> Role { get; set; }
    
        public virtual club_member club_member { get; set; }
        public virtual club_group club_group { get; set; }
    }
}
