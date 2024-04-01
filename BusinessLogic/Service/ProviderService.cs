using BusinessLogic.Interface;

using DataAccess.Data;
using DataAccess.Models;
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
        public AdminDashboard GetProviderData(int regionid)
            {
            var result=_db.Physicians.Include(x=>x.Role).Include(x=>x.Physiciannotifications).Where( x=>x.Regionid==regionid|| regionid==0).Select(x=>new ProviderInfo
            {
                ProviderName = x.Firstname + "" + x.Lastname,
                physicianid=x.Physicianid,
                Role=x.Role.Name,
                notification=x.Physiciannotifications.FirstOrDefault().Isnotificationstopped,
                 OnCall=x.Isnondisclosuredoc,


                ProviderStatus=(PhysicianStatus)x.Status,
                //OnCall

            }).ToList();
            var regions=_db.Regions.ToList();
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.ProviderInfo = result;
            adminDashboard.Regions = regions;
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
            adminDashboard.physicianid=physicianid;
            return adminDashboard;
        }
        public void PostProviderProfile(AdminDashboard adminDashboard)
        {
            Physician physician = new Physician();
            physician.Businessname = adminDashboard.myProfile.Businessname;
            physician.Businesswebsite=adminDashboard.myProfile.BusinessWebsite;
            physician.Adminnotes = adminDashboard.AdminNoes;

        

        }
        public AdminDashboard GetAccessData()
        {
            var result = _db.Roles.Where(x=>x.Isdeleted== new BitArray(new bool[1] { false })).Select(x => new AccountAccess
            {
                Name= x.Name,
                AccountType = (Roles) x.Accounttype,
                roleid=x.Roleid,

            }).ToList();
          
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.accountAccess = result;
          
            return adminDashboard;
        }
        public AdminDashboard GetCreateRoleData(int accounttype)
        {
            var result = _db.Menus.Where(x=>x.Accounttype== accounttype || accounttype==0).ToList();
          

            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.menu = result;

            return adminDashboard;
        }
        public void CreateRolePost(short AccountTypeId, string RoleName, List<int> MenuIds)
        {
            Role roles = new Role();
            roles.Name = RoleName;
            roles.Accounttype = AccountTypeId;
            roles.Createddate = DateTime.Now;
            roles.Createdby = "Admin";
            roles.Isdeleted = new BitArray(new bool[1] { false });
            _db.Add(roles);
            _db.SaveChanges();
            
            Rolemenu rolemenu = new Rolemenu();

            for (int i = 0; i < MenuIds.Count; i++)
            {
                rolemenu.Roleid = roles.Roleid;
                rolemenu.Menuid = MenuIds[i];

            }
            _db.Add(rolemenu);
            _db.SaveChanges();
        }
        public AdminDashboard GetEditRoleData(int roleid)
        {
            var result = _db.Roles.Where(x => x.Roleid == roleid).Select(x=> new EditAccountAccess
            {
                rolename= x.Name,
                AccountType=x.Accounttype,
            }).FirstOrDefault();

            var data = _db.Menus.Where(x => x.Accounttype == result.AccountType).ToList();
           
            var rolemenu = _db.Rolemenus.Where(x => x.Roleid == roleid).ToList();
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.EditaccountAccess = result;
            adminDashboard.menu = data;
            adminDashboard.rolemenus = rolemenu;

            return adminDashboard;
        }
        public void EditRolePost(string rolename, List<int> MenuIds)
        {
            var result = _db.Roles.FirstOrDefault(x => x.Name == rolename);
            if (result != null)
            {
                var existingMenuIds = _db.Rolemenus.Where(x => x.Roleid == result.Roleid).Select(x => x.Menuid).ToList();

                // Find unique MenuIds not already present in the Rolemenu table
                var uniqueMenuIds = MenuIds.Except(existingMenuIds).ToList();

                // Insert unique MenuIds into the Rolemenu table
                foreach (var menuId in uniqueMenuIds)
                {
                    var newRoleMenu = new Rolemenu
                    {
                        Roleid = result.Roleid,
                        Menuid = menuId
                    };
                    _db.Rolemenus.Add(newRoleMenu);
                }

                _db.SaveChanges();
            }
        }
        public void DeleteRolePost(int roleid)
        {
            var result = _db.Roles.Where(x => x.Roleid == roleid).FirstOrDefault();
           
            result.Isdeleted= new BitArray(new bool[1] { true });
            _db.SaveChanges();

        }
        public AdminDashboard UserAccessDataGet(string SearchUsers)
        {
           AdminDashboard model = new AdminDashboard();
           
        //    var data = _db.Aspnetusers.Where(x => SearchUsers == null || x.Name.ToLower().Contains(SearchUsers.ToString()) ||
        //                     x.Name.ToUpper().Contains(SearchUsers.ToString()))
        //                    .Include(x => x.Aspnetuserroles).Select(x => new UserAccessModel
        //                    {
        //                        AccountPOC = x.Name,
        //                        AccountType = _db.Roles.Where(y => y.Roleid == x.Aspnetuserroles.FirstOrDefault().Roleid).FirstOrDefault().Name,
        //                        PhoneNumber = x.Phonenumber,
        //                        status = (UserStatus)db.Users.Where(y => y.Aspnetuserid == x.Id).FirstOrDefault().Status,
        //                        //OpenRequests=
        //                    }).ToList();

        //    model.userAccessModels = data;
            return model;
        }



    }
}
