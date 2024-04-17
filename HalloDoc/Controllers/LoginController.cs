using AspNetCoreHero.ToastNotification.Abstractions;
using BusinessLogic.Interface;
using BusinessLogic.Service;
using DataAccess.Data;
using DataAccess.ViewModel;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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

        [HttpPost]
        public IActionResult AdminLogin(LoginModel loginModel)
        {
           var user=_loginInterface.Login(loginModel);
            if(user==null)  
            {
                _notyf.Custom("Login Failed!", 3, "red", "bi bi-check-circle-fill");
                return View();
            }
            else
            {
                
                var jwtToken = _JwtService.GenerateToken(user);
               
                Response.Cookies.Append("jwt", jwtToken);   
                string Aspnetuserid = user.Id;
                string Username = user.Name;
                int  roleid = (int)user.Aspnetuserroles.FirstOrDefault().Roleid;
                _httpContextAccessor.HttpContext.Session.SetString("Aspnetuserid", Aspnetuserid);
                _httpContextAccessor.HttpContext.Session.SetString("Username", Username);
                _httpContextAccessor.HttpContext.Session.SetInt32("roleid", roleid);
                // Decode JWT token to get username
                var tokenHandler = new JwtSecurityTokenHandler();
                var decodedToken = tokenHandler.ReadJwtToken(jwtToken);
                var usernameClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == "Username");
                var roleClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                if (usernameClaim != null)
                {
                    if (roleClaim != null)
                    {
                        string role = roleClaim.Value;
                        if (role == "1")
                        {
                            // Redirect to admin index
                            return RedirectToAction("Index", "Admin");
                        }
                        else if (role == "2")
                        {
                            // Redirect to provider index
                            return RedirectToAction("Index", "Provider");
                        }
                    }

                }


               
            }
            return RedirectToAction("Index", "Error");

        }
        public IActionResult AdminLogout()
        {
            Response.Cookies.Delete("jwt");
          // _httpContextAccessor.HttpContext.Session.Clear();
           return RedirectToAction("AdminLogin", "Login");
           
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
