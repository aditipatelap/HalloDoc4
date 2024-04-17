using AspNetCoreHero.ToastNotification.Abstractions;
using BusinessLogic.Interface;
using BusinessLogic.Service;
using DataAccess.Data;
using DataAccess.Models;
using DataAccess.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    public class ProviderController : Controller
    {
        private readonly IProviderInterface _providerDataService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProviderController(IProviderInterface providerService, IHttpContextAccessor httpContextAccessor)
        {
            _providerDataService = providerService;
            _httpContextAccessor = httpContextAccessor;

        }

        public IActionResult Index(ProviderDash model)
        {
          
            return View(model);
        }

        public IActionResult GetTabs(string tabid,int statusId, int reqTypeId, int currentpage, string searchstream, string region, int physicianId)
        {

           string aspnetuserid= _httpContextAccessor.HttpContext.Session.GetString("Aspnetuserid");
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
                var req = _providerDataService.GetProviderInfo(aspnetuserid);
                return PartialView("Tabs/_Profile", req);
            }

            return View();
            
        }
        public IActionResult GetPartialView( int statusid, int reqtype, int currentpage,string searchValue, string dropdown,int physicianid)
        {

            var data = _providerDataService.reqByStatus(statusid, reqtype, currentpage, searchValue, dropdown, physicianid);
            return PartialView("_RequestByStatusTable",data);

        }
    }
}
  