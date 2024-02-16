using BusinessLogic.Interface;
using DataAccess.Models;
using DataAccess.ViewModel;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Presentation;
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



        public int Login(LoginModel loginModel)
        {
            int var=0;

           Aspnetuser user = _db.Aspnetusers.Where(x => x.Email == loginModel.Email).FirstOrDefault();
            if(user == null)
            {
                var=1;
                return var;
            }
            if(user.Email != loginModel.Email)
            {
                var = 2;
            }
            if(user.Email==loginModel.Email)
            {
                if(user.Passwordhash!=loginModel.Password)
                var=3;
            }

             if(user.Email==loginModel.Email && user.Passwordhash==loginModel.Password) 
                
            {
                var=4;
            }

            return var;
          
           // if(_db.Aspnetusers.Where(x=> x.Email == loginModel.Email)).FirstOrDefault())
                    






            //Aspnetuser temp1 = _db.Aspnetusers.Where(x => x.Email == loginModel.Email).FirstOrDefault();
            //Aspnetuser temp2 = _db.Aspnetusers.Where(x => x.Passwordhash == loginModel.Password).FirstOrDefault();
            //Aspnetuser temp = _db.Aspnetusers.Where(x => x.Email == loginModel.Email && x.Passwordhash == loginModel.Password).FirstOrDefault();
          //  return _db.Aspnetusers.Any(x => x.Email == loginModel.Email && x.Passwordhash == loginModel.Password);

        }

    }


}
