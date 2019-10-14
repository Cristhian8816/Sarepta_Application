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
    public class PatientsController : Controller
    {       

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SareptaPatients()
        {
            UsersContext db = new UsersContext();
            var data = db.Patients;
            return View(data);
        }        
        
        public IActionResult PatientRegister()
        {                  
            return View();
        }

        public IActionResult Patient(Patients model)
        {
            if (ModelState.IsValid)
            {               
                UsersContext db = new UsersContext();

                var amount_Patients = db.Patients.Count();
                amount_Patients++;

                Patients pat = new Patients();
                pat.PatientId = amount_Patients;
                pat.cedula = model.cedula;
                pat.Name = model.Name;
                pat.CellphoneNumber = model.CellphoneNumber;
                pat.Birthdate = model.Birthdate;
                pat.Email = model.Email;
                pat.Gender = model.Gender;
                pat.CreatedAt = model.CreatedAt;
                pat.Address = model.Address;
                pat.city = model.city;

                db.Patients.Add(pat);
                db.SaveChanges();             

                return RedirectToAction("SareptaPatients");
            }
            else
            {
                return View("PatientRegister");
            }
        }
    }
}