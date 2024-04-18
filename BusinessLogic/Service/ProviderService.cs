using BusinessLogic.Interface;
using DataAccess.Data;
using DataAccess.Models;
using DataAccess.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAccess.ViewModel.Constant;

namespace BusinessLogic.Service
{
    public class ProviderDataService:IProviderInterface
    {
        private readonly ApplicationDbContext _db;
        public ProviderDataService(ApplicationDbContext db)
        {
            _db = db;
        }
        public ProviderDash reqByStatus(int statusId, int reqTypeId, int currentpage, string searchstream, string region, int physicianId)
        {
            currentpage = 1;
           
            List<int> reqStatusId = new List<int>();
            Physician phy = new Physician();

            if (statusId != (int)Status.ToClose && statusId != (int)Status.Active)
            {
                reqStatusId.Add(statusId);
            }
            else if (statusId == (int)Status.Active)
            {

                reqStatusId.Add((int)Requeststatuses.MDEnRoute);
                reqStatusId.Add((int)Requeststatuses.MDonSite);
            }


            var query = from req in _db.Requests
                        join rc in _db.Requestclients
                        on req.Requestid equals rc.Requestid
                        where reqStatusId.Contains(req.Status) && req.Physicianid == physicianId &&
                        (reqTypeId == 0 || req.Requesttypeid == reqTypeId) &&
                        (region == null || rc.City == region) &&
                        (searchstream == null || rc.Firstname.ToLower().Contains(searchstream.ToLower()) || rc.Lastname.ToLower().Contains(searchstream.ToLower()))
                        select new ProviderDashModel
                        {
                            firstName = rc.Firstname,
                            lastName = rc.Lastname,
                            mobileNo = rc.Phonenumber,
                            city = rc.City,
                            state = rc.State,
                            street = rc.Street,
                            zipCode = rc.Zipcode,
                            requestTypeId = req.Requesttypeid,
                            status = req.Status,
                            //RoomNo = rc.RoomNo,
                            Region = rc.State,
                            notes = rc.Notes,
                            DateOfService = req.Lastreservationdate,
                            RequestId = req.Requestid,
                            StatusId = req.Status,
                            Address = rc.Address,
                            isfinalize = _db.Encounters.FirstOrDefault(x => x.RequestId == req.Requestid).IsFinalize,
                        };
           
            int totalrecords = query.Count();//10
            int pagesize = 2;
            int totalPages = (int)Math.Ceiling((double)totalrecords / pagesize);
            var dashboard = query.Skip((currentpage - 1) * pagesize).Take(pagesize).ToList();
            var result = new ProviderDash
            {

                //reqCount = item1.Count,
                regions = _db.Regions.ToList(),
                requestByStatus = dashboard.ToList(),
                statusId = statusId,
                Status = (Status?)statusId,
                CurrentPage = currentpage,
                TotalPages = totalPages,
                PageSize = pagesize,

            };
            return result;

        }
        //public ProviderDash GetRequestStatusCountByPhysician(int physicianId)
        //{
        //    var Request = _db.Requests.Include(x => x.Requestclients).ToList();
        //    ReqCount reqCount = new ReqCount();

        //    reqCount.newReqCount = Request.Count(x => x.Status == (int)Status.NEW && x.Requestid == x.Requestclients.FirstOrDefault().Requestid && x.Physicianid == physicianId);
        //    reqCount.pendingReqCount = Request.Count(x => x.Status == (int)Status.PENDING && x.Physicianid == physicianId && x.Requestid == x.Requestclients.FirstOrDefault().Requestid);
        //    reqCount.activeReqCount = Request.Count(x => x.Status == (int)Status.ACTIVE_MDONSite || x.Status == (int)Status.ACTIVE_MDEnRoute && x.Requestid == x.Requestclients.FirstOrDefault().Requestid && x.Physicianid == physicianId);
        //    reqCount.concludeReqCount = Request.Count(x => x.Status == (int)Status.CONCLUDE && x.Requestid == x.Requestclients.FirstOrDefault().Requestid && x.Physicianid == physicianId);

