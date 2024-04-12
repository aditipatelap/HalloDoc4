    using DataAccess.Models;
using DataAccess.ViewModel;
using DocumentFormat.OpenXml.Presentation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace BusinessLogic.Interface
{
    public  interface IAdminDash
    {

        public AdminDashboard GetDashboardData(string btnname,int statusid, string searchValue, int currentpage, string dropdown, int reqtype);
        public AdminDashboard GetName(int statusid);
        //public MemoryStream ExportALl(int statusid);
        //public int[] RequestCount(int statusid);
        public AdminDashboard GetPatientInfoByStatus(int statusid);
        public AdminDashboard RequestCount();
        public AdminDashboard AssignRequest(int requestid);

        public AdminDashboard CancelCase(int requestid, string patientname);
        public void SubmitAssignReq (AdminDashboard model);
        public void submitCancelCase(AdminDashboard model, int requestid);
        public AdminDashboard BlockCase(int reqid, string patientname);
        public void SubmitBlockCase(AdminDashboard model, int reqid);
        public AdminDashboard TransferRequest(int requestid);
        public void SubmitTransferReq(AdminDashboard model, int requestid);
        public AdminDashboard SendOrder(int requestid);
        public AdminDashboard GetViewNotes(int requestid);
        public void PostViewNotes( AdminDashboard model);
        public JsonArray GetBusiness(int selectedvalue);
        public Healthprofessional GetBusinessDetails(int selectedvalue);    
        //public AdminDashboard GetBusinessDetails(int selectedvalue);
        public void SendOrderReq(AdminDashboard model, int requestid,string adminname);
        public AdminDashboard ClearCase(int requestid);
        public void SubmitClearCase(AdminDashboard model, int requestid, int adminid);
        public AdminDashboard GetViewCase(int requestid);
        public void EditViewCaseData(AdminDashboard model, int requestid);
       // public AdminDashboard GetViewUpload(int requestid);
        public AdminDashboard SendAgreeement(int requestid);
        public AdminDashboard GetEncounterForm(int requestid);
        //public AdminDashboard MyProfileDataGet(string aspnetuserid);
        //public void PostMyProfile(AdminDashboard model, int adminid);
        public void SaveDocument(IFormFile Document, int requestid);
        public AdminDashboard ViewUploadData(int reqid, string patientname, string confirmationno, string email);
        public AdminDashboard ViewUploadDataList(int reqid);
        public void deleteDocument(string filename);
        public void sendEmail(List<string> file, string email, int reqid);
        public void CreateRequestDatapost(AdminDashboard model);
        public bool VerifyAddress(string city);
        //public void CloseCaseDataPost(AdminDashboard model);
        //public void CloseTheCase(int reqid);
        public AdminDashboard CloseCaseData(int RequestID);
        public bool EditCloseCase(AdminDashboard vp, int RequestID);
        public bool CloseCase(int RequestID);

        }
}
