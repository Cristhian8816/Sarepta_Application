using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Sarepta_WebApplication1.Models;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System;

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
            return View(ViewBag.Message);
        }
        
        public IActionResult Login(userAccount user, string token)
        {
            using (UsersContext db = new UsersContext())
            {                
                userAccount usr = db.userAccount.Where(u => u.UserName == user.UserName && u.Password == user.Password).FirstOrDefault();
                if (usr != null)
                {   
                    //Here we desencrypt token
                    var handler = new JwtSecurityTokenHandler();
                    var TokenJSON = handler.ReadToken(token);
                    string TokenString = TokenJSON.ToString();

                    //Here stract password 
                    int passwordStart = TokenString.IndexOf("id") + 5;
                    int passwordEnd = (TokenString.Length-2) - (passwordStart);
                    string passwordToken = TokenString.Substring(passwordStart, passwordEnd);

                    string date = DateTime.Now.ToString("dddMMMddyyyy");
                    string passwordAuth = usr.UserName + date;

                    if (passwordToken == passwordAuth)
                    {
                        ViewBag.Message = usr.UserName;
                        return View("MainSareptaSystem");
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "Autenticacion Invalida");
                        return View("UserLogin");
                    }                                                          
                }
                else
                {
                    ModelState.AddModelError("UserName", "Nombre de Usuario o Contraseña incorrectos");
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
