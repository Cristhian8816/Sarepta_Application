using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sarepta_WebApplication1.Models;

namespace Sarepta_WebApplication1.Controllers
{
    public class PaymentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }      

        public IActionResult PaymentPatientID()
        {
            return View();
        }

        public IActionResult SareptaPayments()
        {
            Patients patient = new Patients();            
            Processes process = new Processes();
            Payments payment = new Payments();

            Patient_Payment patient_payment = new Patient_Payment();

            UsersContext db = new UsersContext();

            ViewBag.Sessionv = ((System.Security.Claims.ClaimsIdentity)((Microsoft.AspNetCore.Http.DefaultHttpContext)HttpContext).User.Identity).Name;
            return View(patient_payment);
        }

        public IActionResult payment(Patient_Payment model, string index)
        {
            using (UsersContext db = new UsersContext())
            {
                Patient_Payment patient_Payment = new Patient_Payment();
                patient_Payment.treatment.Name = index;
                patient_Payment.process.ProcessId = model.process.ProcessId;
                

                return View(patient_Payment);
            }
        }

        public IActionResult idPatient(Patient_Payment model, string p)
        {
            using (UsersContext db = new UsersContext())
            {             
                List<Treatments> treatmentsName = new List<Treatments>();
                var identification = db.Patients.Where(c => c.cedula == model.patient.cedula).FirstOrDefault();               
                if (identification != null)
                {
                    ViewBag.messagge = identification.Name;
                    var ProcessesList = db.Processes.Where(c => c.PatientId == identification.PatientId).ToList();

                    foreach (var processIt in ProcessesList)
                    {
                        var treatmentIt = db.Treatments.Where(x => x.TreatmentId == processIt.TreatmentsId).FirstOrDefault();                     
                        treatmentsName.Add(treatmentIt);
                    }                

                    return View(treatmentsName);
                }
                else
                {
                    ModelState.AddModelError("cedula", "Paciente no encontrado en la base de datos");
                    return View("PaymentPatientID");
                }

            }
        }

        public IActionResult paymentProcess(Patient_Payment model)
        {
            using (UsersContext db = new UsersContext())
            {              
                var amount_Payments = db.Payments.Count();
                amount_Payments++;

                Payments payment = new Payments();
                payment.PaymentId = amount_Payments;
                payment.ProcessId = model.process.ProcessId;
                payment.Pay = model.payment.Pay;
                payment.Status = model.payment.Status;
                payment.Discount = model.payment.Discount;
                payment.PayDate = model.payment.PayDate;

                db.Payments.Add(payment);
                db.SaveChanges();
            }
                return View();
        }
    }
}