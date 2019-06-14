﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sarepta_WebApplication1.Models;

namespace Sarepta_WebApplication1.Controllers
{
    [Authorize]
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
            var patient = new Patients();

            patient.Name = "Cristhian Gòmez";
            patient.CellphoneNumber = "3204575257";
            patient.Email = "Cristhian.julian@hotmail.com";
            patient.Address = "calle 142 # 13-69";
            patient.CreatedAt = DateTime.Now;
            patient.city = "Bucaramanga";

            return View(patient);
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