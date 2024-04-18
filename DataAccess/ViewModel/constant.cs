    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModel
{
    public class Constant
    {
        public enum Month
        {
            January=1,
                February=2,
                March=3,
                April=4,
                May=5,
                June=6, July=7, 
                August=8,
                  September=9,
                  October=10,Novermber=11,
                  December=12,

        }
        public enum Status
        {
            New=1,
                Pending=2,
                Active=3,
                    Conclude=4,
                    ToClose=5,
                    Unpaid=6
        }
        public enum PhysicianStatus
        {
            Active=1,
            Pending=2,
            NotActive=3
        }
        public enum Requeststatuses
        {
            Unassigned=1,
            Accepted=2,
            Cancelled=3,
            MDEnRoute=4,
            MDonSite=5,
                Conclude=6,
                Cancelledbypatient=7,
                Closed=8,
                Unpaid=9,
            Clear=10

        }
        public enum RequestType
        {
            Patient = 1,
            Business = 2,
            Family = 3,
            Concierge = 4

        }
        public enum Roles
        {
            Admin=1,
            Provider = 2,
            Patient =3,
            
        }

    }
}
