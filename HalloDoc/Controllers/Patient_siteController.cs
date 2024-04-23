
using AspNetCoreHero.ToastNotification.Abstractions;
using BusinessLogic.Interface;

using DataAccess.Data;
using DataAccess.ViewModel;
using HalloDoc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.Service;
using static DataAccess.ViewModel.Constant;
using DataAccess.Models;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;

namespace HalloDoc.Controllers
{
    public class Patient_siteController : Controller
    {
        private readonly ILogger<Patient_siteController> _logger;
        private readonly ILoginInterface _loginService;
        private readonly IRequestInterface _requestService;
        private readonly IJwtService _JwtService;
        private readonly ApplicationDbContext _db;
        private readonly INotyfService _notyf;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
      public Patient_siteController(ILogger<Patient_siteController> logger, ILoginInterface loginService, IRequestInterface requestService,
          ApplicationDbContext db, Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment,INotyfService notyf, IHttpContextAccessor httpContextAccessor, 
          IJwtService jwtService)

        {
            _logger = logger;
            _loginService = loginService;
            _requestService = requestService;
            _db = db;
            _env = Environment;
            _notyf = notyf;
            _httpContextAccessor = httpContextAccessor;
            _JwtService = jwtService;
        }
        public IActionResult Patient_Login()
        {
            _logger.LogInformation("recevied");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public IActionResult Patient_Login(LoginModel loginModel)
        //{



        //        var user = _db.Aspnetusers.FirstOrDefault(u => u.Email == loginModel.Email && u.Passwordhash == loginModel.Password);

        //    SessionUtils.SetLoggedInUser(HttpContext.Session, user);

        //    if (user != null)
        //        {

        //            int id = _db.Users.FirstOrDefault(u => u.Aspnetuserid == user.Id).Userid;
        //            _httpContextAccessor.HttpContext.Session.SetInt32("id", id);

        //            string userName = _db.Users.Where(x => x.Aspnetuserid == user.Id).Select(x => x.Firstname + " " + x.Lastname).FirstOrDefault();

        //            //_httpContextAccessor.HttpContext.Session.SetInt32("id", id);
        //            _httpContextAccessor.HttpContext.Session.SetString("PatientName", userName);

        //            _notyf.Custom("Login Successfully!", 3, "green", "bi bi-check-circle-fill");



        //            return RedirectToAction("patientDashboard", "Patient_Site");
        //        }
        //        int var = _loginService.Login(loginModel);
        //        if (var == 3)
        //        {
        //            _notyf.Custom("Password incorrect!", 3, "red", "bi bi-check-circle-fill");
        //            return View();
        //        }

        //        if (var == 2)
        //        {
        //            _notyf.Custom("Email Incorrect", 3, "red", "bi bi-x-circle-fill");
        //            return View();
        //        }



        //    return View();

        //}
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

        public IActionResult Patient_Login(LoginModel loginModel)
        {
            //if (ModelState.IsValid)
            //{
            //var user = _aspNetUsersServices.Login(loginModel);
            var user = _db.Aspnetusers.Include(x => x.Aspnetuserroles).FirstOrDefault(u => u.Email == loginModel.Email);

           
            if (user == null)
            {
                _notyf.Custom("Invalid Email", 3, "red", "bi bi-x-circle-fill");
                return View();
            }
            else
                {
                /* if (user.Aspnetuserroles.FirstOrDefault().Roleid == "3")
                 {
*/
                
                
                   
                    if (user.Passwordhash == loginModel.Password)
                    {
                    var roleid = user.Aspnetuserroles.FirstOrDefault().Roleid;
                    var data = new LoginModel
                    {
                        Email = user.Email,
                        Password = user.Passwordhash,
                        AspNetUserId = user.Id,
                        roleid = roleid,
                        UserName = user.Name,
                    };

                    //SessionUtils.SetLoggedInUser(HttpContext.Session, user);

                    int id = 0;
                        if (_db.Users.Any(u => u.Aspnetuserid == user.Id))
                        {
                            id = _db.Users.FirstOrDefault(u => u.Aspnetuserid == user.Id).Userid;
                        }
                        else
                        {
                            return RedirectToAction("Login", "Admin");
                        }
                        var jwtToken = _JwtService.GenerateToken(data);
                        Response.Cookies.Append("jwt", jwtToken);
                        _httpContextAccessor.HttpContext.Session.SetInt32("id", id);

                        string userName = _db.Users.Where(x => x.Aspnetuserid == user.Id).Select(x => x.Firstname + " " + x.Lastname).FirstOrDefault();
                        _httpContextAccessor.HttpContext.Session.SetString("PatientName", userName);

                        _notyf.Custom("Login Successfully!", 3, "green", "bi bi-check-circle-fill");

                        return RedirectToAction("patientDashboard", "Patient_Site");
                    }
                    else
                    {
                        _notyf.Custom("Invalid Password", 3, "red", "bi bi-x-circle-fill");
                        return View();

                    }
                    /*  }
                      else
                      {
                          _notyf.Custom("Invalid Page", 3, "red", "bi bi-x-circle-fill");
                      }*/
                //}
            }
            return View();
        }


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
            var res=_requestService.GetRegions();
            return View(res);
        }
        [HttpPost]
        public IActionResult PatientReq(patientReq patientReq, Microsoft.AspNetCore.Http.IFormFile file)
        {
            
                if (file != null && file.Length > 0)
            {
                _requestService.UploadFile(file, patientReq);
            }
             

            _requestService.PatientInfo(patientReq);
            _notyf.Custom("Request Submitted Successfully!", 3, "green", "bi bi-check-circle-fill");
            return RedirectToAction("submitReq", "Patient_site");



        }

