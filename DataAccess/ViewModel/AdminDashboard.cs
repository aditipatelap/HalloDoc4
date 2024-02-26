using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModel
{
    public class AdminDashboard
    {
        public string ? Name { get; set; }  
        public string ? Dob { get; set; }
        public string? Requestor { get; set; }
        public string? Physician { get; set; }
        public DateTime? DateofService { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Notes { get; set; } 
        public string? Chat { get; set;}
    }
}
