using AspNetCoreHero.ToastNotification.Abstractions;
using BusinessLogic.Interface;
using DataAccess.Data;
using DataAccess.Models;
using DataAccess.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    public class ProviderController : Controller
    {
        private readonly IProviderInterface _providerDataService;
        public ProviderController(IProviderInterface providerService)
        {
            _providerDataService = providerService;
        }

        public IActionResult Index(ProviderDash model)
        {
          
            return View(model);
        }

        public IActionResult GetTabs(string tabid,int statusId, int reqTypeId, int currentpage, string searchstream, string region, int physicianId)
        {

            tabid = "Dashboard";
            if (tabid == "Dashboard")
            {
                var data = _providerDataService.GetRequestStatusCountByPhysician(physicianId);

                return PartialView("Tabs/_Dashboard",data);
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
  