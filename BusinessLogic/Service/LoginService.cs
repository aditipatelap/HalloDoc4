﻿using BusinessLogic.Interface;
using DataAccess.Models;
using DataAccess.ViewModel;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Net.Mail;
using System.Net;
using Twilio.Http;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Authorization.Infrastructure;

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
        //public Aspnetuser Login(LoginModel loginModel)
        //{
        //    Aspnetuser user = new Aspnetuser();
        //    user = _db.Aspnetusers.Include(x => x.Aspnetuserroles).FirstOrDefault(u => u.Email == loginModel.Email);

        //    if (user.Passwordhash == loginModel.Password)
        //    {
        //        return user;
        //    }



        //    return null;
        //}
        public LoginModel Login(LoginModel loginModel)
        {
            var user = _db.Aspnetusers.Include(x => x.Aspnetuserroles).FirstOrDefault(u => u.Email == loginModel.Email);
            if (user != null)
            {
                var roleid = user.Aspnetuserroles.FirstOrDefault().Roleid;
                var data = new LoginModel
                {
                    Email = user.Email,
                    Password = user.Passwordhash,
                    AspNetUserId = user.Id,
                    roleid = roleid,
                    UserName = user.Name,
                };
                if (roleid == 1)
                {
                    var admin = _db.Admins.Where(x => x.Aspnetuserid == user.Id).FirstOrDefault();
                    data.AdminId = admin.Adminid;
                    data.SubRoleId = admin.Roleid;
                }
                if (roleid == 2)
                {
                    var physician = _db.Physicians.Where(x => x.Aspnetuserid == user.Id).FirstOrDefault();
                    data.PhysicianId = physician.Physicianid;
                    data.SubRoleId = physician.Roleid;
                }
                //if (roleid == 3)
                //{
                //    var users = _db.Users.Where(x => x.Aspnetuserid == user.Id).FirstOrDefault();
                //    data.UserId = users.Userid;
                //    //data.SubRoleId = users.Roleid;
                //}
                return data;
            }
            return null;
        }
        
        public bool sendEmail(string email,string aspnetuserid)
        {
            
            var link = $"https://localhost:44367/Login/ResetPassword?aspnetuserid={aspnetuserid}";

            MailMessage message = new MailMessage();

            message.From = new System.Net.Mail.MailAddress("vanshita.bhansali@etatvasoft.com");
            message.To.Add(new MailAddress(email));
            message.Subject = "Reset you Password";
            message.IsBodyHtml = true;

            message.Body = $"To reset you password click on link below. <a href=\"{link}\">ClickHere</a>";
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "mail.etatvasoft.com";
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential("vanshita.bhansali@etatvasoft.com", "GEtTj-2V%=0u");
            smtp.EnableSsl = true;
            smtp.Send(message);
            smtp.UseDefaultCredentials = false;

            return true;
        }
        public bool checkEmail(LoginModel model)
        {
            var re = _db.Aspnetusers.FirstOrDefault(x => x.Email == model.Email).Email;
            if (re != null)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
        public void ResetPassSave(LoginModel model)
        {
            Aspnetuser aspnetuser = new Aspnetuser();
            var res = _db.Aspnetusers.FirstOrDefault(x => x.Email == model.Email);
            if (res != null)
            {
                res.Passwordhash=model.Password;
                _db.SaveChanges();
            }
        }
        public bool CreateAccount(LoginModel viewPatientReq,string email)
        {
            var isexist = _db.Users.Any(req => req.Email == viewPatientReq.Email);
            if (isexist)
            {
                return false;
            }
            var Aspnetuser = new Aspnetuser();
            var role = new Aspnetuserrole();
            var User = new User();
            var Request = new DataAccess.Models.Request();
            var Requestclient = new Requestclient();
            var U = _db.Requestclients.FirstOrDefault(m => m.Email == viewPatientReq.Email);
            Guid g = Guid.NewGuid();
            Aspnetuser.Id = g.ToString();
            Aspnetuser.Name = U.Firstname;
            Aspnetuser.Passwordhash = viewPatientReq.Password;
            Aspnetuser.Email = viewPatientReq.Email;
            Aspnetuser.Phonenumber = U.Phonenumber;
            Aspnetuser.Createddate = DateTime.Now;
            _db.Aspnetusers.Add(Aspnetuser);
            _db.SaveChanges();
            role.Userid = Aspnetuser.Id;
            role.Roleid = 3; //For Patient Role
            _db.Aspnetuserroles.Add(role);
            _db.SaveChanges();
            var regionAbbreviation = "";
            var regionInfo = _db.Regions
                           .Where(x => x.Name == U.State)
                           .Select(x => new { x.Abbreviation, x.Regionid })
                           .FirstOrDefault();


            if (regionInfo != null)
            {
                regionAbbreviation = regionInfo.Abbreviation;
                User.Regionid = regionInfo.Regionid;
            }
            User.Aspnetuserid = Aspnetuser.Id;
            User.Firstname = U.Firstname;
            User.Lastname = U.Lastname;
            User.Email = viewPatientReq.Email;
            User.Mobile = U.Phonenumber;
            User.Street = U.Street;
            User.City = U.City;
            User.State = U.State;
            User.Zip = U.Zipcode;
            User.Strmonth = U.Strmonth;
            User.Intdate = U.Intdate;
            User.Intyear = U.Intyear;
            User.Status = 1; //for new request
            User.Createdby = Aspnetuser.Id;
            User.Createddate = DateTime.Now;
           

            _db.Users.Add(User);
            _db.SaveChanges();

            var res = (from req in _db.Requests
                       join rc in _db.Requestclients
                       on req.Requestid equals rc.Requestid
                       where rc.Email == viewPatientReq.Email
                       select req.Requestid).ToList();

            foreach (var r in res)
            {
                var req = _db.Requests.FirstOrDefault(req => req.Requestid == r);
                req.Userid = User.Userid;
                _db.Requests.Update(req);
                _db.SaveChanges();
            }
            return true;
            
        }





    }

}



