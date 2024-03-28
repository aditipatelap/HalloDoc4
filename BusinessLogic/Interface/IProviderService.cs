using BusinessLogic.Service;
using DataAccess.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interface
{
    public  interface IProviderService
    {
        public AdminDashboard GetProviderData();
        public AdminDashboard GetProviderAcccountData(int physicianid);
        //public void PostProviderData(AdminDashboard model);

        public void PostProviderData(List<checkboxmodel> model);
    }
}
