using BusinessLogic.Interface;
using ClosedXML.Excel;
using DataAccess.Data;
using DataAccess.Models;
using DataAccess.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Net;
using System.Net.Mail;
using System.Text.Json.Nodes;
using System.Web.Helpers;
using System.Web.Mvc;
using static DataAccess.ViewModel.Constant;

namespace BusinessLogic.Service
{
    public class AdminDashService : IAdminDash
    {
        private readonly DataAccess.Data.ApplicationDbContext _db;

        public AdminDashService(ApplicationDbContext db)
        {
            _db = db;
        }
        public AdminDashboard RequestCount()
        {

            AdminDashboard dash = new AdminDashboard();
            var req = _db.Requests.Include(x =>x.Requestclients).Where(x =>x.Requestid==x.Requestclients.FirstOrDefault().Requestid).ToList();
            dash.newcount = req.Count(x => x.Status == (short)Requeststatus.Unassigned);
            dash.pendingcount = req.Count(x => x.Status == (short)Requeststatus.Accepted);
            dash.activecount = req.Count(x => x.Status == (short)Requeststatus.MDEnRoute || x.Status == (short)Requeststatus.MDonSite);
            dash.toclosecount = req.Count(x => x.Status == (short)Requeststatus.Cancelled || x.Status == (short)Requeststatus.Cancelledbypatient || x.Status == (short)Requeststatus.Closed);
            dash.unpaidcount = req.Count(x => x.Status == (short)Requeststatus.Unpaid);
            dash.concludecount = req.Count(x => x.Status == (short)Requeststatus.Conclude);

            
            return dash;


        }
        //public AdminDashboard GetName(int statusid)
        //  {
        //      AdminDashboard adminDashboard = new AdminDashboard();
        //    adminDashboard.Status = (status)statusid;
        //      return adminDashboard;

        //  }
        //public  RequestCount(int statusid)
        //{


        //    //for()
        //    //int[] arr = new int[5];
        //    //if(statusid==1)
        //    //{
        //    //    arr[0]++;
        //    //}
        //    //if(statusid==2)
        //    //{
        //    //    arr[1]++;
        //    ////}
        //    //if (statusid == 1)
        //    //{
        //    //    Array[0]
        //    //}
        //    //var reqtypes = new int[] { 1, 2, 3 ,4,5,6};
        //    //var reqcount = reqtypes
        //    //    .Select(reqtype => _db.Requests.Count(x => x.Status == reqtype)).ToArray();
        //    ////var requestcount = _db.Requests.GroupBy(x => x.Status).Select(req => req.Count()).ToArray();
        //    //return reqcount;
        //  }

        public AdminDashboard GetName(int statusid)
        {
            AdminDashboard dash = new AdminDashboard();
            dash.Status = (status)statusid;
            return dash;
        }
        

