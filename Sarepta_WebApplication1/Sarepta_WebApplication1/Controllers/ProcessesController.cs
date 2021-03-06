﻿using System;
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
            using (UsersContext db = new UsersContext())
            {
                Patient_Treatment patient_treatment = new Patient_Treatment();

                List<string> treatments = new List<string>();                

                IEnumerable<Treatments> listtreatments = null;

                listtreatments = (from treat in db.Treatments
                                  select new Treatments
                                  {
                                      Name = treat.Name,
                                  }
                        );

                //Completed lists with the information brought
                foreach (var treatment in listtreatments)
                {
                    treatments.Add(treatment.Name);

                }

                List<string> names = new List<string>();
                IEnumerable<Patients> listNames = null;

                listNames = (from patient in db.Patients
                             select new Patients
                             {
                                 Name = patient.Name,
                             }
                        );

                //Completed lists with the information brought
                foreach (var name in listNames)
                {
                    names.Add(name.Name);

                }

                //pass the information from lists to lists's modelview to expose in a view
                patient_treatment.treatments = treatments;
                patient_treatment.names = names;

                return View(patient_treatment);
            }
        }

        public JsonResult searchpatient(string term)
        {
            UsersContext db = new UsersContext();
            List<Patients> patientsSearch = db.Patients.Where(x => x.Name.Contains(term)).Select(x => new Patients
            {
               Name = x.Name,
               cedula= x.cedula,
               PatientId = x.PatientId
            }).ToList();            
            return Json(new { patientsSearch });
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