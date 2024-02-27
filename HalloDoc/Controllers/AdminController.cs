using BusinessLogic.Interface;
using DataAccess.Data;
using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    public class AdminController:Controller
    {
        private readonly DataAccess.Data.ApplicationDbContext _db;
        private readonly IAdminDash _AdminDash;

        public AdminController(ApplicationDbContext db, IAdminDash adminDash)
        {
            _db = db;
            _AdminDash = adminDash;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Dashboard()
        {
            
           
            return View();
            
        }
        public IActionResult _NewPartial()
        {
            return View();
        }
       public IActionResult GetPartialView(string btnName,int statusid)
        {
            var partialview = "_" + btnName;
            
            var result = _AdminDash.GetDashboardData(statusid);
            
            return PartialView(partialview,result);
        }
        
    }
}
