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
    
    public partial class PATIENT_PRESCRPTIONS
    {
        public int ID { get; set; }
        public Nullable<int> APPOINTMENT_ID { get; set; }
        public string DRUG_NAME { get; set; }
        public string DRUG_USAGE { get; set; }
    
        public virtual APPOINTMENT APPOINTMENT { get; set; }
    }
}