using AspNetCoreHero.ToastNotification.Abstractions;
using BusinessLogic.Interface;
using BusinessLogic.Service;
using DataAccess.Data;
using DataAccess.Models;
using DataAccess.ViewModel;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Twilio.Http;

namespace HalloDoc.Controllers
{
    [CustomAuthorize("2")]
    public class ProviderController : Controller
    {
        private readonly IProviderInterface _providerDataService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAdminDash _AdminDash;
        private readonly IProviderService _providerService;
        private readonly ApplicationDbContext _db;
        private readonly INotyfService _notyf;
        public ProviderController(ApplicationDbContext db,IProviderInterface providerDataService, IHttpContextAccessor httpContextAccessor, INotyfService notyf,IAdminDash adminDash, IProviderService providerService)
        {
            _providerDataService = providerDataService;
            _httpContextAccessor = httpContextAccessor;
            _AdminDash = adminDash;
            _providerService = providerService;
            _db = db;
            _notyf = notyf;

        }

        public IActionResult Index(ProviderDash model)
        {
          
            return View(model);
        }

        public IActionResult GetTabs(string tabid,int statusId, int reqTypeId, int currentpage, string searchstream, string region, int physicianId,int requestid,string email,string patientname,string confirmationno)
        {

           string aspnetuserid= _httpContextAccessor.HttpContext.Session.GetString("Aspnetuserid");
            //int physicianid = _db.Physicians.Include(x => x.Aspnetuser).Where(x => x.Aspnetuserid == aspnetuserid).FirstOrDefault().Physicianid;
            physicianId = (int)_httpContextAccessor.HttpContext.Session.GetInt32("physicianid");
            if (tabid == "Dashboard")
            {
                var data = _providerDataService.GetRequestStatusCountByPhysician(physicianId);

                return PartialView("Tabs/_Dashboard",data);
            }
            if (tabid == "MyProfile")
            {
                var req = _providerDataService.GetProviderInfo(aspnetuserid);  
                return PartialView("Tabs/_Profile", req);
            }
            if (tabid == "Scheduling")
            {
                    var req = _providerService.GetRegion(0);
                return PartialView("Tabs/_Scheduling",req);
            }
            if (tabid == "ViewCaseProvider")
            {
                var req = _AdminDash.GetViewCase(requestid);
                return PartialView("Tabs/_ViewCaseProvider", req);
            }
            if (tabid == "ViewNotes")
            {
                var data = _AdminDash.GetViewNotes(requestid);
                return PartialView("Tabs/_ViewNotes", data);
            }
            if (tabid == "ViewUploadsProvider")
            {
                var data = _AdminDash.ViewUploadData(requestid, patientname, confirmationno, email);
                return PartialView("Tabs/_ViewUploadsProvider", data);
            }
            if (tabid == "Encounter")
            {
                var data = _AdminDash.GetEncounterForm(requestid);
                return PartialView("Tabs/_Encounter", data);
            }
            if (tabid == "SendOrdere")
            {
                var orderdetail = _AdminDash.SendOrder(requestid);
                return PartialView("Tabs/_Sendordere", orderdetail);
            }
            if (tabid == "Conclude")
            {
                var orderdetail =_providerDataService.ConcludeCareGet(requestid);
                return PartialView("Tabs/_ConcludeCare",orderdetail);
            }

            return View();
            
        }
        public IActionResult GetPartialView( int statusid, int reqtype, int currentpage,string searchValue, string dropdown,int physicianid)
        {
            physicianid = (int)_httpContextAccessor.HttpContext.Session.GetInt32("physicianid");
            var data = _providerDataService.reqByStatus(statusid, reqtype, currentpage, searchValue, dropdown, physicianid);
            return PartialView("_RequestByStatusTable",data);

        }
        public IActionResult ModalPartialView(string modalName,int requestid)
        {
            TempData["requestid"] = requestid;
            if (modalName == "EncounterPopUp")
            {
                //var result = _AdminDash.AssignRequest(requestid);
                return PartialView("_EncounterPopUp",requestid);

            }
            return View();
        }
        public JsonResult SetTypeOfCare(int reqid, int TOCId)
        {
            bool result = _providerDataService.SetTypeOfCare(reqid, TOCId);

            if (result)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        /**************scheduling******************/
        //scheduling
        public IActionResult LoadSchedulingCalendar(int RegionId, int PhysicianId, string currentView = "")
        {
            int? sessionPhysicianId = _httpContextAccessor.HttpContext?.Session?.GetInt32("physicianId");

            //int physicianId = sessionPhysicianId ?? default;
            int physicianId = 6;
            AdminDashboard model = new AdminDashboard();
            model.PhysicianProfilList = _providerService.GetProvider(RegionId);
            model.eventModel = _providerService.GetEventsByPhysicianID(physicianId);
            model.currentView = currentView;

            return PartialView("Tabs/_Calendar", model);
        }


        public JsonResult CreateShift(scheduleModel scheduleModel)
        {
            var token = Request.Cookies["jwt"];
            var adminId = "";


            if (_providerService.CreateShift(scheduleModel, adminId))
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public JsonResult EditShift(int shiftDetailId, DateTime Shiftdate, TimeOnly startTime, TimeOnly endTime)
        {
            var token = Request.Cookies["jwt"];
            var adminId = "";

            if (_providerService.EditShift(shiftDetailId, Shiftdate, startTime, endTime, adminId))
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public JsonResult DeleteShift(int shiftDetailId)
        {
            var token = Request.Cookies["jwt"];
            var adminId = "";

            if (_providerService.DeleteShift(shiftDetailId, adminId))
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public JsonResult ReturnShift(int shiftDetailId)
        {
            var token = Request.Cookies["jwt"];
            var adminId = "";

            if (_providerService.ReturnShift(shiftDetailId, adminId))
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        public List<Physician> GetPhysicians(int Regionid)
        {
            var obj = _db.Physicians.Where(x => x.Regionid == Regionid).ToList();
            return obj;
        }
        public IActionResult LoadMDOnCallData(int RegionId)
        {
            AdminDashboard model = new AdminDashboard();
            model.PhysicianProfilList = _providerService.GetProvider(RegionId);

            return PartialView("Scheduling/_MDsOnCallData", model);
        }
        public IActionResult LoadRequestedShiftTable()
        {
            int? sessionPhysicianId = _httpContextAccessor.HttpContext?.Session?.GetInt32("physicianId");
            int physicianId = sessionPhysicianId ?? default;
            AdminDashboard model = new AdminDashboard();
            model.eventModel = _providerService.GetEventsByPhysicianID(physicianId);
            return PartialView("Scheduling/_RequestedShiftPartialTable", model);
        }
        //[HttpPost]
        //public IActionResult RequestedShiftUpdate(string ids, int type)
        //{
        //    var adminId = _httpContextAccessor.HttpContext.Session.GetInt32("id").ToString();
        //    _providerService.RequestedShiftUpdate(ids, type, adminId);
        //    return GetTabs("_RequestedShift", default, default, default, default);
        //}
        /*********conclude care*******/
        public IActionResult ConcludeCare(int reqid, string notes)
        {
            int? sessionPhysicianId = _httpContextAccessor.HttpContext?.Session?.GetInt32("physicianId");
            int physicianId = sessionPhysicianId ?? default;
            bool result = _providerDataService.ConcludeCare(reqid, notes, physicianId);
            if (result)
            {
                //_notyf.Success("Request Successfully Concluded !!");
                return GetTabs("home", default, default, default, default,default,default,default,default,default,default);
            }
            else
            {
                // _notyf.Error("Request Failed To Conclude !!");
                return GetTabs("home", default, default, default, default, default, default, default, default, default, default);
            }
        }
        /******encounter****/
        //EncounterForm
        public IActionResult EncounterFormDataPost(AdminDashboard model)
        {

            bool result = _AdminDash.EncounterFormDataPost(model.encounterform);
            if (result)
            {
                _notyf.Success("Encounter Form Successfully Saved !!");
            }
            else
            {
                _notyf.Error("Encounter Form Failed To Save");
            }
            return GetTabs("home", default, default, default, default, default, default, default, default, default, default);
        }
        public IActionResult EncounterFormFinalize(int reqid)
        {
            bool result = _providerDataService.FinalizeEncounterForm(reqid);
            if (result)
            {
                _notyf.Success("Encounter Form Successfully Finalized !!");
            }
            else
            {
                _notyf.Error("Encounter Form Failed To Finalized");
            }
            return GetTabs("home", default, default, default, default, default, default, default, default, default, default);
        }
        

    }
}
  