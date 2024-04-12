using DataAccess.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interface
{
    public  interface IProviderInterface
    {
        public ProviderDash reqByStatus(int statusId, int reqTypeId, int currentpage, string searchstream, string region, int physicianId);
        public ProviderDash GetRequestStatusCountByPhysician(int physicianId);
        //public ProviderDash GetProviderInfo(int physicianId);
    }
}
