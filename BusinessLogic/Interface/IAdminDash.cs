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
        public AdminDashboard AssignRequest();
        public void submitCancelCase(AdminDashboard model, int requestid);

    }
}
