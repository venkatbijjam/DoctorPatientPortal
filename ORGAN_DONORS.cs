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
    
    public partial class ORGAN_DONORS
    {
        public int ORGAN_DONOR_ID { get; set; }
        public string ORGAN_DONOR_NAME { get; set; }
        public string ORGAN_DONOR_CONTACT { get; set; }
        public string ORGAN_DONATING_ORGAN { get; set; }
        public Nullable<System.DateTime> ORGAN_DONATION_SIGNED_DATE { get; set; }
        public string ORGAN_DONOR_ADDRESSS { get; set; }
        public string ORGAN_DONOR_EMAIL { get; set; }
        public Nullable<bool> ORGAN_DONOR_RECORD_ACTIVE { get; set; }
        public Nullable<System.DateTime> UPDATE_DATE { get; set; }
        public Nullable<int> ORGAN_USER_ID { get; set; }
    
        public virtual USER USER { get; set; }
    }
}
