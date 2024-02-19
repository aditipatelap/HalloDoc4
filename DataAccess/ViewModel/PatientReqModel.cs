using DataAccess.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModel
{
    public class patientReq
    { //Request
        [Required(ErrorMessage = "Please enter Your Firstname")]
        public string? Firstname { get; set; }
        [Required(ErrorMessage = "Please enter Your Lastname.")]
        public string? Lastname { get; set; }
        [Required(ErrorMessage = "Please enter Your Phonenumber.")]
        public string? Phonenumber { get; set; }
        [Required(ErrorMessage = "Please enter Your Email Address.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Please enter Your DOB.")]
        public string? DOB { get; set; }
        public string? Notes { get; set; }
        [Required(ErrorMessage = "Please enter Your Street.")]
        public string? Street { get; set; }
        [Required(ErrorMessage = "Please enter Your City.")]
        public string? City { get; set; }
        [Required(ErrorMessage = "Please enter Your State.")]
        public string? State { get; set; }
        [Required(ErrorMessage = "Please enter Your Zipcode.")]
        public string? Zipcode { get; set; }
        [Required(ErrorMessage = "Please enter Your RoomNo.")]
        public string? RoomNo { get; set; }
        public string? File { get; set; }
        public string? createPassword { get; set; }

        public string? confirmPassword { get; set; }
       


    }

    public class familyReq {
        [Required(ErrorMessage = "Please enter Your Firstname")]
        public string? F_Firstname { get; set; }
        [Required(ErrorMessage = "Please enter Your Lastname.")]
        public string? F_Lastname { get; set; }
        [Required(ErrorMessage = "Please enter Your Phonenumber.")]
        public string? F_Phonenumber { get; set; }
        [Required(ErrorMessage = "Please enter Your Email Address.")]
        public string? F_Email { get; set; }
        [Required(ErrorMessage = "Please enter Your Relation with Patient.")]
        public string? Relation { get; set; }

        [Required(ErrorMessage = "Please enter patient firstname")]
        public string? Firstname { get; set; }
        [Required(ErrorMessage = "Please enter patient lastname.")]
        public string? Lastname { get; set; }
        [Required(ErrorMessage = "Please enter patient phonenumber.")]
        public string? Phonenumber { get; set; }
        [Required(ErrorMessage = "Please enter patient email address.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Please enter patient DOB.")]
        public string? DOB { get; set; }
        public string? Notes { get; set; }
        [Required(ErrorMessage = "Please enter patient Street.")]
        public string? Street { get; set; }
        [Required(ErrorMessage = "Please enter patient City.")]
        public string? City { get; set; }
        [Required(ErrorMessage = "Please enter patient State.")]
        public string? State { get; set; }
        [Required(ErrorMessage = "Please enter patient Zipcode.")]
        public string? Zipcode { get; set; }
        public string? RoomNo { get; set; }
        [Required(ErrorMessage = "Please enter patient Country.")]
        public string? Country { get; set; }

    }

    public class conciergeReq
    {
        [Required(ErrorMessage = "Please enter Your Firstname")]
        public string? cFirstname { get; set; }
        [Required(ErrorMessage = "Please enter Your Lastname.")]
        public string? cLastname { get; set; }
        [Required(ErrorMessage = "Please enter Your Phonenumber.")]
        public string? cPhonenumber { get; set; }
        [Required(ErrorMessage = "Please enter Your Email Address.")]
        public string? cEmail { get; set; }
        [Required(ErrorMessage = "Please enter Your Hotel/Property Name.")]
        public string? hotelName{ get; set; }
        [Required(ErrorMessage = "Please enter Your Street.")]
        public string? cStreet { get; set; }
        [Required(ErrorMessage = "Please enter Your City.")]
        public string? cCity { get; set; }
        [Required(ErrorMessage = "Please enter Your State.")]
        public string? cState { get; set; }
        [Required(ErrorMessage = "Please enter Your Zipcode.")]
        public string? cZipcode { get; set; }

        [Required(ErrorMessage = "Please enter Patient Firstname")]
        public string? Firstname { get; set; }
        [Required(ErrorMessage = "Please enter Patient Lastname.")]
        public string? Lastname { get; set; }
        [Required(ErrorMessage = "Please enter Patient Phonenumber.")]
        public string? Phonenumber { get; set; }
        [Required(ErrorMessage = "Please enter patient Email Address.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Please enter patient DOB.")]
        public string? DOB { get; set; }
        public string? Notes { get; set; }
        public string? pRoomNo { get; set; }
    }
    public class businessReq
    {
        [Required(ErrorMessage = "Please enter Your Firstname")]
        public string? bFirstname { get; set; }
        [Required(ErrorMessage = "Please enter Your Lastname.")]
        public string? bLastname { get; set; }
        [Required(ErrorMessage = "Please enter Your Phonenumber.")]
        public string? bPhonenumber { get; set; }
        [Required(ErrorMessage = "Please enter Your Email Address.")]
        public string? bEmail { get; set; }
        [Required(ErrorMessage = "Please enter Your Business/Property Name")]
        public string? businessName { get; set; }
        public string? caseNo { get; set; }

        [Required(ErrorMessage = "Please enter patient firstname")]
        public string? Firstname { get; set; }
        [Required(ErrorMessage = "Please enter patient lastname.")]
        public string? Lastname { get; set; }
        [Required(ErrorMessage = "Please enter patient phonenumber.")]
        public string? Phonenumber { get; set; }
        [Required(ErrorMessage = "Please enter patient email address.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Please enter patient DOB.")]
        public string? DOB { get; set; }
        public string? Notes { get; set; }
        [Required(ErrorMessage = "Please enter patient Street.")]
        public string? Street { get; set; }
        [Required(ErrorMessage = "Please enter patient City.")]
        public string? City { get; set; }
        [Required(ErrorMessage = "Please enter patient State.")]
        public string? State { get; set; }
        [Required(ErrorMessage = "Please enter patient Zipcode.")]
        public string? Zipcode { get; set; }
        public string? RoomNo { get; set; }
        public string? Country { get; set; }
        public string? File { get; set; }
       
    }

    public enum status
    {
        Active = 1,
        Pending = 2
    }


    public class Dashboardmodel
    {
        public DateTime createddate { get; set; }
        public status Status { get; set; }  

        public int id { get; set; }

        
        
    }
    public class Profilemodel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
    }

    //public class Patientmodel
    //{
    //    public string Firstname { get; set; }
    //}
    public class Dashboardpage
    {
        public List<Dashboardmodel> Dashboard { get; set; }
        public Profilemodel Profiles { get; set; }
    }
    //public class dashboardmodel2
    //{
    //    public List<dashboardmodel> dashboards { get; set; }
    //}
    public class Documentmodel
    {
        public string uploader
        {
            get; set;
        }
        public DateTime uploaddate { get; set; }
        public string Firstname { get; set; }
        public int id { get; set; }


    }
    //public class MyViewModel
    //{
    //    public List<Dashboardmodel> item { get; set; }
    //    public List<Dashboardmodel>
    //}

}
