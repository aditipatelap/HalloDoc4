using BusinessLogic.Interface;
using BusinessLogic.Service;
using DataAccess.Data;
using DataAccess.ViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;
using DocumentFormat.OpenXml.ExtendedProperties;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.CompilerServices;
using DocumentFormat.OpenXml.Presentation;
using static DataAccess.ViewModel.Constant;
using System.IO.Compression;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using OfficeOpenXml;
using Microsoft.AspNetCore.Http.HttpResults;
using DataAccess.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Drawing;
using System.Web.Helpers;
using System.IdentityModel.Tokens.Jwt;
using DocumentFormat.OpenXml.Office.CustomUI;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.Differencing;

namespace HalloDoc.Controllers
{
    [CustomAuthorize("1")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAdminDash _AdminDash;
        private readonly INotyfService _notyf;
        private readonly IRequestInterface _requestInterface;
        private readonly ILoginInterface _loginInterface;
        private readonly IProviderService _providerService;
        private readonly IJwtService _jwtService;
        public AdminController(ApplicationDbContext db, IAdminDash adminDash, IHttpContextAccessor httpContextAccessor, INotyfService notyf,
            IRequestInterface requestInterface, ILoginInterface loginInterface, IProviderService providerService, IJwtService jwtService)
        {
            _db = db;
            _AdminDash = adminDash;
            _httpContextAccessor = httpContextAccessor;
            _notyf = notyf;
            _requestInterface = requestInterface;
            _loginInterface = loginInterface;
            _providerService = providerService;
            _jwtService = jwtService;
        }


        public IActionResult Index(AdminDashboard model)
        {
            return View(model);
        }
        [HttpPost]

        public IActionResult GetTabs(AdminDashboard model, int statusid, string btnname, string patientname,
            string confirmationno, string email, string aspnetuserid)
        {
            var result = "Tabs/" + "_" + model.tabid;
            string adminid = _httpContextAccessor.HttpContext.Session.GetString("Adminid");
            //int roleid =(int) _httpContextAccessor.HttpContext.Session.GetInt32("roleid");
            if (model.tabid == "Dashboard")
            {
                var req = _AdminDash.RequestCount();

                return PartialView(result, req);
            }
            if (model.tabid == "MyProfile")
            {
                var req = _providerService.MyProfileDataGet(model.aspnetuserid);
                return PartialView(result, req);
            }
            if (model.tabid == "ProviderLocation")
            {
                AdminDashboard getLocation = _providerService.GetPhysicianlocations();
                return PartialView(result,getLocation);
            }
            if (model.tabid == "Providers")
            {
                var data = _providerService.GetProviderData(model.regionid);
                return PartialView(result, data);
            }
            if (model.tabid == "PhysicianAccountEdit")
            {
                var data = _providerService.EditPhysicianDataGet(aspnetuserid);
                return PartialView(result, data);
            }
            if (model.tabid == "CreateProviderAccount")
            {
                var data = _providerService.CreateProviderAdminDataGet();
                return PartialView(result, data);
            }
            if (model.tabid == "Scheduling")
            {
                var data = _providerService.GetRegion(0);
                return PartialView("Tabs/Scheduling/_Scheduling",data);
            }
            if (model.tabid == "MDsOnCall")
            {
                var data = _providerService.GetRegion(0);
                return PartialView("Tabs/Scheduling/_MDsOnCall", data);
            }
            if (model.tabid == "CreatedShift")
            {
                var data = _providerService.GetRegion(0);
                return PartialView("Tabs/Scheduling/_CreatedShift", data);
            }
            if (model.tabid == "RequestedShift")
            {
                var data = _providerService.GetRegion(0);
                return PartialView("Tabs/Scheduling/_RequestedShift", data);
            }
            if (model.tabid == "Records")
            {
                return PartialView(result);
            }
            if (model.tabid == "Access")
            {
                var data = _providerService.GetAccessData();
                return PartialView(result, data);
            }
            if (model.tabid == "CreateRole")
            {
                var data = _providerService.GetCreateRoleData(model.accounttype);
                return PartialView(result, data);
            }
            if (model.tabid == "EditAccess")
            {
                var data = _providerService.GetEditRoleData(model.roleid);
                return PartialView(result, data);
            }
            if (model.tabid == "UserAccess")
            {
                var data = _providerService.UserAccessDataGet(model.adminaccountfilter,model.CurrentPage);
                return PartialView(result, data);
            }
            if (model.tabid == "CreateAdmin")
            {
                var data = _providerService.CreaeAdminDataGet();
                return PartialView(result, data);
            }
            if (model.tabid == "Partners")
            {
                var data = _providerService.AddBusinessDataGet();
                return PartialView(result, data);
            }
            if (model.tabid == "UpdateBusiness")
            {
                var data = _providerService.EditBusinessDataGet(model.VendorId);
                return PartialView(result, data);
            }
            if (model.tabid == "AddBusiness")
            {
                var data = _providerService.AddBusinessDataGet();
                return PartialView(result, data);
            }
            if (model.tabid == "ViewCase")
            {
                var data = _AdminDash.GetViewCase(model.requestid);
                return PartialView(result, data);
            }
            if (model.tabid == "ViewUpload")
            {
                var data = _AdminDash.ViewUploadData(model.requestid, patientname, confirmationno, email);
                return PartialView(result, data);
            }
            if (model.tabid == "SendOrder")
            {
                    var orderdetail = _AdminDash.SendOrder(model.requestid);

                return PartialView(result, orderdetail);
            }
            if (model.tabid == "EncounterForm")
            {
                var data = _AdminDash.GetEncounterForm(model.requestid);

                return PartialView(result, data);
            }
            if (model.tabid == "ViewNotes")
            {
                var data = _AdminDash.GetViewNotes(model.requestid);

                return PartialView(result, data);
            }
            if (model.tabid == "CreateNewReq")
            {
                return PartialView(result);
            }
            if (model.tabid == "CloseCase")
            {
                var data = _AdminDash.CloseCaseData(model.requestid);

                return PartialView(result, data);
            }
            /*******************records*******************/
            if (model.tabid == "BlockHistory")
            {
                //var data = _providerService.GetBlockHistoryData(model.searchstream);
                return PartialView("Tabs/Records/BlockHistory");
            }
            if (model.tabid == "EmailLogs")
            {
                var data = _providerService.GetEmailLogInfo();
                return PartialView("Tabs/Records/EmailLog",data);
            }
            if (model.tabid == "SmsLogs")
            {
                var data = model = _providerService.GetSMSLogInfo();
                return PartialView("Tabs/Records/SMSLog",data);
            }
            if (model.tabid == "PatientRecord")
            {
                return PartialView("Tabs/Records/PatientHistory");
            }
            if (model.tabid == "SearchRecords")
            {
                var data = _providerService.GetSearchRecordInfo();
                return PartialView("Tabs/Records/SearchRecords",data);
            }

            return View();

        }
        //public IActionResult DocumentList(int requestid)
        //{
        //    var res=_AdminDash.GetViewUpload(requestid);
        //    return View(res);
        //}
        //public IActionResult DownloadAll(int statusid)

        //{
        //    statusid = 1;
        //    MemoryStream ms = _AdminDash.ExportALl(statusid);
        //    return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "data.xlsx");


        //}

        public IActionResult GetPartialView(string btnName, int statusid, string searchValue, int currentpage, string dropdown, int reqtype)
        {

            //var partialview = "Partials/" + "_" + "NewPartial";

            var result = _AdminDash.GetDashboardData(btnName, statusid, searchValue, currentpage, dropdown, reqtype);
            //return PartialView(partialview, result);
            return PartialView("Partials/_StatusView", result);
        }
        public IActionResult GetModalPartialView(string modalName, int requestid, string patientname,int physicianid)
        {

            var partialname = "Partials/" + "_" + modalName;
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
            if (modalName == "CancelCase")
            {
                var result = _AdminDash.CancelCase(requestid, patientname);
                return PartialView(partialname, result);
            }
            if (modalName == "TransferRequest")
            {
                var result = _AdminDash.TransferRequest(requestid);
                return PartialView(partialname, result);
            }
            if (modalName == "ClearCase")
            {
                var result = _AdminDash.ClearCase(requestid);
                return PartialView(partialname, result);
            }
            if (modalName == "SendAgreement")
            {
                var result = _AdminDash.SendAgreeement(requestid);
                return PartialView(partialname, result);
            }
            if (modalName == "SendLink")
            {

                return PartialView(partialname);
            }

            if (modalName == "RequestDTY")
            {

                return PartialView(partialname);
            }
            if (modalName == "ContactProvider")
            {
                var result = _providerService.GetContactProvider(physicianid);
                return PartialView(partialname);

            }
            //if (modalName == "CreateShift")
            //{
            //    var data = _providerService.CreateShiftGet();
            //    return PartialView("Tabs/Scheduling/_CreateShift", data);
            //}
            return PartialView(partialname);

        }
        [HttpPost]
        public FileResult Export(string GridHtml)
        {
            _notyf.Custom("Document Downloaded Successfully!", 3, "green", "bi bi-check-circle-fill");
            return File(Encoding.ASCII.GetBytes(GridHtml), "application/vnd.ms-excel", "Grid.xls");
        }
        public FileResult ExportAll(AdminDashboard model)
        {
            byte[] excelBytes;
            IEnumerable<AdminDash> data = _AdminDash.GetPatientInfoByStatus((int)model.statusid).adminDashes;

            excelBytes = fileToExcel(data);
            _notyf.Custom("Document Downloaded Successfully!", 3, "green", "bi bi-check-circle-fill");
            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "sheet.xlsx");
        }
        public byte[] fileToExcel<T>(IEnumerable<T> data)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Data");
                PropertyInfo[] properties = typeof(T).GetProperties();
                for (int i = 0; i < properties.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = properties[i].Name;
                }
                int row = 2;

