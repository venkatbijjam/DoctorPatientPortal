using System;
using System.Collections.Generic;
using DP_Portal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace DP_Portal.Controllers
{
    public class PatientController : Controller
    {
        private DP_PortalEntities db = new DP_PortalEntities();

        // GET: Patient
        public ActionResult PatientDashboard()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user_name = Session["login_user"].ToString();
            var users = db.USERS.Where(a => a.USER_NAME == user_name).Select(a => a).FirstOrDefault();
            var patient_details = db.PATIENT_DETAILS.Where(a => a.PATIENT_ID == users.USER_ID).FirstOrDefault();
            Patient_Dashboard patient = new Patient_Dashboard();
            patient.user_id = users.USER_ID;
            patient.user_name = users.USER_NAME;
            patient.Patient_FIRST_NAME = users.FIRST_NAME;

            patient.patient_LAST_NAME = users.LAST_NAME;
            patient.EMAIL_ID = users.EMAIL_ID;
            patient.Patient_Mobile_Number = users.MOBILE;

            patient.Date_of_Birth = patient_details == null ? null: patient_details.DATE_OF_BIRTH;
            patient.gender = patient_details == null ? null :patient_details.GENDER;
            patient.height = patient_details==null? null:patient_details.HEIGHT;
            patient.weight = patient_details == null ? null: patient_details.WEIGHT;

            patient.ls_appoinments = db.APPOINTMENTS.Where(a => a.PATIENT_ID == users.USER_ID && a.APPPOINMENT_STATUS == false).Select(a => a).ToList();

            return View(patient);
        }



        public ActionResult patientInfoUpdate(int? id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var user_name = Session["login_user"].ToString();
            var users = db.USERS.Where(a => a.USER_NAME == user_name).Select(a => a).FirstOrDefault();
            var patient_details = db.PATIENT_DETAILS.Where(a => a.PATIENT_ID == users.USER_ID).FirstOrDefault();
            Patient_Dashboard patient = new Patient_Dashboard();
            patient.user_id = users.USER_ID;
            patient.user_name = users.USER_NAME;
            patient.Patient_FIRST_NAME = users.FIRST_NAME;

            patient.patient_LAST_NAME = users.LAST_NAME;
            patient.EMAIL_ID = users.EMAIL_ID;
            patient.Patient_Mobile_Number = users.MOBILE;

            patient.Date_of_Birth = patient_details == null ? null : patient_details.DATE_OF_BIRTH;
            patient.gender = patient_details == null ? null : patient_details.GENDER;
            patient.height = patient_details == null ? null : patient_details.HEIGHT;
            patient.weight = patient_details == null ? null : patient_details.WEIGHT;

            return View(patient);
        }

        [HttpPost]
        public ActionResult patientInfoUpdate(Patient_Dashboard patient_info)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var patient= db.USERS.Where(a => a.USER_ID == patient_info.user_id).Select(a => a).FirstOrDefault();

            patient.FIRST_NAME = patient_info.Patient_FIRST_NAME;
            patient.LAST_NAME = patient_info.Patient_FIRST_NAME;
            patient.MOBILE = patient_info.Patient_Mobile_Number;
            patient.EMAIL_ID = patient_info.EMAIL_ID;
            patient.USER_NAME = patient_info.user_name;

            var patient_detail = db.PATIENT_DETAILS.Where(a => a.PATIENT_ID == patient_info.user_id).Select(a => a).FirstOrDefault();
            if(patient_detail!= null)
            {
                patient_detail.GENDER = patient_info.gender;
                patient_detail.HEIGHT = patient_info.height;
                patient_detail.WEIGHT = patient_info.weight;
                patient_detail.DATE_OF_BIRTH = patient_info.Date_of_Birth;
                db.Entry(patient_detail).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                PATIENT_DETAILS patient_detail_insert = new PATIENT_DETAILS();
                patient_detail_insert.GENDER = patient_info.gender;
                patient_detail_insert.HEIGHT = patient_info.height;
                patient_detail_insert.WEIGHT = patient_info.weight;
                patient_detail_insert.DATE_OF_BIRTH = patient_info.Date_of_Birth;
                patient_detail_insert.PATIENT_ID = patient.USER_ID;
                patient.PATIENT_DETAILS.Add(patient_detail_insert);
            }

            db.Entry(patient).State = EntityState.Modified;
            db.SaveChanges();

         
            return RedirectToAction("PatientDashboard", "Patient");
        }

        public ActionResult AllPatientAppointments()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var user_name = Session["login_user"].ToString();
            var users = db.USERS.Where(a => a.USER_NAME == user_name).Select(a => a).FirstOrDefault();

            var appoints_list = db.APPOINTMENTS.Where(a => a.PATIENT_ID == users.USER_ID).Select(a => a).ToList();

            return View(appoints_list);
        }


        public ActionResult Create()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Slots = new SelectList((from s in db.SLOTS.ToList()
                                            select new
                                            {
                                                ID = s.ID,
                                                Slot = s.START_TIME + " ~ " + s.END_TIME
                                            }),
                                            "ID",
                                            "Slot",
                                            null);
            ViewBag.Available_Doctors = null;
            return View();
        }

        public JsonResult DoctorAvailable(Doctor_Search obj)
        {

          
            var doctor_list = db.USERS.Where(a => a.USERS_TYPE.USER_TYPE_NAME == "DOCTOR").Select(a => a).ToList();



            var results = (from appoint in db.APPOINTMENTS
                           join doctor in db.USERS on appoint.USER.USER_ID equals doctor.USER_ID
                           join details in db.APPOINTMENT_DETAILS on appoint.APPOINTMENT_ID equals details.APPOINTMENT_ID
                           select new
                           {
                               appoint.USER.USER_ID,
                               appoint.APPOINTMENT_DATE,
                               details.APPOINTMENT_STARTTIME,
                               details.APPOINTMENT_ENDTIME
                           }).ToList();

            var slot = db.SLOTS.Where(a => a.ID == obj.slot_id).Select(a => a).FirstOrDefault();

            var notAvaiable = results.Where(a => a.APPOINTMENT_DATE == obj.appointment_Date && a.APPOINTMENT_STARTTIME == slot.START_TIME && a.APPOINTMENT_ENDTIME == slot.END_TIME).Select(a => a).ToList();

            List<int> availableDoctor = results.Except(notAvaiable).Select(a => a.USER_ID).Distinct().ToList();
            //ViewBag.Available_Doctors = new SelectList(availableDoctor, "USER_ID", "LAST_NAME");


            return Json(new { result = "Redirect", url = Url.Action("DoctorList", "Patient", new { slot_start = slot.START_TIME, slot_end = slot.END_TIME, appoint_day = obj.appointment_Date }) });

            //return Json(new { result = "Redirect", url = Url.Action("DoctorList", "Patient", new { doctors_list = doc }) });


        }

        public ActionResult DoctorList(int slot_start, int slot_end, DateTime appoint_day)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var doctor_list = db.USERS.Where(a => a.USERS_TYPE.USER_TYPE_NAME == "DOCTOR").Select(a => a).ToList();
            var results = (from appoint in db.APPOINTMENTS
                           join doctor in db.USERS on appoint.USER.USER_ID equals doctor.USER_ID
                           join details in db.APPOINTMENT_DETAILS on appoint.APPOINTMENT_ID equals details.APPOINTMENT_ID
                           select new
                           {
                               appoint.USER.USER_ID,
                               appoint.USER,
                              
                               appoint.APPOINTMENT_DATE,
                               details.APPOINTMENT_STARTTIME,
                               details.APPOINTMENT_ENDTIME
                           }).ToList();


            var notAvaiable = results.Where(a => a.APPOINTMENT_DATE == appoint_day && a.APPOINTMENT_STARTTIME == slot_start && a.APPOINTMENT_ENDTIME == slot_end).Select(a => a).ToList();

            var availableDoctors = results.Except(notAvaiable).Select(a => a.USER).Distinct().ToList();

            Doctor_Search_list doc = new Doctor_Search_list();

            doc.slot_end = slot_end;
            doc.slot_start = slot_start;
            doc.appointment_Date = appoint_day;
            doc.doctors = availableDoctors;
            ViewBag.Available_Doctors = new SelectList(availableDoctors, "USER_ID", "LAST_NAME");
            doc.doctors_list = availableDoctors;


            return View(doc);
        }

        [HttpPost]
        public ActionResult DoctorList(Doctor_Search_list doc)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            APPOINTMENT app = new APPOINTMENT();

            app.APPOINTMENT_DATE = doc.appointment_Date;
            app.APPPOINMENT_STATUS = true;
            app.BOOKED_DATE = DateTime.Now;
            app.DOCTOR_ID = doc.Available_Doctors;
            app.PATIENT_ID = (int)Session["user_id"];
            app.APPPOINMENT_STATUS = false;
            app.IS_ACTIVE = true;
            APPOINTMENT_DETAILS app_detail = new APPOINTMENT_DETAILS();

            app_detail.APPOINTMENT_ID = app.APPOINTMENT_ID;
            app_detail.APPOINTMENT_STARTTIME = (int)doc.slot_start;
            app_detail.APPOINTMENT_ENDTIME = (int)doc.slot_end;
            app_detail.DISEASE_DETAILS = doc.reason;

            app.APPOINTMENT_DETAILS.Add(app_detail);

            db.APPOINTMENTS.Add(app);
            db.SaveChanges();

            return RedirectToAction("PatientDashboard", "Patient");
        }


        public ActionResult Cancel(int appointment_id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var appoint_Delete = db.APPOINTMENTS.Where(a => a.APPOINTMENT_ID == appointment_id).Select(a => a).FirstOrDefault();
            appoint_Delete.IS_ACTIVE = false;
            db.Entry(appoint_Delete).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("AllPatientAppointments", "Patient");
        }

       
        public ActionResult OrganDonationCreate()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult OrganDonationCreate(ORGAN_DONORS organDonar)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                organDonar.ORGAN_DONATION_SIGNED_DATE = DateTime.Now;
                organDonar.ORGAN_USER_ID = (int)Session["user_id"];
                organDonar.ORGAN_DONOR_RECORD_ACTIVE = true;
                db.ORGAN_DONORS.Add(organDonar);
                db.SaveChanges();
                return RedirectToAction("OrganDonationList");
            }

            return View();
        }

        public ActionResult OrganDonationList()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            int user_id = (int)Session["user_id"];
            var results = db.ORGAN_DONORS.Where(a => a.ORGAN_USER_ID == user_id).Select(a => a).ToList();
            return View(results);
        }


        public ActionResult OrganDonationEdit(int? id)
        {

            var results = db.ORGAN_DONORS.Where(a => a.ORGAN_DONOR_ID == id).Select(a => a).FirstOrDefault();
            return View(results);
        }

        [HttpPost]
        public ActionResult OrganDonationEdit(ORGAN_DONORS organDonar)
        {

            if (ModelState.IsValid)
            {
                organDonar.UPDATE_DATE = DateTime.Now;
                organDonar.ORGAN_DONOR_RECORD_ACTIVE = true;
                db.ORGAN_DONORS.Add(organDonar);
                db.SaveChanges();
                return RedirectToAction("OrganDonationList");
            }
            return RedirectToAction("OrganDonationList");

        }

        public ActionResult OrganDonationOptOut(int? id)
        {
            var organ_donar = db.ORGAN_DONORS.Where(a => a.ORGAN_DONOR_ID == id).Select(a => a).FirstOrDefault();
            organ_donar.ORGAN_DONOR_RECORD_ACTIVE = false;
            organ_donar.UPDATE_DATE = DateTime.Now;
            db.Entry(organ_donar).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("OrganDonationList");
        }

        public ActionResult AllOrgan_List(string searchString=null)
        {
            var results = db.ORGAN_DONORS.Where(a => a.ORGAN_DONOR_RECORD_ACTIVE == true).Select(a => a).ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                results = results.Where(s => s.ORGAN_DONOR_ADDRESSS.Contains(searchString)
                                       || s.ORGAN_DONOR_CONTACT.Contains(searchString)
                                       || s.ORGAN_DONOR_EMAIL.Contains(searchString)
                                       || s.ORGAN_DONATING_ORGAN.Contains(searchString) 
                                       || s.ORGAN_DONOR_NAME.Contains(searchString)).ToList();
            }
            return View(results);
            
        }
    }
}