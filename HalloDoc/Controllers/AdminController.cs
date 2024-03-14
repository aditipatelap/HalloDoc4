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

namespace HalloDoc.Controllers
{
    //[CustomAuthorize("1")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAdminDash _AdminDash;
        private readonly INotyfService _notyf;
        private readonly IRequestInterface _requestInterface;

        public AdminController(ApplicationDbContext db, IAdminDash adminDash, IHttpContextAccessor httpContextAccessor, INotyfService notyf,IRequestInterface requestInterface)
        {
            _db = db;
            _AdminDash = adminDash;
            _httpContextAccessor = httpContextAccessor;
            _notyf = notyf;
            _requestInterface= requestInterface;
        }
       
        public IActionResult AdminLogin() {
            ClaimsPrincipal claimuser = HttpContext.User;
            if(claimuser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> AdminLogin(LoginModel loginModel)

        {

            var user = _db.Aspnetusers.FirstOrDefault(u => u.Email == loginModel.Email && u.Passwordhash == loginModel.Password);

            if (user != null)
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,loginModel.Email),
                    new Claim("OtherProperties","Example Role")
                };
                ClaimsIdentity claimsidentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                //Authenticationproperties
                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsidentity), properties);
                //int id = _db.Users.FirstOrDefault(u => u.Aspnetuserid == user.Id).Userid;


                //string userName = _db.Users.Where(x => x.Aspnetuserid == user.Id).Select(x => x.Firstname + " " + x.Lastname).FirstOrDefault();




                return RedirectToAction("Dashboard", "Admin");
            }
            else
            {
                _notyf.Custom("Login Failed!", 3, "green", "bi bi-check-circle-fill");

            }
            return View();

        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);  


            return RedirectToAction("AdminLogin", "Admin");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetTabs(string Tabid,int requestid)
        {
            var result ="Tabs/"+ "_" + Tabid;
            if(Tabid=="Dashboard")
            {
                var req = _AdminDash.RequestCount();

                return PartialView(result,req);
            }
            if(Tabid=="MyProfile")
            {
                return PartialView(result);
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
                var model=_AdminDash.GetViewUpload(requestid);
                return PartialView(result,model);
            }
            if (Tabid == "SendOrder")
            {
                var orderdetail= _AdminDash.SendOrder(requestid);

                return PartialView(result,orderdetail);
            }
            return View();

        }
        //public IActionResult PaginatedDashboard()
        //{
        //    _AdminDash.GetDashboardData(statusid, searchValue);
        //    return PartialView(partialview, result);
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
               var result = _AdminDash.CancelCase(requestid);
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

            return PartialView(partialname);

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
    }
}
