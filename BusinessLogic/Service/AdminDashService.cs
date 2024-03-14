using BusinessLogic.Interface;
using DataAccess.Data;
using DataAccess.Models;
using DataAccess.ViewModel;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Metadata.Ecma335;
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
            if (statusid != (short)Requeststatus.MDEnRoute || statusid == (short)Requeststatus.MDonSite && statusid != (short)Requeststatus.Cancelled || statusid != (short)Requeststatus.Cancelledbypatient || statusid == (short)Requeststatus.Closed)
            {
                id.Add(statusid);
            }
            else if (statusid == (short)Requeststatus.MDEnRoute || statusid == (short)Requeststatus.MDonSite)
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
                             (dropdown == null || Requestclient.Address.Contains(dropdown))&&
                          (searchValue == null || Requestclient.Firstname.Contains(searchValue))&&
                          (reqtype==0 || Request.Requesttypeid==reqtype )
                             select new AdminDash
                             {
                                 Name = Requestclient.Firstname + " " + Requestclient.Lastname,
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

            
            //else
            //{

            //    var dashboard = from Request in _db.Requests
            //                    join Requestclient in _db.Requestclients on Request.Requestid equals Requestclient.Requestid
            //                    where id.Contains(Request.Status) && Requestclient.Firstname.Contains(searchValue) && Request.Isdeleted == false
            //                    select new AdminDash
            //                    {
            //                        Name = Requestclient.Firstname + " " + Requestclient.Lastname,
            //                        Requestor = Request.Firstname + " " + Request.Lastname,
            //                        RequestedDate = Request.Createddate,
            //                        PatientPhone = Requestclient.Phonenumber,
            //                        RequestorPhone = Request.Phonenumber,
            //                        Address = Requestclient.Address,
            //                        Notes = Requestclient.Notes,
            //                        requestid = Request.Requestid,

            //                        // Dob=Convert.ToDateTime(Requestclient.Intdate.ToString() + "-" + Requestclient.Strmonth + "-" + Requestclient.Intyear.ToString()),
            //                        RequestTypeid = Request.Requesttypeid
            //                    };
            //    list = dashboard.ToList();
            //}
           


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
        public AdminDashboard GetViewCase(int requestid)
        {
            AdminDashboard dashboard = new AdminDashboard();

            var items = _db.Requestclients
                           .Where(req => req.Request.Requestid == requestid)
                            .Select(req => new ViewCase()
                            {
                                RequestId = requestid,
                                //RequestTypeId = Requesttypeid,
                                // ConfNo = req.Address.Substring(0, 2) + req.IntDate.ToString() + req.StrMonth + req.IntYear.ToString() + req.Lastname.Substring(0, 2) + req.FirstName.Substring(0, 2) + "002",
                                Symptoms = req.Notes,
                                FirstName = req.Firstname,
                                LastName = req.Lastname,
                                DOB = new DateTime((int)req.Intyear, Convert.ToInt32(req.Strmonth.Trim()), (int)req.Intdate),
                                Mobile = req.Phonenumber,
                                Email = req.Email,
                                Address = req.Address
                            }).FirstOrDefault();
            dashboard.viewcase = items;
            
            return dashboard;
        }
        //public ViewCaseModel EditViewCaseData(int RequestID, int RequestTypeId, ViewCaseModel vp)
        //{
        //    var userToUpdate = _context.RequestClients.FirstOrDefault(x => x.RequestId == RequestID);
        //    if (userToUpdate != null)
        //    {
        //        userToUpdate.PhoneNumber = vp.Mobile;
        //        userToUpdate.Email = vp.Email;
        //        _context.Update(userToUpdate);
        //        _context.SaveChanges();
        //    }
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

        public AdminDashboard CancelCase(int requestid)
        {
            var casetag = _db.Casetags.ToList();
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.Caserequest = casetag;
            adminDashboard.requestid = requestid;
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
        public AdminDashboard GetViewUpload(int requestid)
        {
            var items = _db.Requestwisefiles.Where(x => x.Requestid == requestid).Select(m => new ViewUpload
            {

                uploaddate = m.Createddate,
                uploader = m.Filename
            }).ToList();
            var result = new AdminDashboard()
            {
                 ViewUpload= items,
               
            };

            return result;

        }
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




    }
}
