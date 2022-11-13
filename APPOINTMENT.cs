//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DP_Portal
{
    using System;
    using System.Collections.Generic;
    
    public partial class APPOINTMENT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public APPOINTMENT()
        {
            this.APPOINTMENT_DETAILS = new HashSet<APPOINTMENT_DETAILS>();
            this.PATIENT_PRESCRPTIONS = new HashSet<PATIENT_PRESCRPTIONS>();
        }
    
        public int APPOINTMENT_ID { get; set; }
        public Nullable<int> PATIENT_ID { get; set; }
        public Nullable<int> DOCTOR_ID { get; set; }
        public System.DateTime APPOINTMENT_DATE { get; set; }
        public System.DateTime BOOKED_DATE { get; set; }
        public Nullable<bool> IS_ACTIVE { get; set; }
        public Nullable<bool> APPPOINMENT_STATUS { get; set; }
    
        public virtual USER USER { get; set; }
        public virtual USER USER1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<APPOINTMENT_DETAILS> APPOINTMENT_DETAILS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PATIENT_PRESCRPTIONS> PATIENT_PRESCRPTIONS { get; set; }
    }
}
