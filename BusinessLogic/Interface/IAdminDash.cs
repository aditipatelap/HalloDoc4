using DataAccess.Models;
using DataAccess.ViewModel;
using DocumentFormat.OpenXml.Presentation;
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

        public AdminDashboard GetDashboardData(int statusid, string searchValue, int currentpage, string dropdown, int reqtype);
        public AdminDashboard GetName(int statusid);
        public MemoryStream ExportALl(int statusid);
        //public int[] RequestCount(int statusid);
        public AdminDashboard RequestCount();
        public AdminDashboard AssignRequest(int requestid);

        public AdminDashboard CancelCase(int requestid);
        public void SubmitAssignReq (AdminDashboard model, int requestid);
        public void submitCancelCase(AdminDashboard model, int requestid);
        public AdminDashboard BlockCase(int reqid, string patientname);
        public void SubmitBlockCase(AdminDashboard model, int reqid);
        public AdminDashboard TransferRequest(int requestid);
        public void SubmitTransferReq(AdminDashboard model, int requestid);
        public AdminDashboard SendOrder(int requestid);
        public JsonArray GetBusiness(int selectedvalue);
        public Healthprofessional GetBusinessDetails(int selectedvalue);    
        public void SendOrderReq(AdminDashboard model, int requestid,string adminname);
        public AdminDashboard ClearCase(int requestid);
        public void SubmitClearCase(AdminDashboard model, int requestid, int adminid);
        public AdminDashboard GetViewCase(int requestid);
        public AdminDashboard GetViewUpload(int requestid);
        public AdminDashboard SendAgreeement(int requestid);



        }
}
