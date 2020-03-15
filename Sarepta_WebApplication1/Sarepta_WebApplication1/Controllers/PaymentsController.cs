using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
                int sumPays = 0;                
                foreach (var treatPayList in treatmentPayList)
                {
                    sumPays += treatPayList.Pay;                    
                }

                var statusTreatment = processIt.real_Cost - sumPays;
                
                Payments payment = new Payments();

                
                if(statusTreatment - model.payment.Pay > 0)
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
                   
                    SendEmailUser(model);
                    return View("PaymentRegisterSucess");
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
                    SendEmailUser(model);

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

        public static string SendEmailUser(Patient_Payment model)
        {
            var hora = DateTime.Now;
            var message = "";
            try
            {
                var mail = new MailMessage();
                mail.From = new MailAddress("sarepta.odontologia@gmail.com", "Sarepta Consultory");                
                mail.To.Add(model.patient.Email);
                mail.Subject = "Notificacion de  " + "PAGO";
                mail.IsBodyHtml = true;
                //mail.Body = "Se notifica que se ha realizado un pago a las: " + hora;
                mail.Body = htmlEmailRow(model);

                var SmtpServer = new SmtpClient("smtp.gmail.com");
                SmtpServer.Port = 587;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.UseDefaultCredentials = false;

                // Agrega tu correo y tu contraseña, hemos usado el servidor de Outlook.
                SmtpServer.Credentials = new System.Net.NetworkCredential("sarepta.odontologia@gmail.com", "piel2802");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                message = "Email enviado con exito";
            }
            catch (Exception e)
            {
                message = e.Message;
                return message;
                throw;
            }
            return message;
        }

        private static string htmlEmailRow(Patient_Payment model)
        {
            var templatePath = string.Empty;
            templatePath = "email\\PaymentNotificationToClient.txt";
            string rawTemplate = LoadNotificationTemplateHTML(templatePath);
            string resultHTML = BuildResultHTMLFromObjects(rawTemplate, model);
            return resultHTML;
        }

        private static string LoadNotificationTemplateHTML(string fileName)
        {
            var serverPath = "C:\\Users\\crist\\Documents\\Repositories\\Sarepta_Application\\Sarepta_WebApplication1\\Sarepta_WebApplication1\\Controllers\\";                             
            string filePath = string.Concat(serverPath, fileName);
            string htmlContent = System.IO.File.ReadAllText(filePath);           
            return htmlContent;
        }


        private static string BuildResultHTMLFromObjects(string rawTemplate, Patient_Payment model)
        {
            string resultHTML = string.Empty;

            rawTemplate = rawTemplate.Replace("Nombre", model.patient.Name);
            rawTemplate = rawTemplate.Replace("valor", string.Format("{0:n}", model.payment.Pay));

            resultHTML = rawTemplate;
            return resultHTML;
        }

    }
}