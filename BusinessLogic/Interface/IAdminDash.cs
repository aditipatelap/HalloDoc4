using DataAccess.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interface
{
    public  interface IAdminDash
    {

        public AdminDashboard GetDashboardData(int statusid, string searchValue);
        public AdminDashboard GetName(int statusid);
        //public int[] RequestCount(int statusid);
        public AdminDashboard RequestCount();
        public AdminDashboard AssignRequest(int requestid);

        public AdminDashboard CancelCase(int requestid);
        public void SubmitAssignReq (AdminDashboard model, int requestid);
        public void submitCancelCase(AdminDashboard model, int requestid);
        public AdminDashboard BlockCase(int reqid, string patientname);
        public void SubmitBlockCase(AdminDashboard model, int reqid);



        }
}
