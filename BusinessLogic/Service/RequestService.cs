using BusinessLogic.Interface;
using DataAccess.Models;
using DataAccess.ViewModel;
using System.Net.Mail;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Hosting;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

using System.Net;
using static DataAccess.ViewModel.Profilemodel;
using Aspose.Email;
using MimeKit;
using static BusinessLogic.Service.requestService;
using Org.BouncyCastle.Ocsp;
using Org.BouncyCastle.Asn1.Ocsp;

namespace BusinessLogic.Service
{


    public class requestService : IRequestInterface
    {
        private readonly DataAccess.Data.ApplicationDbContext _db;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        public requestService(DataAccess.Data.ApplicationDbContext db, Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment)
        {
            _db = db;
            _env = Environment;

        }
        public enum RequestType {
            Patient = 1,
            Business = 2,
            Family = 3,
            Concierge = 4

        }
        public enum Status
        {
            New = 1,
            Active = 2,
            Pending = 3,
            Unassigned = 4

        }
        public void PatientInfo(patientReq patientReq)
        {
            Aspnetuser user = _db.Aspnetusers.Where(x => x.Email == patientReq.Email).FirstOrDefault();
            User users=_db.Users.Where(x => x.Email == patientReq.Email).FirstOrDefault();
            Aspnetuser Au = new Aspnetuser();
            User usr = new User();
            DataAccess.Models.Request req = new DataAccess.Models.Request();
            Requestclient rc = new Requestclient();
            Requestwisefile rf = new Requestwisefile();
            
            if (user == null)
            {
                Au.Id = Guid.NewGuid().ToString();
                Au.Email = patientReq.Email;
                Au.Name = patientReq.Firstname + " " + patientReq.Lastname;
                Au.Passwordhash = patientReq.createPassword;
                Au.Phonenumber = patientReq.Phonenumber;
                Au.Createddate = DateTime.Now;

                _db.Aspnetusers.Add(Au);
                _db.SaveChanges();


                usr.Aspnetuserid = Au.Id;

                usr.Firstname = patientReq.Firstname;
                usr.Lastname = patientReq.Lastname;
                usr.Email = patientReq.Email;
                usr.Mobile = patientReq.Phonenumber;
                usr.City = patientReq.City;
                usr.State = patientReq.State;
                usr.Zip = patientReq.Zipcode;
                usr.Createdby = Au.Id;
                usr.Createddate = DateTime.Now.Date;

                _db.Users.Add(usr);
                _db.SaveChanges();


            }



            req.Createddate = DateTime.Now;
            
            req.Isurgentemailsent = new BitArray(1);
            req.Firstname = patientReq.Firstname;
            req.Lastname = patientReq.Lastname;
            req.Phonenumber = patientReq.Phonenumber;
            req.Email = patientReq.Email;
            req.Requesttypeid = (int)RequestType.Patient;
            req.Status = (short)Status.Unassigned;
            if (user != null)
            {
                req.Userid = users.Userid;
                
            }
            _db.Requests.Add(req);
            _db.SaveChanges();

            rc.Requestid = req.Requestid;
            rc.Notes = patientReq.Notes;
            rc.Firstname = patientReq.Firstname;
            rc.Lastname = patientReq.Lastname;
            rc.Phonenumber = patientReq.Phonenumber;
            rc.Email = patientReq.Email;
            rc.Street = patientReq.Street;
            rc.City = patientReq.City;
            rc.State = patientReq.State;
            rc.Zipcode = patientReq.Zipcode;
            rc.Regionid = 1;

            _db.Requestclients.Add(rc);
            _db.SaveChanges();

            if (patientReq.file != null)
            {


                rf.Requestid = req.Requestid;

                rf.Filename = patientReq.file;
                rf.Createddate = DateTime.Now;

                _db.Requestwisefiles.Add(rf);
                _db.SaveChanges();
            }
            


        }
    

