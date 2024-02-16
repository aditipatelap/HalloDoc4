
using AspNetCoreHero.ToastNotification.Abstractions;
using BusinessLogic.Interface;
using BusinessLogic.Service;
using DataAccess.Data;
using DataAccess.Models;
using DataAccess.ViewModel;

using HalloDoc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Cryptography.Xml;

namespace HalloDoc.Controllers
{
    public class Patient_siteController : Controller
    {
        private readonly ILogger<Patient_siteController> _logger;
        private readonly IUserInterface _loginService;
        private readonly IRequestInterface _requestService;
        private readonly DataAccess.Data.ApplicationDbContext _db;
        private readonly INotyfService _notyf;


        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        public Patient_siteController(ILogger<Patient_siteController> logger, IUserInterface loginService, IRequestInterface requestService, ApplicationDbContext db, Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment,INotyfService notyf)

        {
            _logger = logger;
            _loginService = loginService;
            _requestService = requestService;
            _db = db;
            _env = Environment;
            _notyf = notyf;
        }
        public IActionResult Patient_Login()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Patient_Login(LoginModel loginModel)
        {
           
            int var=_loginService.Login(loginModel);
           
            if(var==1)
            {
               // _notyf.Error("email incorrect ");

                ViewBag.Message = "User does not exist";
                return View();
                
            }
            //else if(var==2)
            //{
            //    _notyf.Error("Email incorrect");
            //    // ViewBag.Message = "Email not found";
            //    return View();
            //}
         else   if(var==3)
            {
                _notyf.Error("Password Incorrect");
                // ViewBag.Message = "password incorrect";
                return View();
            }

         else   if (var == 4)
            {
               
                return RedirectToAction("patientDashboard", "Patient_site");
            }
            return View();
                     

            //if (ModelState.IsValid)
            //{


            //    if (_loginService.Login(loginModel))
            //    {
            //        TempData["ToastMessage"] = "Login successful!";
            //        TempData["ToastType"] = "toast-success";
            //        return RedirectToAction("familyFriendReq", "Patient_site");
            //    }
            //    else
            //    {
            //        TempData["ToastMessage"] = "Invalid username or password.";
            //        TempData["ToastType"] = "toast-error";
            //        return RedirectToAction("Patient_Login", "Patient_site");
            //    }
            //}
           
        }
        //Check email
        public JsonResult CheckEmailExists(string email)
        {
            bool emailExists = db.Aspnetusers.Any(u => u.Email == email);
            return Json(new { emailExists = emailExists });
        }


        //public IActionResult Patient_Login()
        //{


        //    return View();
        //}
        //public IActionResult Patient_Login1(LoginModel loginModel)
        //{

        //    var user = _db.Aspnetusers.FirstOrDefault(u => u.Email == loginModel.Email);
        //    if (user == null)
        //    {
        //        ViewBag.Error("Invalid email");

        //    }

        //    if (user != null)
        //    {
        //        if (user.Passwordhash == loginModel.Password)
        //        {
        //            ViewBag.Error("Invalid Password");

        //        }



        //    }
        //    else
        //    {
        //        ViewBag.Error("Valid");
        //    }
        //    return View();

        //}
        public IActionResult resetPassword()
        {
            return View();
        }
        public IActionResult submitReq()
        {
            return View();
        }
        public IActionResult patientReq()
        {

            return View();
        }
        public IActionResult patientDashboard()
        {
         
            var result = _requestService.DisplayDashboard();
           


            return View(result);

           
        }
        



        //public void Upload(IFormFile file)
        //{
        //    _requestService.UploadFile(file);

        //}

        //public IActionResult DisplayData()
        //{

        //    List<dashboardmodel> data = _requestService.Getpatientinfo();
        //    return View(data);
        //}
        //[HttpGet]
        //public IActionResult DashboardInfo()
        //{
        //    var infos = _requestService.GetPatientInfo();
        //    var view = new dashboardmodel2{dashboards = infos };
        //    return View(view);
        //}
        // [HttpGet]
        //public ActionResult patientDashboard()
        //{

        //}

        [HttpPost]
        public IActionResult PatientReq(patientReq patientReq, Microsoft.AspNetCore.Http.IFormFile file)
        {
            
                //if (file != null && file.Length > 0)
                //{
                //    var uploadsFolder = Path.Combine(_env.WebRootPath, "Files");
                //    var filePath = Path.Combine(uploadsFolder, file.FileName);

                //    // Ensure the uploads directory exists
                //    Directory.CreateDirectory(uploadsFolder);

                //    using (var fileStream = new FileStream(filePath, FileMode.Create))
                //    {
                //        file.CopyTo(fileStream);
                //    }

                //    // Save the file name in the model
                //    patientReq.file = file.FileName;
                //    ViewBag.Message = "File uploaded successfully!";
                //}
                //else
                //{
                //    ViewBag.Error = "No file selected or file is empty.";
                //}
                _requestService.UploadFile(file,patientReq);
            if(file!=null && file.Length>0)
            {
                ViewBag.Message = "File uploaded successfully";
            }

                _requestService.PatientInfo(patientReq);
            
                return RedirectToAction("submitReq", "Patient_site");
            
            return View();
        }
   
    
        public IActionResult familyFriendReq()
        {
            return View();
        }
        [HttpPost]
        public IActionResult FamilyFriendReq(familyReq familyReq)
        {
            if (ModelState.IsValid)
            { 

                return RedirectToAction("submitReq", "Patient_site");
            }
            return View();
        }
        public IActionResult conciergeReq()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ConciergeReq(conciergeReq conciergeReq)
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction("submitReq", "Patient_site");
            }
            return View();
        }
        public IActionResult businessReq()
        {
            return View();
        }
        [HttpPost]
        public IActionResult BusinessReq( businessReq businessReq)
        {
            if (ModelState.IsValid)
            {
                _requestService.BusinessReq(businessReq);
                return RedirectToAction("submitReq", "Patient_site");
            }
            return View();
        }
        //public IActionResult patientDashboard()
        //{
        //    return View();
        //}
        public IActionResult Document()
        {
            List<Documentmodel>itemlist = _requestService.ViewDocument();

            return View(itemlist);
        }
       

           [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}