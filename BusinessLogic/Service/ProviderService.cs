using BusinessLogic.Interface;
using DataAccess.Data;
using DataAccess.ViewModel;
using Microsoft.EntityFrameworkCore;
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
            var result=_db.Physicians.Select(x=>new ProviderInfo
            {
                ProviderName = x.Firstname + "" + x.Lastname,
                physicianid=x.Physicianid
            }).ToList();
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.ProviderInfo = result;
            return adminDashboard;
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
