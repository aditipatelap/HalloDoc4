using BusinessLogic.Interface;
using DataAccess.Data;
using DataAccess.ViewModel;
using Microsoft.AspNetCore.Mvc;


namespace HalloDoc.Controllers
{
    public class AdminController : Controller
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
            var req = _AdminDash.RequestCount();


            return View(req);

        }
        public IActionResult _NewPartial()
        {
            return View();
        }
        public IActionResult GetPartialView(string btnName, int statusid, string searchValue)
        {
            var partialview = "_" + btnName;

            var result = _AdminDash.GetDashboardData(statusid, searchValue);

            return PartialView(partialview, result);
        }
        public IActionResult ViewCase()
        {

            return View();
        }
        public IActionResult CancelCase()
        {

            return View();
        }
        public IActionResult GetModalPartialView(string modalName,int requestid)
        {
            var partialname = "_" + modalName;
            if (modalName == "AssignRequest")
            {
                var result = _AdminDash.AssignRequest();
                return PartialView(partialname, result);

            }
            if(modalName=="CancelCase")
            {
               // var result = _AdminDash.CancelCase(requestid);
               // return PartialView(partialname, result);
            }

            return PartialView(partialname);

        }
        //[HttpPost]
        //public IActionResult _AssignRequest()
        //{
        //    var data=
        //     return View();
        //}
        [HttpPost]
        public IActionResult _CancelCase(AdminDashboard model,int requestid)

        {
            _AdminDash.submitCancelCase(model, requestid);


            return View();
        }
        public IActionResult GetDropDown(int selectedvalue)
        {

            var physicians = _db.Physicians.Where(x => x.Regionid == selectedvalue).Select(x => new { x.Firstname, x.Physicianid }).ToList();
            return Json(physicians);
        }
    }
}
