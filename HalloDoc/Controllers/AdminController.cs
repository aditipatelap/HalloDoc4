using BusinessLogic.Interface;
using DataAccess.Data;
using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    public class AdminController:Controller
    {
        private readonly DataAccess.Data.ApplicationDbContext _db;

        public AdminController(ApplicationDbContext db)
        {
            _db=db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Dashboard()
        {
            return View();  
        }
    }
}
