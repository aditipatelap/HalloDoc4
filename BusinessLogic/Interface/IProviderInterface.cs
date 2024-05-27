using DataAccess.ViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interface
{
    public  interface IProviderInterface
    {
        public ProviderDash reqByStatus(int statusId, int reqTypeId, int currentpage, string searchstream, string region, int physicianId);
        public ProviderDash GetRequestStatusCountByPhysician(int physicianId);
        public ProviderDash GetProviderInfo(string aspnetuserid);
        public bool SetTypeOfCare(int requestid, int TOCId);
        /*******conclude care*/
        public AdminDashboard ConcludeCareGet(int requestid);

        public bool ConcludeCare(int reqId, string notes, int phyId);
        /*******enciunter*/
        public bool FinalizeEncounterForm(int reqid);
        /*transferreq*/
        public void TransferCaseDataPost(ProviderDash model);
        public void AcceptRequest(int Requestid, int physicianid);
        public void SaveDocument(IFormFile Document, int reqid, int Physicianid);
        public bool CreateShift(scheduleModel scheduleModel, string aspnetuserid);
        /*viewnote****/
        public void PostViewNotes(AdminDashboard model, string aspnetuserid);
        public void SendMailLink(AdminDashboard model, int physicianid);
        public bool ConcludeHouseCall(int reqid);
        //good to have
        public List<TimesheetModel> SearchDataByRangeTimeSheet(DateTime startDate, int Physicianid);
        public bool CheckFinalize(DateTime startDate, int Physicianid);
        public List<InvoicingModel> SearchDataByRangeInvoicing(DateTime startDate, int Physicianid);
        public List<InvoicingModel> SearchDataByRangeReimbursement(DateTime startDate, int Physicianid);
        public int GetOnCallHours(int physicianId, DateTime date);
        public bool SaveTimeSheet(List<InvoicingModel> invoicingModels, int Physicianid);
        public bool SaveReimbursement(InvoicingModel invoicingModels, int Physicianid);
        public void FinalizeTimesheet(DateTime startDate, int Physicianid);
        public bool DeleteReimbursement(InvoicingModel invoicingModels, int Physicianid);
    }
}