        public AdminDashboard GetDashboardData(int statusid, string searchValue,int currentpage,string dropdown,int reqtype )
            {

            List<int> id = new List<int>();
            if (statusid != (short)Status.Active && statusid != (short)Status.ToClose)
            {
                id.Add(statusid);
            }
            else if (statusid == (short)Status.Active)
            {
                id.Add((short)Requeststatus.MDEnRoute);
                id.Add((short)Requeststatus.MDonSite);

            }
            else
            {
                id.Add((short)Requeststatus.Cancelled);
                id.Add((short)Requeststatus.Cancelledbypatient);
                id.Add((short)Requeststatus.Closed);

            }
            // List<AdminDash> list = new List<AdminDash>();

            var dashboard = (from Request in _db.Requests
                             join Requestclient in _db.Requestclients on Request.Requestid equals Requestclient.Requestid
                             // join Physician in _db.Physicians on Request.Physicianid equals Physician.Physicianid
                             where id.Contains(Request.Status) && Request.Isdeleted == false &&
                             (dropdown == null || Requestclient.Address.Contains(dropdown)) &&
                          (searchValue == null || Requestclient.Firstname.Contains(searchValue) || Requestclient.Lastname.Contains(searchValue) || Request.Firstname.Contains(searchValue) || Request.Lastname.Contains(searchValue)) &&
                          (reqtype == 0 || Request.Requesttypeid == reqtype)
                             select new AdminDash
                             {
                                 Name = Requestclient.Firstname + " " + Requestclient.Lastname,
                                 IntDate = Requestclient.Intdate,
                                 IntYear = Requestclient.Intyear,
                                 StrMonth = Requestclient.Strmonth,
                                 Requestor = Request.Firstname + " " + Request.Lastname,
                                 RequestedDate = Request.Createddate,
                                 PatientPhone = Requestclient.Phonenumber,
                                 RequestorPhone = Request.Phonenumber,

                                 Address = Requestclient.Address,
                                 Notes = Requestclient.Notes,
                                 requestid = Request.Requestid,
                                 //PhysicianName=Physician.Firstname+" "+Physician.Lastname,
                                 // Dob=Convert.ToDateTime(Requestclient.Intdate.ToString() + "-" + Requestclient.Strmonth + "-" + Requestclient.Intyear.ToString()),
                                 RequestTypeid = Request.Requesttypeid
                             });

               
            int totalrecords = dashboard.Count();
            int pagesize = 1;
            int totalPages = (int)Math.Ceiling((double)totalrecords/pagesize);
           var  paginateddashboard = dashboard.Skip((currentpage-1)*pagesize).Take(pagesize).ToList();

            
          
            AdminDashboard adminDashboard = new AdminDashboard()
            {
                Dashboards = paginateddashboard,
                CurrentPage=currentpage,
                TotalPages=totalPages,
                PageSize=pagesize,
                Status = (status)statusid

            };
            return adminDashboard;
        }
        //    public MemoryStream ExportALl(int statusid)
        //    {
        //        List<int> id = new List<int>();
        //        if (statusid != (short)Requeststatus.MDEnRoute || statusid == (short)Requeststatus.MDonSite && statusid != (short)Requeststatus.Cancelled || statusid != (short)Requeststatus.Cancelledbypatient || statusid == (short)Requeststatus.Closed)
        //        {
        //            id.Add(statusid);
        //        }
        //        else if (statusid == (short)Requeststatus.MDEnRoute || statusid == (short)Requeststatus.MDonSite)
        //        {
        //            id.Add((short)Requeststatus.MDEnRoute);
        //            id.Add((short)Requeststatus.MDonSite);

        //        }
        //        else
        //        {
        //            id.Add((short)Requeststatus.Cancelled);
        //            id.Add((short)Requeststatus.Cancelledbypatient);
        //            id.Add((short)Requeststatus.Closed);

        //        }
        //        var dashboard = (from Request in _db.Requests
        //                         join Requestclient in _db.Requestclients on Request.Requestid equals Requestclient.Requestid
        //                         // join Physician in _db.Physicians on Request.Physicianid equals Physician.Physicianid
        //                         where id.Contains(Request.Status) && Request.Isdeleted == false 


        //                         select new AdminDash
        //                         {
        //                             Name = Requestclient.Firstname + " " + Requestclient.Lastname,
        //                             Requestor = Request.Firstname + " " + Request.Lastname,
        //                             RequestedDate = Request.Createddate,
        //                             PatientPhone = Requestclient.Phonenumber,
        //                             RequestorPhone = Request.Phonenumber,

        //                             Address = Requestclient.Address,
        //                             Notes = Requestclient.Notes,
        //                             requestid = Request.Requestid,
        //                             //PhysicianName=Physician.Firstname+" "+Physician.Lastname,
        //                             // Dob=Convert.ToDateTime(Requestclient.Intdate.ToString() + "-" + Requestclient.Strmonth + "-" + Requestclient.Intyear.ToString()),
        //                             RequestTypeid = Request.Requesttypeid
        //                         }).ToList();
        //        using (var workbook = new XLWorkbook())
        //        {
        //            var worksheet = workbook.Worksheets.Add("Dashboard");

        //            // Add headers
        //            var properties = typeof(AdminDash).GetProperties();
        //            for (int i = 0; i < properties.Length; i++)
        //            {
        //                worksheet.Cell(1, i + 1).Value = properties[i].Name;
        //            }

        //            // Add data
        //            for (int i = 0; i < dashboard.Count; i++)
        //            {
        //                for (int j = 0; j < properties.Length; j++)
        //                {
        //                    var value= properties[j].GetValue(dashboard[i]);

        //                    if(value is DateTime)
        //                    {
        //                        worksheet.Cell(i + 2, j + 1).Value = ((DateTime)value).ToString("yyyyy-MM-dd   HH:mm:ss");

