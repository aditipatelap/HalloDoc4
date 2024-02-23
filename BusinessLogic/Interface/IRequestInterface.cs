using DataAccess.Models;
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
        void PatientInfo(patientReq patientReq);
        public void BusinessReq(businessReq businessReq);
        public void familyreq(familyReq familyReq);
        public void ConciergeReq(conciergeReq conciergereq);

       

        public void UploadFile(IFormFile file,patientReq patientreq);

        public Dashboardpage ViewDocument(int userid, int id);
        public Dashboardpage DisplayDashboard( int id);
        //public void UserData(Profilemodel pm, int id);
        public void SaveProfileData(Profilemodel model, int id);
        public void FileUpload( IFormFile file, int id);
        public patientReq Information(patientReq patientreq, int id);
        public void Someoneelse(patientReq model, int id);

      public void mail(string email);

        //* public List<Dashboardmodel> FetchUserProfile();

    }
}
