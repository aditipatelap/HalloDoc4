using DataAccess.Models;
using DataAccess.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interface
{
    public interface ILoginInterface
    {

        public LoginModel Login(LoginModel loginModel);
        public bool checkEmail(LoginModel model);
        public bool sendEmail(string email,string aspnetuserid);
        public void ResetPassSave(LoginModel model);
        public bool CreateAccount(LoginModel viewPatientReq,string email);

    }
}
