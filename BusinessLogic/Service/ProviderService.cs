using BusinessLogic.Interface;

using DataAccess.Data;
using DataAccess.Models;
using DataAccess.ViewModel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Web.WebPages;
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
                status=(PhysicianStatus)x.Status,
                physicianid=x.Physicianid,
                
               

            }).FirstOrDefault();
            AdminDashboard adminDashboard= new AdminDashboard();
            adminDashboard.myProfile = result;  
            //adminDashboard.physicianid = result.physicianid;
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
        public AdminDashboard UserAccessDataGet(int adminaccountfilter)


        {
            AdminDashboard model = new AdminDashboard();
            UserAccessModel userAccessModel = new UserAccessModel();
            var data = _db.Aspnetusers
                            .Include(x => x.Aspnetuserroles).Where(x=>x.Aspnetuserroles.FirstOrDefault().Roleid == adminaccountfilter || adminaccountfilter==0).Select(x => new UserAccessModel
                            {
                                AccountPOC = x.Name,
                                AccountType=(Roles)x.Aspnetuserroles.FirstOrDefault().Roleid,
                                PhoneNumber = x.Phonenumber,
                                status = (PhysicianStatus)_db.Admins.Where(y => y.Aspnetuserid == x.Id).Select(x => x.Status) .Union(_db.Users.Where(y => y.Aspnetuserid == x.Id).Select(x => x.Status)
                               .Union(_db.Physicians.Where(y => y.Aspnetuserid == x.Id).Select(x => x.Status))).FirstOrDefault(),
                               // UserId = _db.Users.Where(y => y.Aspnetuserid == x.Id).Select(x => x.Userid)
                   // .Union(_db.Physicians.Where(y => y.Aspnetuserid == x.Id).Select(x => x.Physicianid)).FirstOrDefault().union,
                                //OpenRequests=
                            }).ToList();
                
            model.userAccessModels = data;
            return model;

        }

        public List<Physicianlocation> GetPhysicianlocations()
        {
            var phyLocation = _db.Physicianlocations.ToList();
            return phyLocation;
        }

        /*********create admin**********/
        public AdminDashboard CreaeAdminDataGet()

        {
            var regions=_db.Regions.ToList();
            var roles = _db.Roles.ToList();
            AdminDashboard model = new AdminDashboard();    
            model.Regions = regions;
            model.RoleModel= roles;
            return model;
        }
        public void CreateAdminDataPost(AdminDashboard model)
        {
            Aspnetuser aspuser = new Aspnetuser();
            if (_db.Aspnetusers.Any(x => x.Email != model.myProfile.Email))
            {
                aspuser.Id = Guid.NewGuid().ToString();
                aspuser.Name = "ADMIN." + model.myProfile.FirstName + "." + model.myProfile.LastName;
                aspuser.Email = model.myProfile.Email;
                aspuser.Phonenumber = model.myProfile.PhoneNumber;
                aspuser.Createddate = DateTime.Now;

                // Hash the password
                var passwordHasher = new PasswordHasher<Aspnetuser>();
                aspuser.Passwordhash = passwordHasher.HashPassword(aspuser, model.myProfile.Password);

                _db.Aspnetusers.Add(aspuser);
                _db.SaveChanges();

                Aspnetuserrole aspnetrole = new Aspnetuserrole();

                aspnetrole.Userid = aspuser.Id;
                aspnetrole.Roleid = model.myProfile.roleid;

                _db.Aspnetuserroles.Add(aspnetrole);
                _db.SaveChanges();

                Admin admin = new Admin();

                admin.Aspnetuserid = aspuser.Id;
                admin.Firstname = model.myProfile.FirstName;
                admin.Lastname = model.myProfile.LastName;
                admin.Email = model.myProfile.Email;
                admin.Mobile = model.myProfile.PhoneNumber;
                admin.Address1 = model.myProfile.Address1;
                admin.Address2 = model.myProfile.Address2;

                admin.Status = (short)PhysicianStatus.Active;
                admin.Roleid = model.myProfile.roleid;
                admin.Regionid = model.myProfile.regionid;
                admin.Zip = model.myProfile.Zipcode;
                admin.Altphone = model.myProfile.Mobile;
                admin.Createdby = aspuser.Id;
                admin.Createddate = DateTime.Now;
                admin.Isdeleted = new BitArray(new bool[1] { false });

                _db.Admins.Add(admin);
                _db.SaveChanges();
            }


            //var list = model.AdministratorModel.selectedRegions.Split(",").Select(int.Parse).ToList();

            //foreach (var i in list)
            //{
            //    var a = new Adminregion
            //    {
            //        Adminid = data.Adminid,
            //        Regionid = i
            //    };
            //    _context.Adminregions.Add(a);
            //}
            //_context.SaveChanges();

            //return true;
        }

        /*** create provider**/
       