        //                    }
        //                    else
        //                    {
        //                        worksheet.Cell(i + 2, j + 1).Value = value != null ? value.ToString() : string.Empty;
        //                    }
        //                }
        //            }

        //            // Create a memory stream to store the Excel file
        //            using (var stream = new MemoryStream())
        //            {
        //                // Save the workbook to the memory stream
        //                workbook.SaveAs(stream)
        //;

        //                // Set the position to the beginning of the stream
        //                stream.Seek(0, SeekOrigin.Begin);
        //                return stream;

        //                // Return the Excel file as a stream response
        //               // return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "dashboard_data.xlsx");
        //            }
        //        }
        //    }

        public AdminDashboard GetPatientInfoByStatus(int statusid)
        {
            List<int> id = new List<int>();
            if (statusid != (short)Status.Active || statusid == (short)Status.ToClose)
            {
                id.Add(statusid);
            }
            else if (statusid == (short)Status.Active)
            {
                id.Add((short)Requeststatus.MDEnRoute);
                id.Add((short)Requeststatus.MDonSite);

            }
            else
            {
                id.Add((short)Requeststatus.Cancelled);
                id.Add((short)Requeststatus.Cancelledbypatient);
                id.Add((short)Requeststatus.Closed);

            }
            var query = (from Request in _db.Requests
                         join Requestclient in _db.Requestclients on Request.Requestid equals Requestclient.Requestid
                         // join Physician in _db.Physicians on Request.Physicianid equals Physician.Physicianid
                         where id.Contains(Request.Status) && Request.Isdeleted == false
                         select new AdminDash
                         {
                             Name = Requestclient.Firstname + " " + Requestclient.Lastname,
                             IntDate = Requestclient.Intdate,
                             IntYear = Requestclient.Intyear,
                             StrMonth = Requestclient.Strmonth,
                             Requestor = Request.Firstname + " " + Request.Lastname,
                             RequestedDate = Request.Createddate,
                             PatientPhone = Requestclient.Phonenumber,
                             RequestorPhone = Request.Phonenumber,

                             Address = Requestclient.Address,
                             Notes = Requestclient.Notes,
                             requestid = Request.Requestid,
                             //PhysicianName=Physician.Firstname+" "+Physician.Lastname,
                             // Dob=Convert.ToDateTime(Requestclient.Intdate.ToString() + "-" + Requestclient.Strmonth + "-" + Requestclient.Intyear.ToString()),
                             RequestTypeid = Request.Requesttypeid
                         });


            var data = new AdminDashboard
            {
              adminDashes  = query.ToList(),
            };

            return data;
        }
        public AdminDashboard GetViewCase(int requestid)
        {
            AdminDashboard dashboard = new AdminDashboard();

            var items = _db.Requestclients
                           .Where(req => req.Request.Requestid == requestid)
                            .Select(req => new ViewCase()
                            {
                                RequestId = requestid,
                                //RequestTypeId = Requesttypeid,
                                // ConfNo = req.Address.Substring(0, 2) + req.IntDate.ToString() + (Month)req.StrMonth + req.IntYear.ToString() + req.Lastname.Substring(0, 2) + req.FirstName.Substring(0, 2) + "002",
                                Symptoms = req.Notes,
                                FirstName = req.Firstname,
                                LastName = req.Lastname,
                                //DOB = new DateTime((int)req.Intyear, Convert.ToInt32(req.Strmonth.Trim()), (int)req.Intdate),
                                Mobile = req.Phonenumber,
                                Email = req.Email,
                                Address = req.Address,
                                ConfNo = req.Request.Confirmationnumber,
                            }).FirstOrDefault();
            dashboard.viewcase = items;
            dashboard.requestid = requestid;
            //dashboard.ConfirmationNo=items.
            //dashboard.statusid = statusid;
            //dashboard.btnname=btnname;
            
            return dashboard;
        }
        public void EditViewCaseData(AdminDashboard model, int requestid)
            {
            var requestclient = _db.Requestclients.FirstOrDefault(x => x.Requestid == requestid);
            if (requestclient != null)
            {
                requestclient.Phonenumber = model.viewcase.Mobile;
                requestclient.Email = model.viewcase.Email;
            }
            _db.SaveChanges();
        }
         public AdminDashboard AssignRequest(int requestid)
        {


            var regions = _db.Regions.ToList();
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.Regions = regions;
            //adminDashboard.patientname= patientname;
            adminDashboard.requestid = requestid;


            return adminDashboard;
        }

