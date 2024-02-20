
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Patient_Login(LoginModel loginModel)
        {

            //Aspnetuser user = _db.Aspnetusers.FirstOrDefault(aspnetuser => aspnetuser.Email == loginModel.Email && aspnetuser.Passwordhash ==loginModel.Password);
            //if (user != null)
            //{
            //    User patientUser = _db.Users.FirstOrDefault(u => u.Aspnetuserid == user.Id);
            //    TempData["success"] = "Login Successful";
            //    HttpContext.Session.SetInt32("userId", patientUser.Userid);
            //    return RedirectToAction("Dashboard");
            //}
            if (ModelState.IsValid)
            {
                //var user = _aspNetUsersServices.Login(loginModel);
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
                else
                {
                    _notyf.Custom("Login Failed", 3, "red", "bi bi-x-circle-fill");
                    return View();
                }
            }
            else
            {
                return View();
            }

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
            int id = (int)_httpContextAccessor.HttpContext.Session.GetInt32("id");



            var result = _requestService.DisplayDashboard(id);


            return View(result);


        }

        /*  u public IActionResult UpdateProfle(Profilemodel profilemodel)
          {
              int id = (int)_httpContextAccessor.HttpContext.Session.GetInt32("id");

              _requestService.UserData(profilemodel, id);
              return View();
          }
        
  */

        [HttpPost]
        public ActionResult UpdateProfile(Dashboardpage model)
        {
            //int id = (int)_httpContextAccessor.HttpContext.Session.GetInt32("id");
            //_requestService.SaveProfileData(model, id);

            return RedirectToAction("patientDashboard");

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
            int id = 2;
            var itemlist = _requestService.ViewDocument(id);

            return View(itemlist);
        }
        [HttpPost]
        public IActionResult Document(IFormFile file,int id)
        {
            
           _requestService.FileUpload( file,  id);
            return RedirectToAction("Document", "Patient_site");
        }
        public IActionResult Information() {
            return View();
        }
       public IActionResult Someonelse()
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