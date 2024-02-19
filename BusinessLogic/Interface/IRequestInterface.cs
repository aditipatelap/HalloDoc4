using DataAccess.ViewModel;

using Microsoft.AspNetCore.Http;
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

        // List<dashboardmodel> GetPatientInfo();

        public void UploadFile(IFormFile file,patientReq patientreq);

        public List<Documentmodel> ViewDocument(int id);
        public Dashboardpage DisplayDashboard( int id);
        public void UserData(Profilemodel profilemodel, int id);

       //* public List<Dashboardmodel> FetchUserProfile();

    }
}