        [CustomAuthorize("3")]
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
            _notyf.Custom("Profile  Updated Successfully!", 3, "green", "bi bi-check-circle-fill");
            return RedirectToAction("patientDashboard");

        }


        public IActionResult familyFriendReq()
        {
            var res = _requestService.GetfamilyRegions();
            return View(res);
           
        }
        [HttpPost]
        public IActionResult familyFriendReq(familyReq familyReq)

        {
            var res = _db.Users.Where(x => x.Email == familyReq.Email).FirstOrDefault();
            if (res==null)
            {
                _requestService.mail(familyReq.Email);
               
            }
            _requestService.familyreq(familyReq);
            _notyf.Custom("Request Submitted Successfully!", 3, "green", "bi bi-check-circle-fill");
            return RedirectToAction("submitReq", "Patient_site");
        }
        public IActionResult conciergeReq()
        {
            var res = _requestService.GetConciergeRegions();
            return View(res);
            
        }
        [HttpPost]
        public IActionResult ConciergeReq(conciergeReq conciergeReq)
        {
            var res = _db.Users.Where(x => x.Email == conciergeReq.Email).FirstOrDefault();
            if (res == null)
            {
                _requestService.mail(conciergeReq.Email);

            }
            _requestService.ConciergeReq(conciergeReq);
           
            _notyf.Custom("Request Submitted Successfully!", 3, "green", "bi bi-check-circle-fill");
            return RedirectToAction("submitReq", "Patient_site");
            
            
        }
        public IActionResult businessReq()
        {
            var res = _requestService.GetbusinessRegions();
            return View(res);

        }
        [HttpPost]
        public IActionResult BusinessReq( businessReq businessReq)
        {
            var res = _db.Users.Where(x => x.Email == businessReq.Email).FirstOrDefault();
            if (res == null)
            {
                _requestService.mail(businessReq.Email);

            }
            _requestService.BusinessReq(businessReq);
            
            _notyf.Custom("Request Submitted Successfully!", 3, "green", "bi bi-check-circle-fill");
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
            if (file == null)
            {
                _notyf.Custom("Please select file to upload!", 3, "red", "bi bi-check-circle-fill");
            }
            else
            {
                _requestService.FileUpload(file, id);
                _notyf.Custom("Document Submitted Successfully!", 3, "green", "bi bi-check-circle-fill");
            }
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
            _notyf.Custom("Request Submitted Successfully!", 3, "green", "bi bi-check-circle-fill");
            return RedirectToAction("patientDashboard", "Patient_site");
        }
        public IActionResult Someonelse()
        {
            var res=_requestService.SomeOneelse();
            return View(res);
        }

        [HttpPost]
        public IActionResult Someonelse(patientReq patientreq, Microsoft.AspNetCore.Http.IFormFile file,int id)
        {
            if (file != null && file.Length > 0)
            {

                _requestService.UploadFile(file, patientreq);


            }
            var res = _db.Users.Where(x => x.Email == patientreq.Email).FirstOrDefault();
            if (res == null)
            {
                _requestService.mail(patientreq.Email);

            }
            _requestService.Someoneelse(patientreq,id);
            _notyf.Custom("Request Submitted Successfully!", 3, "green", "bi bi-check-circle-fill");
            return RedirectToAction("patientDashboard", "Patient_site");
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
        public IActionResult SendAgreement(int requestid)
        {
            if(_requestService.CheckStatus(requestid))

            {
                var result = _requestService.GetSendAgreement(requestid);
                
                return View(result);

            }
            else
            {
                //return View("");
                _notyf.Custom("Agreement Already Approved", 3, "Goldenrod", "bi bi-x-circle-fill");
            }
            return RedirectToAction("PatientLogin", "Login");

        }
        [HttpPost]
        public IActionResult PostSendAgreement(int requestid)
        {
           _requestService.PostSendAgreement(requestid);
            _notyf.Custom("Agreement Approved Successfully!!", 3, "deepskyblue", "bi bi-check2");
            return RedirectToAction("PatientLogin", "Login");
        }
       
        [HttpPost]
        public IActionResult SubmitSendAgree(AdminDashboard model,int requestid)
        {
             _requestService.CancelAgreemnt(model, requestid);
            return RedirectToAction("PatientLogin", "Login");
        
        }
    }
}