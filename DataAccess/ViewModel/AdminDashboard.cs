using DataAccess.Models;
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
        public int requestid { get; set; }
        public RequestType Status { get; set; }  



    }
    public class BlockReq
    {
        public string Blockreason { get; set; }
        
    }
    public class AssignReq
    {
        public string Description { get; set; }
        public string physicianname { get; set; }

    }
    public class CancelReq
    {
        public string reason
        {
            get; set;
        }
        public string description
        {
            get; set;
        }
    }

    public class AdminDashboard
    {
        public IEnumerable<Casetag> Caserequest { get; set; }
        public IEnumerable<Physician> Physicians { get; set; }
        public List<AdminDash> Dashboards { get; set; }
        public BlockReq blockreq { get; set;}
        public CancelReq cancelreq { get; set; }    
        public AssignReq assignreq { get; set; }    
        public IEnumerable<Region> Regions {get; set;}
        public int newcount { get; set; }
        public int pendingcount { get; set; }
        public int activecount { get; set; }
        public int toclosecount { get; set; }
        public int concludecount { get; set; }
        public int unpaidcount { get; set; }
       public status Status { get; set; }
        public int requestid { get; set; }
        public string patientname { get;set; }

    }
}