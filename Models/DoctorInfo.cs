using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DP_Portal.Models
{
    public class DoctorInfo
    {
        public int? Mobile_Number { get; set; }
        public string EMAIL_ID { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string user_name { get; set; }
        public int? user_id { get; set; }
        public string Mcode { get; set; }
        public string Specality { get; set; }
        public string address { get; set; }
    }
}