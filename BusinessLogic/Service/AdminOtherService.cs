using BusinessLogic.Interface;
using Microsoft.AspNetCore.Http;
using DataAccess.Data;
using DataAccess.Models;
using DataAccess.ViewModel;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Net.Http;
using System.Web.WebPages;
using static DataAccess.ViewModel.Constant;

using System.Drawing.Printing;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System.Net.Mail;
namespace BusinessLogic.Service
{
    public class ProviderService : IProviderService
    {
        private readonly DataAccess.Data.ApplicationDbContext _db;

        public ProviderService(ApplicationDbContext db)
        {
            _db = db;
        }
        public AdminDashboard GetProviderData(int regionid)
        {
            var result = _db.Physicians.Include(x => x.Role).Include(x => x.Physiciannotifications).Where(x => x.Regionid == regionid || regionid == 0).Select(x => new ProviderInfo
            {
                ProviderName = x.Firstname + "" + x.Lastname,
                physicianid = x.Physicianid,
                Role = x.Role.Name,
                notification = x.Physiciannotifications.FirstOrDefault().Isnotificationstopped,
                OnCall = x.Isnondisclosuredoc,


                ProviderStatus = (PhysicianStatus)x.Status,
                //OnCall

            }).ToList();
            var regions = _db.Regions.ToList();
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.ProviderInfo = result;
            adminDashboard.Regions = regions;
            return adminDashboard;
        }
        public void PostProviderData(List<checkboxmodel> model)
        {
            foreach (var item in model)
            {
                var result = _db.Physiciannotifications.Where(x => x.Pysicianid == item.physicianid).FirstOrDefault();
                if (result != null)
                {

                    result.Isnotificationstopped = new BitArray(new bool[1] { item.checkbox });

                }
            }
            _db.SaveChanges();
        }
        public AdminDashboard GetProviderAcccountData(int physicianid)
        {
            var result = _db.Physicians.Include(x => x.Aspnetuser).Where(x => x.Physicianid == physicianid).Select(x => new Profile
            {
                UserName = x.Aspnetuser.Name,
                FirstName = x.Firstname,
                LastName = x.Lastname,
                Email = x.Email,
                PhoneNumber = x.Mobile,
                Address1 = x.Address1,
                Address2 = x.Address2,
                City = x.City,
                Zipcode = x.Zip,
                Businessname = x.Businessname,
                BusinessWebsite = x.Businesswebsite,
                status = (PhysicianStatus)x.Status,
                physicianid = x.Physicianid,



            }).FirstOrDefault();
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.myProfile = result;
            //adminDashboard.physicianid = result.physicianid;
            return adminDashboard;
        }
        public void PostProviderProfile(AdminDashboard adminDashboard)
        {
            Physician physician = new Physician();
            physician.Businessname = adminDashboard.myProfile.Businessname;
            physician.Businesswebsite = adminDashboard.myProfile.BusinessWebsite;
            physician.Adminnotes = adminDashboard.AdminNoes;



        }
        public AdminDashboard GetAccessData()
        {
            var result = _db.Roles.Where(x => x.Isdeleted == new BitArray(new bool[1] { false })).Select(x => new AccountAccess
            {
                Name = x.Name,
                AccountType = (Roles)x.Accounttype,
                roleid = x.Roleid,

            }).ToList();

            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.accountAccess = result;

            return adminDashboard;
        }
        public AdminDashboard GetCreateRoleData(int accounttype)
        {
            var result = _db.Menus.Where(x => x.Accounttype == accounttype || accounttype == 0).ToList();


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
            var result = _db.Roles.Where(x => x.Roleid == roleid).Select(x => new EditAccountAccess
            {
                rolename = x.Name,
                AccountType = x.Accounttype,
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

            result.Isdeleted = new BitArray(new bool[1] { true });
            _db.SaveChanges();

        }
        public AdminDashboard UserAccessDataGet(int adminaccountfilter)


        {
            AdminDashboard model = new AdminDashboard();
            UserAccessModel userAccessModel = new UserAccessModel();
            var data = _db.Aspnetusers
                            .Include(x => x.Aspnetuserroles).Include(x => x.Physicians).Include(x => x.AdminAspnetusers).Where(x => x.Aspnetuserroles.FirstOrDefault().Roleid == adminaccountfilter || adminaccountfilter == 0).Select(x => new UserAccessModel
                            {
                                AccountPOC = x.Name,
                                AccountType = (Roles)x.Aspnetuserroles.FirstOrDefault().Roleid,
                                PhoneNumber = x.Phonenumber,
                                status = (PhysicianStatus)_db.Admins.Where(y => y.Aspnetuserid == x.Id).Select(x => x.Status).Union(_db.Users.Where(y => y.Aspnetuserid == x.Id).Select(x => x.Status)
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
            var regions = _db.Regions.ToList();
            var roles = _db.Roles.ToList();
            AdminDashboard model = new AdminDashboard();
            model.Regions = regions;
            model.RoleModel = roles;
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
                aspnetrole.Roleid = (short)Roles.Admin;

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

        public AdminDashboard PartnerDataGet(int ProfessionId, string vendorsearch)
        {
            var data = _db.Healthprofessionals.Where(x => (ProfessionId == 0 || x.Profession == ProfessionId)
                                                     && (vendorsearch == null || x.Vendorname.ToLower().Contains(vendorsearch.ToLower()))

                                                     && x.Isdeleted == new BitArray(new bool[1] { false })).Select(x => new PartnerModel
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
            //string ipAddress = GetUserIpAddress(HttpContext httpContext);
            Healthprofessional healthprofessional = new Healthprofessional();
            healthprofessional.Vendorname = model.AddBusinessModel.BusinessName;
            healthprofessional.Profession = model.AddBusinessModel.ProfessionID;
            healthprofessional.Faxnumber = model.AddBusinessModel.FAXNumber;
            healthprofessional.Address = model.AddBusinessModel.street;
            healthprofessional.City = model.AddBusinessModel.city;
            healthprofessional.State = model.AddBusinessModel.state;
            healthprofessional.Zip = model.AddBusinessModel.zip;
            healthprofessional.Createddate = DateTime.Now;
            healthprofessional.Email = model.AddBusinessModel.Email;
            healthprofessional.Phonenumber = model.AddBusinessModel.PHoneNumber;
            healthprofessional.Businesscontact = model.AddBusinessModel.BusinessContanct;
            healthprofessional.Regionid = _db.Regions.Where(x => x.Name == model.AddBusinessModel.city).FirstOrDefault().Regionid;
            healthprofessional.Isdeleted = new BitArray(new bool[1] { false });
            //healthprofessional.Ip=ipAddress;
            _db.Add(healthprofessional);
            _db.SaveChanges();
        }

        private string GetUserIpAddress(HttpContext httpContext)
        {
            string ipAddress = httpContext.Connection.RemoteIpAddress.ToString();

            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = httpContext.Request.Headers["X-Forwarded-For"];
            }

            return ipAddress;
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
                street = x.Address,
                ModifiedDate = DateTime.Now,

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
        //public AdminDashboard SchedulingDataGet(int RegionId)
        //        {
        //    var data = _db.Regions.ToList();
        //    //var data1 = _db.Physicians.ToList();
        //    AdminDashboard adminDashboard = new AdminDashboard();
        //    adminDashboard.Regions = data;
        //    //adminDashboard.Physicians = data1;
        //    return adminDashboard;
        //}
        //public AdminDashboard CreateShiftGet()
        //{
        //    var data = _db.Regions.ToList();
        //    var data1 = _db.Physicians.ToList();
        //    AdminDashboard adminDashboard = new AdminDashboard();
        //    adminDashboard.Regions = data;
        //    adminDashboard.Physicians = data1;
        //    return adminDashboard;

        //}
        //public void AddShift(ScheduleModel model, List<string?>? chk, string adminId)
        //{
        //    //AdminDashboard data = new AdminDashboard();
        //    //var model = data.ScheduleModel;

        //    var shiftid = _db.Shifts.Where(u => u.Physicianid == model.Physicianid).Select(u => u.Shiftid).ToList();
        //    if (shiftid.Count() > 0)
        //    {
        //        foreach (var obj in shiftid)
        //        {
        //            var shiftdetailchk = _db.Shiftdetails.Where(u => u.Shiftid == obj && u.Shiftdate == DateOnly.FromDateTime( model.Shiftdate)).ToList();
        //            if (shiftdetailchk.Count() > 0)
        //            {
        //                foreach (var item in shiftdetailchk)
        //                {
        //                    if ((model.Starttime >= item.Starttime && model.Starttime <= item.Endtime) || (model.Endtime >= item.Starttime && model.Endtime <= item.Endtime))
        //                    {
        //                        //TempData["error"] = "Shift is already assigned in this time";
        //                        return;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    Shift shift = new Shift
        //    {
        //        Physicianid = model.Physicianid,
        //        Startdate = DateOnly.FromDateTime(model.Shiftdate),
        //        Repeatupto = model.repeatcount,
        //        Createddate = DateTime.Now,
        //        Createdby = adminId,
        //    };
        //    foreach (var obj in chk)
        //    {
        //        shift.Weekdays += obj;
        //    }
        //    if (model.repeatcount > 0)
        //    {
        //        shift.Isrepeat = new BitArray(new[] { true });
        //    }
        //    else
        //    {
        //        shift.Isrepeat = new BitArray(new[] { false });
        //    }
        //    _db.Shifts.Add(shift);
        //    _db.SaveChanges();

        //    DateTime curdate = model.Shiftdate;
        //   Shiftdetail shiftdetail = new Shiftdetail();
        //    shiftdetail.Shiftid = shift.Shiftid;
        //    shiftdetail.Shiftdate = DateOnly.FromDateTime( model.Shiftdate);
        //    shiftdetail.Regionid = model.Regionid;
        //    shiftdetail.Starttime = model.Starttime;
        //    shiftdetail.Endtime = model.Endtime;
        //    shiftdetail.Isdeleted = new BitArray(new[] { false });
        //    _db.Shiftdetails.Add(shiftdetail);
        //    _db.SaveChanges();
        //    Shiftdetailregion shiftregionnews = new Shiftdetailregion
        //    {
        //        Shiftdetailid = shiftdetail.Shiftdetailid,
        //        Regionid = model.Regionid,
        //        Isdeleted = new BitArray(new[] { false })
        //    };
        //    _db.Shiftdetailregions.Add(shiftregionnews);
        //    _db.SaveChanges();
        //    var dayofweek = model.Shiftdate.DayOfWeek.ToString();
        //    int valueforweek;
        //    if (dayofweek == "Sunday")
        //    {
        //        valueforweek = 0;
        //    }
        //    else if (dayofweek == "Monday")
        //    {
        //        valueforweek = 1;
        //    }
        //    else if (dayofweek == "Tuesday")
        //    {
        //        valueforweek = 2;
        //    }
        //    else if (dayofweek == "Wednesday")
        //    {
        //        valueforweek = 3;
        //    }
        //    else if (dayofweek == "Thursday")
        //    {
        //        valueforweek = 4;
        //    }
        //    else if (dayofweek == "Friday")
        //    {
        //        valueforweek = 5;
        //    }
        //    else
        //    {
        //        valueforweek = 6;
        //    }

        //    if (shift.Isrepeat[0] == true)
        //    {
        //        for (int j = 0; j < shift.Weekdays.Count(); j++)
        //        {
        //            var z = shift.Weekdays;
        //            var p = shift.Weekdays.ElementAt(j).ToString();
        //            int ele = Int32.Parse(p);
        //            int x;
        //            if (valueforweek > ele)
        //            {
        //                x = 6 - valueforweek + 1 + ele;
        //            }
        //            else
        //            {
        //                x = ele - valueforweek;
        //            }
        //            if (x == 0)
        //            {
        //                x = 7;
        //            }
        //            DateTime newcurdate = model.Shiftdate.AddDays(x);
        //            for (int i = 0; i < model.repeatcount; i++)
        //            {
        //                Shiftdetail shiftdetailnew = new Shiftdetail
        //                {
        //                    Shiftid = shift.Shiftid,
        //                    Shiftdate = DateOnly.FromDateTime( newcurdate),
        //                    Regionid = model.Regionid,
        //                    Starttime = model.Starttime,
        //                    Endtime = model.Endtime,

        //                    Isdeleted = new BitArray(new[] { false })
        //                };
        //                _db.Shiftdetails.Add(shiftdetailnew);
        //                _db.SaveChanges();
        //                Shiftdetailregion shiftregionnew = new Shiftdetailregion
        //                {
        //                    Shiftdetailid = shiftdetailnew.Shiftdetailid,
        //                    Regionid = model.Regionid,
        //                    Isdeleted = new BitArray(new[] { false })
        //                };
        //                _db.Shiftdetailregions.Add(shiftregionnew);
        //                _db.SaveChanges();
        //                newcurdate = newcurdate.AddDays(7);
        //            }
        //        }

        //    }
        //}
        /// Scheduling /
        public List<PhysicianProfile> GetProvider(int Regionid)
        {
            var list = _db.Physicians
                .Select(p => new PhysicianProfile
                {
                    physicianid = p.Physicianid,
                    UserName = "MD." + p.Lastname + "." + p.Firstname,
                    firstName = p.Firstname,
                    lastName = p.Lastname,
                    roleid = p.Roleid,
                    role = _db.Roles.FirstOrDefault(r => r.Roleid == p.Roleid).Name,
                    Regionid = p.Regionid,
                    phone = p.Mobile,
                    email = p.Email,
                    status = p.Status,
                    IsPhoto = p.Photo != null ? new BitArray(new bool[1] { true }) : new BitArray(new bool[1] { false }),
                    PhotoName = p.Photo,
                })
                .OrderBy(p => p.firstName)
                .ToList();

            if (Regionid != 0)
            {
                list = list.Where(r => r.Regionid == Regionid).ToList();
            }

            return list;
        }
        //public List<PhysicianProfile> GetProvider(int Regionid)
        //{
        //    var list = _db.Physicians
        //        .Select(p => new PhysicianProfile
        //        {
        //            physicianid = p.Physicianid,
        //            UserName = "MD." + p.Lastname + "." + p.Firstname,
        //            firstName = p.Firstname,
        //            lastName = p.Lastname,
        //            roleid = p.Roleid,
        //            role = _db.Roles.FirstOrDefault(r => r.Roleid == p.Roleid).Name,
        //            Regionid = p.Regionid,
        //            phone = p.Mobile,
        //            email = p.Email,
        //            status = p.Status,
        //            IsPhoto = p.Photo != null ? new BitArray(new bool[1] { true }) : new BitArray(new bool[1] { false }),
        //            PhotoName = p.Photo,
        //        })
        //        .OrderBy(p => p.firstName)
        //        .ToList();

        //    if (Regionid != 0)
        //    {
        //        list = list.Where(r => r.Regionid == Regionid).ToList();
        //    }

        //    return list;
        //}
        
        public List<EventModel> GetEvents(int RegionId, bool currentMonthShift)
        {
            
            var data = _db.Shiftdetails
                .Where(s => s.Isdeleted == new BitArray(new bool[1] { false }))
                .Include(s => s.Shift)
                .Select(
                    s => new EventModel
                    {
                        Shiftdetailid = s.Shiftdetailid,
                        Shiftdate = s.Shiftdate,
                        Shiftid = s.Shiftid,
                        Physicianid = s.Shift.Physicianid,
                        PhysicianName = _db.Physicians.Where(p => p.Physicianid == s.Shift.Physicianid).FirstOrDefault().Firstname,
                        Starttime = (s.Shiftdate.AddHours(s.Starttime.Hour).AddMinutes(s.Starttime.Minute).AddSeconds(s.Starttime.Second)).ToString("yyyy-MM-ddTHH:mm:ss"),
                        Endtime = (s.Shiftdate.AddHours(s.Endtime.Hour).AddMinutes(s.Endtime.Minute).AddSeconds(s.Endtime.Second)).ToString("yyyy-MM-ddTHH:mm:ss"),
                        Isdeleted = s.Isdeleted,
                        Status = s.Status,
                        Regionid = s.Regionid,
                        Regionname = _db.Regions.Where(r => r.Regionid == s.Regionid).FirstOrDefault().Name,
                        color = s.Status == 1 ? "#F4CAED" : "#a9e1c8"

                    })
                .ToList();

            if (RegionId != 0)
            {
                data = data.Where(r => r.Regionid == RegionId).ToList();
            }
            if (currentMonthShift)
            {
                var month = DateTime.Now.Month;
                data = data.Where(r => r.Shiftdate.Month == month).ToList();
            }
            return data;
        }

        public bool CreateShift(scheduleModel scheduleModel, string adminId)
        {
            var data = _db.Shiftdetails.Include(s => s.Shift).ToList();
            //////
            foreach (var i in data)
            {
                if (i.Shift.Physicianid == scheduleModel.Physicianid && i.Shiftdate == new DateTime(scheduleModel.ShiftDate.Year, scheduleModel.ShiftDate.Month, scheduleModel.ShiftDate.Day))
                {
                    if (scheduleModel.StartTime >= i.Starttime && scheduleModel.StartTime <= i.Endtime || scheduleModel.EndTime >= i.Starttime && scheduleModel.EndTime <= i.Endtime)
                    {
                        return false;
                    }
                }
            }

            var shift = new Shift
            {
                Physicianid = scheduleModel.Physicianid,
                Startdate = DateOnly.FromDateTime(scheduleModel.ShiftDate),
                Isrepeat = new BitArray(new bool[1] { scheduleModel.isRepeat }),
                Weekdays = scheduleModel.SelectedDayIds,
                Repeatupto = scheduleModel.RepeatEnd,
                Createdby = adminId,
                Createddate = DateTime.Now
            };
            _db.Shifts.Add(shift);
            _db.SaveChanges();

            var shiftdetail = new Shiftdetail
            {
                Shiftid = shift.Shiftid,
                Shiftdate = new DateTime(scheduleModel.ShiftDate.Year, scheduleModel.ShiftDate.Month, scheduleModel.ShiftDate.Day),
                Regionid = scheduleModel.Regionid,
                Starttime = scheduleModel.StartTime,
                Endtime = scheduleModel.EndTime,
                Status = 1,
                Isdeleted = new BitArray(new bool[1] { false }),
            };

            _db.Shiftdetails.Add(shiftdetail);
            _db.SaveChanges();

            var shiftdetailregion = new Shiftdetailregion
            {
                Shiftdetailid = shiftdetail.Shiftdetailid,
                Regionid = scheduleModel.Regionid,
                Isdeleted = new BitArray(new bool[1] { false }),
            };

            _db.Shiftdetailregions.Add(shiftdetailregion);
            _db.SaveChanges();

            if (shift.Weekdays != null)
            {
                var list = scheduleModel.SelectedDayIds.Split(",").Select(int.Parse).ToList();
                //////////////
                var currentDate = scheduleModel.ShiftDate.AddDays(1);
                int occurrences = 0;
                int totalShift = scheduleModel.RepeatEnd * list.Count;

                while (occurrences < totalShift)
                {
                    if (list.Contains((int)currentDate.DayOfWeek))
                    {
                        bool canCreate = true;
                        foreach (var i in data)
                        {
                            if (i.Shift.Physicianid == scheduleModel.Physicianid && i.Shiftdate == new DateTime(scheduleModel.ShiftDate.Year, scheduleModel.ShiftDate.Month, scheduleModel.ShiftDate.Day))
                            {
                                if (scheduleModel.StartTime >= i.Starttime && scheduleModel.StartTime <= i.Endtime || scheduleModel.EndTime >= i.Starttime && scheduleModel.EndTime <= i.Endtime)
                                {
                                    canCreate = false;
                                }
                            }
                        }

                        if (canCreate)
                        {
                            shiftdetail = new Shiftdetail
                            {
                                Shiftid = shift.Shiftid,
                                Shiftdate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day),
                                Regionid = scheduleModel.Regionid,
                                Starttime = scheduleModel.StartTime,
                                Endtime = scheduleModel.EndTime,
                                Status = 1,
                                Isdeleted = new BitArray(new bool[1] { false }),
                            };

                            _db.Shiftdetails.Add(shiftdetail);
                            _db.SaveChanges();

                            shiftdetailregion = new Shiftdetailregion
                            {
                                Shiftdetailid = shiftdetail.Shiftdetailid,
                                Regionid = scheduleModel.Regionid,
                                Isdeleted = new BitArray(new bool[1] { false }),
                            };

                            _db.Shiftdetailregions.Add(shiftdetailregion);
                            _db.SaveChanges();

                            occurrences++;
                        }
                    }
                    currentDate = currentDate.AddDays(1);
                }

            }
            return true;
        }
        public bool EditShift(int shiftDetailId, DateTime Shiftdate, TimeOnly startTime, TimeOnly endTime, string adminId)
        {
            var data = _db.Shiftdetails.Include(s => s.Shift).ToList();

            var physicianId = _db.Shiftdetails.FirstOrDefault(s => s.Shiftdetailid == shiftDetailId).Shift.Physicianid;
            ///'/
            foreach (var i in data)
            {
                if (i.Shiftdetailid != shiftDetailId)
                {
                    if (i.Shift.Physicianid == physicianId && i.Shiftdate == new DateTime(Shiftdate.Year, Shiftdate.Month, Shiftdate.Day))
                    {
                        if (startTime >= i.Starttime && startTime <= i.Endtime || endTime >= i.Starttime && endTime <= i.Endtime)
                        {
                            return false;
                        }
                    }
                }
            }

            var shiftdetail = _db.Shiftdetails.FirstOrDefault(s => s.Shiftdetailid == shiftDetailId);

            shiftdetail.Shiftdate = Shiftdate;
            shiftdetail.Starttime = startTime;
            shiftdetail.Endtime = endTime;
            shiftdetail.Modifiedby = adminId;
            shiftdetail.Modifieddate = DateTime.Now;

            _db.SaveChanges();

            return true;
        }

        public bool DeleteShift(int shiftDetailId, string adminId)
        {
            var shiftdetail = _db.Shiftdetails.FirstOrDefault(s => s.Shiftdetailid == shiftDetailId);

            shiftdetail.Isdeleted = new BitArray(new bool[1] { true });
            shiftdetail.Modifiedby = adminId;
            shiftdetail.Modifieddate = DateTime.Now;
            _db.SaveChanges();

            return true;
        }

        public bool ReturnShift(int shiftDetailId, string adminId)
        {
            var shiftdetail = _db.Shiftdetails.FirstOrDefault(s => s.Shiftdetailid == shiftDetailId);

            if (shiftdetail.Status == 1)
            {
                shiftdetail.Status = 2;
            }
            else
            {
                shiftdetail.Status = 1;
            }
            shiftdetail.Modifiedby = adminId;
            shiftdetail.Modifieddate = DateTime.Now;
            _db.SaveChanges();

            return true;
        }
        //Requested shift
        public void RequestedShiftUpdate(string ids, int type, string adminId)
        {
            List<string> selectedId = ids.Split(",").ToList();

            foreach (var i in selectedId)
            {
                var id = int.Parse(i);
                var data = _db.Shiftdetails.FirstOrDefault(s => s.Shiftdetailid == id);

                if (type == 1)
                {
                    if (data.Status == 1)
                    {
                        data.Status = 2;
                    }
                    else
                    {
                        data.Status = 1;
                    }
                }
                else
                {
                    data.Isdeleted = new BitArray(new bool[1] { true });
                }
                //data.Modifiedby = adminId;
                data.Modifiedby = "8695ab30-4e1f-43cb-a944-fcf2670344d7";
                data.Modifieddate = DateTime.Now;
                _db.SaveChanges();
            }
        }
        /************************************records***********************/
        //Records
        public AdminDashboard GetSearchRecordInfo()
        {
            Records records = new Records();
            records.requeststatuses = _db.Requeststatuses.ToList();
            records.requesttypes = _db.Requesttypes.ToList();

            var result = new AdminDashboard()
            {
                Records = records,
            };
            return result;
        }
        public AdminDashboard GetRecordTableInfo(AdminDashboard model, int currentpage)
        {

            var list = (from req in _db.Requests
                        join reqClient in _db.Requestclients on req.Requestid equals reqClient.Requestid
                        join reqType in _db.Requesttypes on req.Requesttypeid equals reqType.Requesttypeid
                        join phy in _db.Physicians on req.Physicianid equals phy.Physicianid
                        join reqNotes in _db.Requestnotes on req.Requestid equals reqNotes.Requestid
                        join reqStatus in _db.Requeststatuses on req.Status equals reqStatus.Statusid
                        where model.searchstream == null || (model.Status == 0 || req.Status == model.searchstream.Status)
                        && (model.searchstream.Name == null || model.searchstream.Name.Contains(reqClient.Firstname))
                        && (model.searchstream.reqType == 0 || model.searchstream.reqType == req.Requesttypeid)
                        && (model.searchstream.providername == null || model.searchstream.providername.Contains(phy.Firstname))
                        && (model.searchstream.email.Contains(reqClient.Email) || model.searchstream.email == null)
                        && (req.Lastreservationdate.Equals(model.searchstream.dateofservice) || model.searchstream.lastdateofservice == default)
                        && (model.searchstream.dateofservice == default || req.Lastreservationdate.Equals(model.searchstream.dateofservice))
                        select new Records
                        {
                            RequestId = req.Requestid,
                            PatientFirstName = reqClient.Firstname,
                            PatientLastName = reqClient.Lastname,
                            Requestor = reqType.Name,
                            DateOfService = DateTime.Now,
                            CloseCaseDate = DateTime.Now,
                            Email = req.Email,
                            PhoneNumber = reqClient.Phonenumber ?? "",
                            Address = reqClient.Address ?? "",
                            Zip = reqClient.Zipcode ?? "",
                            AdminNote = reqNotes.Adminnotes ?? "",
                            RequestStatus = reqStatus.Statusname,
                            Physician = phy.Firstname + " " + phy.Lastname,
                            PhysicianNote = reqNotes.Physiciannotes,
                            CancelledByPhysicianNote = "abc",
                            PatientNote = "abc"
                        }).ToList();
            int totalrecords = list.Count();
            int pagesize = 2;
            int totalPages = (int)Math.Ceiling((double)totalrecords / pagesize);
            var paginateddashboard = list.Skip((model.CurrentPage - 1) * pagesize).Take(pagesize).ToList();


            var result = new AdminDashboard()
            {
                RecordsList = paginateddashboard,
                CurrentPage = model.CurrentPage,
                TotalPages = totalPages,
                PageSize = pagesize,
                ToatCount = totalrecords
            };
            return result;
        }
        public void DeleteRequestFromSearchRecordsMethod(int requestid)
        {
            var data3 = _db.Requestclients.Where(x => x.Requestid == requestid).FirstOrDefault();
            if (data3 != null)
            {
                _db.Remove(data3);
            }
            var data4 = _db.Requestbusinesses.Where(x => x.Requestid == requestid).FirstOrDefault();
            if (data4 != null)
            {
                _db.Remove(data4);
            }
            var data2 = _db.Requestwisefiles.Where(x => x.Requestid == requestid).ToList();
            if (data2 != null)
            {
                _db.RemoveRange(data2);
            }
            var data1 = _db.Requeststatuslogs.Where(x => x.Requestid == requestid).ToList();
            if (data1 != null)
            {
                _db.RemoveRange(data1);
            }
            var data5 = _db.Requestconcierges.Where(x => x.Requestid == requestid).FirstOrDefault();
            if (data5 != null)
            {
                _db.Remove(data5);
            }
            var data6 = _db.Requestnotes.Where(x => x.Requestid == requestid).FirstOrDefault();
            if (data6 != null)
            {
                _db.Remove(data6);
            }
            var data7 = _db.Blockrequests.Where(x => x.Requestid == requestid).FirstOrDefault();
            if (data7 != null)
            {
                _db.Remove(data7);
            }
            //var data8 = _db.EncounterForms.Where(x => x.RequestId == requestid).FirstOrDefault();
            //if (data8 != null)
            //{
            //    _db.Remove(data8);

            //}
            var data = _db.Requests.Where(x => x.Requestid == requestid).FirstOrDefault();
            if (data != null)
            {
                _db.Remove(data);
            }
            var data9 = _db.Orderdetails.Where(x => x.Requestid == requestid).ToList();
            if (data9 != null)
            {
                _db.RemoveRange(data9);
            }
            _db.SaveChanges();

        }
        //BlockReqHistory
        public AdminDashboard GetBlockHistoryData(AdminDashboard model)
        {
            var data1 = (from req in _db.Requests
                         join reqclient in _db.Requestclients on req.Requestid equals reqclient.Requestid
                         join blockreq in _db.Blockrequests on req.Requestid equals blockreq.Requestid
                         where ((model.searchstream == null) || ((model.searchstream.Name == null
                           || reqclient.Firstname.ToLower().Contains(model.searchstream.Name.ToLower())
                           || reqclient.Lastname.ToLower().Contains(model.searchstream.Name.ToLower()))
                           && (model.searchstream.createdDate == default || blockreq.Createddate == model.searchstream.createdDate)
                                       && (string.IsNullOrEmpty(model.searchstream.number) || blockreq.Phonenumber.Contains(model.searchstream.number))
                                 && (string.IsNullOrEmpty(model.searchstream.email) || blockreq.Email.Equals(model.searchstream.email))))
                         select new BlockedHistory
                         {
                             PatientName = reqclient.Firstname + reqclient.Lastname,
                             PhoneNumber = reqclient.Phonenumber,
                             Email = reqclient.Email,
                             CreatedDate = blockreq.Createddate,
                             Notes = blockreq.Reason,
                             IsActive = blockreq.Isactive,
                             BlockedRequestID = req.Requestid,
                         }).ToList();

            int totalrecords = data1.Count();
            int pagesize = 2;
            int totalPages = (int)Math.Ceiling((double)totalrecords / pagesize);
            var paginateddashboard = data1.Skip((model.CurrentPage - 1) * pagesize).Take(pagesize).ToList();

            var result = new AdminDashboard
            {
                BlockedHistory = paginateddashboard,
                CurrentPage = model.CurrentPage,
                TotalPages = totalPages,
                PageSize = pagesize,
                ToatCount = totalrecords
            };
            return result;
        }
        public bool unblockreq(int blockreqid)
        {


            Blockrequest r = _db.Blockrequests.Where(x => x.Requestid == blockreqid).FirstOrDefault();
            r.Isactive[0] = true;
            r.Modifieddate = DateTime.Now;
            _db.Blockrequests.Update(r);
            _db.SaveChanges();

            Request req = _db.Requests.Where(x => x.Requestid == blockreqid).FirstOrDefault();
            req.Status = 1;
            req.Modifieddate = DateTime.Now;
            _db.Requests.Update(req);
            _db.SaveChanges();

            return true;


        }
        //Emaillog
        public AdminDashboard GetEmailLogInfo()
        {
            EmailLogList emaillog = new EmailLogList();
            emaillog.Roles = _db.Aspnetroles.ToList();

            var result = new AdminDashboard
            {
                EmailLog = emaillog,
            };
            return result;
        }
        public AdminDashboard GetEmailLogTableInfo(AdminDashboard model)
        {
            var data = (from req in _db.Requests
                        join reqclient in _db.Requestclients on req.Requestid equals reqclient.Requestid
                        join emailLog in _db.Emaillogs on req.Requestid equals emailLog.Requestid
                        where (model.EmailLog == null ||
                        (model.EmailLog.ReceiverName == null || reqclient.Firstname.ToLower().Contains(model.EmailLog.ReceiverName.ToLower()) || reqclient.Lastname.ToLower().Contains(model.EmailLog.ReceiverName.ToLower()))
                        && (model.EmailLog.Email == null || emailLog.Emailid.Contains(model.EmailLog.Email))
                        && (model.EmailLog.CreatedDate == default || emailLog.Createdate.Date == model.EmailLog.CreatedDate.Value.Date)
                        && (model.EmailLog.SentDate == default || emailLog.Sentdate.Value.Date == model.EmailLog.CreatedDate.Value.Date)
                        && (model.EmailLog.RoleId == 0 || emailLog.Roleid == model.EmailLog.RoleId))
                        select new EmailLogModel
                        {
                            Receipient = reqclient.Firstname + reqclient.Lastname,
                            Action = "Request Monthly Data",
                            Email = emailLog.Emailid,
                            CreatedDate = emailLog.Createdate,
                            SentDate = emailLog.Sentdate,
                            Sent = emailLog.Isemailsent == new BitArray(1, true) ? "Yes" : "No",
                            SentTries = emailLog.Senttries,
                            ConfirmationNumber = emailLog.Confirmationnumber,
                            RoleName = (Roles)emailLog.Roleid,
                            //sentries
                        }).ToList();
            int totalrecords = data.Count();
            int pagesize = 2;
            int totalPages = (int)Math.Ceiling((double)totalrecords / pagesize);
            var paginateddashboard = data.Skip((model.CurrentPage - 1) * pagesize).Take(pagesize).ToList();


            var result = new AdminDashboard()
            {
                emailLogModel = paginateddashboard,
                CurrentPage = model.CurrentPage,
                TotalPages = totalPages,
                PageSize = pagesize,
                ToatCount = totalrecords

            };
            return result;
        }
        //SMSlog
        public AdminDashboard GetSMSLogInfo()
        {
            SMSLogList SMSlog = new SMSLogList();
            SMSlog.Roles = _db.Aspnetroles.ToList();

            var result = new AdminDashboard
            {
                SMSLog = SMSlog,
            };
            return result;
        }
        public AdminDashboard GetSMSLogTableInfo(SMSLogList model)
        {
            var data = (from req in _db.Requests
                        join reqclient in _db.Requestclients on req.Requestid equals reqclient.Requestid
                        join smsLog in _db.Smslogs on req.Requestid equals smsLog.Requestid
                        where (model == null ||
                        (model.ReceiverName == null || reqclient.Firstname.Contains(model.ReceiverName))
                        && (model.PhoneNumber == null || smsLog.Mobilenumber == model.PhoneNumber)
                        && (model.CreatedDate == default || smsLog.Createdate.Date == model.CreatedDate.Value.Date)
                        && (model.SentDate == default || smsLog.Sentdate.Value.Date == model.CreatedDate.Value.Date)
                        && (model.RoleId == 0 || smsLog.Roleid == model.RoleId))
                        select new SMSLogModel
                        {
                            Receipient = reqclient.Firstname + reqclient.Lastname,
                            Action = "Request Monthly Data",
                            number = smsLog.Mobilenumber,
                            CreatedDate = smsLog.Createdate,
                            SentDate = smsLog.Sentdate,
                            Sent = smsLog.Issmssent == new BitArray(1, true) ? "Yes" : "No",
                            SentTries = smsLog.Senttries,
                            ConfirmationNumber = smsLog.Confirmationnumber,
                            //RoleName = (Roles)smsLog.Roleid
                        }).ToList();


            var result = new AdminDashboard()
            {
                SMSLogModel = data,
            };
            return result;
        }
        //PatientHistory
        public AdminDashboard PatientHistory(AdminDashboard obj)
        {
            var list = (from user in _db.Users
                        join req in _db.Requests on user.Userid equals req.Userid
                        where (obj.searchstream == null || (obj.searchstream.FirstName == null || user.Firstname.ToLower().Contains(obj.searchstream.FirstName.ToLower()))
                       && (obj.searchstream.LastName == null || user.Lastname.ToLower().Contains(obj.searchstream.LastName.ToLower()))
                       && (obj.searchstream.number == null || user.Mobile == obj.searchstream.number)
                       && (obj.searchstream.email == null || user.Email.ToLower().Contains(obj.searchstream.email.ToLower())))
                        select new PatientHistoryModel()
                        {
                            FirstName = user.Firstname,
                            LastName = user.Lastname,
                            Address = user.City + ", " + user.Street + ", " + user.State,
                            Email = user.Email,
                            Phone = user.Mobile,
                            UserId = user.Userid,
                        }).Distinct().ToList();
            int totalrecords = list.Count();
            int pagesize = 2;
            int totalPages = (int)Math.Ceiling((double)totalrecords / pagesize);
            var paginateddashboard = list.Skip((obj.CurrentPage - 1) * pagesize).Take(pagesize).ToList();

            var result = new AdminDashboard()
            {
                PatientHistory = list,
                CurrentPage = obj.CurrentPage,
                TotalPages = totalPages,
                PageSize = pagesize,
                ToatCount = totalrecords
            };
            return result;
        }
        public AdminDashboard PatientRecords(int id, int currentpage)
        {
            var list = (from user in _db.Users
                        join req in _db.Requests on user.Userid equals req.Userid
                        join reqClient in _db.Requestclients on req.Requestid equals reqClient.Requestid
                        where req.Userid == id
                        select new PatientRecordModel()
                        {
                            Name = user.Firstname + " " + user.Lastname,
                            CreatedDate = req.Createddate,
                            ConfirmationNumber = req.Confirmationnumber,
                            ProviderName = _db.Physicians.FirstOrDefault(x => x.Physicianid == req.Physicianid).Firstname ?? "-",
                            //ConcludedDate=
                            Status = _db.Requeststatuses.FirstOrDefault(x => x.Statusid == req.Status).Statusname,
                            RequestClientId = reqClient.Requestclientid,
                            RequestId = req.Requestid,
                            DocumentCount = _db.Requestwisefiles.Where(x => x.Requestid == reqClient.Requestid).ToList().Count,
                        });
            var data = list.ToList();
            int totalrecords = data.Count();
            int pagesize = 2;
            int totalPages = (int)Math.Ceiling((double)totalrecords / pagesize);
            var paginateddashboard = list.Skip((currentpage - 1) * pagesize).Take(pagesize).ToList();


            var result = new AdminDashboard()
            {
                PatientsRecord = paginateddashboard,
                CurrentPage = currentpage,
                TotalPages = totalPages,
                PageSize = pagesize,
                ToatCount = totalrecords
            };
            return result;
        }
        public AdminDashboard GetRegion(int reqid)
        {
            var regions = _db.Regions.ToList();
            AdminDashboard adminDashboardModel = new AdminDashboard();
            adminDashboardModel.Regions = regions;
            adminDashboardModel.requestid = reqid;
            return adminDashboardModel;
        }
        /******sms*************/
        //    public void SendSMS(AdminDashboard model)
        //    {
        //        string link = $"https://localhost:7265/Patient/SubmitRequest";
        //        string SMSTemplate = $"For Submit your Request: {link}";

        //        var accountsid = "AC3f07238a0c7428a2c1861ee8d0da5275";
        //        var authtoken = "5f682d44e9cb371de282ab1336c5a676";
        //        TwilioClient.Init(accountsid, authtoken);

        //        /*var messageoptions = new CreateMessageOptions(
        //          new PhoneNumber("+919510155988"));*/
        //        var messageoptions = new CreateMessageOptions(
        //          new PhoneNumber(model.SendLinkModel.PhoneNumber));
        //        messageoptions.From = new PhoneNumber("+17209618754");
        //        messageoptions.Body = SMSTemplate;
        //        MessageResource.Create(messageoptions);

        //        var AdminId = _db.Admins.Where(x => x.Aspnetuserid == model.aspnetuserid).FirstOrDefault().Adminid;

        //        SMSLogEntry(SMSTemplate, model.SendLinkModel.PhoneNumber, AdminId, default);
        //    }
        public void SMSLogEntry(string SMSTemplate, string MobileNo, int AdminId, int PhysicianId)
        {
            Smslog smslog = new Smslog();
            if (PhysicianId == 0)
            {
                smslog.Smstemplate = SMSTemplate;
                smslog.Mobilenumber = MobileNo;
                smslog.Roleid = (int)Roles.Admin;
                smslog.Adminid = AdminId;
                smslog.Createdate = DateTime.Now;
                smslog.Sentdate = DateTime.Now;
                smslog.Senttries = 1;
                smslog.Issmssent = new BitArray(new bool[1] { true });
            }
            else
            {
                smslog.Smstemplate = SMSTemplate;
                smslog.Mobilenumber = MobileNo;
                smslog.Roleid = (int)Roles.Admin;
                smslog.Adminid = AdminId;
                smslog.Createdate = DateTime.Now;
                smslog.Sentdate = DateTime.Now;
                smslog.Senttries = 1;
                smslog.Issmssent = new BitArray(new bool[1] { true });
                smslog.Physicianid = PhysicianId;
            }

            _db.Add(smslog);
            _db.SaveChanges();
        }

       public AdminDashboard GetContactProvider(int physicianid)
        {
            AdminDashboard model =new AdminDashboard();
            model.physicianid = physicianid;
            return model;
        }

        public void ContactProvider(AdminDashboard model)
        {
            var data = _db.Physicians.FirstOrDefault(r => r.Physicianid == model.physicianid);

            if (model.PhysicianProfile.radioSMSEmail == 1 || model.PhysicianProfile.radioSMSEmail == 3)
            {
                string SMSTemplate = model.PhysicianProfile.NotificationMassage;
                var accountsid = "AC3f07238a0c7428a2c1861ee8d0da5275";
                var authtoken = "5f682d44e9cb371de282ab1336c5a676";
                TwilioClient.Init(accountsid, authtoken);

                /*  var messageoptions = new CreateMessageOptions(
                    new PhoneNumber(data.Mobile));*/
                var messageoptions = new CreateMessageOptions(
                  new PhoneNumber("+919510155988"));
                messageoptions.From = new PhoneNumber("+17209618754");
                messageoptions.Body = SMSTemplate;
                MessageResource.Create(messageoptions);

                var AdminId = _db.Admins.Where(x => x.Aspnetuserid == "8695ab30-4e1f-43cb-a944-fcf2670344d7").FirstOrDefault().Adminid;

                SMSLogEntry(SMSTemplate, data.Mobile, AdminId, data.Physicianid);
            }
        }
        //    if (model.PhysicianProfile.radioSMSEmail == 2 || model.PhysicianProfile.radioSMSEmail == 3)
        //    {
        //        var SubjectName = "Contact Provider";
        //        var EmailTemplate = model.PhysicianProfile.NotificationMassage;

        //        MailMessage message = new MailMessage();

        //        message.From = new mailto:mailaddress("vidhi.makani@etatvasoft.com");
        //        message.To.Add(new MailAddress(data.Email));
        //        message.Subject = SubjectName;
        //        message.IsBodyHtml = true;
        //        message.Body = EmailTemplate;
        //        SmtpClient smtp = new SmtpClient();
        //        smtp.Host = "mail.etatvasoft.com";
        //        smtp.Port = 587;
        //        smtp.Credentials = new mailto:networkcredential("vidhi.makani@etatvasoft.com", "jKAO+h]uA+vk");
        //        smtp.EnableSsl = true;
        //        smtp.Send(message);
        //        smtp.UseDefaultCredentials = false;

        //        emailLogEntry(EmailTemplate, SubjectName, data.Email, 0);


        //    }
        //}

    }
}
