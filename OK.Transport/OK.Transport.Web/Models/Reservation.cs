//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OK.Transport.Web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reservation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Reservation()
        {
            this.ReservationActions = new HashSet<ReservationAction>();
        }
    
        public int Id { get; set; }
        public string Code { get; set; }
        public int JourneyId { get; set; }
        public int PaymentId { get; set; }
    
        public virtual Payment Payment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReservationAction> ReservationActions { get; set; }
        public virtual Journey Journey { get; set; }
    }
}
