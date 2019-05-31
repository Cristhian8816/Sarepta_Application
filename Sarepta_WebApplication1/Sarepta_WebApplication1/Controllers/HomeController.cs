using System;
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

        public IActionResult Login(userAccount user)
        {
            using (UsersContext db = new UsersContext())
            {
                var usr = db.userAccount.Single(u => u.UserName == user.UserName && u.Password == user.Password);
                if (usr != null)
                {                    
                    return RedirectToAction("PatientRegister");
                }
                else
                {
                    ModelState.AddModelError("", "Nombre de Usuario o password estan mal");
                }
            }

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

        public IActionResult SareptaProcesses()
        {
            Patients patient = new Patients();
            Treatments treatment = new Treatments();
            Processes process = new Processes();

            Patient_Treatment patient_treatment = new Patient_Treatment();

            return View(patient_treatment);
        }

        public IActionResult proceso(Patient_Treatment model)
        {          
            using (UsersContext db = new UsersContext())
            {
                var identification = db.Patients.Single(c => c.cedula  == model.patient.cedula);
                var tratamiento = db.Treatments.Single(t => t.Name == model.treatment.Name);
                if (identification != null && tratamiento != null)
                {
                    Patients patient = new Patients();
                    Processes process = new Processes();

                    var amount_processes = db.Processes.Count();
                    amount_processes++;
                    process.ProcessId = amount_processes;
                    process.PatientId = identification.PatientId;
                    process.TreatmentsId = tratamiento.TreatmentId;
                    process.Home = model.process.Home;
                    process.Tooth = model.process.Tooth;
                    process.Laboratory = model.process.Laboratory;
                    process.Consultory = model.process.Consultory;
                    process.Assistant = model.process.Assistant;
                    process.Materials = model.process.Materials;
                    process.Transport = model.process.Transport;
                    process.Date = model.process.Date;

                    db.Processes.Add(process);
                    db.SaveChanges();
                    //colocar vista de exitoso ingreso de procedimiento
                }
                else
                {
                    ModelState.AddModelError("", "Paciente no encontrado en la base de datos");
                    //regresar a la vista del formulario
                }
            }                      
            return RedirectToAction("SareptaPatients");
        }

        public IActionResult SareptaTreatments()
        {
            UsersContext db = new UsersContext();
            var data = db.Treatments;
            return View(data);            
        }

        public IActionResult SaveRecord(Patients model)
        {
            if(ModelState.IsValid)
             {
                try
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
                    pat.Address = model.Address;
                    pat.CreatedAt = model.CreatedAt;
                    pat.city = model.city;

                    db.Patients.Add(pat);

                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return RedirectToAction("SareptaPatients");
            }
            else
            {
                return View("PatientRegister");
            }
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
