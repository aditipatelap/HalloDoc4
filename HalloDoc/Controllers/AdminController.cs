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
        public AdminController(ApplicationDbContext db, IAdminDash adminDash, IHttpContextAccessor httpContextAccessor, INotyfService notyf,
            IRequestInterface requestInterface,ILoginInterface loginInterface)
        {
            _db = db;
            _AdminDash = adminDash;
            _httpContextAccessor = httpContextAccessor;
            _notyf = notyf;
            _requestInterface= requestInterface;
            _loginInterface = loginInterface;
        }
       
       
        public IActionResult Index(AdminDashboard model)
        {
            return View(model);
        }

        public IActionResult GetTabs(string Tabid,int requestid,int statusid,string btnname, string patientname, string confirmationno, string email)
        {
            var result ="Tabs/"+ "_" + Tabid;
           string adminid = _httpContextAccessor.HttpContext.Session.GetString("Adminid");
            if (Tabid=="Dashboard")
            {
                var req = _AdminDash.RequestCount();

                return PartialView(result,req);
            }
            if(Tabid=="MyProfile")
            {
                var req = _AdminDash.GetMyProfile(adminid);
                return PartialView(result,req);
            }
            if (Tabid == "ProviderLocation")
            {
                return PartialView(result);
            }
            if (Tabid == "Providers")
            {
                return PartialView(result);
            }
            if (Tabid == "Records")
            {
                return PartialView(result);
            }
            if (Tabid == "Access")
            {
                return PartialView(result);
            }
            if (Tabid == "Partners")
            {
                return PartialView(result);
            }
            if (Tabid == "ViewCase")
            {
               var model = _AdminDash.GetViewCase(requestid);
                return PartialView(result,model);
            }
            if (Tabid == "ViewUpload")
            {
                var model=_AdminDash.ViewUploadData(requestid,patientname, confirmationno,  email);
                return PartialView(result,model);
            }   
            if (Tabid == "SendOrder")
            {
                var orderdetail= _AdminDash.SendOrder(requestid);

                return PartialView(result,orderdetail);
            }
            if (Tabid == "EncounterForm")
            {
                var model = _AdminDash.GetEncounterForm(requestid);

                return PartialView(result, model);
            }
            if (Tabid == "ViewNotes")
            {
                var model = _AdminDash.GetViewNotes(requestid);

                return PartialView(result, model);
            }
            if (Tabid == "CreateNewReq")
            {
             

                return PartialView(result);
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

            var result = _AdminDash.GetDashboardData(statusid, searchValue,currentpage,dropdown,reqtype);

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
            if (modalName == "CloseCase")
            {

                return PartialView(partialname);
            }

            return PartialView(partialname);

        }
        [HttpPost]
        public FileResult Export(string GridHtml)
        {
            return File(Encoding.ASCII.GetBytes(GridHtml), "application/vnd.ms-excel", "Grid.xls");
        }
        public FileResult ExportAll(AdminDashboard model)
        {
            byte[] excelBytes;
            IEnumerable<AdminDash> data = _AdminDash.GetPatientInfoByStatus((int)model.statusid).adminDashes;
            
            excelBytes = fileToExcel(data);
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
             return RedirectToAction("Index", "Admin");
        }
       
        public IActionResult CancelCaseReq(AdminDashboard model,int requestid)

        {
            _AdminDash.submitCancelCase(model, requestid);


            return RedirectToAction("Index", "Admin");
        }
        
        public IActionResult BlockReq(AdminDashboard model, int requestid)

        {   
            _AdminDash.SubmitBlockCase(model, requestid);


            return RedirectToAction("Index", "Admin");
        }
        public IActionResult TransferReq(AdminDashboard model, int requestid)
        {
            _AdminDash.SubmitTransferReq(model, requestid);
            return RedirectToAction("Index", "Admin");
        }
        [HttpPost]
        public IActionResult SendOrder(AdminDashboard model, int requestid,string adminname)
        {
            _AdminDash.SendOrderReq(model, requestid,adminname);
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
            return RedirectToAction("Index", "Admin");
        }
        public IActionResult ViewUpload()
        {

            return View();
        }
        public IActionResult EditViewCaseData(AdminDashboard model, int requestid)
            {
            _AdminDash.EditViewCaseData(model, requestid);
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

            return RedirectToAction("Index", "Admin");
        }
        public IActionResult PostMyProfile(string adminid,AdminDashboard adminDashboard)
        {
            if (ModelState.IsValid)
            {
                _AdminDash.PostMyProfile(adminid, adminDashboard);
            }
            return View();
        }
        [HttpPost]
        public JsonResult PostViewNotes( AdminDashboard model)
        
        {
            _AdminDash.PostViewNotes(model);
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

    }
}
