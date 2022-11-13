using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DP_Portal.Models
{
    public class Patient_Dashboard
    {
       
        public int? Patient_Mobile_Number { get; set; }
        public string EMAIL_ID { get; set; }
        public string Patient_FIRST_NAME { get; set; }
        public string patient_LAST_NAME { get; set; }
        public DateTime? Date_of_Birth { get; set; }
        public string gender { get; set; }
        public string height { get; set; }
        public string weight { get; set; }
        public string user_name { get; set; }
        public int? user_id { get; set; }
        public List<APPOINTMENT> ls_appoinments { get; set; }

    }
}