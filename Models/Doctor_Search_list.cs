using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DP_Portal.Models
{
    public class Doctor_Search_list
    {
        public int? slot_start { get; set; }
        public int? slot_end { get; set; }
        public List<DP_Portal.USER> doctors { get; set; }
        public DateTime appointment_Date { get; set; }
        public string reason { get; set; }
        public int Available_Doctors { get; set; }
        public List<USER> doctors_list { get; set; }
    }
}