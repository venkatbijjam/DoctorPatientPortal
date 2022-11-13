using DP_Portal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DP_Portal.Controllers
{
    public class DoctorController : Controller
    {
        private DP_PortalEntities db = new DP_PortalEntities();

       
        public ActionResult DoctorDashboard(string code = null)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Models.DoctorDashboard doctor = new DoctorDashboard();
            DateTime aDate = DateTime.Now;
            var user = (int)Session["user_id"];
            //var results = db.USERS.Where(a => a.ISACTIVE == true).Select(a => a).ToList();
            var results = db.APPOINTMENTS.Where(a => a.IS_ACTIVE == true && a.USER.USER_ID == user).Select(a => a).ToList();

            doctor.today_appointments = results.Where(a => a.APPOINTMENT_DATE.ToString("MM/dd/yyyy") == aDate.ToString("MM/dd/yyyy")).Count();
            doctor.tommorow_appointments = results.Where(a => a.APPOINTMENT_DATE.ToString("MM/dd/yyyy")  == aDate.AddDays(+1).ToString("MM/dd/yyyy")).Count();
            doctor.weekly_appointment = results.Where( a => a.APPOINTMENT_DATE >= aDate && a.APPOINTMENT_DATE <=aDate.AddDays(+7) ).Count();
            doctor.upcoming_appointments= results.Where(a => a.APPOINTMENT_DATE >= aDate).Count();

            doctor.appointment_details = results.Where(a => a.APPOINTMENT_DATE >= aDate).ToList();
            if (code == "TODAY")
            {
                doctor.appointment_details = results.Where(a => a.APPOINTMENT_DATE.ToString("MM/dd/yyyy") == aDate.ToString("MM/dd/yyyy")).ToList();
            }
            else if (code == "TMR")
            {
                doctor.appointment_details = results.Where(a => a.APPOINTMENT_DATE.ToString("MM/dd/yyyy") == aDate.AddDays(+1).ToString("MM/dd/yyyy")).ToList();

            }
            else if (code == "WEEK")
            {
                doctor.appointment_details = results.Where(a => a.APPOINTMENT_DATE >= aDate && a.APPOINTMENT_DATE <= aDate.AddDays(+7)).ToList();

            }

            //doctor.tommorow_appointments = results.Where(a => a.USERS_TYPE.USER_TYPE_NAME == "PATIENT").Count();
            //doctor.weekly_appointment = results.Where(a => a.USERS_TYPE.USER_TYPE_NAME == "ADMIN").Count();
            //admin.total_count = results.Count();
            //if (code == "" || code == null)
            //{
            //    admin.users_details = results;
            //}
            //else
            //{
            //    admin.users_details = results.Where(a => a.USERS_TYPE.USER_TYPE_NAME == code).Select(a => a).ToList();

            //}


            return View(doctor);
        }


        public ActionResult DoctorInfo()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var user = (int)Session["user_id"];

            Models.DoctorInfo doc = new DoctorInfo();

            var doc_info = db.USERS.Where(a => a.USER_ID == user).Select(a => a).FirstOrDefault();
            var doc_spec_info = db.DOCTOR_SPEC.Where(a => a.DOCTOR_ID == user).Select(a => a).FirstOrDefault();

            doc.FIRST_NAME = doc_info.FIRST_NAME;
            doc.LAST_NAME = doc_info.LAST_NAME;
            doc.Mobile_Number = doc_info.MOBILE;
            doc.EMAIL_ID = doc_info.EMAIL_ID;
            doc.user_id = doc_info.USER_ID;
            doc.user_name = doc_info.USER_NAME;

            doc.Specality = doc_spec_info == null ? null : doc_spec_info.DOCTOR_SPEC1;
            doc.address = doc_spec_info == null ? null : doc_spec_info.DOCTOR_ADDRESS;
            doc.Mcode = doc_spec_info == null ? null : doc_spec_info.DOCTOR_MCODE;

            return View(doc);
        }

        public ActionResult DoctorInfoEdit()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var user = (int)Session["user_id"];

            Models.DoctorInfo doc = new DoctorInfo();

            var doc_info = db.USERS.Where(a => a.USER_ID == user).Select(a => a).FirstOrDefault();
            var doc_spec_info = db.DOCTOR_SPEC.Where(a => a.DOCTOR_ID == user).Select(a => a).FirstOrDefault();

            doc.FIRST_NAME = doc_info.FIRST_NAME;
            doc.LAST_NAME = doc_info.LAST_NAME;
            doc.Mobile_Number = doc_info.MOBILE;
            doc.EMAIL_ID = doc_info.EMAIL_ID;
            doc.user_id = doc_info.USER_ID;
            doc.user_name = doc_info.USER_NAME;

            doc.Specality = doc_spec_info == null ? null : doc_spec_info.DOCTOR_SPEC1;
            doc.address = doc_spec_info == null ? null : doc_spec_info.DOCTOR_ADDRESS;
            doc.Mcode = doc_spec_info == null ? null : doc_spec_info.DOCTOR_MCODE;

            return View(doc);
        }

        [HttpPost]
        public ActionResult DoctorInfoEdit(DoctorInfo doc)
        {
          
            var user = (int)Session["user_id"];

          

            var doc_info = db.USERS.Where(a => a.USER_ID == user).Select(a => a).FirstOrDefault();

            doc_info.FIRST_NAME = doc.FIRST_NAME;
            doc_info.LAST_NAME = doc.LAST_NAME;
            doc_info.MOBILE = doc.Mobile_Number;
            doc_info.EMAIL_ID = doc.EMAIL_ID;
            doc_info.USER_NAME = doc.user_name;
            var doc_spec = db.DOCTOR_SPEC.Where(a => a.DOCTOR_ID == user).Select(a => a).FirstOrDefault();
            if(doc_spec == null)
            {
                DOCTOR_SPEC spec = new DOCTOR_SPEC();
                spec.DOCTOR_ID = doc_info.USER_ID;
                spec.DOCTOR_ADDRESS = doc.address;
                spec.DOCTOR_MCODE = doc.Mcode;
                spec.DOCTOR_SPEC1 = doc.Specality;
                doc_info.DOCTOR_SPEC.Add(spec);
            }
            else
            {


                doc_spec.DOCTOR_ADDRESS = doc.address;
                doc_spec.DOCTOR_MCODE = doc.Mcode;
                doc_spec.DOCTOR_SPEC1 = doc.Specality;
                db.Entry(doc_spec).State = EntityState.Modified;
                db.SaveChanges();
            }

            db.Entry(doc_info).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DoctorInfo", "Doctor");
        }

        
        

        public ActionResult DoctorAppointment()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var user = (int)Session["user_id"];
            var results = db.APPOINTMENTS.Where(a=>a.USER.USER_ID == user).Select(a => a).ToList();
            return View(results);
        }

        public ActionResult DoctorPastAppointments()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            DateTime aDate = DateTime.Now;
            var user=(int)Session["user_id"];
            var results = db.APPOINTMENTS.Where(a => a.IS_ACTIVE == true && a.USER.USER_ID == user).Select(a => a).ToList();

            var data = results.Where(a => a.APPOINTMENT_DATE < aDate).ToList();

            return View(data);
        }

        public ActionResult DoctorFutureAppointments()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            DateTime aDate = DateTime.Now;
            var user = (int)Session["user_id"];
            var results = db.APPOINTMENTS.Where(a => a.IS_ACTIVE == true && a.USER.USER_ID == user).Select(a => a).ToList();

            var data = results.Where(a => a.APPOINTMENT_DATE >= aDate).ToList();

            return View(data);
        }


        public ActionResult DoctorMissedAppointments()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            DateTime aDate = DateTime.Now;
            var user = (int)Session["user_id"];
            var results = db.APPOINTMENTS.Where(a => a.IS_ACTIVE == true && a.USER.USER_ID == user).Select(a => a).ToList();

            var data = results.Where(a => a.APPOINTMENT_DATE < aDate && a.APPPOINMENT_STATUS ==false).ToList();

            return View(data);
        }
        public ActionResult Details(int? Appointment_id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            AppointmentDetails appointmentDetails = new AppointmentDetails();
            var results= (from appoint in db.APPOINTMENTS
                          join patient_detail in db.PATIENT_DETAILS on appoint.USER1.USER_ID equals patient_detail.PATIENT_ID
             join details in db.APPOINTMENT_DETAILS on appoint.APPOINTMENT_ID equals details.APPOINTMENT_ID 
             select new
             {
                 appoint.USER,
                 appoint.USER1,
                 appoint.BOOKED_DATE,
                 appoint.APPOINTMENT_DATE,
                 appoint.APPOINTMENT_ID,
                 details.APPOINTMENT_STARTTIME,
                 details.APPOINTMENT_ENDTIME,
                 details.DISEASE_DETAILS,
                 patient_detail.GENDER,
                 patient_detail.DATE_OF_BIRTH,
                 patient_detail.HEIGHT,
                 patient_detail.WEIGHT
             }).Where(a=>a.APPOINTMENT_ID == Appointment_id).FirstOrDefault();
            
            appointmentDetails.Patient_FIRST_NAME = results.USER1.FIRST_NAME;
            appointmentDetails.patient_LAST_NAME = results.USER1.LAST_NAME;
            appointmentDetails.Consulting_Doctor = results.USER.FIRST_NAME + " " + results.USER1.LAST_NAME;
            appointmentDetails.Consulting_Doctor_Contact = results.USER.MOBILE;
            appointmentDetails.EMAIL_ID = results.USER1.EMAIL_ID;
            appointmentDetails.Patient_Mobile_Number = results.USER1.MOBILE;

            appointmentDetails.Appointment_Id = Appointment_id;
            appointmentDetails.Appointment_Date = results.APPOINTMENT_DATE;
            appointmentDetails.Appointment_start_time = results.APPOINTMENT_STARTTIME;
            appointmentDetails.Appointment_end_time = results.APPOINTMENT_ENDTIME;
            appointmentDetails.Disease_Details = results.DISEASE_DETAILS;

            appointmentDetails.Date_of_Birth = results.DATE_OF_BIRTH.ToString();
            appointmentDetails.gender = results.GENDER;
            appointmentDetails.height = results.HEIGHT;
            appointmentDetails.weight = results.WEIGHT;
            var pres = db.PATIENT_PRESCRPTIONS.Where(a => a.APPOINTMENT_ID == Appointment_id).Select(a => a).ToList();
            List<pres> ls_p = new List<pres>();

            foreach (var item in pres)
            {

                pres p = new pres();
                p.DRUG_USAGE = item.DRUG_USAGE;
                p.DRUG_NAME = item.DRUG_NAME;

                ls_p.Add(p);

            }
            appointmentDetails.list_patient_prescriptions = ls_p;
            return View(appointmentDetails);
        }

        public ActionResult Prescription()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public JsonResult Prescription(List<PATIENT_PRESCRPTIONS> prescrption)
        {
           
            foreach (var item in prescrption)
            {
                db.PATIENT_PRESCRPTIONS.Add(item);
                db.SaveChanges();
            }

           var appoint_id= prescrption.Select(a => a.APPOINTMENT_ID).Distinct().FirstOrDefault();
            var results = db.APPOINTMENTS.Where(a=>a.APPOINTMENT_ID== appoint_id).Select(a => a).FirstOrDefault();
            results.APPPOINMENT_STATUS = true;

            db.Entry(results).State = EntityState.Modified;
            db.SaveChanges();



            return Json(new { result = "Redirect", url = Url.Action("Prescription_print", "Doctor", new { id = prescrption.Select(a=>a.APPOINTMENT_ID).FirstOrDefault()}) });


        }


        public ActionResult Prescription_print(int id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            AppointmentDetails appointmentDetails = new AppointmentDetails();
            var results = (from appoint in db.APPOINTMENTS
                           join patient_detail in db.PATIENT_DETAILS on appoint.USER1.USER_ID equals patient_detail.PATIENT_ID
                           join details in db.APPOINTMENT_DETAILS on appoint.APPOINTMENT_ID equals details.APPOINTMENT_ID
                           select new
                           {
                               appoint.USER,
                               appoint.USER1,
                               appoint.BOOKED_DATE,
                               appoint.APPOINTMENT_DATE,
                               appoint.APPOINTMENT_ID,
                               details.APPOINTMENT_STARTTIME,
                               details.APPOINTMENT_ENDTIME,
                               details.DISEASE_DETAILS,
                               patient_detail.GENDER,
                               patient_detail.DATE_OF_BIRTH,
                               patient_detail.HEIGHT,
                               patient_detail.WEIGHT
                           }).Where(a=>a.APPOINTMENT_ID==id).FirstOrDefault();


            var pres = db.PATIENT_PRESCRPTIONS.Where(a => a.APPOINTMENT_ID == id).Select(a => a).ToList();
            appointmentDetails.Patient_FIRST_NAME = results.USER1.FIRST_NAME;
            appointmentDetails.patient_LAST_NAME = results.USER1.LAST_NAME;
            appointmentDetails.Consulting_Doctor = results.USER.FIRST_NAME + " " + results.USER1.LAST_NAME;
            appointmentDetails.Consulting_Doctor_Contact = results.USER.MOBILE;
          
            appointmentDetails.Patient_Mobile_Number = results.USER1.MOBILE;

            appointmentDetails.Appointment_Id = id;
            appointmentDetails.Appointment_Date = DateTime.UtcNow;
      
            appointmentDetails.Disease_Details = results.DISEASE_DETAILS;

            List<pres> ls_p = new List<pres>();

            foreach (var item in pres)
            {

                pres p = new pres();
                p.DRUG_USAGE = item.DRUG_USAGE;
                p.DRUG_NAME = item.DRUG_NAME;

                ls_p.Add(p);
                
            }
            appointmentDetails.list_patient_prescriptions =ls_p;
            return View(appointmentDetails);
        }
    }
}