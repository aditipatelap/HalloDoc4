using BusinessLogic.Interface;
using DataAccess.Data;
using DataAccess.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginInterface _loginInterface;
        private readonly ApplicationDbContext _db;
        public LoginController(ApplicationDbContext db,ILoginInterface loginInterface)
        {
            _db = db;
            _loginInterface = loginInterface;
        }
        public IActionResult AdminLogin()
        {

            return View();
        }

        [HttpPost]
        public IActionResult AdminLogin(LoginModel loginModel)
        {
           // _loginInterface.Login(loginModel);
          return RedirectToAction("Index", "Admin");
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
