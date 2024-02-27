using BusinessLogic.Interface;
using DataAccess.Data;
using DataAccess.Models;
using DataAccess.ViewModel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Service
{
    public  class AdminDashService:IAdminDash
    {
        private readonly DataAccess.Data.ApplicationDbContext _db;

        public AdminDashService(ApplicationDbContext db)
        {
            _db = db;
        }
       
        
        public AdminDashboard GetDashboardData(int statusid)
        {
            List<int> id=new List<int>();
            
           if(statusid==1)
            {
                id.Add(1);
            }
           if(statusid==2)
            {
                id.Add(2);
            }
           if(statusid==3)
            {
                id.Add(4);
                id.Add(5);

            }
           if(statusid==4)
            {
                id.Add(6);
            }
            if (statusid == 5)
            {
                id.Add(3);
                id.Add(7);
                id.Add(8);
            }
            if (statusid == 6)
            {
                id.Add(9);
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
            int count = viewModel.Count;
            
            return new AdminDashboard
            {
                count = count,
                Dashboards = viewModel
            };
        }
    }
}
