﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sarepta_WebApplication1.Models;

namespace Sarepta_WebApplication1.Controllers
{    
    public class HomeController : Controller
    {        
        public IActionResult Index()
        {           
            return View();            
        }             

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult RegisterVIew()
        {

            return View();
        }

        public IActionResult UserRegisterSucess()
        {
            return View();
        }

        public IActionResult Register(userAccount account)
        {
            if(ModelState.IsValid)
            {               
                using (UsersContext db = new UsersContext())
                {
                    var amount_Users = db.userAccount.Count();
                    amount_Users++;
                    account.UserID = amount_Users;
                    db.userAccount.Add(account);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = account.FirstName + " " + account.LastName + " usuario satisfactoriamente creado";
                return View("UserRegisterSucess");
            }
            else
            {
                return View("RegisterVIew");
            }          
        }

        public IActionResult Userlogin()
        {
            return View();
        }

        public IActionResult MainSareptaSystem()
        {
            return View();
        }

        public IActionResult Login(userAccount user)
        {
            using (UsersContext db = new UsersContext())
            {                
                userAccount usr = db.userAccount.Where(u => u.UserName == user.UserName && u.Password == user.Password).FirstOrDefault();
                if (usr != null)
                {
                    return RedirectToAction("MainSareptaSystem");
                }
                else
                {
                    ModelState.AddModelError("UserName", "Nombre de Usuario o Password son incorrectos");
                    return View("UserLogin");
                }                      
            }          
        }

        public IActionResult SareptaTreatments()
        {
            UsersContext db = new UsersContext();
            var data = db.Treatments;
            return View(data);
        }

        public IActionResult PatientProcesses()
        {
            UsersContext db = new UsersContext();

            IEnumerable<Patient_Treatment> model = null;

            model = (from pat in db.Patients
                     join pro in db.Processes
                     on pat.PatientId equals pro.PatientId
                     join tre in db.Treatments
                     on pro.TreatmentsId equals tre.TreatmentId
                     select new Patient_Treatment
                     {
                         ID = pat.PatientId,
                         Name = pat.Name,
                         Date = pro.Date,
                         treatment_Name = tre.Name,
                     }
                    );
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
