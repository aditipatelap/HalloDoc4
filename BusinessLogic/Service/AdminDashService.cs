using BusinessLogic.Interface;
using DataAccess.Data;
using DataAccess.Models;
using DataAccess.ViewModel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using MailKit;
using Microsoft.EntityFrameworkCore;
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
            dash.pendingcount = req.Count(x => x.Status == (short)Requeststatus.Accepted);
            dash.pendingcount = req.Count(x => x.Status == (short)Requeststatus.Accepted);
            dash.pendingcount = req.Count(x => x.Status == (short)Requeststatus.Accepted);
            dash.pendingcount = req.Count(x => x.Status == (short)Requeststatus.Accepted);
           
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
      
        public AdminDashboard GetDashboardData(int statusid)
        {
            List<int> id=new List<int>();
            if(statusid != 3 && statusid != 5)
            {
                id.Add(statusid);
            }
            else if(statusid  == 3)
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
            var dashboard = from Request in _db.Requests
                            join Requestclient in _db.Requestclients on Request.Requestid equals Requestclient.Requestid
                            where id.Contains(Request.Status)
                            select new AdminDash
                            {
                                Name = Requestclient.Firstname+" "+Requestclient.Lastname,
                                Requestor=Request.Firstname+" "+Request.Lastname,
                                RequestedDate=Request.Createddate,
                                PatientPhone=Requestclient.Phonenumber,
                                RequestorPhone=Request.Phonenumber,
                                Address=Requestclient.Address,
                                Notes=Requestclient.Notes,
                               

                             // Dob=Convert.ToDateTime(Requestclient.Intdate.ToString() + "-" + Requestclient.Strmonth + "-" + Requestclient.Intyear.ToString()),
                                RequestTypeid =Request.Requesttypeid
                            };
            var viewModel=dashboard.ToList();

            AdminDashboard adminDashboard = new AdminDashboard()
            {
                Dashboards = viewModel,
                Status=(status)statusid
                
            };
            return adminDashboard;
        }
    }
}