                foreach (var item in data)
                {
                    for (int i = 0; i < properties.Length; i++)
                    {
                        worksheet.Cells[row, i + 1].Value = properties[i].GetValue(item);
                    }
                    row++;
                }
                byte[] excelBytes = package.GetAsByteArray();
                return excelBytes;
            }
        }
        //send link 
       
        public IActionResult SendLinkDataPost(AdminDashboard model)
        {
          var adminid =(int) _httpContextAccessor.HttpContext.Session.GetInt32("Adminid");

            _AdminDash.SendMailLink(model, adminid);
            _notyf.Custom("Link Send Successfully", 3, "green", "bi bi-check-circle-fill");
            return Ok(new { message = "Data saved successfully." });

        }

        [HttpPost]
        public IActionResult AssignReq(AdminDashboard model)
        {
            _AdminDash.SubmitAssignReq(model);
            _notyf.Custom("Case Assign Successfully!", 3, "green", "bi bi-check-circle-fill");
            return Ok(new { message = "Data saved successfully." });

        }

        public IActionResult CancelCaseReq(AdminDashboard model, int requestid)

        {
            _AdminDash.submitCancelCase(model, requestid);
            _notyf.Custom("Case Cancelled Successfully!", 3, "green", "bi bi-check-circle-fill");

            return RedirectToAction("Index", "Admin");
        }

        public IActionResult BlockReq(AdminDashboard model, int requestid)

        {
            _AdminDash.SubmitBlockCase(model, requestid);

            _notyf.Custom("Case Blocked Successfully!", 3, "green", "bi bi-check-circle-fill");
            return RedirectToAction("Index", "Admin");
        }
        public IActionResult TransferReq(AdminDashboard model, int requestid)
        {
            _AdminDash.SubmitTransferReq(model, requestid);
            _notyf.Custom("Case Transferred Successfully!", 3, "green", "bi bi-check-circle-fill");
            return RedirectToAction("Index", "Admin");
        }
        [HttpPost]
        public IActionResult SendOrder(AdminDashboard model, int requestid, string adminname)
        {
            _AdminDash.SendOrderReq(model, requestid, adminname);
            _notyf.Custom("Order sent  Successfully!", 3, "green", "bi bi-check-circle-fill");
            return RedirectToAction("Index", "Admin");
        }
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
        public IActionResult SubmitClearCase(AdminDashboard model, int requestid, int adminid)
        {
            _AdminDash.SubmitClearCase(model, requestid, adminid);
            _notyf.Custom("Case Cleared Successfully!", 3, "green", "bi bi-check-circle-fill");
            return RedirectToAction("Index", "Admin");
        }
        public IActionResult ViewUpload()
        {

            return View();
        }
        public IActionResult EditViewCaseData(AdminDashboard model, int requestid)
        {
            _AdminDash.EditViewCaseData(model, requestid);
            _notyf.Custom("Data Saved Successfully!", 3, "green", "bi bi-check-circle-fill");
            return RedirectToAction("Index", "Admin");


        }
        public IActionResult GetDropDown(int selectedvalue)
        {

            var physicians = _db.Physicians.Where(x => x.Regionid == selectedvalue).Select(x => x.Firstname).ToList();
            return Json(physicians);
        }
        [HttpPost]
        public IActionResult SendAgreement(string email, int requestid)
        {
            _requestInterface.SendMailService(email, requestid);
            _notyf.Custom("Agrremtn mail sent Successfully!", 3, "green", "bi bi-check-circle-fill");

            return RedirectToAction("Index", "Admin");
        }
        //[HttpPost]
        //public IActionResult PostMyProfile( AdminDashboard adminDashboard,int adminid)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _AdminDash.PostMyProfile( adminDashboard,adminid);
        //    }
        //    return View();
        //}
        [HttpPost]
        public JsonResult PostViewNotes(AdminDashboard model)

        {
            _AdminDash.PostViewNotes(model);
            //_notyf.Custom("Notes Updated Successfully!", 3, "green", "bi bi-check-circle-fill");
            return Json(new { success = true });
            //return RedirectToAction("GetTabs", new { Tabid = "ViewNotes", requestid = requestid, statusid = 0, btnname = 0 });
        }
        /**********view uploads*****************/
        public IActionResult ViewUploadsList(int requestid)
        {
            var model = _AdminDash.ViewUploadDataList(requestid);
            return PartialView("Tabs/_ViewUploadPartial", model);
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

        [HttpPost]
        public IActionResult DeleteDocument(string filename, int requestid)
                {
            _AdminDash.deleteDocument(filename);
            _notyf.Information("dOCUMENT dELETED sUCCESSFULLY ...");

            return ViewUploadsList(requestid);

        }
        public IActionResult SendEmail(List<string> file, string email, int requestid)
        {
            _AdminDash.sendEmail(file, email, requestid);
            _notyf.Custom("Email Sent Successfully!!", 3, "deepskyblue", "bi bi-check2");
            return ViewUploadsList(requestid);
        }
        /*******create req*/
        [HttpPost]
        public IActionResult CreateRequestDatapost(AdminDashboard model)
        {
            _AdminDash.CreateRequestDatapost(model);

            _notyf.Custom("Request Created Successfully!!", 3, "deepskyblue", "bi bi-check2");
            return RedirectToAction("Index", "Admin");
        }
        public IActionResult VerifyAddress(string city)
        {
            if (city != null)
            {
                if (_AdminDash.VerifyAddress(city))
                {
                    return Json(new { success = true, message = "service avaliable " });
                }
                else
                {
                    return Json(new { success = false, message = "service not avaliable " });
                }
            }
            else
            {
                return Json(new { success = false, message = "Please Enter City Name" });
            }

        }
        /****close case**********/
        //[HttpPost]
        //      public JsonResult CloseCaseDataPost(AdminDashboard model)
        //      {
        //          _AdminDash.CloseCaseDataPost(model);
        //          return Json(new { success = true });
        //      }

        //      [HttpPost]
        //      public IActionResult CloseTheCase(int reqid)
        //      {
        //          _AdminDash.CloseTheCase(reqid);
        //          _notyf.Custom("Case CLosed Successfully!!", 3, "deepskyblue", "bi bi-check2");
        //          string url = Url.Action("Dashboard", "Admin");
        //          return Json(new { url });
        //      }
        public IActionResult EditCloseCase(AdminDashboard vc, int requestid)
        {
            bool result = _AdminDash.EditCloseCase(vc, requestid);
            if (result)
            {
                _notyf.Success("Case Edited Successfully..");
                return RedirectToAction("Index");
            }
            else
            {
                _notyf.Error("Case Not Edited...");
                return RedirectToAction("Index"); 
            }
        }
        public IActionResult CloseCaseUnpaid(int RequestID)
        {
            bool result = _AdminDash.CloseCase(RequestID);
            if (result)
            {
                _notyf.Success("Case Closed...");
                _notyf.Information("You can see Closed case in unpaid State...");
            }
            else
            {
                _notyf.Error("there is some error in CloseCase...");
            }
            return RedirectToAction("Index", "Admin");
        }
        /////////**********provider*********/
        ///[
        [HttpPost]
        public IActionResult PostProviderData(List<checkboxmodel> dataToSend)
        {

            _providerService.PostProviderData(dataToSend);

            _notyf.Information("Information updtaed ...");
            return Ok(new { message = "Data saved successfully." });
        }
        public IActionResult PostProviderProfile(AdminDashboard adminDashboard)
        {
            _providerService.PostProviderProfile(adminDashboard);
            _notyf.Information("Information updtaed ...");
            AdminDashboard admin = new AdminDashboard();
            admin.tabid = "PhysicianAccountEdit";
            admin.physicianid = adminDashboard.physicianid;



            return GetTabs(admin, default, default, default, default, default, default);
        }
        public IActionResult ContactProvider(AdminDashboard model)
        {
            _providerService.ContactProvider(model);
            _notyf.Success("Sms Sent Successfully");
            return Ok(new { message = "Data saved successfully." });

        }
        public IActionResult CreateRolesPost(short AccountTypeId, string RoleName, List<int> MenuIds)
        {
            _providerService.CreateRolePost(AccountTypeId, RoleName, MenuIds);
            _notyf.Success("Role Created Successfully");
            AdminDashboard admin = new AdminDashboard();
            admin.tabid = "CreateRole";
            return GetTabs(admin, default, default, default, default, default, default);

        }
        public IActionResult EditRolePost(string rolename, List<int> MenuIds)
        {
            _providerService.EditRolePost(rolename, MenuIds);

            _notyf.Success("Role Updated Successfully");
            AdminDashboard admin = new AdminDashboard();
            admin.tabid = "Access";
            return GetTabs(admin, default, default, default, default, default, default);
        }
        public IActionResult DeleteRolePost(int roleid)
        {
            _providerService.DeleteRolePost(roleid);
            _notyf.Success("Role Deleted Successfully");
            AdminDashboard admin = new AdminDashboard();
            admin.tabid = "Access";
            return GetTabs(admin, default, default, default, default, default, default);
        }
        public IActionResult OpenEditAccess(int accounttypeid, string aspnetuserid)
        {
            if (accounttypeid == 1)
            {
                AdminDashboard adminDashboard = new AdminDashboard();
                adminDashboard.tabid = "MyProfile";
                adminDashboard.aspnetuserid=aspnetuserid;

                return GetTabs(adminDashboard, default, default, default, default, default, aspnetuserid);
            }
            if (accounttypeid == 2)
            {
                AdminDashboard adminDashboard = new AdminDashboard();
                adminDashboard.tabid = "PhysicianAccountEdit";
                adminDashboard.aspnetuserid = aspnetuserid;

                return GetTabs(adminDashboard, default, default, default, default, default, aspnetuserid);
            }
           
            return Ok();
        }
       
        /*******provider location****/
        // provider location

        /** create admin**/
        [HttpPost]
        public IActionResult CreateAdminDataPost(AdminDashboard model)
        {
            _providerService.CreateAdminDataPost(model);
            _notyf.Information("Admin Created Successfully ...");
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.tabid = "CreateAdmin";
            return GetTabs(adminDashboard, default, default, default, default, default, default);
        }
        /**verify email**/
        public JsonResult CheckEmail(string email)
        {
            var userCheck = _db.Aspnetusers.Any(u =>
                u.Email == email);
            return Json(userCheck);
        }
        /**create provider**/
        public IActionResult CreateProviderDataPost(AdminDashboard model)
        {
            _providerService.CreateProviderDataPost(model);
            _notyf.Information("Provider Created Successfully ...");
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.tabid = "CreateProviderAccount";
            return GetTabs(adminDashboard, default, default, default, default, default, default);
        }

        /***my profile**/

        [HttpPost]
        public IActionResult MyProfileResetPassDataUpdate(AdminDashboard model)
        {
            if (model.myProfile.Password != null)
            {
                _providerService.MyProfileResetPassDataUpdate(model);
                _notyf.Information("INfo Updated Successfully ...");
                AdminDashboard adminDashboard = new AdminDashboard();
                adminDashboard.tabid = "MyProfile";
                adminDashboard.aspnetuserid = model.myProfile.Aspnetid;
                return GetTabs(adminDashboard, default, default, default, default, default, default);
            }
            else
            {
                return Json(new { success = false });
            }

        }
        [HttpPost]
        public IActionResult MyProfileDetailsDataUpdate(AdminDashboard model)
        {
            _providerService.MyProfileDetailsDataUpdate(model);
            _notyf.Information("INfo Updated Successfully ...");
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.tabid = "MyProfile";
            adminDashboard.aspnetuserid = model.myProfile.Aspnetid;
            return GetTabs(adminDashboard, default, default, default, default, default, default);

        }
        [HttpPost]
        public IActionResult MyProfileAddressDataUpdate(AdminDashboard model)
        {
            _providerService.MyProfileAddressDataUpdate(model);
            _notyf.Information("INfo Updated Successfully  ...");
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.tabid = "MyProfile";
            adminDashboard.aspnetuserid = model.myProfile.Aspnetid;
            return GetTabs(adminDashboard, default, default, default, default, default, default);
        }
        /******provider datapost***/
          /// Edit Physician /
  [HttpPost]
        public JsonResult PhysicianResetPassDataUpdate(string password, int physicianid)
        {
            if (password != null)
            {
                _providerService.PhysiscianResetPassDataUpdate(password, physicianid);
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
        [HttpPost]
        public JsonResult PhysicianAccountInfoDataUpdate(AdminDashboard model)
        {
            _providerService.PhysiscianAccountInfoDataUpdate(model);
            return Json(new { success = true });
        }
        [HttpPost]
        public JsonResult PhysicianInfoDataUpdate(AdminDashboard model)
        {
            _providerService.PhysicianInfoDataUpdate(model);
            return Json(new { success = true });
        }
        [HttpPost]
        public JsonResult PhysicianAddressDataUpdate(AdminDashboard model)
        {
            _providerService.PhysicianAddressDataUpdate(model);
            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult ProviderProfileDataUpdate(AdminDashboard model)
        {
            _providerService.ProviderProfileDataUpdate(model);
            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult ProviderOnboardingDataUpdate(AdminDashboard model)
        {
            _providerService.ProviderOnboardingDataUpdate(model);
            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult DeleteProviderAccount(int physicianid)
        {
            _providerService.DeleteProviderAccount(physicianid);
            return Json(new { success = true });
        }

        public IActionResult ViewDocument(string documentPath)
        {
            // Check if the file exists
            if (!System.IO.File.Exists(documentPath))
            {
                return NotFound();
            }

            // Read the file content
            var fileBytes = System.IO.File.ReadAllBytes(documentPath);
            var fileName = Path.GetFileName(documentPath);

            // Return the file as a FileStreamResult
            return File(fileBytes, "application/pdf", fileName);
        }

        /*** partner**/

        //Partner

        [HttpGet]
        public IActionResult GetPartnerTable(int ProfessionId,string vendorsearch,int currentpage)
        {
            var data = _providerService.PartnerDataGet(ProfessionId, vendorsearch,currentpage);
            return PartialView("Tabs/_PartnersTable", data);
        }
        [HttpPost]
        public IActionResult AddBusinessDataPost(AdminDashboard data)
        {
            _providerService.AddBusinessDataPost(data);
            _notyf.Success("Vendor Created Successfully");
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.tabid = "Partners";
            return GetTabs(adminDashboard, default, default, default, default, default, default);
        }

        [HttpPost]
        public IActionResult EditBusinessDataUpdate(AdminDashboard data)
        {
            _providerService.EditBusinessDataUpdate(data);
            _notyf.Information("Info Updated Successfully  ...");
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.tabid = "Partners";

            return GetTabs(adminDashboard, default, default, default, default, default, default);
        }
        [HttpPost]
        public IActionResult DeleteVendorDataPost(int VendorID)
        {
            _providerService.DeleteBusinessMethod(VendorID);
            _notyf.Success("Vendor Deleted Successfully");
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.tabid = "Partners";
            return GetTabs(adminDashboard, default, default, default, default, default, default);
        }
        /***scheduling**/


        //[HttpGet]

        //public async Task<IActionResult> SchedulingDataGet(int RegionId)
        //{


        //    var physician = _db.Physicians
        //        .Where(p => p.Regionid == RegionId || RegionId == 0)
        //        .Select(p => new
        //        {
        //            physicianId = p.Physicianid,
        //            firstName = p.Firstname,
        //            lastName = p.Lastname
        //        })
        //        .ToList();

        //    return Json(physician);
        //}
        //public IActionResult AddShift(AdminDashboard model)
        //{
        //    //string adminId = Crredntials.AspNetUserId();
        //    var chk = Request.Form["repeatdays"].ToList();
        //    _providerService.AddShift(model.ScheduleModel, chk, "2");
        //    _notyf.Information("Shift Created  Successfully  ...");
        //    AdminDashboard adminDashboard = new AdminDashboard();
        //    adminDashboard.tabid = "Scheduling";
        //    return GetTabs(adminDashboard, default, default, default, default, default, default);

        //}
        //public JsonResult GetEvents()
        //{
        //    var shiftDetails = _db.Shiftdetails
        //        .Select(sd => new
        //        {
        //            //id = sd.Shiftdetailid,
        //            //title = sd.phy,
        //            start = sd.Starttime,
        //            end = sd.Endtime
        //        });
        //    return Json(shiftDetails);
        //}
                /// Scheduling /

        [CustomAuthorize("1")]
        /*public IActionResult LoadSchedulingTab()
        {
            int reqid = 0;
            var model = _adminDashboard.GetRegion(reqid);
            return PartialView("Tabs/Provider/_Scheduling", model);
        }
*/
        public IActionResult LoadSchedulingCalendar(int RegionId, string currentView = "")
        {
            AdminDashboard model = new AdminDashboard();
            model.PhysicianProfilList = _providerService.GetProvider(RegionId);
            model.eventModel = _providerService.GetEvents(RegionId);
            model.currentView = currentView;

            return PartialView("Tabs/Scheduling/_SchedulingCalender", model);
        }

        public JsonResult CreateShift(scheduleModel scheduleModel)
        {
            var token = Request.Cookies["jwt"];
            var adminId = "";
            if (_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            {
                adminId = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserID").Value;
            }

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
            if (_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            {
                adminId = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserID").Value;
            }

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
            if (_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            {
                adminId = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserID").Value;
            }

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
            if (_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            {
                adminId = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserID").Value;
            }

            if (_providerService.ReturnShift(shiftDetailId, adminId))
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        public List<Physician> GetPhysicians(int Regionid)
        {
            return _db.Physicians.Where(p => p.Regionid == Regionid).ToList();
        }

        // MD or ProviderOnCall
        //public IActionResult LoadMDOnCall()
        //{
        //    AdminDashboard model = new AdminDashboard();
        //    _providerService.GetRegion(0);
        //    model.PhysicianProfilList = _providerService.GetProvider(0);

        //    return PartialView("_MDsOnCall", model);
        //}

        public IActionResult LoadMDOnCallData(int RegionId)
        {
            AdminDashboard model = new AdminDashboard();
            model= _providerService.GetProviderOnCallData(0);

            return PartialView("Tabs/Scheduling/_MDsOnCallData", model);
        }

        public IActionResult LoadRequestedShift()
        {
            //_providerService.GetRegion(0);

            return PartialView("_requestedShift");
        }

        public IActionResult LoadRequestedShiftTable(int RegionId = 0, bool currentMonthShift = false)
        {
            AdminDashboard model = new AdminDashboard();
            model.eventModel = _providerService.GetEvents(RegionId, currentMonthShift);

            return PartialView("Tabs/Scheduling/_RequestedShiftTable", model);
        }
        public JsonResult RequestedShiftUpdate(string ids, int type)
        {
            var token = Request.Cookies["jwt"];
            var adminId = "";
            //if (_JWTService.ValidateToken(token, out JwtSecurityToken jwtToken))
            //{
            //    adminId = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserID").Value;
            //}

            _providerService.RequestedShiftUpdate(ids, type, adminId);
            return Json(new { success = true });
        }
        /********************records**********************/

        [HttpPost]
        public IActionResult SearchRecordPartialTable(AdminDashboard model,int currentpage)
        {
            var info = _providerService.GetRecordTableInfo(model,currentpage);
            return PartialView("Tabs/Records/PartialTable/SearchRecordsPartialTable", info);
        }
        public IActionResult DeleteRequestFromSearchRecordsMethod(int requestid)
        {

            _providerService.DeleteRequestFromSearchRecordsMethod(requestid);
            _notyf.Custom("Request Deleted Permanently!", 3, "green", "bi bi-check-circle-fill");
            AdminDashboard model = new AdminDashboard();
            int currentpage = 1 ;
            return SearchRecordPartialTable(model,currentpage);
        }

          /******** BlockHostory Data Get***/
          [HttpPost]
        public IActionResult BlockedHistoryPartialTable(AdminDashboard model)
            {
            var info = _providerService.GetBlockHistoryData(model);
            return PartialView("Tabs/Records/PartialTable/BlockHistoryPartialTable", info);
        }
        //Unblock
        [HttpPost]
        public IActionResult UnblockRequest(int blockreqId)
        {
            _providerService.unblockreq(blockreqId);
            _notyf.Custom("Request Unblocked Successfully!", 3, "green", "bi bi-check-circle-fill");
            AdminDashboard model = new AdminDashboard();
            
            return BlockedHistoryPartialTable(model);

        }
        [HttpPost]
        public IActionResult EmailLogPartialTable(AdminDashboard model)
        {
            var info = _providerService.GetEmailLogTableInfo(model);
            return PartialView("Tabs/Records/PartialTable/EmailLogPartialTable", info);
        }
        [HttpPost]
        public IActionResult SMSLogPartialTable(AdminDashboard model)
        {
            var Info = _providerService.GetSMSLogTableInfo(model.SMSLog);
            return PartialView("Tabs/Records/PartialTable/SMSLogPartialTable", Info);
        }
        [HttpPost]
        public IActionResult PatientHistoryPartialTable(AdminDashboard model)
        {
            var Info = _providerService.PatientHistory(model);
            return PartialView("Tabs/Records/PartialTable/PatientHistoryPartialTable", Info);
        }
        [HttpPost]
        public IActionResult PatientRecordsPartialTable(int reqId,int currentpage)
        {
            var Info = _providerService.PatientRecords(reqId,currentpage);
            return PartialView("Tabs/Records/PartialTable/PatientRecordsPartialTable", Info);
        }

    }
}
