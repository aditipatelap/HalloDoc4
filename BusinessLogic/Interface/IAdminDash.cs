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
        
        public List<AdminDashboard> GetDashboardData(int statusid);
    }
}
