using AspNetCoreHero.ToastNotification.Abstractions;
using BusinessLogic.Interface;
using BusinessLogic.Service;
using DataAccess.Data;
using DataAccess.ViewModel;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static DataAccess.ViewModel.Constant;

namespace HalloDoc.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginInterface _loginInterface;
        private readonly ApplicationDbContext _db;
        private readonly INotyfService _notyf;
        private readonly IJwtService _JwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginController(ApplicationDbContext db,ILoginInterface loginInterface, INotyfService notyf, IJwtService jwtService, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _loginInterface = loginInterface;
            _notyf = notyf;
            _JwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult AdminLogin()
        {

            return View();
        }

        //[HttpPost]
        //public IActionResult AdminLogin(LoginModel loginModel)
        //{
        //   var user=_loginInterface.Login(loginModel);
        //    if(user==null)  
        //    {
        //        _notyf.Custom("Login Failed!", 3, "red", "bi bi-check-circle-fill");
        //        return View();
        //    }
        //    else
        //    {

        //        var jwtToken = _JwtService.GenerateToken(user);

        //        Response.Cookies.Append("jwt", jwtToken);   
        //        string Aspnetuserid = user.Id;
        //        string Username = user.Name;
        //        int  roleid = (int)user.Aspnetuserroles.FirstOrDefault().Roleid;
        //        int physicianid = _db.Physicians.FirstOrDefault(x => x.Aspnetuserid == Aspnetuserid).Physicianid;
        //        _httpContextAccessor.HttpContext.Session.SetString("Aspnetuserid", Aspnetuserid);
        //        _httpContextAccessor.HttpContext.Session.SetString("Username", Username);
        //        _httpContextAccessor.HttpContext.Session.SetInt32("roleid", roleid);
        //        _httpContextAccessor.HttpContext.Session.SetInt32("physicianid", physicianid);

        //        // Decode JWT token to get username
        //        var tokenHandler = new JwtSecurityTokenHandler();
        //        var decodedToken = tokenHandler.ReadJwtToken(jwtToken);
        //        var usernameClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == "Username");
        //        var roleClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
        //        if (usernameClaim != null)
        //        {
        //            if (roleClaim != null)
        //            {
        //                string role = roleClaim.Value;
        //                if (role == "1")
        //                {
        //                    // Redirect to admin index
        //                    return RedirectToAction("Index", "Admin");
        //                }
        //                else if (role == "2")
        //                {
        //                    // Redirect to provider index
        //                    return RedirectToAction("Index", "Provider");
        //                }
        //            }

        //        }



        //    }
        //    return RedirectToAction("Index", "Error");

        //}
       [ HttpPost]
        public IActionResult AdminLogin(LoginModel loginModel)
        {
            try
            {
                  var user = _loginInterface.Login(loginModel);
                if (user != null)
                {
                    //var passwordHasher = new PasswordHasher<LoginModel>();
                    //if (passwordHasher.VerifyHashedPassword(user, user.Password, loginModel.Password) == PasswordVerificationResult.Success)
                    if(user.Password==loginModel.Password)
                    {
                        var jwtToken = _JwtService.GenerateToken(user);
                        Response.Cookies.Append("jwt", jwtToken);

                        _httpContextAccessor.HttpContext.Session.SetString("Username", user.UserName);
                        _httpContextAccessor.HttpContext.Session.SetString("Aspnetuserid", user.AspNetUserId);

                        if (user.roleid == (int)Roles.Admin)
                        {
                            _httpContextAccessor.HttpContext.Session.SetInt32("Adminid", user.AdminId);
                            _notyf.Custom("Login Successfully!", 3, "green", "bi bi-check-circle-fill");
                            return RedirectToAction("Index", "Admin");
                        }
                        if (user.roleid == (int)Roles.Provider)
                        {
                            _httpContextAccessor.HttpContext.Session.SetInt32("physicianid", user.PhysicianId);
                            _notyf.Custom("Login Successfully!", 3, "green", "bi bi-check-circle-fill");
                            return RedirectToAction("Index", "Provider");
                        }
                        //if (user.Roleid == (int)Roles.Patient)
                        //{
                        //    _httpContextAccessor.HttpContext.Session.SetInt32("id", user.UserId);
                        //    _notyf.Custom("Login Successfully!", 3, "green", "bi bi-check-circle-fill");
                        //    return RedirectToAction("Dashboard", "Patient");
                        //}
                        else
                        {
                            return RedirectToAction("AccessDenied", "Home");
                        }
                    }
                    else
                    {
                        _notyf.Custom("Invalid Password", 3, "red", "bi bi-x-circle-fill");
                        return View();
                    }
                }
                else
                {
                    _notyf.Custom("Invalid Email", 3, "red", "bi bi-x-circle-fill");
                    return View();
                }
                
            }
            catch (Exception ex)
            {
                _notyf.Error("An error occurred while processing your request. Please try again later.");
                return View();
            }
           
        }

        public IActionResult AdminLogout()
        {
            Response.Cookies.Delete("jwt");
          // _httpContextAccessor.HttpContext.Session.Clear();
           return RedirectToAction("AdminLogin", "Login");
           
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(LoginModel login)
        {
            if (_loginInterface.checkEmail(login))
            {
                //var token = _JwtService.GenerateToken(login);
                if (_loginInterface.sendEmail(login.Email,login.AspNetUserId))
                {
                    _notyf.Custom("Email sent Successfully!!", 3, "deepskyblue", "bi bi-check2");
                }
                return View();
            }
            else
            {
                _notyf.Custom("Request for reset password Failed", 3, "Goldenrod", "bi bi-x-circle-fill");
            }
            return View();


        }
        public IActionResult CreateAccount()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateAccount(LoginModel model)
        {
            _loginInterface.CreateAccount(model);
            _notyf.Custom("Accoutn created Successfully!!", 3, "deepskyblue", "bi bi-check2");
            return RedirectToAction("PatientLogin", "Login");
        }

        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ResetPassword(LoginModel model)
        {
           _loginInterface.ResetPassSave(model);
            _notyf.Custom("Accoutn created Successfully!!", 3, "deepskyblue", "bi bi-check2");
            return RedirectToAction("PatientLogin", "Login");

        }
        public IActionResult PatientLogin()
        {

            return View();
        }
        [HttpPost]
        public IActionResult PatientLogin(LoginModel loginModel)
        {
            return RedirectToAction("patientDashboard", "Patient_site");
        }



    }
}
