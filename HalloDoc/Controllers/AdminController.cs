using BusinessLogic.Interface;
using DataAccess.Data;
using DataAccess.ViewModel;
using Microsoft.AspNetCore.Mvc;


namespace HalloDoc.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAdminDash _AdminDash;

        public AdminController(ApplicationDbContext db, IAdminDash adminDash, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _AdminDash = adminDash;
            _httpContextAccessor = httpContextAccessor;
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
        
        public IActionResult GetModalPartialView(string modalName,int requestid, string patientname)
        {
            //_httpContextAccessor.HttpContext.Session.SetInt32("reqid", reqid);
            //_httpContextAccessor.HttpContext.Session.SetString("patientName", patientname);
            TempData["reqid"] = _httpContextAccessor.HttpContext.Session.GetInt32("reqid");
            TempData["patientName"] = _httpContextAccessor.HttpContext.Session.GetString("patientName");
            var partialname = "_" + modalName;
            if (modalName == "AssignRequest")
            {
                var result = _AdminDash.AssignRequest(requestid);
                return PartialView(partialname, result);

            }
            if (modalName == "BlockCase")
            {
                var result = _AdminDash.BlockCase(requestid, patientname);
                return PartialView(partialname, result);

            }
            if (modalName=="CancelCase")
            {
               var result = _AdminDash.CancelCase(requestid);
                return PartialView(partialname, result);
            }

            return PartialView(partialname);

        }
        public IActionResult AssignReq(AdminDashboard model, int requestid)
        {
            _AdminDash.SubmitAssignReq(model, requestid);
             return RedirectToAction("Dashboard", "Admin");
        }
       
        public IActionResult CancelCaseReq(AdminDashboard model,int requestid)

        {
            _AdminDash.submitCancelCase(model, requestid);


            return RedirectToAction("Dashboard", "Admin");
        }
        
        public IActionResult BlockReq(AdminDashboard model, int requestid)

        {   
            _AdminDash.SubmitBlockCase(model, requestid);


            return RedirectToAction("Dashboard","Admin");
        }
        public IActionResult ViewUpload()
        {
            return View();
        }
        public IActionResult GetDropDown(int selectedvalue)
        {

            var physicians = _db.Physicians.Where(x => x.Regionid == selectedvalue).Select(x =>  x.Firstname).ToList();
            return Json(physicians);
        }
    }
}
