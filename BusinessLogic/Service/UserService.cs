using BusinessLogic.Interface;
using DataAccess.Models;
using DataAccess.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public class LoginService : IUserInterface
    {

        private readonly DataAccess.Data.ApplicationDbContext _db;

        public LoginService(DataAccess.Data.ApplicationDbContext db)
        {
            _db = db;
        }



        public bool Login(LoginModel loginModel)
        {

            //Aspnetuser temp1 = _db.Aspnetusers.Where(x => x.Email == loginModel.Email).FirstOrDefault();
            //Aspnetuser temp2 = _db.Aspnetusers.Where(x => x.Passwordhash == loginModel.Password).FirstOrDefault();
            //Aspnetuser temp = _db.Aspnetusers.Where(x => x.Email == loginModel.Email && x.Passwordhash == loginModel.Password).FirstOrDefault();
            return _db.Aspnetusers.Any(x => x.Email == loginModel.Email && x.Passwordhash == loginModel.Password);

        }
    }


}
