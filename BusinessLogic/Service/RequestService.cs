using BusinessLogic.Interface;
using DataAccess.Models;
using DataAccess.ViewModel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace BusinessLogic.Service
{


    public class requestService : IRequestInterface
    {
        private readonly DataAccess.Data.ApplicationDbContext _db;
        private readonly IHostingEnvironment _env;
        public requestService(DataAccess.Data.ApplicationDbContext db, IHostingEnvironment Environment)
        {
            _db = db;
            _env = Environment;

        }
        public void PatientInfo(patientReq patientReq)
        {
            Request req = new Request();
            req.Requesttypeid = 1;
            req.Status = 1;
            req.Createddate = DateTime.Now;
            req.Isurgentemailsent = new BitArray(1);
            req.Firstname = patientReq.Firstname;
            req.Lastname = patientReq.Lastname;
            req.Phonenumber = patientReq.Phonenumber;
            req.Email = patientReq.Email;


            _db.Requests.Add(req);
            _db.SaveChanges();
            Requestwisefile reqfile = new Requestwisefile();
            reqfile.Requestid = 21;
            reqfile.Createddate = DateTime.Now;
            reqfile.Filename = patientReq.File;
            _db.Requestwisefiles.Add(reqfile);
            _db.SaveChanges();


            //_db.Requests.Add(req);
            //_db.SaveChanges();
            //Requestclient rc = new Requestclient();
            //rc.Requestid = 1;
            //rc.Notes = patientReq.Notes;
            //rc.Firstname = patientReq.Firstname;
            //rc.Lastname = patientReq.Lastname;
            //rc.Phonenumber = patientReq.Phonenumber;
            //rc.Email = patientReq.Email;
            //rc.Street = patientReq.Street;

            //rc.City = patientReq.City;
            //rc.State = patientReq.State;
            //rc.Zipcode = patientReq.Zipcode;

            //_db.Requestclients.Add(rc);
            //_db.SaveChanges();
        }
        public void BusinessReq(businessReq businessReq)
        {
            Request req = new Request();
            req.Requesttypeid = 1;
            req.Status = 1;
            req.Createddate = DateTime.Now;
            req.Isurgentemailsent = new BitArray(1);
            req.Firstname = businessReq.bFirstname;
            req.Lastname = businessReq.bLastname;
            req.Phonenumber = businessReq.bPhonenumber;
            req.Email = businessReq.bEmail;

            _db.Requests.Add(req);
            _db.SaveChanges();

            Requestclient rc = new Requestclient();
            rc.Requestid = 1;
            rc.Notes = businessReq.Notes;
            rc.Firstname = businessReq.Firstname;
            rc.Lastname = businessReq.Lastname;
            rc.Phonenumber = businessReq.Phonenumber;
            rc.Email = businessReq.Email;
            rc.Street = businessReq.Street;
            rc.City = businessReq.City;
            rc.State = businessReq.State;
            rc.Zipcode = businessReq.Zipcode;

            _db.Requestclients.Add(rc);
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



            var uploadsFolder = Path.Combine(_env.WebRootPath, "Files");
            var filePath = Path.Combine(uploadsFolder, file.FileName);

            // Ensure the uploads directory exists
            //Directory.CreateDirectory(uploadsFolder);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            patientreq.File = file.FileName;


            // Save the file name in the mode
            //
            //
        }

        public List<Documentmodel> ViewDocument()
        {
            var items = _db.Requestwisefiles.Select(m => new Documentmodel
            {
                uploaddate = m.Createddate,
                uploader = m.Filename
            }).ToList();

            return items;

        }
        public Dashboardpage DisplayDashboard()
        {
            
            var items1= _db.Requests.Select(item => new Dashboardmodel
            {
                createddate = item.Createddate,
                Firstname = item.Firstname,
                Lastname = item.Lastname
            }).ToList();
           
            User singleUser= _db.Users.FirstOrDefault(u=> u.Userid==3);
            string usr = singleUser.Firstname;

            var result = new Dashboardpage
            {
                Dashboard = items1,
                SingleUser = usr


            };

            return result;

        }
       


        //public List<Dashboardmodel> FetchUserProfile()
        //{
        //    var items = _db.Users.Select(m => new Dashboardmodel
        //    {

        //        Firstname = m.Firstname
        //    }).ToList();

        //    return items;
        //}

    }

} 

