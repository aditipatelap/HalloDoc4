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
using System.IdentityModel.Tokens.Jwt;
using System.IO.Compression;
using Twilio.Http;
using Path = System.IO.Path;

namespace HalloDoc.Controllers
{
    [CustomAuthorize("2")]
    public class ProviderController : Controller
    {
        private readonly IProviderInterface _providerDataService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAdminDash _AdminDash;
        private readonly IProviderService _providerService;
        private readonly IRequestInterface _requestInterface;
        private readonly ApplicationDbContext _db;
        private readonly INotyfService _notyf;
        private readonly IJwtService _jwtService;
        public ProviderController(ApplicationDbContext db,IProviderInterface providerDataService, IHttpContextAccessor httpContextAccessor,
            IRequestInterface requestInterface,INotyfService notyf,IAdminDash adminDash, IProviderService providerService, IJwtService jwtService)
        {
            _providerDataService = providerDataService;
            _httpContextAccessor = httpContextAccessor;
            _AdminDash = adminDash;
            _providerService = providerService;
            _requestInterface = requestInterface;
            _jwtService = jwtService;
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
            if (tabid == "Invoicing")
            {
                //var orderdetail = _providerDataService.ConcludeCareGet(requestid);
                return PartialView("Tabs/_invoicing");
            }
            if (tabid == "CreateReq")
            {
                var res = _db.Regions.ToList();
                AdminDashboard admin = new AdminDashboard();
                admin.Regions = res;
                admin.physicianid = physicianId;
                //var orderdetail = _providerDataService.ConcludeCareGet(requestid);
                return PartialView("Tabs/_CreateRequest", admin);
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
            if (modalName == "Accept")
            {
                //var result = _AdminDash.AssignRequest(requestid);
                return PartialView("Tabs/_Accept",requestid);

            }
            if (modalName == "EncounterPopUp")
            {
                //var result = _AdminDash.AssignRequest(requestid);
                return PartialView("_EncounterPopUp",requestid);
                    
            }
            if (modalName == "SendAgreement")
            {
                var result = _AdminDash.SendAgreeement(requestid);
                return PartialView("_SendAgreement", result);

            }
            if (modalName == "TransferReq")
            {
                //var result = _providerService.GetRegion(0);
                ProviderDash providerDash = new ProviderDash();
                providerDash.RequestId = requestid;
                return PartialView("Tabs/_TransferReq", providerDash);

            }
            if (modalName == "Finalize")
            {
                //var result = _providerService.GetRegion(0);
                
                return PartialView("Tabs/_Finalize", requestid);

            }
            if (modalName == "SendLink")
            {
                //var result = _providerService.GetRegion(0);
                AdminDashboard adminDashboard = new AdminDashboard();
                adminDashboard.requestid = requestid;   
                return PartialView("Tabs/_SendLink", adminDashboard);

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
            int sessionPhysicianId = (int)_httpContextAccessor.HttpContext?.Session?.GetInt32("physicianid");

            //int physicianId = sessionPhysicianId ?? default;
           
            AdminDashboard model = new AdminDashboard();
            model.PhysicianProfilList = _providerService.GetProvider(RegionId);
            model.eventModel = _providerService.GetEventsByPhysicianID(sessionPhysicianId);
            model.currentView = currentView;

            return PartialView("Tabs/_Calendar", model);
        }


        public JsonResult CreateShift(scheduleModel scheduleModel)
        {
            //var token = Request.Cookies["jwt"];
            //var adminId = "";
            string aspnetuserid = _httpContextAccessor.HttpContext.Session.GetString("Aspnetuserid");

            if (_providerDataService.CreateShift(scheduleModel, aspnetuserid))
            {
                _notyf.Success("Request Successfully Crerated !!");
            }
            _notyf.Success("Error Creating shift !!");
            return Json(new { success = false });
        }

        public JsonResult EditShift(int shiftDetailId, DateTime Shiftdate, TimeOnly startTime, TimeOnly endTime)
        {
            string adminId = _httpContextAccessor.HttpContext.Session.GetString("Aspnetuserid");

            if (_providerService.EditShift(shiftDetailId, Shiftdate, startTime, endTime, adminId))
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public JsonResult DeleteShift(int shiftDetailId)
        {
            string adminId = _httpContextAccessor.HttpContext.Session.GetString("Aspnetuserid");

            if (_providerService.DeleteShift(shiftDetailId, adminId))
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public JsonResult ReturnShift(int shiftDetailId)
        {
            string adminId = _httpContextAccessor.HttpContext.Session.GetString("Aspnetuserid");
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
        /*conclude house call*/
        public IActionResult ConcludeHouseCall(int reqid)
        {
            int? sessionPhysicianId = _httpContextAccessor.HttpContext?.Session?.GetInt32("physicianId");
            int physicianId = sessionPhysicianId ?? default;
            bool result = _providerDataService.ConcludeHouseCall(reqid);

            if (result)
            {
                _notyf.Success("Request Successfully Concluded !!");
                return GetTabs("Dashboard", default, default, default, default, default, default, reqid, default, default, default);
            }
            else
            {
                _notyf.Error("Request Failed To Conclude !!");
                return GetTabs("Dashboard", default, default, default, default, default, default, reqid, default, default, default);

            }
        }
        /*********conclude care*******/
        public JsonResult ConcludeCare(int reqid, string notes)
        {
            int? sessionPhysicianId = _httpContextAccessor.HttpContext?.Session?.GetInt32("physicianid");
            int physicianId = sessionPhysicianId ?? default;
            bool result = _providerDataService.ConcludeCare(reqid, notes, physicianId);
            if (result)
            {
                _notyf.Success("Request Successfully Concluded !!");
                return Json(new { success = true });
                //return GetTabs("Conclude", default, default, default, default,default,default,reqid,default,default,default);
            }
            else
            {
                 _notyf.Error("Request Failed To Conclude !!");
                return Json(new { success = false });
                //return GetTabs("Conclude", default, default, default, default, default, default, reqid, default, default, default);
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
            return GetTabs("Dashboard", default, default, default, default, default, default, default, default, default, default);
        }
        public IActionResult EncounterFormFinalize(int Reqid)
        {
            bool result = _providerDataService.FinalizeEncounterForm(Reqid);
            if (result)
            {
                _notyf.Success("Encounter Form Successfully Finalized !!");
            }
            else
            {
                _notyf.Error("Encounter Form Failed To Finalized");
            }
            return GetTabs("Dashboard", default, default, default, default, default, default, default, default, default, default);
        }
        /*****accept*/
        public IActionResult AcceptRequest(int Requestid)
        {
            var physicianid = (int)_httpContextAccessor.HttpContext.Session.GetInt32("physicianid");

            _providerDataService.AcceptRequest(Requestid, physicianid);
            _notyf.Success("Request Accepted Successfully");
            //return GetTabs("Dashboard", default, default, default, default, default, default, Requestid, default, default, default);
            return Json(new { success = true });
            // return RedirectToAction("Dashboard");
        }
        [HttpPost]
        public IActionResult TransferCaseData(ProviderDash model)
        {
            model.PhysicianId = (int)_httpContextAccessor.HttpContext.Session.GetInt32("physicianid");
            _providerDataService.TransferCaseDataPost(model);
            //return Json(new { success = true });
            return RedirectToAction("Index","Provider");
            //return GetTabs("Dashboard", default, default, default, default, default, default, default, default, default, default);
        }
        public IActionResult EditViewCaseData(AdminDashboard model, int requestid)
        {
            _AdminDash.EditViewCaseData(model, requestid);
            _notyf.Custom("Data Saved Successfully!", 3, "green", "bi bi-check-circle-fill");
            return GetTabs("Dashboard", default, default, default, default, default, default, default, default, default, default);


        }
        [HttpPost]
        public JsonResult PostViewNotes(AdminDashboard model)

        {
            string aspnetuserid = _httpContextAccessor.HttpContext.Session.GetString("Aspnetuserid");
            _providerDataService.PostViewNotes(model, aspnetuserid);
            _notyf.Custom("Notes Updated Successfully!", 3, "green", "bi bi-check-circle-fill");
            //return Json(new { success = true });
            return Json(new { success = true });
            //return GetTabs("ViewNotes", default, default, default, default, default, default, model.requestid, default, default, default);
        }
        [HttpPost]
        public IActionResult DeleteDocument(string filename, int requestid)
        {
            _AdminDash.deleteDocument(filename);
            _notyf.Information("dOCUMENT dELETED sUCCESSFULLY ...");

            return ViewUploadsList(requestid);

        }
        public ActionResult DownloadAll(string file)
        {
            List<string> files = file.Split(",").ToList();

            var tempFileName = Guid.NewGuid().ToString() + ".zip";
            var tempFilePath = Path.Combine(Path.GetTempPath(), tempFileName);

            using (var zip = ZipFile.Open(tempFilePath, ZipArchiveMode.Create))
            {
                foreach (var fileName in files)
                {
                    var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/documents/" + fileName);
                    zip.CreateEntryFromFile(uploadsFolderPath, fileName);
                }
            }
            var bytes = System.IO.File.ReadAllBytes(tempFilePath);
            System.IO.File.Delete(tempFilePath);

            return File(bytes, "application/zip", "Documents.zip");
        }
        public IActionResult SendEmail(List<string> file, string email, int requestid)
        {
            _AdminDash.sendEmail(file, email, requestid);
            _notyf.Custom("Email Sent Successfully!!", 3, "deepskyblue", "bi bi-check2");
            return ViewUploadsList(requestid);
        }
        /*send order*/

        public IActionResult GetDropDownofBusinessname(int selectedvalue)
        {
            var model = _AdminDash.GetBusiness(selectedvalue);
            return Json(model);
        }
        public IActionResult GetBusinessDetails(int selectedvalue)
        {
            var model = _AdminDash.GetBusinessDetails(selectedvalue);
            return Json(model);
        }
        [HttpPost]
        public IActionResult SendOrder(AdminDashboard model, int requestid)
        {
            string aspnetuserid = _httpContextAccessor.HttpContext.Session.GetString("Aspnetuserid");
            _AdminDash.SendOrderReq(model, requestid, aspnetuserid);
            _notyf.Custom("Order sent  Successfully!", 3, "green", "bi bi-check-circle-fill");
            return RedirectToAction("Index", "Provider");
        }
        [HttpPost]
        public IActionResult ViewDocument(IFormFile Document, int requestid)
        {
            if (Document != null)
            {
                _AdminDash.SaveDocument(Document, requestid);
                _notyf.Custom("Document Uploaded Successfully!", 3, "green", "bi bi-check-circle-fill");
            }
            else
            {
                _notyf.Custom("Please Select Document", 3, "red", "bi-check-square-fill");
            }
            return ViewUploadsList(requestid);
        }
        public IActionResult ViewUploadsList(int requestid)
        {
            var model = _AdminDash.ViewUploadDataList(requestid);
            return PartialView("Tabs/_ViewUploadPartial", model);
        }   
        [HttpPost]
        public IActionResult SendAgreement(AdminDashboard model)
        {
            _requestInterface.SendMailService(model.email,model.requestid);
            _notyf.Custom("Agrremtn mail sent Successfully!", 3, "green", "bi bi-check-circle-fill");

            return RedirectToAction("Index", "Provider");
        }
       
        [HttpPost]
        public IActionResult CreateRequestDatapost(AdminDashboard model)
        {
            _AdminDash.CreateRequestDatapost(model);

            _notyf.Custom("Request Created Successfully!!", 3, "deepskyblue", "bi bi-check2");
            return RedirectToAction("Index", "Provider");
        }
        public IActionResult GeneratePDF(int requestid)
        {
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard = _AdminDash.GetEncounterForm(requestid);

            if (adminDashboard == null)
            {
                return NotFound();
            }

            return new Rotativa.AspNetCore.ViewAsPdf("Tabs/Encounterpdf", adminDashboard)
            {
                FileName = "encounter.pdf"
            };
        }
        public JsonResult UploadDocument(int Requestid, IFormFile document)
        {
            var PhysicianId = (int)_httpContextAccessor.HttpContext.Session.GetInt32("physicianid");
            _providerDataService.SaveDocument(document, Requestid, PhysicianId);
            _notyf.Custom("Document uploded Successfully", 3, "green", "bi bi-check-circle-fill");
            return Json(new { success = true });
            //return GetTabs("Conclude", default, default, default, default, default, default, Requestid, default, default, default);
        }
        public IActionResult SendLinkDataPost(AdminDashboard model)
        {
            var adminid = (int)_httpContextAccessor.HttpContext.Session.GetInt32("physicianid");

            _providerDataService.SendMailLink(model, adminid);
            _notyf.Custom("Link Send Successfully", 3, "green", "bi bi-check-circle-fill");
            return RedirectToAction("Index", "Provider");

        }
        /*Invoicing*/
        public IActionResult LoadInvoicingTab()
        {
            return PartialView("Tabs/_invoicing");
        }
        public IActionResult SearchDataByRange(DateTime startDate)
        {
            var token = Request.Cookies["jwt"];
            var aspuserid = "";
            if (_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            {
                aspuserid = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserID").Value;
            }

            ProviderDash model = new ProviderDash();
            model.TimesheetModel = _providerDataService.SearchDataByRangeTimeSheet(startDate, aspuserid);
            model.IsFinalize = _providerDataService.CheckFinalize(startDate, aspuserid);

            return PartialView("Tabs/_timeSheet", model);
        }

        public IActionResult LoadFinalizeInvoicing(DateTime startDate)
        {
            var token = Request.Cookies["jwt"];
            var aspuserid = "";
            if (_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            {
                aspuserid = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserID").Value;
            }

            ProviderDash model = new ProviderDash();
            model.InvoicingModel = _providerDataService.SearchDataByRangeInvoicing(startDate, aspuserid);
            model.startDate = startDate;

            return PartialView("Tabs/_finalizeInvoicing", model);
        }

        public IActionResult LoadTimeSheetReimbursement(DateTime startDate)
        {
            var token = Request.Cookies["jwt"];
            var aspuserid = "";
            if (_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            {
                aspuserid = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserID").Value;
            }

            ProviderDash model = new ProviderDash();
            model.InvoicingModel = _providerDataService.SearchDataByRangeReimbursement(startDate, aspuserid);

            return PartialView("Tabs/_timeSheetReimbursement", model);
        }

        public IActionResult SaveTimeSheet(List<InvoicingModel> invoicingModels)
        {
            var token = Request.Cookies["jwt"];
            var aspuserid = "";
            if (_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            {
                aspuserid = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserID").Value;
            }

            _providerDataService.SaveTimeSheet(invoicingModels, aspuserid);
            return Json(new { success = true });
        }

        public IActionResult SaveReimbursement(InvoicingModel invoicingModels)
        {
            var token = Request.Cookies["jwt"];
            var aspuserid = "";
            if (_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            {
                aspuserid = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserID").Value;
            }

            _providerDataService.SaveReimbursement(invoicingModels, aspuserid);
            return Json(new { success = true });
        }
        public IActionResult FinalizeTimesheet(DateTime startDate)
        {
            var Physicianid = (int)_httpContextAccessor.HttpContext.Session.GetInt32("physicianid");

            _providerDataService.FinalizeTimesheet(startDate, Physicianid); 
            return Json(new { success = true });
        }





    }
}
  