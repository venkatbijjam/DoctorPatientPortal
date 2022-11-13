using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DP_Portal.Models
{
    public class AdminDashboard
    {
        public int doctor_count { get; set; }
        public int patient_count { get; set; }
        public int admin_count { get; set; }
        public int total_count { get; set; }
        public List<USER> users_details { get; set; }
    }
}