        public void SubmitAssignReq(AdminDashboard model, int requestid)
        {
            var id = _db.Physicians.Where(x => x.Firstname == model.assignreq.physicianname).FirstOrDefault();
            var request = _db.Requests.Where(x => x.Requestid == requestid).FirstOrDefault();


            request.Physicianid = id.Physicianid;
            request.Status = (short)Requeststatus.Accepted;

            _db.SaveChanges();


        }
        public AdminDashboard BlockCase(int reqid, string patientname)
        {
            AdminDashboard adminDashboard = new AdminDashboard();

            adminDashboard.patientname = patientname;

            adminDashboard.requestid = reqid;

            return adminDashboard;
        }
        public void SubmitBlockCase(AdminDashboard model, int reqid)
        {
            Blockrequest blockrequest = new Blockrequest();
            var request = _db.Requests.Where(x => x.Requestid == reqid).FirstOrDefault();
            request.Status = (short)Requeststatus.Clear;
            BlockReq blockReq = new BlockReq();
            blockrequest.Reason = model.blockreq.Blockreason;
            blockrequest.Email = request.Email;
            //reqid must be not null
            //blockrequest.Requestid =reqid;
            _db.Blockrequests.Add(blockrequest);
            _db.SaveChanges();
            _db.SaveChanges();
        }

        public AdminDashboard CancelCase(int requestid,string patientname)
        {
            var casetag = _db.Casetags.ToList();
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.Caserequest = casetag;
            adminDashboard.requestid = requestid;
            adminDashboard.patientname = patientname;
            return adminDashboard;
        }
        public void submitCancelCase(AdminDashboard model, int requestid)
        {
            var result = _db.Requests.Where(x => x.Requestid == requestid).FirstOrDefault();
            result.Status = (short)Requeststatus.Cancelled;

            _db.SaveChanges();


        }
        public AdminDashboard TransferRequest(int requestid)
        {
            var regions = _db.Regions.ToList();
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.Regions = regions;
            adminDashboard.requestid = requestid;
            return adminDashboard;
        }
        public void SubmitTransferReq(AdminDashboard model, int requestid)
        {
            var id = _db.Physicians.Where(x => x.Firstname == model.assignreq.physicianname).FirstOrDefault();
            var request = _db.Requests.Where(x => x.Requestid == requestid).FirstOrDefault();
            request.Physicianid = id.Physicianid;
            _db.SaveChanges();
        }
        public JsonArray GetBusiness(int selectedvalue)
        {
            AdminDashboard model = new AdminDashboard();
            var data = new JsonArray();
            model.Healthprofessionals = _db.Healthprofessionals.Where(x => x.Profession == selectedvalue).ToList();
            foreach (var i in model.Healthprofessionals)
            {
                data.Add(new { businessId = i.Vendorid, businessName = i.Vendorname });
            }
            return data;
        }
        //public  SendOrders GetVendorDetails(int selectedvalue)
        //{
        //    SendOrders orders = new SendOrders();
        //  orders result = _db.Healthprofessionals.Where(x => x.Profession==selectedvalue).FirstOrDefault();
        //    //var result = new
        //    //{
        //    //    faxnumber = data.Faxnumber,
        //    //    businesscontact = data.Businesscontact,
        //    //    email = data.Email

        //    //};
        //    return orders;
        //}
        public Healthprofessional GetBusinessDetails(int selectedvalue)
        {
            Healthprofessional businessDetails = _db.Healthprofessionals.First(x => x.Vendorid == selectedvalue);

            return businessDetails;
        }
        public AdminDashboard SendOrder(int requestid)
        {

            //var regions = _db.Professions.ToList();
            AdminDashboard adminDashboard = new AdminDashboard();
            var healthprof = _db.Healthprofessionals.ToList();
            var healthproftypes = _db.Healthprofessionaltypes.ToList();

            // adminDashboard.sendorder.Email=
            adminDashboard.Healthprofessionals = healthprof;
            adminDashboard.healthprofessionaltypes = healthproftypes;


            adminDashboard.requestid = requestid;
            return adminDashboard;
        }

