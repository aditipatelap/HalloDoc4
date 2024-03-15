using BusinessLogic.Interface;
using DataAccess.Models;
using DataAccess.ViewModel;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Service
{
    public class LoginService : ILoginInterface
    {

        private readonly DataAccess.Data.ApplicationDbContext _db;

        public LoginService(DataAccess.Data.ApplicationDbContext db)
        {
            _db = db;
        }
        //public int Login(LoginModel loginModel)
        //{
        //    int var = 0;

        //    Aspnetuser user = _db.Aspnetusers.Where(x => x.Email == loginModel.Email).FirstOrDefault();
        //    if (user == null)
        //    {
        //        var = 1;
        //        return var;
        //    }
        //    if (user.Email != loginModel.Email)
        //    {
        //        var = 2;
        //    }
        //    if (user.Email == loginModel.Email)
        //    {
        //        if (user.Passwordhash != loginModel.Password)
        //            var = 3;
        //    }

        //    if (user.Email == loginModel.Email && user.Passwordhash == loginModel.Password)

        //    {
        //        var = 4;
        //    }

        //    return var;

            // if(_db.Aspnetusers.Where(x=> x.Email == loginModel.Email)).FirstOrDefault())







            //Aspnetuser temp1 = _db.Aspnetusers.Where(x => x.Email == loginModel.Email).FirstOrDefault();
            //Aspnetuser temp2 = _db.Aspnetusers.Where(x => x.Passwordhash == loginModel.Password).FirstOrDefault();
            //Aspnetuser temp = _db.Aspnetusers.Where(x => x.Email == loginModel.Email && x.Passwordhash == loginModel.Password).FirstOrDefault();
            //  return _db.Aspnetusers.Any(x => x.Email == loginModel.Email && x.Passwordhash == loginModel.Password);

        //}

        //public Aspnetuser PatientLogin(LoginModel loginModel)
        //{
        //    var user = _db.Aspnetusers.Include(x => x.Aspnetuserroles).FirstOrDefault(u => u.Email == loginModel.Email);

        //    if (user.Passwordhash == loginModel.Password)
        //    { 
        //        if (_db.Users.Any(u => u.Aspnetuserid == user.Id))
        //        {
        //            id = _db.Users.FirstOrDefault(u => u.Aspnetuserid == user.Id).Userid;
        //        }
               
        //        return user;

        //    if (user == null)
        //    {
        //        _notyf.Custom("Invalid Email", 3, "red", "bi bi-x-circle-fill");
        //        return View();
        //    }
        //}
        //public Aspnetuser Login(LoginModel loginModel)
        //{
        //    var user = _db.Aspnetusers.Include(x => x.Aspnetuserroles).FirstOrDefault(u => u.Email == loginModel.Email && u.Passwordhash == loginModel.Password);
           
        //    if (user.Passwordhash == loginModel.Password)
        //    {

        //        return user;
        //    }

        //}
        public Aspnetuser Login(LoginModel loginModel)
        {
            Aspnetuser user = new Aspnetuser();
            user = _db.Aspnetusers.Include(x => x.Aspnetuserroles).FirstOrDefault(u => u.Email == loginModel.Email);
           
                if (user.Passwordhash == loginModel.Password)
                {
                return user;
                }
            
            return null;
        }
    }

    }



