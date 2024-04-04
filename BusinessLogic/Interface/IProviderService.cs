using BusinessLogic.Service;
using DataAccess.Models;
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
        public AdminDashboard GetProviderData(int regionid);
        public AdminDashboard GetProviderAcccountData(string aspnetuserid);
        //public void PostProviderData(AdminDashboard model);

        public void PostProviderData(List<checkboxmodel> model);
        public void PostProviderProfile(AdminDashboard adminDashboard);
        public AdminDashboard GetAccessData();
        public AdminDashboard GetCreateRoleData(int accounttype);
        public void CreateRolePost(short AccountTypeId, string RoleName, List<int> MenuIds);
        public AdminDashboard GetEditRoleData(int roleid);
        public void EditRolePost(string rolename, List<int> MenuIds);
        public void DeleteRolePost(int roleid);
        public AdminDashboard UserAccessDataGet(int adminaccountfilter);

        public List<Physicianlocation> GetPhysicianlocations();
        /*** create admin***/
        public AdminDashboard CreaeAdminDataGet();
        public void CreateAdminDataPost(AdminDashboard model);
        /**create provider**/
        public AdminDashboard CreateProviderAdminDataGet();
        public void CreateProviderDataPost(AdminDashboard model);
        /**myprofile**/
        public AdminDashboard MyProfileDataGet(string aspnetuserid);
        public void MyProfileResetPassDataUpdate(AdminDashboard model);
        public void MyProfileDetailsDataUpdate(AdminDashboard model);
        public void MyProfileAddressDataUpdate(AdminDashboard model);

    }
}
