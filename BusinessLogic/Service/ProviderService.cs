using BusinessLogic.Interface;
using DataAccess.Data;
using DataAccess.Models;
using DataAccess.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static DataAccess.ViewModel.Constant;
using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

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

           
            List<int> reqStatusId = new List<int>();
            Physician phy = new Physician();

            if ( statusId != (int)Status.Active)
            {
                reqStatusId.Add(statusId);
            }
            if (statusId == (int)Status.New)
            {
                reqStatusId.Add((int)Requeststatuses.assignedbyphysician);
                //reqStatusId.Add((int)Requeststatuses.MDEnRoute);
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
                //Isfinalize

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
            dash.newcount = Request.Count(x => x.Status == (int)Requeststatuses.assignedbyphysician && x.Requestid == x.Requestclients.FirstOrDefault().Requestid && x.Physicianid == physicianId);
            dash.pendingcount = Request.Count(x => x.Status == (int)Requeststatuses.Accepted && x.Physicianid == physicianId && x.Requestid == x.Requestclients.FirstOrDefault().Requestid);
            dash.activecount = Request.Count(x => (x.Status == (int)Requeststatuses.MDonSite || x.Status == (int)Requeststatuses.MDEnRoute && x.Requestid == x.Requestclients.FirstOrDefault().Requestid) && (x.Physicianid == physicianId));
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
        /*view notes*/

        public void PostViewNotes(AdminDashboard model,string aspnetuserid)
        {


            var data = _db.Requestnotes.FirstOrDefault(x => x.Requestid == model.requestid);
            if (data != null)
            {
                data.Physiciannotes = model.AdditionalNotes;
                data.Modifieddate = DateTime.Now;
                data.Modifiedby = aspnetuserid;
            }
            else
            {
                var data2 = new Requestnote();
                data2.Requestid = model.requestid;
                data2.Adminnotes = model.AdditionalNotes;
                data2.Createdby = aspnetuserid;
                data2.Createddate = DateTime.Now;
                _db.Add(data2);
            }
            _db.SaveChanges();
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
                .Where(x => x.Requestid == requestid && x.Isdeleted == new BitArray(new bool[1] { false }))
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
                    var data = _db.Requests.Where(x => x.Requestid == reqId).FirstOrDefault();

                    if (data != null)
                    {
                        data.Status = (int)Requeststatuses.Closed;
                        _db.SaveChanges();
                    }
                    Requeststatuslog rsl = new Requeststatuslog();
                    rsl.Physicianid = phyId;
                    rsl.Notes = notes;
                    rsl.Createddate = DateTime.Now;
                    rsl.Status = (int)Requeststatuses.Closed;
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
        /******accept****/
       
 public void AcceptRequest(int Requestid, int physicianid)
        {
            var request = _db.Requests.FirstOrDefault(r => r.Requestid == Requestid);

            if (request != null)
            {
                request.Status = 2;
                request.Accepteddate = DateTime.Now;
                request.Physicianid = physicianid;
                request.Modifieddate = DateTime.Now;
            }
            _db.SaveChanges();

            var requestStatusLog = new Requeststatuslog
            {
                Requestid = Requestid,
                Status = 2,
                Physicianid = physicianid,
                Notes = "Accepted by physician",
                Createddate = DateTime.Now
            };
            _db.Requeststatuslogs.Add(requestStatusLog);
            _db.SaveChanges();
        }
        /*transferreq********/
        public void TransferCaseDataPost(ProviderDash model)
        {
            var request = _db.Requests.FirstOrDefault(r => r.Requestid == model.RequestId);

            if (request != null)
            {
                request.Status = 1;
                request.Physicianid = model.PhysicianId;
                request.Modifieddate = DateTime.Now;
                request.Physicianid = null;
            }
            _db.SaveChanges();

            var requestStatusLog = new Requeststatuslog
            {
                Requestid = model.RequestId,
                Status = 1,
                Physicianid = model.PhysicianId,
                Notes = model.PhysicianNotes,
                Createddate = DateTime.Now
            };
            _db.Requeststatuslogs.Add(requestStatusLog);
            _db.SaveChanges();
        }

        ///***pdf****/
        ///
        string fileName = "";
        string filePath = "";

        public void SaveDocument(IFormFile Document, int reqid, int Physicianid)
        {
            if (Document != null)
            {
                fileName = $"{Guid.NewGuid().ToString()}_{Document.FileName}";
                filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Documents", fileName);

                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Documents");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                using var stream = new FileStream(filePath, FileMode.Create);
                Document.CopyTo(stream);

                var requestWiseFile = new Requestwisefile
                {
                    Requestid = reqid,
                    Filename = fileName,
                    Createddate = DateTime.Now,
                    Isdeleted = new BitArray(new bool[1] {false}),
                    Ispatientrecords = new BitArray(new bool[1]),
                    Physicianid = Physicianid,
                };
                _db.Requestwisefiles.Add(requestWiseFile);
                _db.SaveChanges();
            }
        }
        /****scheduling*/
        public bool CreateShift(scheduleModel scheduleModel, string aspnetid)
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
                Physicianid  = _db.Aspnetusers
    .Include(x => x.Physicians)
    .Where(x => x.Id == aspnetid && x.Physicians.Any())
    .FirstOrDefault()
    .Physicians
    .FirstOrDefault()
    .Physicianid,
            Startdate = DateOnly.FromDateTime(scheduleModel.ShiftDate),
                Isrepeat = new BitArray(new bool[1] { scheduleModel.isRepeat }),
                Weekdays = scheduleModel.SelectedDayIds,
                Repeatupto = scheduleModel.RepeatEnd,
                Createdby = aspnetid,
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

        /*********create req*********/
        public void CreateRequestDatapost(AdminDashboard model)
        {
            Request request = new Request();
            Requestclient requestclient = new Requestclient();
            Requestnote requestnote = new Requestnote();

            var date = model.CreateReqquestModel.DOB.Day;
            var month = model.CreateReqquestModel.DOB.Month.ToString();
            var year = model.CreateReqquestModel.DOB.Year;

            var countOfRequests = _db.Requests.Count(r => r.Createddate.Date == DateTime.Today);
            var regionAbbreviation = "";
            var regionlist = _db.Regions.Select(x => x.Name).ToList();

            var regionInfo = _db.Regions
                            .Where(x => x.Name == model.CreateReqquestModel.State)
                            .Select(x => new { x.Abbreviation, x.Regionid })
                            .FirstOrDefault();


            if (regionInfo != null)
            {
                regionAbbreviation = regionInfo.Abbreviation;
            }

            var confirmationNumber = regionAbbreviation +
                                     DateTime.Now.Day.ToString("D2") +
                                     DateTime.Now.Month.ToString("D2") +
                                     DateTime.Now.Year.ToString().Substring(2, 2) +
                                     model.CreateReqquestModel.LastName.Substring(0, 2).ToUpper() +
                                     model.CreateReqquestModel.FirstName.Substring(0, 2).ToUpper() +
                                     (countOfRequests + 1).ToString("D4");


            request.Requesttypeid = (int)DataAccess.ViewModel.Constant.RequestType.Business;
            request.Status = (int)Requeststatuses.Unassigned;
            request.Createddate = DateTime.Now;
            request.Isurgentemailsent = new BitArray(new bool[1] { true });
            request.Firstname = model.CreateReqquestModel.FirstName;
            request.Lastname = model.CreateReqquestModel.LastName;
            request.Phonenumber = model.CreateReqquestModel.PhoneNumber;
            request.Email = model.CreateReqquestModel.Email;
            request.Confirmationnumber = confirmationNumber;
            request.Physicianid = model.physicianid;
            _db.Add(request);
            _db.SaveChanges();

            requestclient.Requestid = request.Requestid;
            requestclient.Regionid = regionInfo.Regionid;
            requestclient.Email = model.CreateReqquestModel.Email;
            requestclient.Firstname = model.CreateReqquestModel.FirstName;
            requestclient.Lastname = model.CreateReqquestModel.LastName;
            requestclient.Phonenumber = model.CreateReqquestModel.PhoneNumber;
            requestclient.City = model.CreateReqquestModel.City;
            requestclient.Street = model.CreateReqquestModel.Street;
            requestclient.State = model.CreateReqquestModel.State;
            requestclient.Zipcode = model.CreateReqquestModel.ZipCode;
            requestclient.Address = model.CreateReqquestModel.Street + ' ' + model.CreateReqquestModel.City + ' ' + model.CreateReqquestModel.State;
            requestclient.Intyear = year;
            requestclient.Strmonth = month;
            requestclient.Intdate = date;

            _db.Add(requestclient);
            _db.SaveChanges();


            requestnote.Adminnotes = model.CreateReqquestModel.Notes;
            requestnote.Requestid = request.Requestid;
            requestnote.Createddate = DateTime.Now;
            requestnote.Createdby = "admin";

            _db.Add(requestnote);
            _db.SaveChanges();

            var SubjectName = "Create Account";
            var EmailTemplate = "Request for you is generated. To check your request status click on below link to generate account. <a href=\"https://localhost:7265/Patient/CreateAccount\">ClickHere</a>";
            var isEmailSent = "false";

            MailMessage message = new MailMessage();

            message.From = new System.Net.Mail.MailAddress("vanshita.bhansali@etatvasoft.com");
            message.To.Add(new MailAddress(requestclient.Email));
            message.Subject = SubjectName;
            message.IsBodyHtml = true;
            message.Body = EmailTemplate;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "mail.etatvasoft.com";
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential("vanshita.bhansali@etatvasoft.com", "GEtTj-2V%=0u");
            smtp.EnableSsl = true;
            smtp.Send(message);
            smtp.UseDefaultCredentials = false;
            isEmailSent = "true";

            if (isEmailSent == "true")
            {
                var emailLog = new DataAccess.Models.Emaillog
                {
                    Emailtemplate = EmailTemplate,
                    Subjectname = SubjectName,
                    Emailid = requestclient.Email,
                    Createdate = DateTime.Now,
                    Requestid = request.Requestid,
                    Confirmationnumber = request.Confirmationnumber,
                    Roleid = (int)Roles.Patient,
                    Physicianid = request.Physicianid,
                    Sentdate = DateTime.Now,
                    Isemailsent = new BitArray(new bool[] { true }),
                };
                _db.Emaillogs.Add(emailLog);
                _db.SaveChanges();
            }
        }
        //send link
        public void SendMailLink(AdminDashboard model, int physicianid)
        {
            var usr = _db.Users.Where(u => u.Email == model.sendLink.Email).FirstOrDefault();
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

            message.From = new System.Net.Mail.MailAddress("vanshita.bhansali@etatvasoft.com");
            message.To.Add(new System.Net.Mail.MailAddress(model.sendLink.Email));
            message.Subject = "Request submit page Link";
            message.IsBodyHtml = true;
            var resetLink = "https://localhost:44367/Patient/submitReq";
            message.Body = "Submit Request Page Link :  " + resetLink;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "mail.etatvasoft.com";
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential("vanshita.bhansali@etatvasoft.com", "GEtTj-2V%=0u");
            smtp.EnableSsl = true;
            smtp.Send(message);
            smtp.UseDefaultCredentials = false;
            var isEmailSent = false;

            var IsSMSSend = false;
            string link = $"https://localhost:44367/Patient/submitReq";
            string SMSTemplate = $"For Submit your Request: {link}";

            var accountsid = "AC4e906b8950220baa3121323a2a3ec1f8";
            var authtoken = "5037610beaf3ced4e3e5b3c0a9796101";
            TwilioClient.Init(accountsid, authtoken);

            var messageoptions = new CreateMessageOptions(
              new PhoneNumber(model.sendLink.PhoneNumber));
            messageoptions.From = new PhoneNumber("+16562269587");
            messageoptions.Body = SMSTemplate;
            MessageResource.Create(messageoptions);

            IsSMSSend = true;
            isEmailSent = true;
            if (IsSMSSend)
            {
                Smslog smslog = new Smslog();
                smslog.Smstemplate = SMSTemplate;
                smslog.Mobilenumber = model.sendLink.PhoneNumber;
                smslog.Roleid = model.roleid;
                smslog.Createdate = DateTime.Now;
                smslog.Sentdate = DateTime.Now;
                smslog.Issmssent = new BitArray(new bool[1] { true });
                smslog.Senttries = 1;
                smslog.Adminid = physicianid;
                _db.Add(smslog);
                _db.SaveChanges();
            }

            var SubjectName = "Submit Request";
            string link1 = $"https://localhost:44367/Patient/submitReq";
            string EmailTemplate = $"For Submit your Request:<a href=\"{link}\">ClickHere</a>";

            //var EmailTemplate = "You Can Submit Request<a href=\"https://localhost:44367/Patient/submitReq\">ClickHere</a> to view";
            var EmailTemplate1 = "You can submit your request";
            if (isEmailSent)
            {
                var emailLog = new Emaillog
                {
                    Emailtemplate = EmailTemplate1,
                    Subjectname = SubjectName,
                    Emailid = model.sendLink.Email,
                    Createdate = DateTime.Now,
                    Roleid = (int)Roles.Patient,
                    Sentdate = DateTime.Now,
                    Adminid = physicianid,
                    Isemailsent = new BitArray(new bool[] { true }),
                    Senttries = 1,
                };
                _db.Emaillogs.Add(emailLog);
                _db.SaveChanges();
            }
        }

    }

}
