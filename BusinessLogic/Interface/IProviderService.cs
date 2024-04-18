using BusinessLogic.Service;
using DataAccess.Models;
using DataAccess.ViewModel;
using Microsoft.AspNetCore.Mvc;
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
        public AdminDashboard UserAccessDataGet(int adminaccountfilter,int currentpage);
        /****post  edit provider data*/
        public AdminDashboard EditPhysicianDataGet(string aspnetuserid);
        public void PhysiscianResetPassDataUpdate(string password, int physicianid);
        public void PhysiscianAccountInfoDataUpdate(AdminDashboard model);
        public void PhysicianInfoDataUpdate(AdminDashboard model);
        public void PhysicianAddressDataUpdate(AdminDashboard model);
        public void ProviderProfileDataUpdate(AdminDashboard model);
        public void ProviderOnboardingDataUpdate(AdminDashboard model);
        public void DeleteProviderAccount(int physicianid);


        /*** create admin***/
        public AdminDashboard CreaeAdminDataGet();
        public void CreateAdminDataPost(AdminDashboard model);
        /*provider location*/
        public AdminDashboard GetPhysicianlocations();
        /**create provider**/
        public AdminDashboard CreateProviderAdminDataGet();
        public void CreateProviderDataPost(AdminDashboard model);
        public AdminDashboard GetContactProvider(int physicianid);
        public void ContactProvider(AdminDashboard model);

        /**myprofile**/
        public AdminDashboard MyProfileDataGet(string aspnetuserid);
        public void MyProfileResetPassDataUpdate(AdminDashboard model);
        public void MyProfileDetailsDataUpdate(AdminDashboard model);
        public void MyProfileAddressDataUpdate(AdminDashboard model);
        /**partner**/
        public AdminDashboard PartnerDataGet(int ProfessionId, string vendorsearch,int currentpage);
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
        /*** Scheduling *******/
        public AdminDashboard GetRegion(int reqid);
        public List<EventModel> GetEventsByPhysicianID(int Physicianid);
        public AdminDashboard GetProviderOnCallData(int Regionid);
 public List<PhysicianProfile> GetProvider(int Regionid);
        public List<EventModel> GetEvents(int RegionId, bool currentMonthShift = false);
        public bool CreateShift(scheduleModel scheduleModel, string adminId);
        public bool EditShift(int shiftDetailId, DateTime Shiftdate, TimeOnly startTime, TimeOnly endTime, string adminId);
        public bool DeleteShift(int shiftDetailId, string adminId);
        public bool ReturnShift(int shiftDetailId, string adminId);
        public void RequestedShiftUpdate(string ids, int type, string adminId);
        /********************records********/
        public AdminDashboard GetSearchRecordInfo();

        public AdminDashboard GetRecordTableInfo(AdminDashboard model,int currentpage);
        public void DeleteRequestFromSearchRecordsMethod(int requestid);
        public AdminDashboard GetBlockHistoryData(AdminDashboard model);
        bool unblockreq(int blockreqid);
        // Emaillog
        public AdminDashboard GetEmailLogTableInfo(AdminDashboard model);
        AdminDashboard GetEmailLogInfo();
        // SMSlog
        public AdminDashboard GetSMSLogTableInfo(SMSLogList model);
        AdminDashboard GetSMSLogInfo();

        //Patient Records
        public AdminDashboard PatientHistory(AdminDashboard obj);
        public AdminDashboard PatientRecords(int id, int currentpage);

    }
    
}
