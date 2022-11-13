using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DP_Portal.Models
{
    public class AppointmentDetails
    {
        public int? Appointment_Id { get; set; }
        public int? Patient_Mobile_Number { get; set; }
        public string EMAIL_ID { get; set; }
        public string Patient_FIRST_NAME { get; set; }
        public string patient_LAST_NAME { get; set; }
        public string Consulting_Doctor { get; set; }
        public int? Consulting_Doctor_Contact { get; set; }

        public string Date_of_Birth { get; set; }
        public string gender { get; set; }
        public string height { get; set; }
        public string weight { get; set; }
        public DateTime Appointment_Date { get; set; }
        public int Appointment_start_time { get; set; }
        public int Appointment_end_time { get; set; }
        public string Disease_Details { get; set; }
        public List<pres> list_patient_prescriptions { get; set; }
        
    }
}