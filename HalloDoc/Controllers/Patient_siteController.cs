
using AspNetCoreHero.ToastNotification.Abstractions;
using BusinessLogic.Interface;
using DataAccess.Data;
using DataAccess.ViewModel;
using HalloDoc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HalloDoc.Controllers
{
    public class Patient_siteController : Controller
    {
        private readonly ILogger<Patient_siteController> _logger;
        private readonly IUserInterface _loginService;
        private readonly IRequestInterface _requestService;
        private readonly DataAccess.Data.ApplicationDbContext _db;
        private readonly INotyfService _notyf;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        public Patient_siteController(ILogger<Patient_siteController> logger, IUserInterface loginService, IRequestInterface requestService, ApplicationDbContext db, Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment,INotyfService notyf, IHttpContextAccessor httpContextAccessor)

        {
            _logger = logger;
            _loginService = loginService;
            _requestService = requestService;
            _db = db;
            _env = Environment;
            _notyf = notyf;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Patient_Login()
        {
            _logger.LogInformation("recevied");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Patient_Login(LoginModel loginModel)
        {

            
           
                var user = _db.Aspnetusers.FirstOrDefault(u => u.Email == loginModel.Email && u.Passwordhash == loginModel.Password);

                if (user != null)
                {

                    int id = _db.Users.FirstOrDefault(u => u.Aspnetuserid == user.Id).Userid;
                    _httpContextAccessor.HttpContext.Session.SetInt32("id", id);

                    string userName = _db.Users.Where(x => x.Aspnetuserid == user.Id).Select(x => x.Firstname + " " + x.Lastname).FirstOrDefault();

                    //_httpContextAccessor.HttpContext.Session.SetInt32("id", id);
                    _httpContextAccessor.HttpContext.Session.SetString("PatientName", userName);

                    _notyf.Custom("Login Successfully!", 3, "green", "bi bi-check-circle-fill");



                    return RedirectToAction("patientDashboard", "Patient_Site");
                }
                int var = _loginService.Login(loginModel);
                if (var == 3)
                {
                    _notyf.Custom("Password incorrect!", 3, "red", "bi bi-check-circle-fill");
                    return View();
                }

                if (var == 2)
                {
                    _notyf.Custom("Email Incorrect", 3, "red", "bi bi-x-circle-fill");
                    return View();
                }

               
            
            return View();

        }
        //int var=_loginService.Login(loginModel);
           
        //    if(var==1)
        //    {
        //       // _notyf.Error("email incorrect ");

        //        ViewBag.Message = "User does not exist";
        //        return View();
                
        //    }
        //    //else if(var==2)
        //    //{
        //    //    _notyf.Error("Email incorrect");
        //    //    // ViewBag.Message = "Email not found";
        //    //    return View();
        //    //}
        // else   if(var==3)
        //    {
        //        _notyf.Error("Password Incorrect");
        //        // ViewBag.Message = "password incorrect";
        //        return View();
        //    }

        // else   if (var == 4)
        //    {
               
        //        return RedirectToAction("patientDashboard", "Patient_site");
        //    }
        //    return View();
                     

           
        public IActionResult CreateAccount()
        {

            return View();
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
        [HttpPost]
        public IActionResult PatientReq(patientReq patientReq, Microsoft.AspNetCore.Http.IFormFile file)
        {
            
                if (file != null && file.Length > 0)
            {
                _requestService.UploadFile(file, patientReq);
            }
             

            _requestService.PatientInfo(patientReq);

            return RedirectToAction("submitReq", "Patient_site");



        }


        public IActionResult patientDashboard()
        {
            int id = (int)_httpContextAccessor.HttpContext.Session.GetInt32("id");



            var result = _requestService.DisplayDashboard(id);


            return View(result);


        }

        

        [HttpPost]
        public ActionResult UpdateProfile(Dashboardpage model)
        {
            int id = (int)_httpContextAccessor.HttpContext.Session.GetInt32("id");
            _requestService.SaveProfileData(model.Profiles, id);

            return RedirectToAction("patientDashboard");

        }



     
     

       
    
        public IActionResult familyFriendReq()
        {
            return View();
        }
        [HttpPost]
        public IActionResult familyFriendReq(familyReq familyReq)

        {
               _requestService.familyreq(familyReq);
               _requestService.mail(familyReq.Email);
            return RedirectToAction("submitReq", "Patient_site");
        }
        public IActionResult conciergeReq()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ConciergeReq(conciergeReq conciergeReq)
        {

            _requestService.ConciergeReq(conciergeReq);
            _requestService.mail(conciergeReq.Email);
          return RedirectToAction("submitReq", "Patient_site");
            
            
        }
        public IActionResult businessReq()
        {
            return View();
        }
        [HttpPost]
        public IActionResult BusinessReq( businessReq businessReq)
        {
           
         
                _requestService.BusinessReq(businessReq);
            _requestService.mail(businessReq.Email);
            return RedirectToAction("submitReq", "Patient_site");
            
          
        }
   
        public IActionResult Document(int id)
        {
            int userid = (int)_httpContextAccessor.HttpContext.Session.GetInt32("id");
           
            Dashboardpage model = new Dashboardpage();
            
                var result = _requestService.ViewDocument(userid,id);
            

            return View(result);
        }
        [HttpPost]  
        public IActionResult Document(IFormFile file,int id)
        {
            
           _requestService.FileUpload( file,  id);
            return RedirectToAction("Document", "Patient_site");
        }
        public IActionResult Information(patientReq patientreq) {
            int id = (int)_httpContextAccessor.HttpContext.Session.GetInt32("id");
            

          patientreq  = _requestService.Information(patientreq,id);
            return View(patientreq);
        }

        [HttpPost]
        public IActionResult Information(patientReq patientreq, Microsoft.AspNetCore.Http.IFormFile file)
        {
            if (file != null && file.Length > 0)
            {

                _requestService.UploadFile(file, patientreq);


            }
           // _requestService.UploadFile(patientreq.file, id);
          _requestService.PatientInfo(patientreq);
            return View();
        }
        public IActionResult Someonelse()
        {
            //familyReq reqeust = new familyReq();
            return View();
        }

        [HttpPost]
        public IActionResult Someonelse(patientReq patientreq, Microsoft.AspNetCore.Http.IFormFile file,int id)
        {
            if (file != null && file.Length > 0)
            {

                _requestService.UploadFile(file, patientreq);


            }
            _requestService.Someoneelse(patientreq,id);
            return View();
        }
       

        [HttpPost]
        public IActionResult resetPassword(LoginModel model)
        {
            _requestService.mail(model.Email);
            return View();
        }
        public IActionResult Dash()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}