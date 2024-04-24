using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DataAccess.CustomModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using BusinessLogic.Interfaces;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace Assignment.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly IAssignmentInterface _assignmentInterface;
        private readonly INotyfService _notyf;

        public AssignmentController(IAssignmentInterface assignmentInterface, INotyfService notyf)
        {
            _assignmentInterface = assignmentInterface;
            _notyf = notyf;
        }
      
        public IActionResult Index()
        {
           
            return View();
        }  
        public IActionResult MainDataGet(string searchval, int currentpage)
        {
            var res = _assignmentInterface.GetProjectData(searchval, currentpage);
            return PartialView("_MainData", res);
        }
        public IActionResult ModalPartialView() {
            var res = _assignmentInterface.AddFormDataGEt();
            return PartialView("_AddForm",res);
        }
        
        public IActionResult EditModalPartialView(int id)
        {
            var res=_assignmentInterface.EditFormDataGet(id);
            return PartialView("_EditForm",res);
        }
        [HttpPost]
        public IActionResult EditFormData(MainModel mainModel)
        {
             _assignmentInterface.EditFormDataPost(mainModel.AddData);
            _notyf.Success("Data Edited Successfully!!");
            return RedirectToAction("Index");
        }
        public IActionResult Add(MainModel mainModel)
        {   
            _assignmentInterface.AddFormDataPost(mainModel.AddData);
            _notyf.Success("Data Added Successfully!!");
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int Projectid)
        {
            _assignmentInterface.DeleteFormDataPost(Projectid);
            _notyf.Success("Data Deleted Successfully!!");
            return RedirectToAction("Index");   
        }


    }
}
