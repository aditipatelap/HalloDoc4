using BusinessLogic.Interface;
using BusinessLogic.Models;
using DataAccess.Data;
using DataAccess.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using static DataAccess.ViewModel.Constant;

namespace BusinessLogic.Service
{
    public class ProviderService:IProviderService
    {
        private readonly DataAccess.Data.ApplicationDbContext _db;

        public ProviderService(ApplicationDbContext db)
        {
            _db = db;
        }
        public AdminDashboard GetProviderData()
        {
            var result=_db.Physicians.Include(x=>x.Role).Include(x=>x.Physiciannotifications).Select(x=>new ProviderInfo
            {
                ProviderName = x.Firstname + "" + x.Lastname,
                physicianid=x.Physicianid,
                Role=x.Role.Name,
                notification=x.Physiciannotifications.FirstOrDefault().Isnotificationstopped,
          


                ProviderStatus=(PhysicianStatus)x.Status,
                //OnCall

            }).ToList();
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.ProviderInfo = result;
            return adminDashboard;
        }
         public void PostProviderData(List<checkboxmodel> model)    
        {
            foreach (var item in model)
            {
                var result = _db.Physiciannotifications.Where(x=>x.Pysicianid==item.physicianid).FirstOrDefault();
                if (result != null)
                {

                    result.Isnotificationstopped = new BitArray(new bool[1] { item.checkbox });

                }



            }
            _db.SaveChanges();
        }
        public AdminDashboard GetProviderAcccountData(int physicianid)
        {
            var result = _db.Physicians.Include(x=>x.Aspnetuser).Where(x => x.Physicianid == physicianid).Select(x => new Profile
            {
                UserName=x.Aspnetuser.Name,
                FirstName=x.Firstname,
                LastName=x.Lastname,
                Email=x.Email,
                PhoneNumber=x.Mobile,
                Address1 = x.Address1,
                Address2 = x.Address2,
                City=x.City,
                Zipcode=x.Zip,
                Businessname=x.Businessname,
                BusinessWebsite=x.Businesswebsite,
                status=(PhysicianStatus)x.Status


            }).FirstOrDefault();
            AdminDashboard adminDashboard= new AdminDashboard();
            adminDashboard.myProfile = result;
            return adminDashboard;
        }
        

    }
}
