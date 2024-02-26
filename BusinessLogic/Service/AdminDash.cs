using BusinessLogic.Interface;
using DataAccess.Data;
using DataAccess.ViewModel;

namespace BusinessLogic.Service
{
    public  class AdminDash:IAdminDash
    {
        private readonly DataAccess.Data.ApplicationDbContext _db;

        public AdminDash(ApplicationDbContext db)
        {
            _db = db;
        }
        public string DeterminePartialView(string btn)
        {
            if(btn =="newbtn")
            {
                return "_NewPartial";
            }
            if (btn == "pendingbtn")
            {
                return "_PendingPartial";
            }
            if(btn=="activebtn")
            {
                return "_ActivePartial";
            }
            if (btn == "concludebtn")
            {
                return "_ConcludePartial";
            }
            if (btn == "toclosebtn")
            {
                return "_ToClosePartial";
            }
            if (btn == "unpaidbtn")
            {
                return "_UnpaidPartial";
            }
            else
                return "";
        }
        public AdminDashboard GetDashboardData(int id)
        {
            
            AdminDashboard dashboard = new AdminDashboard();
            //_db.Requests.Where().FirstOrDefault();

            return dashboard;
        }
    }
}
