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

        public AdminDashboard GetDashboardData(int statusid);
        public AdminDashboard GetName(int statusid);
        //public int[] RequestCount(int statusid);
        public AdminDashboard RequestCount();

    }
}
