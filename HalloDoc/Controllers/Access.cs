//using DataAccess.ViewModel;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;

//namespace HalloDoc.Controllers
//{
//    public class Access : Controller
//    {
//        public IActionResult AdminLogin()
//        {
//            ClaimsPrincipal claimuser = HttpContext.User;
//            if (claimuser.Identity.IsAuthenticated)
//            {
//                return RedirectToAction("Dashboard", "Admin");
//            }
//            return View();

//        }
//        [HttpPost]
//        public async Task<IActionResult> AdminLogin(LoginModel loginModel)

//        {
//            var user = _db.Aspnetusers.FirstOrDefault(u => u.Email == loginModel.Email && u.Passwordhash == loginModel.Password);

//            if (user != null)
//            {
//                List<Claim> claims = new List<Claim>()
//                {
//                    new Claim(ClaimTypes.NameIdentifier,loginModel.Email),
//                    new Claim("OtherProperties","Example Role")
//                };
//                ClaimsIdentity claimsidentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

//                //Authenticationproperties
//                AuthenticationProperties properties = new AuthenticationProperties()
//                {
//                    AllowRefresh = true,
//                };
//                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsidentity), properties);



//                //_httpContextAccessor.HttpContext.Session.SetString("id", user.Id);




//                //    _httpContextAccessor.HttpContext.Session.SetString("Admin", user.Name);

//                //   // _notyf.Custom("Login Successfully!", 3, "green", "bi bi-check-circle-fill");



//                return RedirectToAction("Dashboard", "Admin");
//            }

//            _notyf.Custom("Login Failed!", 3, "red", "bi bi-check-circle-fill");
//            return View();

//        }
//    }
//}