        public void familyreq(familyReq familyReq)
        {

            DataAccess.Models.Request req =new DataAccess.Models.Request();  
            Requestclient rc = new Requestclient();
            Requestwisefile rf = new Requestwisefile();


            req.Createddate = DateTime.Now;
            req.Isurgentemailsent = new BitArray(1);
            req.Firstname = familyReq.F_Firstname;
            req.Lastname = familyReq.F_Lastname;
            req.Phonenumber = familyReq.F_Phonenumber;
            req.Email = familyReq.Email;
            req.Requesttypeid = (int)RequestType.Patient;
            req.Status = (short)Status.Unassigned;
            _db.Requests.Add(req);
            _db.SaveChanges();

            rc.Requestid = req.Requestid;
            rc.Notes = familyReq.Notes;
            rc.Firstname = familyReq.Firstname;
            rc.Lastname = familyReq.Lastname;
            rc.Phonenumber = familyReq.Phonenumber;
            rc.Email = familyReq.Email;
            rc.Street = familyReq.Street;
            rc.City = familyReq.City;
            rc.State = familyReq.State;
            rc.Zipcode = familyReq.Zipcode;
            rc.Regionid = 1;

            _db.Requestclients.Add(rc);
            _db.SaveChanges();

            if (familyReq.file != null)
            {


                rf.Requestid = req.Requestid;

                rf.Filename = familyReq.file;
                rf.Createddate = DateTime.Now;

                _db.Requestwisefiles.Add(rf);
                _db.SaveChanges();
            }

           


        }

        public void ConciergeReq(conciergeReq conciergereq)
        {
            
            DataAccess.Models.Request req = new DataAccess.Models.Request();
            Requestclient rc = new Requestclient();


            req.Createddate = DateTime.Now;
            req.Isurgentemailsent = new BitArray(1);
            req.Firstname = conciergereq.cFirstname;
            req.Lastname = conciergereq.cLastname;
            req.Phonenumber = conciergereq.cPhonenumber;
            req.Email = conciergereq.Email;
            req.Requesttypeid = (int)RequestType.Patient;
            req.Status = (short)Status.Unassigned;
            _db.Requests.Add(req);
            _db.SaveChanges();

            rc.Requestid = req.Requestid;
            rc.Notes = conciergereq.Notes;
            rc.Firstname = conciergereq.Firstname;
            rc.Lastname = conciergereq.Lastname;
            rc.Phonenumber = conciergereq.Phonenumber;
            rc.Email = conciergereq.Email;
            
            rc.Regionid = 1;

            _db.Requestclients.Add(rc);
            _db.SaveChanges();

            Concierge concierge = new()
            {
                Conciergename = conciergereq.cFirstname,
                Address = conciergereq.hotelName,
                Street = conciergereq.cStreet,
                City = conciergereq.cCity,
                State = conciergereq.cState,
                Zipcode = conciergereq.cZipcode,
                Createddate = DateTime.Now,
                Regionid = 1
            };

            _db.Concierges.Add(concierge);
            _db.SaveChanges();

            Requestconcierge reqConcierge = new()
            {
                Requestid = req.Requestid,
                Conciergeid = concierge.Conciergeid,
               
            };

            _db.Requestconcierges.Add(reqConcierge);
            _db.SaveChanges();




        }


        public void BusinessReq(businessReq businessReq)
        {
            DataAccess.Models.Request req = new DataAccess.Models.Request();
             Requestclient rc = new Requestclient();
             req.Createddate = DateTime.Now;
            req.Isurgentemailsent = new BitArray(1);
            req.Firstname = businessReq.bFirstname;
            req.Lastname = businessReq.bLastname;
            req.Phonenumber = businessReq.bPhonenumber;
            req.Email = businessReq.Email;
            req.Requesttypeid = (int)RequestType.Business;
            ;req.Status = (short)Status.Unassigned;
            _db.Requests.Add(req);
            _db.SaveChanges();

            rc.Requestid = req.Requestid;
            rc.Notes = businessReq.Notes;
            rc.Firstname = businessReq.Firstname;
            rc.Lastname = businessReq.Lastname;
            rc.Phonenumber = businessReq.Phonenumber;
            rc.Email = businessReq.Email;

            rc.Regionid = 1;

            _db.Requestclients.Add(rc);
            _db.SaveChanges();

            Aspnetuser user=  _db.Aspnetusers.Where(x => x.Email==businessReq.Email).FirstOrDefault();

            Business business = new()
            {
               
                Name = businessReq.businessName,
                Phonenumber = businessReq.bPhonenumber,
                Createddate = DateTime.Now,
                Createdby=user.Id
               
            };

            _db.Businesses.Add(business);
            _db.SaveChanges();



        }
        //public List<dashboardmodel> Getpatientinfo()
        //{
        //    return _db.Requests.Select(x => new dashboardmodel
        //    {
        //        createddate=x.Createddate,
        //        Firstname=x.Firstname

        //    }).ToList();