        public void SendOrderReq(AdminDashboard model, int requestid, string adminname)
        {
            Healthprofessional hf = new Healthprofessional();
            hf.Email = model.sendorder.Email;
            hf.Faxnumber = model.sendorder.FaxNumber;
            hf.Businesscontact = model.sendorder.BusinessContact;


            Orderdetail orderdetail = new Orderdetail();

            orderdetail.Vendorid = model.sendorder.VendorId;
            orderdetail.Businesscontact = model.sendorder.BusinessContact;
            orderdetail.Email = model.sendorder.Email;
            orderdetail.Faxnumber = model.sendorder.FaxNumber;
            orderdetail.Noofrefill = model.sendorder.NUmberofrefill;
            orderdetail.Requestid = requestid;
            orderdetail.Createddate = DateTime.Now;
            orderdetail.Createdby = adminname;


            _db.Orderdetails.Add(orderdetail);
            _db.SaveChanges();

        }
        public AdminDashboard ClearCase(int requestid)
        {
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.requestid = requestid;
            return adminDashboard;

        }

        public void SubmitClearCase(AdminDashboard model, int requestid, int adminid)
        {
            var req = _db.Requests.Where(x => x.Requestid == requestid).FirstOrDefault();
            if (req != null)
            {
                req.Isdeleted = true;
                req.Status = (short)Requeststatus.Clear;
                Requeststatuslog reqlog = new Requeststatuslog();
                reqlog.Requestid = requestid;
                reqlog.Status = req.Status;
                // reqlog.Adminid = adminid;
                reqlog.Createddate = DateTime.Now;
                _db.Requeststatuslogs.Add(reqlog);
            }
            _db.SaveChanges();

        }
        //public AdminDashboard GetViewUpload(int requestid)
        //{
        //    var items = _db.Requestwisefiles.Where(x => x.Requestid == requestid).Select(m => new ViewUpload
        //    {

        //        uploaddate = m.Createddate,
        //        uploader = m.Filename
        //    }).ToList();
        //    var result = new AdminDashboard()
        //    {
        //         ViewUpload= items,
               
        //    };

        //    return result;

       // }
        public AdminDashboard SendAgreeement(int requestid)
        {
            var req = _db.Requestclients.Where(x => x.Requestid == requestid).Select(x => new AgreementReq
            {
                PhoneNumber = x.Phonenumber,
                EmailID = x.Email,
                Requestid = x.Requestid
            }).FirstOrDefault();
            AdminDashboard model = new AdminDashboard()
            {

                agreementReq = req
            };

            return model;
        }
        public AdminDashboard GetEncounterForm(int requestid)
        {
            var items = _db.Requestclients.Where(x => x.Requestid == requestid).Select(m => new EncounterForm
            {

                FirstName = m.Firstname,
                LastName=m.Lastname,
                Email=m.Email,
                Location=m.Address,
                //Dob = $"{m.Intdate}/{m.Strmonth}/{m.Intyear}",
                Dob = Convert.ToDateTime(m.Intdate.ToString() + "/" + m.Strmonth + "/" + m.Intyear.ToString()),


            }).FirstOrDefault();
            var result = new AdminDashboard()
            {
                encounterform = items
            };
            return result;

        }
       
        //public AdminDashboard GetEncounterForm(int requestid)
        //{
        //    var items = _db.Requestclients.Where(x => x.Requestid == requestid).Select(m => new EncounterForm
        //    {

        //        FirstName = m.Firstname,
        //        LastName = m.Lastname,
        //        Email = m.Email

        //    }).FirstOrDefault();
        //    var result = new AdminDashboard()
        //    {
        //        encounterform = items
        //    };
        //    return result;

        //}
        public AdminDashboard GetViewNotes(int requestid)
        {
            
                AdminDashboard model = new AdminDashboard();
                model.requestid = requestid;
                var data = _db.Requeststatuslogs.Include(x => x.Physician).Where(x => x.Requestid == requestid).Select(x => new ViewNotesModel
                {
                    firstName = x.Physician.Firstname,
                    lastName = x.Physician.Lastname,
                    CreatedDate = x.Createddate,
                    TransferNotes = x.Notes

                }).ToList();

                var data2 =_db.Requestnotes.Where(x => x.Requestid == requestid).FirstOrDefault();
                model.viewnotes = data;
                if (data2 != null)
                {


                    model.AdminNoes = data2.Adminnotes;
                    model.PhysicianNotes = data2.Physiciannotes;

                }
                else
                {
                    model.AdminNoes = null;
                    model.PhysicianNotes = null;

                }
                return model;
            }
        
