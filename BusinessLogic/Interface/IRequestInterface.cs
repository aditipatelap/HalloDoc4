using DataAccess.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interface
{
   public interface IRequestInterface
    {
        public void PatientInfo(patientReq patientReq);
        public void BusinessReq(businessReq businessReq);

    }
}