        //    var result = new ProviderDash
        //    {
        //        reqCount = reqCount,
        //        regions = _db.Regions.ToList(),
        //    };
        //    return result;
        //}
        public ProviderDash GetRequestStatusCountByPhysician(int physicianId)
        {
           
            ProviderDash dash = new ProviderDash();
            var Request = _db.Requests.Include(x => x.Requestclients).Where(x => x.Requestid == x.Requestclients.FirstOrDefault().Requestid).ToList();
            dash.newcount = Request.Count(x => x.Status == (int)Requeststatuses.Unassigned && x.Requestid == x.Requestclients.FirstOrDefault().Requestid && x.Physicianid == physicianId);
            dash.pendingcount = Request.Count(x => x.Status == (int)Requeststatuses.Accepted && x.Physicianid == physicianId && x.Requestid == x.Requestclients.FirstOrDefault().Requestid);
            dash.activecount = Request.Count(x => x.Status == (int)Requeststatuses.MDonSite || x.Status == (int)Requeststatuses.MDEnRoute && x.Requestid == x.Requestclients.FirstOrDefault().Requestid && x.Physicianid == physicianId);
            dash.conclude = Request.Count(x => x.Status == (int)Requeststatuses.Conclude && x.Requestid == x.Requestclients.FirstOrDefault().Requestid && x.Physicianid == physicianId);


            return dash;


        }
        public ProviderDash GetAllRegions()
        {
            var result = new ProviderDash
            {
                regions = _db.Regions.ToList(),
            };
            return result;
        }
        public ProviderDash GetProviderInfo(string aspnetuserid)
        {
            //var phy = _db.Physicians.Include(x => x.Aspnetuser).Where(x => x.Physicianid == physicianId).Select(x=>new ProviderInfoModel

            //ProviderInfo obj = new()
            //{
            //    PhysicianId = phy.Physicianid,
            //    Password = phy.Aspnetuser.Passwordhash,
            //    UserName = phy.Aspnetuser.Username,
            //    FirstName = phy.Firstname,
            //    LastName = phy.Lastname,
            //    Email = phy.Email,
            //    PhoneNumber = phy.Mobile,
            //    MedicalLicenseNumber = phy.Medicallicense,
            //    NPINumber = phy.Npinumber,
            //    SyncEmail = phy.Syncemailaddress,
            //    Address1 = phy.Address1,
            //    Address2 = phy.Address2,
            //    City = phy.City,
            //    State = phy.City,
            //    Zip = phy.Zip,
            //    Phone = phy.Altphone, 
            //    BusinessName = phy.Businessname,
            //    BusinessWebsite = phy.Businesswebsite,
            //    Photo1 = phy.Photo,
            //    Signature1 = phy.Signature,
            //    AdminNote = phy.Adminnotes,
            //    RoleId = phy.Roleid,
            //    regionId = phy.Regionid,
            //};

            //var result = new ProviderDash
            //{
            //    regions = GetAllRegions().regions,
            //    ProviderInfo = obj,
            //};
            //return result;
          var id = aspnetuserid.Trim();

            var result = _db.Physicians.Include(x => x.Aspnetuser).Where(x => x.Aspnetuserid == id).Select(x => new ProviderInfoModel
            {
                UserName = x.Aspnetuser.Name,
                FirstName = x.Firstname,
                LastName = x.Lastname,
                Email = x.Email,
                PhoneNumber = x.Mobile,
                Address1 = x.Address1,
                Address2 = x.Address2,
                City = x.City,
                Zip = x.Zip,
                BusinessName = x.Businessname,
                BusinessWebsite = x.Businesswebsite,
               // State = (PhysicianStatus)x.Status,
                PhysicianId = x.Physicianid,
                aspId = x.Aspnetuserid



            }).FirstOrDefault();
            ProviderDash adminDashboard = new ProviderDash();
            adminDashboard.ProviderInfomodel = result;
            adminDashboard.regions = GetAllRegions().regions;
            //adminDashboard.physicianid = result.physicianid;
            return adminDashboard;
        }
        /***********encounter********/
          public bool SetTypeOfCare(int requestid, int TOCId)
        {
            var req = _db.Requests.Where(x => x.Requestid == requestid).FirstOrDefault();
            if (req != null)
            {
                if (TOCId == 1)
                {
                    req.Status = (int)(Requeststatuses.MDonSite);
                    req.Modifieddate = DateTime.Now;
                }
                else
                {
                    req.Status = (int)(Requeststatuses.Conclude);
                    req.Modifieddate = DateTime.Now;
                }
                _db.SaveChanges();
                return true;
            }
            else { return false; }
        }
        /****conclude care**/
        public AdminDashboard ConcludeCareGet(int requestid)
        {
            var items = _db.Requestwisefiles
                .Where(x => x.Requestid == requestid && x.Isdeleted == null)
                .Select(m => new ViewDoc
                {
                    uploaddate = m.Createddate,
                    uploader = m.Filename
                })
                .ToList();

            var req = _db.Requestclients.Include(x => x.Request).FirstOrDefault(x => x.Requestid == requestid);
            Profile patientInfo = new Profile();
            patientInfo.FirstName = req.Firstname;
            patientInfo.LastName = req.Lastname;
            patientInfo.ConfirmationNo = req.Request.Confirmationnumber;
            //var Month = ConvertMonthToInt(req.Strmonth);

            var result = new AdminDashboard
            {
                myProfile = patientInfo,
                doc = items,
                requestid = requestid,
            };
            return result;
        }
        public bool ConcludeCare(int reqId, string notes, int phyId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var data = _db.Requestclients.Include(x => x.Request).Where(x => x.Requestid == reqId).FirstOrDefault();

                    if (data != null)
                    {
                        data.Request.Status = (int)Status.ToClose;
                        _db.SaveChanges();
                    }
                    Requeststatuslog rsl = new Requeststatuslog();
                    rsl.Physicianid = phyId;
                    rsl.Notes = notes;
                    rsl.Createddate = DateTime.Now;
                    rsl.Status = (int)(Status.ToClose);
                    rsl.Requestid = reqId;


                    _db.Requeststatuslogs.Add(rsl);
                    _db.SaveChanges();
                    Requestclosed requestclosed = new Requestclosed();
                    requestclosed.Requestid = reqId;
                    requestclosed.Requeststatuslogid = rsl.Requeststatuslogid;
                    requestclosed.Phynotes = notes;
                    _db.Requestcloseds.Add(requestclosed);
                    _db.SaveChanges();
                    var data1 = _db.Requestnotes.FirstOrDefault(x => x.Requestid == reqId);
                    if (data1 != null)
                    {
                        data1.Physiciannotes = notes;
                        data1.Modifieddate = DateTime.Now;
                        data1.Modifiedby = phyId.ToString();
                    }
                    else
                    {
                        Requestnote requestnote = new Requestnote();
                        requestnote.Requestid = reqId;
                        requestnote.Intdate = DateTime.Now.Day;
                        requestnote.Strmonth = DateTime.Now.Month.ToString();
                        requestnote.Intyear = DateTime.Now.Year;
                        requestnote.Createddate = DateTime.Now;
                        requestnote.Createdby = phyId.ToString();
                        requestnote.Physiciannotes = notes;
                        _db.Requestnotes.Add(requestnote);
                        _db.SaveChanges();
                    }


                    transaction.Commit();
                    return true;
                }
                catch
                {

                    transaction.Rollback();
                    return false;
                }

            }
        }
        /*******encounter****/

        //EncounterForm DataPost
        public bool FinalizeEncounterForm(int reqid)
        {
            var model = _db.Encounters.FirstOrDefault(x => x.RequestId == reqid);

            if (model != null)
            {
                model.IsFinalize = true;
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }
        ///***pdf****/
        ///

    }

}