        public void PostViewNotes( AdminDashboard model)
        {
            
          
                var data = _db.Requestnotes.FirstOrDefault(x => x.Requestid == model.requestid);
                if (data != null)
                {
                    data.Adminnotes = model.AdditionalNotes;
                    data.Modifieddate = DateTime.Now;
                    data.Modifiedby = "Admin";
                }
                else
                {
                    var data2 = new Requestnote();
                    data2.Requestid = model.requestid;
                    data2.Adminnotes = model.AdditionalNotes;
                    data2.Createdby = "Admin";
                    data2.Createddate = DateTime.Now;
                    _db.Add(data2);
                }
                _db.SaveChanges();
            }
        
        public AdminDashboard GetMyProfile(string adminid)
        {
            var result=_db.Admins.Include(x=>x.Aspnetuser).Where(x => x.Aspnetuserid=="19").FirstOrDefault();
            var data = new Profile
            {
                
                FirstName = result.Firstname,
                LastName = result.Lastname,
                Email = result.Email,
                Address1 = result.Address1,
                Address2 = result.Address2,
                Zipcode = result.Zip,
                UserName=result.Aspnetuser.Name
               
            };
            var model = new AdminDashboard()
            {
                myProfile = data
            };
               
            return model;

        }
        public void PostMyProfile(string adminid,AdminDashboard adminDashboard)
        {
            var result = _db.Admins.Include(x => x.Aspnetuser).Where(x => x.Aspnetuserid == adminid).FirstOrDefault();

            result.Firstname = adminDashboard.myProfile.FirstName;
            //LastName = result.Lastname,
            //Email = result.Email,
            //Address1 = result.Address1,
            //Address2 = result.Address2,
            //Zipcode = result.Zip,
            //UserName = result.Aspnetuser.Name


            _db.SaveChanges();

        }
        /**************view uploads**********/
        /*View Uploads*/
            
        string fileName = "";
        string filePath = "";

