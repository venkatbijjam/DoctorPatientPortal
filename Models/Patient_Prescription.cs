using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DP_Portal.Models
{
    public class Patient_Prescription
    {
        public int ID { get; set; }
        public Nullable<int> APPOINTMENT_ID { get; set; }
        public string DRUG_NAME { get; set; }
        public string DRUG_USAGE { get; set; }

    }
}