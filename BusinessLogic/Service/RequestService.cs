using BusinessLogic.Interface;
using DataAccess.Models;
using DataAccess.ViewModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{ 

   
        public class requestService : IRequestInterface
        {
        private readonly DataAccess.Data.ApplicationDbContext _db;
        public requestService(DataAccess.Data.ApplicationDbContext db)
        {
            _db = db;
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

            Requestclient rc = new Requestclient();
            rc.Requestid = 1;
            rc.Notes = patientReq.Notes;
            rc.Firstname = patientReq.Firstname;
            rc.Lastname = patientReq.Lastname;
            rc.Phonenumber = patientReq.Phonenumber;
            rc.Email = patientReq.Email;
            rc.Street = patientReq.Street;
            rc.City = patientReq.City;
            rc.State = patientReq.State;
            rc.Zipcode = patientReq.Zipcode;

            _db.Requestclients.Add(rc);
            _db.SaveChanges();
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



    }
    
}
