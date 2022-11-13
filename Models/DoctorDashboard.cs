using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DP_Portal.Models
{
    public class DoctorDashboard
    {
  


        public int today_appointments { get; set; }
        public int tommorow_appointments { get; set; }
        public int weekly_appointment { get; set; }
        public int upcoming_appointments { get; set; }
        public List<APPOINTMENT> appointment_details { get; set; }
    }
}