        //}
        //public List<dashboardmodel> GetPatientInfo()
        //{
        //    var user = _db.Requests.Where(x => x.Requestid == 21).FirstOrDefault();
        //    return new List<dashboardmodel>
        //    {
        //        new dashboardmodel {createddate = user.Createddate , currentStatus = (user.Status == 1 ? "PENDING" : "ACTIVE"),document = "DOC.JPG" },
        //        new dashboardmodel {createddate = DateTime.Now, currentStatus = "pending", document="myname.jpg"},
        //        new dashboardmodel {createddate = DateTime.Now, currentStatus = "active", document="hername.jpg"}
        //    };
        //}


        public void UploadFile(IFormFile file, patientReq patientreq)
        {

            if (file != null)
            {

                var uploadsFolder = Path.Combine(_env.WebRootPath, "Files");
                var filePath = Path.Combine(uploadsFolder, file.FileName);

                // Ensure the uploads directory exists
                //Directory.CreateDirectory(uploadsFolder);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                patientreq.file = file.FileName;
            }

            // Save the file name in the mode
            //
            //
        }
        public void FileUpload(IFormFile file, int id)
        {

            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "Files");
                var filePath = Path.Combine(uploadsFolder, file.FileName);

                // Ensure the uploads directory exists
                Directory.CreateDirectory(uploadsFolder);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                Requestwisefile requestwisefile = new Requestwisefile();
                // Save the file name in the model

