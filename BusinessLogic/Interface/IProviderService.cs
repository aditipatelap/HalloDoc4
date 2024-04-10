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
        public AdminDashboard GetProviderAcccountData(int physicianid);
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
        /**partner**/
        public AdminDashboard PartnerDataGet(int ProfessionId);
        public AdminDashboard AddBusinessDataGet();
        public void AddBusinessDataPost(AdminDashboard model);
        public AdminDashboard EditBusinessDataGet(int VendorID);
        public void EditBusinessDataUpdate(AdminDashboard model);
        public AdminDashboard DeleteVendorDataGet(int VendorId);
        public void DeleteBusinessMethod(int vendorID);
        //public AdminDashboard SchedulingDataGet(int RegionId);
        /*** scheduling**/
       // public AdminDashboard CreateShiftGet();
        //public void AddShift(ScheduleModel model, List<string?>? chk, string adminId);
         /// Scheduling /
 public List<PhysicianProfile> GetProvider(int Regionid);
        public List<EventModel> GetEvents(int RegionId, bool currentMonthShift = false);
        public bool CreateShift(scheduleModel scheduleModel, string adminId);
        public bool EditShift(int shiftDetailId, DateTime Shiftdate, TimeOnly startTime, TimeOnly endTime, string adminId);
        public bool DeleteShift(int shiftDetailId, string adminId);
        public bool ReturnShift(int shiftDetailId, string adminId);
        /********************records********/
        public AdminDashboard GetSearchRecordInfo();
        public AdminDashboard GetRecordTableInfo(searchstream model);
        public AdminDashboard GetBlockHistoryData(searchstream model);
        bool unblockreq(int blockreqid);
        // Emaillog
        public AdminDashboard GetEmailLogTableInfo(EmailLogList model);
        AdminDashboard GetEmailLogInfo();
        // SMSlog
        public AdminDashboard GetSMSLogTableInfo(SMSLogList model);
        AdminDashboard GetSMSLogInfo();

        //Patient Records
        public AdminDashboard PatientHistory(searchstream obj);
        public AdminDashboard PatientRecords(int id);

    }
    
}
