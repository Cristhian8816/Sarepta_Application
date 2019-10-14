using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sarepta_WebApplication1.Models;
using Microsoft.AspNetCore.Http;

namespace Sarepta_WebApplication1.Controllers
{  
    public class ProcessesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SareptaProcesses()
        {
            Patients patient = new Patients();
            Treatments treatment = new Treatments();
            Processes process = new Processes();

            Patient_Treatment patient_treatment = new Patient_Treatment();

            ViewBag.Sessionv = ((System.Security.Claims.ClaimsIdentity)((Microsoft.AspNetCore.Http.DefaultHttpContext)HttpContext).User.Identity).Name;
            return View(patient_treatment);
        }

        public IActionResult process(Patient_Treatment model)
        {
            using (UsersContext db = new UsersContext())
            {               
                var identification = db.Patients.Where(c => c.cedula == model.patient.cedula).FirstOrDefault();
                var treatmentName = db.Treatments.Where(t => t.Name == model.treatment.Name).FirstOrDefault();
                if (identification != null && treatmentName != null)
                {
                    Patients patient = new Patients();
                    Processes process = new Processes();

                    var amount_processes = db.Processes.Count();
                    amount_processes++;
                    process.ProcessId = amount_processes;
                    process.PatientId = identification.PatientId;
                    process.TreatmentsId = treatmentName.TreatmentId;
                    process.Home = model.process.Home;
                    process.Tooth = model.process.Tooth;
                    process.Laboratory = model.process.Laboratory;
                    process.Consultory = model.process.Consultory;
                    process.Assistant = model.process.Assistant;
                    process.Materials = model.process.Materials;
                    process.Transport = model.process.Transport;
                    process.Date = model.process.Date;
                    process.real_Cost = model.process.real_Cost;

                    db.Processes.Add(process);
                    db.SaveChanges();

                    return RedirectToAction("ProcessRegisterSucess");
                }
                else
                {
                    ModelState.AddModelError("cedula", "Paciente no encontrado en la base de datos");
                    return View("SareptaProcesses");
                }              
            }
        }

        public IActionResult ProcessRegisterSucess()
        {
            return View();
        }
    }
}