                requestwisefile.Requestid =id ;
                requestwisefile.Filename = file.FileName;
                requestwisefile.Createddate = DateTime.Now;
                _db.Requestwisefiles.Add(requestwisefile);
                _db.SaveChanges();
                //ViewBag.Message = "File uploaded successfully!";
            }

        }

        public Dashboardpage ViewDocument(int id)
        {
            //var items1 = _db.Requests.Where(x => x.Userid == id).ToList();
            

            var items = _db.Requestwisefiles.Where(x => x.Requestid == id ).Select(m => new Documentmodel
            {

                uploaddate = m.Createddate,
                uploader = m.Filename
            }).ToList();
            User singleUser = _db.Users.FirstOrDefault(u => u.Userid == id);
            Profilemodel user = new Profilemodel();

            user.FirstName = singleUser.Firstname;
            user.LastName = singleUser.Lastname;
            user.Email = singleUser.Email;
            user.PhoneNumber = singleUser.Mobile;
            user.Street = singleUser.Street;
            user.City = singleUser.City;


            var result = new Dashboardpage()
            {
                models = items,
                Profiles = user


            };
           
            return result;

        }


        public Dashboardpage DisplayDashboard(int Userid)
        {

         //   int id = (int)_httpContextAccessor.HttpContext.Session.GetInt32("id");

            var items1 = _db.Requests.Include(x => x.Requestwisefiles).Where(x => x.Userid == Userid).Select(x => new Dashboardmodel
            {
                createddate = x.Createddate,
                Status = (status)x.Status,
                id = x.Requestid,
                Fcount = x.Requestwisefiles.Count()

            }).ToList();
            User singleUser = _db.Users.FirstOrDefault(u => u.Userid == Userid);
            Profilemodel user = new Profilemodel();

            user.FirstName = singleUser.Firstname;
            user.LastName = singleUser.Lastname;
            user.Email = singleUser.Email;
            user.PhoneNumber = singleUser.Mobile;
            user.Street = singleUser.Street;
            user.City = singleUser.City;
                

            var result = new Dashboardpage()
            {
                Dashboard = items1,
                Profiles = user


            };

            return result;

        }



        /*   public void UserData(Profilemodel pm, int id)
           {


               User existuser = _db.Users.FirstOrDefault(u => u.Userid == id);
               if (existuser != null)
               {
                   existuser.Firstname = pm.FirstName;
                   existuser.Lastname = pm.LastName;
                   existuser.Email = pm.Email;
                   existuser.State = pm.State;
                //   existuser.Street = profilemodel.Street
                  // existuser.Zip = profilemodel.Zipcode;
   //_db.Update(existuser)

               }
               _db.SaveChanges();

           }*/

        
        public void SaveProfileData(Profilemodel model, int id)
        {
            var user = _db.Users.FirstOrDefault(x => x.Userid == id);

            if (user != null)
            {
                user.Firstname = model.FirstName;
                user.Lastname = model.LastName;
                //DOB
                user.Mobile = model.PhoneNumber;
                // user.Email = model.Email;
                user.Street = model.Street;
                user.City = model.City;
                user.State = model.State;
                user.Zip = model.Zipcode;
                user.Modifieddate = DateTime.Now;
                    
            }
            _db.SaveChanges();
        }
        public patientReq Information(patientReq patientreq,int id)
        {
            User singleUser = _db.Users.FirstOrDefault(u => u.Userid ==id );
            patientReq user = new patientReq();

            user.Firstname = singleUser.Firstname;
            user.Lastname = singleUser.Lastname;
            user.Email = singleUser.Email;
            user.Phonenumber = singleUser.Mobile;
            user.Street = singleUser.Street;
            user.City = singleUser.City;

            return user;

        }


        public void Someoneelse(patientReq model, int id)
        {

            {

                DataAccess.Models.Request request = new DataAccess.Models.Request();
                Requestclient requestclient = new Requestclient();

                var Year = model.DOB.Year;
                var Month = model.DOB.Month.ToString();
                var Date = model.DOB.Day;


                var user = _db.Users.FirstOrDefault(x => x.Userid == id);



                if (user != null)
                {

                    request.Requesttypeid = 1;
                    request.Status = 2;
                    request.Userid = user.Userid;
                    request.Createddate = DateTime.Now;
                    request.Isurgentemailsent = new BitArray(new bool[1]);
                    request.Firstname = user.Firstname;
                    request.Lastname = user.Lastname;
                    request.Phonenumber = user.Mobile;
                    request.Email = user.Email;
                    request.Relationname = model.relation;



                    _db.Requests.Add(request);
                    _db.SaveChanges();

                }

                requestclient.Requestid = request.Requestid;
                requestclient.Regionid = 1;
                requestclient.Firstname = model.Firstname;
                requestclient.Lastname = model.Lastname;
                requestclient.Phonenumber = model.Phonenumber;
                requestclient.Address = model.Street + ' ' + model.City + ' ' + model.State;
                requestclient.Email = model.Email;
                requestclient.Notes = model.Notes;
                requestclient.Intyear = Year;
                requestclient.Strmonth = Month;
                requestclient.Intdate = Date;
                requestclient.Street = model.Street;
                requestclient.State = model.State;
                requestclient.City = model.City;
                requestclient.Zipcode = model.Zipcode;

                _db.Requestclients.Add(requestclient);
                _db.SaveChanges();

                //if (File != null)
                //{
                //    var requestWiseFile = new Requestwisefile
                //    {
                //        Requestid = request.Requestid,    
                //        Filename = file,
                //        Createddate = DateTime.Now,
                //        Ispatientrecords = new BitArray(new bool[1]),
                //    };

                //    _db.Requestwisefiles.Add(requestWiseFile);
                //    _db.SaveChanges();
                //}
               // return true;
            }

        }





        //public void sendmail(string to, string body)
        //{
        //    var smtpClient = new SmtpClient("mail.etatvasoft.com")
        //    {
        //        Port = 587,
        //        Credentials = new NetworkCredential("vanshita.bhansali@etatvasoft.com", "Vans@2211"),
        //        EnableSsl = true,
        //    };
        //    var mailMessage = new System.Net.Mail.MailMessage
        //    {
        //        From = new System.Net.Mail.MailAddress("vanshita.bhansali@etatvasoft.com"),

        //        Body = body,
        //        IsBodyHtml = false,
        //    };

        //    mailMessage.To.Add(to);

        //    smtpClient.Send(mailMessage);

        //}



        //public void sendmail(string to, string body)
        //{
        //    var emailtosend = new MimeMessage();
        //    emailtosend.from.add(MailboxAddress.Parse("vanshita.bhansali@etatvasoft.com"));
        //    emailtosend.to.add(mailboxaddress.parse(to));
        //    emailtosend.body = new textpart(mimekit.text.textformat.html) { text = htmlmessage };
        //    using (var emailclient = new smtpclient())
        //    {

        //    }

        //}

        public void mail(string email) {

            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

            message.From = new System.Net.Mail.MailAddress("vanshita.bhansali@etatvasoft.com");
            message.To.Add(new System.Net.Mail.MailAddress(email));
            message.Subject = "Create Account";
            message.IsBodyHtml = true;
            message.Body = "Request for you is generated by your family member or friend. To check your request status click on below link to generate account. <a href=\"https://localhost:7011/Patient_Site/Patient_Login\">ClickHere</a>";
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "mail.etatvasoft.com";
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential("vanshita.bhansali@etatvasoft.com", "GEtTj-2V%=0u");   
            smtp.EnableSsl = true;
            smtp.Send(message);
            smtp.UseDefaultCredentials = false;
        }

}

} 

    