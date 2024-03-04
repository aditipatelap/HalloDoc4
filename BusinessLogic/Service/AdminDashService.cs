using BusinessLogic.Interface;
using DataAccess.Data;
using DataAccess.Models;
using DataAccess.ViewModel;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using MailKit;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Reflection.Metadata.Ecma335;
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
            var req = _db.Requests.ToList();
            dash.newcount = req.Count(x => x.Status == (short)Requeststatus.Unassigned);
            dash.pendingcount= req.Count(x => x.Status == (short)Requeststatus.Accepted);
            dash.activecount= req.Count(x => x.Status == (short)Requeststatus.MDEnRoute || x.Status == (short)Requeststatus.MDonSite);
            dash.toclosecount = req.Count(x => x.Status == (short)Requeststatus.Cancelled || x.Status==(short)Requeststatus.Cancelledbypatient || x.Status==(short)Requeststatus.Closed);
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

        public AdminDashboard GetDashboardData(int statusid, string searchValue)
        {
            List<int> id = new List<int>();
            if (statusid != 3 && statusid != 5)
            {
                id.Add(statusid);
            }
            else if (statusid == 3)
            {
                id.Add(4);
                id.Add(5);

            }
            else
            {
                id.Add(3);
                id.Add(7);
                id.Add(8);

            }
            List<AdminDash> list = new List<AdminDash>();

            if (searchValue == null)
            {
                var dashboard = from Request in _db.Requests
                                join Requestclient in _db.Requestclients on Request.Requestid equals Requestclient.Requestid
                                where id.Contains(Request.Status)
                                select new AdminDash
                                {
                                    Name = Requestclient.Firstname + " " + Requestclient.Lastname,
                                    Requestor = Request.Firstname + " " + Request.Lastname,
                                    RequestedDate = Request.Createddate,
                                    PatientPhone = Requestclient.Phonenumber,
                                    RequestorPhone = Request.Phonenumber,
                                    Address = Requestclient.Address,
                                    Notes = Requestclient.Notes,
                                    requestid=Request.Requestid,

                                    // Dob=Convert.ToDateTime(Requestclient.Intdate.ToString() + "-" + Requestclient.Strmonth + "-" + Requestclient.Intyear.ToString()),
                                    RequestTypeid = Request.Requesttypeid
                                };

                list = dashboard.ToList();
            }
            else
            {

                var dashboard = from Request in _db.Requests
                                join Requestclient in _db.Requestclients on Request.Requestid equals Requestclient.Requestid
                                where id.Contains(Request.Status) && Requestclient.Firstname.Contains(searchValue) 
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

                                    // Dob=Convert.ToDateTime(Requestclient.Intdate.ToString() + "-" + Requestclient.Strmonth + "-" + Requestclient.Intyear.ToString()),
                                    RequestTypeid = Request.Requesttypeid
                                };
                list = dashboard.ToList();
            }


            AdminDashboard adminDashboard = new AdminDashboard()
            {
                Dashboards = list,
                Status = (status)statusid

            };
            return adminDashboard;
        }
        public AdminDashboard AssignRequest(int requestid)
        {


                var regions = _db.Regions.ToList();
                AdminDashboard adminDashboard = new AdminDashboard();
                adminDashboard.Regions = regions;
            //adminDashboard.patientname= patientname;
            adminDashboard.requestid= requestid;

            
                return adminDashboard;
          }
        public void SubmitAssignReq(AdminDashboard model,int requestid)
        {
           var id= _db.Physicians.Where(x => x.Firstname==model.assignreq.physicianname).FirstOrDefault();
            var request=_db.Requests.Where(x => x.Requestid== requestid).FirstOrDefault();


            request.Physicianid=id.Physicianid;
            request.Status = 2;
            
            _db.SaveChanges();


        }
        public AdminDashboard BlockCase(int reqid, string patientname)
        {


           
            AdminDashboard adminDashboard = new AdminDashboard();
           
            adminDashboard.patientname = patientname;
            
            adminDashboard.requestid=reqid;

            return adminDashboard;
        }
        public void SubmitBlockCase(AdminDashboard model,int reqid)
        {
            Blockrequest blockrequest = new Blockrequest();
            
            
           
           var request= _db.Requests.Where(x => x.Requestid == reqid).FirstOrDefault();
            request.Status= 10;
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
        public void  submitCancelCase(AdminDashboard model, int requestid)
        {
            var result = _db.Requests.Where(x => x.Requestid == requestid).FirstOrDefault();
            result.Status = 3;

            _db.SaveChanges();


        }




    }
}