public AdminDashboard CreateProviderAdminDataGet()
        {

            var role = _db.Roles.ToList();
            var region = _db.Regions.ToList();

            AdminDashboard adminDashboardModel = new AdminDashboard();
            adminDashboardModel.RoleModel = role;
            adminDashboardModel.Regions = region;

            return adminDashboardModel;
        }

        public void CreateProviderDataPost(AdminDashboard model)
        {
            Aspnetuser aspuser = new Aspnetuser();

            aspuser.Id = Guid.NewGuid().ToString();
            aspuser.Name = "MD." + model.PhysicianProfile.firstName + "." + model.PhysicianProfile.lastName;
            aspuser.Email = model.PhysicianProfile.email;
            aspuser.Phonenumber = model.PhysicianProfile.phone;
            aspuser.Createddate = DateTime.Now;

            // Hash the password
            var passwordHasher = new PasswordHasher<Aspnetuser>();
            aspuser.Passwordhash = passwordHasher.HashPassword(aspuser, model.PhysicianProfile.Password);

            _db.Aspnetusers.Add(aspuser);
            _db.SaveChanges();

            Aspnetuserrole aspnetrole = new Aspnetuserrole();

            aspnetrole.Userid = aspuser.Id;
            aspnetrole.Roleid = (int)model.PhysicianProfile.roleid;

            _db.Aspnetuserroles.Add(aspnetrole);
            _db.SaveChanges();

            Physician physician = new Physician();

            physician.Aspnetuserid = aspuser.Id;
            physician.Firstname = model.PhysicianProfile.firstName;
            physician.Lastname = model.PhysicianProfile.lastName;
            physician.Email = model.PhysicianProfile.email;
            physician.Mobile = model.PhysicianProfile.phone;
            physician.Medicallicense = model.PhysicianProfile.LicenseNo;
            physician.Address1 = model.PhysicianProfile.Address1;
            physician.Address2 = model.PhysicianProfile.Address2;
            physician.City = model.PhysicianProfile.city;
            physician.Createdby = "Admin";
            physician.Businessname = model.PhysicianProfile.BusinessName;
            physician.Businesswebsite = model.PhysicianProfile.BusinessWebsite;
            physician.Roleid = model.PhysicianProfile.roleid;
            physician.Regionid = model.PhysicianProfile.status;
            physician.Status = 1;
            physician.Altphone = model.PhysicianProfile.phonenumber;
            physician.Isdeleted = new BitArray(new bool[1] { false });
            physician.Npinumber = model.PhysicianProfile.NIPNo;
            physician.Zip = model.PhysicianProfile.Zip;
            physician.Syncemailaddress = model.PhysicianProfile.SynchronizationEmailAddress;
            physician.Medicallicense = model.PhysicianProfile.LicenseNo;
            physician.Adminnotes = model.PhysicianProfile.notes;
            physician.Createddate = DateTime.Now;
            if (model.PhysicianProfile.Photo != null)
            {
                physician.Photo = FileUpload(model.PhysicianProfile.Photo, physician.Physicianid);
                physician.Photo = model.PhysicianProfile.Photo.FileName;
            }
            if (model.PhysicianProfile.AgreementDocument != null)
            {
                SaveDocument(model.PhysicianProfile.AgreementDocument, model.PhysicianProfile.physicianid, "agreementdoc", "Isagreementdoc", physician);
            }
            else
            {
                physician.Isagreementdoc = new BitArray(new bool[1] { false });
            }
            if (model.PhysicianProfile.BackgroundDocument != null)
            {
                SaveDocument(model.PhysicianProfile.BackgroundDocument, model.PhysicianProfile.physicianid, "backgrounddoc", "Isbackgrounddoc", physician);
            }
            else
            {
                physician.Isbackgrounddoc = new BitArray(new bool[1] { false });
            }
            if (model.PhysicianProfile.Iscredentialdoc != null)
            {
                SaveDocument(model.PhysicianProfile.CredentialDocument, model.PhysicianProfile.physicianid, "credentialdoc", "Iscredentialdoc", physician);
            }
            else
            {
                physician.Iscredentialdoc = new BitArray(new bool[1] { false });
            }
            if (model.PhysicianProfile.NonDisclosureDocument != null)
            {
                SaveDocument(model.PhysicianProfile.NonDisclosureDocument, model.PhysicianProfile.physicianid, "nondisclosuredoc", "Isnondisclosuredoc", physician);
            }
            else
            {
                physician.Isnondisclosuredoc = new BitArray(new bool[1] { false });
            }
            if (model.PhysicianProfile.LicenseDocument != null)
            {
                SaveDocument(model.PhysicianProfile.LicenseDocument, model.PhysicianProfile.physicianid, "licenseedoc", "Islicensedoc", physician);
            }
            else
            {
                physician.Islicensedoc = new BitArray(new bool[1] { false });
            }

            _db.Physicians.Add(physician);
            _db.SaveChanges();

            Physiciannotification physiciannotification = new Physiciannotification();
            physiciannotification.Pysicianid = physician.Physicianid;
            physiciannotification.Isnotificationstopped = new BitArray(new bool[1] { false });

            _db.Physiciannotifications.Add(physiciannotification);
            _db.SaveChanges();
        }
        string FileUpload(IFormFile file, int physicianId)
        {
            if (file != null && file.Length > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/PhysicianDocuments/" + physicianId);

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string filePath = Path.Combine(folderPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                return filePath;
            }
            return null;
        }
        private void SaveDocument(IFormFile document, int physicianId, string subfolder, string propertyName, Physician physician)
        {
            var propertyInfo = typeof(Physician).GetProperty(propertyName);
            if (document != null)
            {
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/PhysicianDocument/{physicianId}");
                string filePath = Path.Combine(folderPath, subfolder);

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    document.CopyTo(stream);
                }
                propertyInfo.SetValue(physician, new BitArray(new bool[1] { true }));

            }
        }
        
     /***my profile**/
        public AdminDashboard MyProfileDataGet(string aspnetuserid)
        {
            aspnetuserid = "19";
            var data = _db.Admins.Include(x => x.Aspnetuser).Where(x => x.Aspnetuser.Id == aspnetuserid).Select(x => new Profile
            {
                UserName = x.Aspnetuser.Name,
                Password = x.Aspnetuser.Passwordhash,
                status = (PhysicianStatus)x.Status,
                FirstName = x.Firstname,
                LastName = x.Lastname,
                Email = x.Email,
                ConfirmEmail = x.Email,
                PhoneNumber = x.Mobile,
                Mobile = x.Altphone,
                Address1 = x.Address1,
                Address2 = x.Address2,
               
                Zipcode = x.Zip,
                regionid = x.Regionid,
                roleid = x.Roleid,
                adminid = x.Adminid,
                //city = x.city,

            }).FirstOrDefault();

            var RegionCheckbox = _db.Adminregions.Include(x => x.Region).Where(x => x.Adminid == data.adminid).Select(x => new RegionCheckbox
            {
                RegionId = x.Regionid,
                Regionname = x.Region.Name,
            }).ToList();

            var role = _db.Roles.ToList();
            var region = _db.Regions.ToList();

            AdminDashboard adminDashboardModel = new AdminDashboard();
            //adminDashboardModel.myProfile.adminid = data.adminid;
            adminDashboardModel.myProfile = data;
            adminDashboardModel.myProfile.Aspnetid = aspnetuserid;
            adminDashboardModel.RoleModel = role;
            adminDashboardModel.Regions = region;
            adminDashboardModel.regionCheckbox = RegionCheckbox;
            return adminDashboardModel;
        }

        public void MyProfileResetPassDataUpdate(AdminDashboard model)
        {
            var data = _db.Aspnetusers.FirstOrDefault(x => x.Id == model.myProfile.Aspnetid);
            // Hash the password
            var passwordHasher = new PasswordHasher<Aspnetuser>();
            if (data != null)
            {
                data.Passwordhash = passwordHasher.HashPassword(data, model.myProfile.Password);
                _db.SaveChanges();
            }
        }

        public void MyProfileDetailsDataUpdate(AdminDashboard model)
        {
            var data = _db.Admins.FirstOrDefault(x => x.Aspnetuserid == model.myProfile.Aspnetid);
            if (data != null)
            {
                data.Firstname = model.myProfile.FirstName;
                data.Lastname = model.myProfile.LastName;
                data.Email = model.myProfile.Email;
                data.Email = model.myProfile.ConfirmEmail;
                data.Mobile = model.myProfile.PhoneNumber;
                data.Modifiedby = "Admin";
                data.Modifieddate = DateTime.Now;
                _db.SaveChanges();
            }
            var aspnetuser = _db.Aspnetusers.FirstOrDefault(x => x.Id == model.myProfile.Aspnetid);
            if (aspnetuser != null)
            {
                aspnetuser.Name = model.myProfile.FirstName + " " + model.myProfile.LastName;
                aspnetuser.Email = model.myProfile.Email;
                aspnetuser.Phonenumber = model.myProfile.PhoneNumber;
                aspnetuser.Modifieddate = DateTime.Now;
                _db.SaveChanges();
            }

            var region = _db.Adminregions.Where(x => x.Adminid == model.myProfile.adminid).ToList();
            _db.Adminregions.RemoveRange(region);
            _db.SaveChanges();

            foreach (var i in model.RegionArray)
            {
                Adminregion adminregion = new Adminregion();
                adminregion.Adminid = model.myProfile.adminid;
                adminregion.Regionid = i;
                _db.Add(adminregion);
                _db.SaveChanges();
            }
        }
        public void MyProfileAddressDataUpdate(AdminDashboard model)
        {
            var data = _db.Admins.FirstOrDefault(x => x.Aspnetuserid == model.myProfile.Aspnetid);
            if (data != null)
            {
                data.Address1 = model.myProfile.Address1;
                data.Address2 = model.myProfile.Address2;
                //data.City = model.myProfile.city;
                data.Zip = model.myProfile.Zipcode;
                data.Mobile = model.myProfile.Mobile;
                data.Regionid = model.myProfile.regionid;
                _db.SaveChanges();
            }
        }
        /*** partner**/
        //partner
        public AdminDashboard PartnerDataGet(int ProfessionId)
        {
            var data = _db.Healthprofessionals.Where(x => (ProfessionId == 0 || x.Profession == ProfessionId) && x.Isdeleted == new BitArray(new bool[1] { false })).Select(x => new PartnerModel
            {
                BusinessName = x.Vendorname,
                Profession = _db.Healthprofessionaltypes.Where(y => y.Healthprofessionalid == x.Profession).FirstOrDefault().Professionname,
                email = x.Email,
                phonenumber = x.Phonenumber,
                businessContact = x.Businesscontact,
                faxnumber = x.Faxnumber,
                businessId = x.Vendorid,
            }).ToList();
            AdminDashboard model = new AdminDashboard();
            model.PartnerModel = data;
            return model;
        }

        public AdminDashboard AddBusinessDataGet()
        {
            var data = _db.Healthprofessionaltypes.ToList();
            AdminDashboard adminModel = new AdminDashboard();
            adminModel.healthprofessionaltypes = data;

            return adminModel;
        }
        public void AddBusinessDataPost(AdminDashboard model)
        {
            Healthprofessional healthprofessional = new Healthprofessional();
            healthprofessional.Vendorname = model.AddBusinessModel.BusinessName;
            healthprofessional.Profession = model.AddBusinessModel.ProfessionID;
            healthprofessional.Faxnumber = model.AddBusinessModel.FAXNumber;
            healthprofessional.Address = model.AddBusinessModel.street + " " + model.AddBusinessModel.city + " " + model.AddBusinessModel.state + " " + model.AddBusinessModel.zip;
            healthprofessional.City = model.AddBusinessModel.city;
            healthprofessional.State = model.AddBusinessModel.state;
            healthprofessional.Zip = model.AddBusinessModel.zip;
            healthprofessional.Createddate = DateTime.Now;
            healthprofessional.Email = model.AddBusinessModel.Email;
            healthprofessional.Phonenumber = model.AddBusinessModel.PHoneNumber;
            healthprofessional.Businesscontact = model.AddBusinessModel.BusinessContanct;
            healthprofessional.Regionid = _db.Regions.Where(x => x.Name == model.AddBusinessModel.city).FirstOrDefault().Regionid;
            healthprofessional.Isdeleted = new BitArray(new bool[1] { false });
            _db.Add(healthprofessional);
            _db.SaveChanges();
        }

        public AdminDashboard EditBusinessDataGet(int VendorID)
        {
            AdminDashboard model = new AdminDashboard();

            var data1 = _db.Healthprofessionaltypes.ToList();
            var data = _db.Healthprofessionals.Where(x => x.Vendorid == VendorID).Select(x => new AddBusiness
            {
                BusinessName = x.Vendorname,
                FAXNumber = x.Faxnumber,
                PHoneNumber = x.Phonenumber,
                Email = x.Email,
                BusinessContanct = x.Businesscontact,
                city = x.City,
                state = x.State,
                zip = x.Zip,
                ProfessionID = x.Profession,
                vendorID = x.Vendorid,

            }).FirstOrDefault();
            model.AddBusinessModel = data;
            model.healthprofessionaltypes = data1;
            return model;
        }
        public void EditBusinessDataUpdate(AdminDashboard model)
        {
            var data = _db.Healthprofessionals.Where(x => x.Vendorid == model.AddBusinessModel.vendorID).FirstOrDefault();
            if (data != null)
            {
                data.Vendorname = model.AddBusinessModel.BusinessName;
                data.Faxnumber = model.AddBusinessModel.FAXNumber;
                data.Phonenumber = model.AddBusinessModel.PHoneNumber;
                data.Businesscontact = model.AddBusinessModel.BusinessContanct;
                data.Address = model.AddBusinessModel.street + " " + model.AddBusinessModel.city + " " + model.AddBusinessModel.state + " " + model.AddBusinessModel.zip;
                data.City = model.AddBusinessModel.city;
                data.State = model.AddBusinessModel.state;
                data.Zip = model.AddBusinessModel.zip;
                data.Profession = model.AddBusinessModel.ProfessionID;
                data.Email = model.AddBusinessModel.Email;
                data.Modifieddate = DateTime.Now;

                _db.SaveChanges();
            }
        }

        public AdminDashboard DeleteVendorDataGet(int VendorId)
        {
            AdminDashboard model = new AdminDashboard();
            model.VendorId = VendorId;

            return model;
        }
        public void DeleteBusinessMethod(int vendorID)
        {
            var data = _db.Healthprofessionals.Where(x => x.Vendorid == vendorID).FirstOrDefault();
            if (data != null)
            {
                data.Isdeleted = new BitArray(new bool[1] { true });
                data.Modifieddate = DateTime.Now;

                _db.SaveChanges();
            }
        }
        /***scheduling***/
        public AdminDashboard SchedulingDataGet(int RegionId)
        {
            var data = _db.Regions.ToList();
            var data1 = _db.Physicians.ToList();
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.Regions = data;
            adminDashboard.Physicians = data1;
            return adminDashboard;
        }

    }
}
