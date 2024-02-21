using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    public class AdminController:Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
