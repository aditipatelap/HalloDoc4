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
        //public ProviderDash GetProviderInfo(int physicianId);
        //public ProviderDash GetProviderAcccountData(string aspnetuserid);
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
    }
}
