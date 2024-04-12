using AspNetCoreHero.ToastNotification.Abstractions;
using BusinessLogic.Interface;
using BusinessLogic.Service;
using DataAccess.Data;
using DataAccess.ViewModel;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;

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
                string Adminid = user.Id;
                string Username = user.Name;
                _httpContextAccessor.HttpContext.Session.SetString("Adminid", Adminid);
                _httpContextAccessor.HttpContext.Session.SetString("Username", Username);

                // Decode JWT token to get username
                var tokenHandler = new JwtSecurityTokenHandler();
                var decodedToken = tokenHandler.ReadJwtToken(jwtToken);
                var usernameClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == "Username");
                if (usernameClaim != null)
                {
                    string username = usernameClaim.Value;
                    var model=new AdminDashboard { UserName = username};

                    // Use the username as needed
                   
                    return RedirectToAction("Index", "Admin",model);

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
