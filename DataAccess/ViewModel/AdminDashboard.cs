using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModel
{
    public class AdminDash
    {
        public string? Name { get; set; }
        public string Dob { get; set; }
        public string? Requestor { get; set; }
        public string? Physician { get; set; }
        public DateTime? RequestedDate { get; set; }
        public DateTime? DateofService { get; set; }
        public string? PatientPhone { get; set; }
        public string? RequestorPhone { get; set; }
        public string? Address { get; set; }
        public string? Notes { get; set; }
        public int RequestTypeid { get; set; }
        public string Region { get; set; }




    }
    public class AdminDashboard
    {
        public List<AdminDash> Dashboards { get; set; }
        public int newcount { get; set; }
        public int pendingcount { get; set; }
        public int activecount { get; set; }
        public int toclosecount { get; set; }
        public int concludecount { get; set; }
        public int unpaidcount { get; set; }
       public status Status { get; set; }


    }
}