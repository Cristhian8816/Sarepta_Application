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

        public IActionResult payment(int[] TreatmentId)
        {
            using (UsersContext db = new UsersContext())
            {
                Patient_Payment payment_processes = new Patient_Payment();
                payment_processes.ID = TreatmentId[0]; 
                return View(payment_processes);
            }
        }

        public IActionResult idPatient(Patient_Payment model, string p)
        {
            using (UsersContext db = new UsersContext())
            {
                Treatment_Process treatmentProcess = new Treatment_Process(); 
                List<int> processesIDs = new List<int>();                
                List<string> treatmentsName = new List<string>();
                var identification = db.Patients.Where(c => c.cedula == model.patient.cedula).FirstOrDefault();               
                if (identification != null)
                {
                    ViewBag.messagge = identification.Name;
                    var ProcessesList = db.Processes.Where(c => c.PatientId == identification.PatientId).ToList();

                    foreach (var processIt in ProcessesList)
                    {
                        var treatmentIt = db.Treatments.Where(x => x.TreatmentId == processIt.TreatmentsId).FirstOrDefault();                     
                        treatmentsName.Add(treatmentIt.Name);
                        processesIDs.Add(processIt.ProcessId);
                    }

                    treatmentProcess.IDs = processesIDs;
                    treatmentProcess.Name = treatmentsName;

                    return View(treatmentProcess);
                }
                else
                {
                    ModelState.AddModelError("cedula", "Paciente no encontrado en la base de datos");
                    return View("PaymentPatientID");
                }

            }
        }       

        public IActionResult paymentProcess(Patient_Payment model, int pro_ID)
        {
            using (UsersContext db = new UsersContext())
            {
                var treatmentCost = 0;
                var processIt = db.Processes.Where(x => x.ProcessId == pro_ID).FirstOrDefault();
                if(processIt != null)
                {
                   var  treatmentIt = db.Treatments.Where(x => x.TreatmentId == processIt.TreatmentsId).FirstOrDefault();
                   treatmentCost = treatmentIt.Price;
                }              
                
                var treatmentPayList = db.Payments.Where(x => x.ProcessId == pro_ID).ToList();
                int sumPays = 0;
                List<int> ids = new List<int>();
                foreach (var treatPayList in treatmentPayList)
                {
                    sumPays += treatPayList.Pay;
                    ids.Add(treatPayList.PaymentId); 
                }

                var statusTreatment = treatmentCost - sumPays;
                Payments payment = new Payments();

                if (statusTreatment > 0)
                {
                    var amount_Payments = db.Payments.Count();
                    amount_Payments++;

                    
                    payment.PaymentId = amount_Payments;
                    payment.ProcessId = pro_ID;
                    payment.Pay = model.payment.Pay;
                    payment.PayType = model.payment.PayType;
                    payment.Discount = model.payment.Discount;
                    payment.PayDate = model.payment.PayDate;
                    payment.Status = "should";
                    payment.Finished = 0;

                    db.Payments.Add(payment);
                    db.SaveChanges();
                    return View("PaymentRegisterSucess");
                }
                else
                {
                    foreach (var id in ids)
                    {
                        payment.PaymentId = id;
                        payment.Status = "payment";
                        payment.Finished = 1;

                        db.Payments.Add(payment);
                        db.SaveChanges();
                        ModelState.AddModelError("Status", "El procedimiento esta pagado");
                    }
                    return View("payment");
                }                          
            }               
        }
    }
}