        public void SaveDocument(IFormFile Document, int requestid)
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
                    Requestid = requestid,
                    Filename = fileName,
                    Createddate = DateTime.Now,
                    Isdeleted = new BitArray(new bool[1] { false }),
                    Ispatientrecords = new BitArray(new bool[1]),
                    Adminid = 4,
                };
                _db.Requestwisefiles.Add(requestWiseFile);
                _db.SaveChanges();
            }
        }
        public AdminDashboard ViewUploadData(int requestid, string patientname, string confirmationno, string email)
        {
            AdminDashboard adminDashboardModel = new AdminDashboard();
            adminDashboardModel.requestid = requestid;
            adminDashboardModel.patientname = patientname;
            adminDashboardModel.ConfirmationNo = confirmationno;
            adminDashboardModel.Email = email;
            return adminDashboardModel;
        }
        public AdminDashboard ViewUploadDataList(int requestid)
        {
            var items = _db.Requestwisefiles.Include(x => x.Request).Where(x => x.Requestid == requestid && x.Isdeleted == new BitArray(new bool[1] { false })).Select(m => new ViewUpload
            {
                CreatedDate = m.Createddate,
                Month = (Month)m.Createddate.Month,
                FileName = m.Filename,
                Requestid = m.Requestid,
                UploaderName = m.Request.Firstname + " " + m.Request.Lastname,
                Adminid = m.Adminid,
            }).ToList();
            var adminname = "";
            if (items.Any() && items.FirstOrDefault().Adminid != null)
            {
                var adminid = items.First().Adminid;
                var admin = _db.Admins.FirstOrDefault(x => x.Adminid == adminid);
                adminname = admin.Firstname + " " + admin.Lastname;
            }

            AdminDashboard adminDashboardModel = new AdminDashboard();
            adminDashboardModel.ViewUpload = items;
            adminDashboardModel.requestid = requestid;
            adminDashboardModel.Adminname = adminname;

            return adminDashboardModel;

        }
        public void deleteDocument(string filename)
        {
            var requestwisefile = _db.Requestwisefiles.FirstOrDefault(r => r.Filename == filename);
            if (requestwisefile != null)
            {
                requestwisefile.Isdeleted = new BitArray(new bool[1] { true });
            }
            _db.SaveChanges();
        }
        public void sendEmail(List<string> file, string email, int requestid)
        {
            var SubjectName = "Email with Document";
            var EmailTemplate =
                $"Your files for Requestid {requestid} are as attached.";

            MailMessage message = new MailMessage();
            message.From = new System.Net.Mail.MailAddress("vanshita.bhansali@etatvasoft.com");
            message.To.Add(new MailAddress(email));
            message.Subject = SubjectName;
            message.IsBodyHtml = true;
            message.Body = EmailTemplate;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "mail.etatvasoft.com";
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential("vanshita.bhansali@etatvasoft.com", "GEtTj-2V%=0u");
            smtp.EnableSsl = true;

            foreach (var filename in file)
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Documents/" + filename);
                Attachment a = new Attachment(filePath);
                message.Attachments.Add(a);
            }

            smtp.Send(message);
            smtp.UseDefaultCredentials = false;

            // emailLogEntry(EmailTemplate, SubjectName, email, reqid);
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
                            .Where(x => x.Name == model.CreateReqquestModel.City)
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
            request.Status = (int)Requeststatus.Unassigned;
            request.Createddate = DateTime.Now;
            request.Isurgentemailsent = new BitArray(new bool[1] { true });
            request.Firstname = model.CreateReqquestModel.FirstName;
            request.Lastname = model.CreateReqquestModel.LastName;
            request.Phonenumber = model.CreateReqquestModel.PhoneNumber;
            request.Email = model.CreateReqquestModel.Email;
            request.Confirmationnumber = confirmationNumber;

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
                var emailLog = new Emaillog
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
        public bool VerifyAddress(string city)
        {
            var verify = _db.Regions.Where(x => x.Name == city).FirstOrDefault();
            if (verify != null)
            {
                return true;
            }
            else { return false; }
        }
         /***close case*********/
   //public AdminDashboard CloseCaseData(int reqid)
   //     {
   //         var items = _db.Requestwisefiles.Include(x => x.Request).Where(x => x.Requestid == reqid).Select(m => new ViewUpload
   //         {
   //             CreatedDate = m.Createddate,
   //             FileName = m.Filename,
   //             Month = (Month)m.Createddate.Month,
   //             UploaderName = m.Request.Firstname + " " + m.Request.Lastname,

   //         }).ToList();

   //         var data = _db.Requestclients.Include(x => x.Request).Where(x => x.Requestid == reqid).Select(x => new CloseCaseModel
   //         {
   //             FirstName = x.Firstname,
   //             LastName = x.Lastname,
   //             DOB = Convert.ToDateTime(x.Intdate.ToString() + "-" + x.Strmonth + "-" + x.Intyear.ToString()),
   //             PhoneNumber = x.Phonenumber,
   //             Email = x.Email,
   //             ConfirmationNo = x.Request.Confirmationnumber,
   //         }).FirstOrDefault();


   //         AdminDashboard model = new AdminDashboard();
   //         model.ViewUpload = items;
   //         model.CloseCaseModel = data;
   //         model.UserName = data.FirstName + " " + data.LastName;
   //         model.ConfirmationNo = data.ConfirmationNo;
   //         model.requestid = reqid;

   //         return model;
   //     }

        //public void CloseCaseDataPost(AdminDashboard model)
        //{
        //    var requestclient = _db.Requestclients.FirstOrDefault(x => x.Requestid == model.requestid);
        //    if (requestclient != null)
        //    {
        //        requestclient.Phonenumber = model.CloseCaseModel.PhoneNumber;
        //        requestclient.Email = model.CloseCaseModel.Email;
        //        _db.SaveChanges();
        //    }
        //}
        //public void CloseTheCase(int reqid)
        //{
        //    var request = _db.Requests.FirstOrDefault(x => x.Requestid == reqid);
        //    if (request != null)
        //    {
        //        request.Status = (int)Requeststatus.Unpaid;
        //        request.Modifieddate = DateTime.Now;
        //        _db.SaveChanges();
        //    }
        //}



    }
}
