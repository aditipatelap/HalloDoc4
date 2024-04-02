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
        public AdminController(ApplicationDbContext db, IAdminDash adminDash, IHttpContextAccessor httpContextAccessor, INotyfService notyf,
            IRequestInterface requestInterface,ILoginInterface loginInterface,IProviderService providerService)
        {
            _db = db;
            _AdminDash = adminDash;
            _httpContextAccessor = httpContextAccessor;
            _notyf = notyf;
            _requestInterface= requestInterface;
            _loginInterface = loginInterface;
           _providerService = providerService;
        }
       
       
        public IActionResult Index(AdminDashboard model)
        {
            return View(model);
        }

        public IActionResult GetTabs(AdminDashboard model, int statusid, string btnname, string patientname,
            string confirmationno, string email, string aspnetuserid)
        {
            var result ="Tabs/"+ "_" + model.tabid;
           string adminid = _httpContextAccessor.HttpContext.Session.GetString("Adminid");
            if (model.tabid == "Dashboard")
            {
                var req = _AdminDash.RequestCount();

                return PartialView(result,req);
            }
            if(model.tabid == "MyProfile")
            {
                var req = _AdminDash.MyProfileDataGet(aspnetuserid);
                return PartialView(result,req);
            }
            if (model.tabid == "ProviderLocation")
            {
                
                return PartialView(result);
            }
            if (model.tabid == "Providers")
            {
                var data = _providerService.GetProviderData(model.regionid);
                return PartialView(result, data);
            }
            if (model.tabid == "PhysicianAccountEdit")
            {
                var data = _providerService.GetProviderAcccountData(aspnetuserid);
                return PartialView(result,data);
            }
            if (model.tabid == "Records")
            {
                return PartialView(result);
            }
            if (model.tabid == "Access")
            {
                var data = _providerService.GetAccessData();
                return PartialView(result,data);
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
                var data = _providerService.UserAccessDataGet(model.searchuseraccess);
                return PartialView(result, data);
            }
            if (model.tabid == "CreateAdmin")
            {
                
                return PartialView(result);
            }
            if (model.tabid == "Partners")
            {
                return PartialView(result);
            }
            if (model.tabid == "ViewCase")
            {
               var data = _AdminDash.GetViewCase(model.requestid);
                return PartialView(result, data);
            }
            if (model.tabid == "ViewUpload")
            {
                var data = _AdminDash.ViewUploadData(model.requestid,patientname, confirmationno,  email);
                return PartialView(result, data);
            }   
            if (model.tabid == "SendOrder")
            {
                var orderdetail= _AdminDash.SendOrder(model.requestid);

                return PartialView(result,orderdetail);
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

        public IActionResult GetPartialView(string btnName, int statusid, string searchValue,int currentpage ,  string dropdown,int reqtype)
        {

            var partialview = "Partials/" + "_" + btnName; 

            var result = _AdminDash.GetDashboardData(btnName, statusid, searchValue,currentpage,dropdown,reqtype);

            return PartialView(partialview, result);
        }
        public IActionResult GetModalPartialView(string modalName,int requestid, string patientname)
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
            if (modalName=="CancelCase")
            {
               var result = _AdminDash.CancelCase(requestid,patientname);
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
                return PartialView(partialname,result);
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
        public IActionResult AssignReq(AdminDashboard model, int requestid)
        {
            _AdminDash.SubmitAssignReq(model, requestid);
            _notyf.Custom("Case Assign Successfully!", 3, "green", "bi bi-check-circle-fill");
            return RedirectToAction("Index", "Admin");
        }
       
        public IActionResult CancelCaseReq(AdminDashboard model,int requestid)

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
        public IActionResult SendOrder(AdminDashboard model, int requestid,string adminname)
        {
            _AdminDash.SendOrderReq(model, requestid,adminname);
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
        public IActionResult SubmitClearCase(AdminDashboard model, int requestid,int adminid)
        {
            _AdminDash.SubmitClearCase(model, requestid,adminid);
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

            var physicians = _db.Physicians.Where(x => x.Regionid == selectedvalue).Select(x =>  x.Firstname).ToList();
            return Json(physicians);
        }
        [HttpPost]
        public IActionResult SendAgreement(string email, int requestid)
        {
            _requestInterface.SendMailService(email,requestid);
            _notyf.Custom("Agrremtn mail sent Successfully!", 3, "green", "bi bi-check-circle-fill");

            return RedirectToAction("Index", "Admin");
        }
        [HttpPost]
        public IActionResult PostMyProfile( AdminDashboard adminDashboard,int adminid)
        {
            if (ModelState.IsValid)
            {
                _AdminDash.PostMyProfile( adminDashboard,adminid);
            }
            return View();
        }
        [HttpPost]
        public JsonResult PostViewNotes( AdminDashboard model)
        
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
            _notyf.Custom("Document Deleted Successfully!!", 3, "deepskyblue", "bi bi-check2");
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
        public IActionResult PostProviderData( List<checkboxmodel> dataToSend)
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
            admin.physicianid= adminDashboard.physicianid;

            

            return GetTabs(admin,default,default,default,default,default,default);
        }
        public IActionResult CreateRolesPost(short AccountTypeId,string RoleName,List<int> MenuIds)
        {
            _providerService.CreateRolePost(AccountTypeId, RoleName, MenuIds);
            _notyf.Information("Role Created Successfully ...");
            AdminDashboard admin = new AdminDashboard();
            admin.tabid = "CreateRole";
            return GetTabs(admin, default, default, default, default, default, default);

        }
        public IActionResult EditRolePost(string rolename,List<int> MenuIds)
        {
            _providerService.EditRolePost(rolename, MenuIds);

            _notyf.Information("Role Updated Successfully ...");
            AdminDashboard admin = new AdminDashboard();
            admin.tabid = "Access";
            return GetTabs(admin, default, default, default, default, default, default);
        }
        public IActionResult DeleteRolePost (int roleid)
        {
            _providerService.DeleteRolePost(roleid);
            _notyf.Information("Role Deleted Successfully ...");
            AdminDashboard admin = new AdminDashboard();
            admin.tabid = "Access";
            return GetTabs(admin, default, default, default, default, default, default);
        }
        public IActionResult OpenEditAccess(int accounttypeid,string aspnetuserid)
        {
            if(accounttypeid==1)
            {
                AdminDashboard adminDashboard = new AdminDashboard();
                adminDashboard.tabid = "MyProfile";
               return GetTabs(adminDashboard, default, default, default, default, default, aspnetuserid);
            }
            if (accounttypeid == 2)
            {
                AdminDashboard adminDashboard = new AdminDashboard();
                adminDashboard.tabid = "PhysicianAccountEdit";

               return  GetTabs(adminDashboard, default, default, default, default, default, aspnetuserid);
            }
            if (accounttypeid == 3)
            {
                AdminDashboard adminDashboard = new AdminDashboard();
                adminDashboard.tabid = "MyProfile";
               return GetTabs(adminDashboard, default, default, default, default, default, aspnetuserid);
            }
            return Ok();
        }
        public IActionResult GetLocation()
        {
            List<Physicianlocation> getLocation = _providerService.GetPhysicianlocations();
            return Ok(getLocation);
        }
    }
}
