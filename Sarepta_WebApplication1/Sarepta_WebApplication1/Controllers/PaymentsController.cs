using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Sarepta_WebApplication1.Models;

namespace Sarepta_WebApplication1.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly IHostingEnvironment _env;

        public PaymentsController(IHostingEnvironment env)
        {
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }      

        public IActionResult PaymentPatientID()
        {
            return View();
        }

        public IActionResult idPatient(Patient_Payment model, string p)
        {
            using (UsersContext db = new UsersContext())
            {
                Treatment_Process treatmentProcess = new Treatment_Process();
                List<int> processesIDs = new List<int>();
                List<int> paymentsProcess = new List<int>();
                List<string> treatmentsName = new List<string>();
                var identification = db.Patients.Where(c => c.cedula == model.patient.cedula).FirstOrDefault();
                if (identification != null)
                {
                    ViewBag.messagge = identification.Name;
                    var ProcessesList = db.Processes.Where(c => c.PatientId == identification.PatientId).ToList();
                    var i = 0;
                    foreach (var processIt in ProcessesList)
                    {
                        var treatmentIt = db.Treatments.Where(x => x.TreatmentId == processIt.TreatmentsId).FirstOrDefault();
                        treatmentsName.Add(treatmentIt.Name);
                        processesIDs.Add(processIt.ProcessId);

                        var shouldAmount = 0;
                        var payAmount = 0;
                        var PaymentList = db.Payments.Where(z => z.ProcessId == processesIDs[i]).ToList();
                        foreach (var paymentIt in PaymentList)
                        {
                            payAmount += paymentIt.Pay;
                        }
                        shouldAmount = processIt.real_Cost - payAmount;
                        paymentsProcess.Add(shouldAmount);
                        i++;
                    }

                    treatmentProcess.IDs = processesIDs;
                    treatmentProcess.Name = treatmentsName;
                    treatmentProcess.paymentsProcess = paymentsProcess;

                    return View(treatmentProcess);
                }
                else
                {
                    ModelState.AddModelError("cedula", "Paciente no encontrado en la base de datos");
                    return View("PaymentPatientID");
                }
            }
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

        public IActionResult paymentProcess(Patient_Payment model, int pro_ID)        {
            
            using (UsersContext db = new UsersContext())
            {                
                var processIt = db.Processes.Where(x => x.ProcessId == pro_ID).FirstOrDefault();                

                var  treatmentIt = db.Treatments.Where(x => x.TreatmentId == processIt.TreatmentsId).FirstOrDefault();                                            
                
                var treatmentPayList = db.Payments.Where(x => x.ProcessId == pro_ID).ToList();

                var patientIt = db.Patients.Where(x => x.PatientId == processIt.PatientId).FirstOrDefault();

                model.patient = patientIt;
                model.treatment = treatmentIt;
                int sumPays = 0;                
                foreach (var treatPayList in treatmentPayList)
                {
                    sumPays += treatPayList.Pay;                    
                }

                var statusTreatment = processIt.real_Cost - sumPays;
                model.amountPending = statusTreatment -model.payment.Pay;

                Payments payment = new Payments();
                Notifications emailNotification = new Notifications();

                string webRootPath = _env.WebRootPath;

                if (statusTreatment - model.payment.Pay > 0)
                {                    
                    var amount_Payments = db.Payments.Count();                    
                    amount_Payments++;

                    payment.PaymentId = amount_Payments;
                    payment.ProcessId = pro_ID;
                    payment.Pay = model.payment.Pay;
                    payment.PayType = model.payment.PayType;
                    if (treatmentPayList.Count() == 0)
                    {
                        var discount = treatmentIt.Price - processIt.real_Cost;
                        payment.Discount = discount;
                    }
                    else
                    {
                        payment.Discount = 0;
                    }                    
                    payment.PayDate = model.payment.PayDate;
                    payment.Status = "should";
                    payment.Finished = 0;

                    db.Payments.Add(payment);
                    db.SaveChanges();

                    emailNotification.SendEmailUser(model, webRootPath);
                    emailNotification.SendEmailHost(model, webRootPath);
                    return View("PaymentRegisterSucess");
                    //return Json(a);
                }
                else if(statusTreatment - model.payment.Pay == 0)
                {                    
                    var amount_Payments = db.Payments.Count();                    
                    amount_Payments++;

                    payment.PaymentId = amount_Payments;
                    payment.ProcessId = pro_ID;
                    payment.Pay = model.payment.Pay;
                    payment.PayType = model.payment.PayType;
                    if (treatmentPayList.Count() == 0)
                    {
                        var discount = treatmentIt.Price - processIt.real_Cost;
                        payment.Discount = discount;
                    }
                    else
                    {
                        payment.Discount = 0;
                    }
                    payment.PayDate = model.payment.PayDate;
                    payment.Status = "payment";
                    payment.Finished = 1;

                    

                    db.Payments.Add(payment);
                    db.SaveChanges();
                    emailNotification.SendEmailUser(model, webRootPath);
                    emailNotification.SendEmailHost(model, webRootPath);

                    foreach (var treatPayList in treatmentPayList)
                    {
                        treatPayList.Status = "payment";
                        treatPayList.Finished = 1;

                        db.SaveChanges();                        
                    }                    
                    return View("PaymentRegisterSucess");
                }
                else
                {
                    Patient_Payment payment_processes = new Patient_Payment();
                    payment_processes.ID = pro_ID;                    
                    ModelState.AddModelError("Status", "El monto que se esta pagando no puede ser mayor al saldo");
                    return View("payment", payment_processes);                         
                }                                         
            }               
        }        
    }
}