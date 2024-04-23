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

        //  [RegularExpression("^[0-9]{10}$", ErrorMessage = "Phone number must be of 10 digit")]
        //// [StringLength(10, ErrorMessage = "Phone number must be of 10 digit") ]
        // [Required(ErrorMessage = "Please enter Your Phonenumber.")]
        [Required(ErrorMessage = "number is required")]
        [RegularExpression(@"^\+\d{1,3}\d{10}$", ErrorMessage = "Phone number must be in the format +[Country Code][10 Digits].")]
        public string? Phonenumber { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "BirthDate is required.")]
        public DateTime DOB { get; set; }
        public string? Notes { get; set; }
       
        public string ? Street { get; set; }
   
        public string ? City { get; set; }
    
        public string  State { get; set; }
        public string Region { get; set; }
        public string Address { get; set; }

        public IEnumerable<Region> Regions { get; set; }
        [RegularExpression(@"^\d{6}$", ErrorMessage = "ZipCode must be exactly 6 digits.")]
        [Required(ErrorMessage = "Please enter ZipCode.")]
        public string ? Zipcode { get; set; }
     
        public string? RoomNo { get; set; }
        public string file { get; set; }
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Password must contain at least one uppercase letter and one digit")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 20 characters long")]
        [Required]
        public string? createPassword { get; set; }
        public string Status { get; set; }
        [Required]
        [Compare("createPassword", ErrorMessage = "Password and Confirmation Password must match.")]
        public string? confirmPassword { get; set; }
       public string relation { get; set; }
        public RequestType Requesttypeid { get; set; }


    }
    public class patientmailmodel
    {
        public string email { get; set; }
    }

    public class familyReq {
        [Required(ErrorMessage = "Please enter Your Firstname")]
        public string? F_Firstname { get; set; }
        public IEnumerable<Region> Regions { get; set; }
        [Required(ErrorMessage = "The field is required")]
        public string? F_Lastname { get; set; }
        [Required(ErrorMessage = "BirthDate is required.")]

        public DateTime DOB { get; set; }
        [Required(ErrorMessage = "number is required")]
        [RegularExpression(@"^\+\d{1,3}\d{10}$", ErrorMessage = "Phone number must be in the format +[Country Code][10 Digits].")]

        public string? F_Phonenumber { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address.")]


        public string? F_Email { get; set; }
        [Required(ErrorMessage = "Please enter Your Relation with Patient.")]
        public string? Relation { get; set; }

        [Required(ErrorMessage = "Please enter patient firstname")]
        public string? Firstname { get; set; }
        [Required(ErrorMessage = "Please enter patient lastname.")]
        public string? Lastname { get; set; }
        [Required(ErrorMessage = "number is required")]
        [RegularExpression(@"^\+\d{1,3}\d{10}$", ErrorMessage = "Phone number must be in the format +[Country Code][10 Digits].")]

        public string? Phonenumber { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }
    
        public string? Notes { get; set; }
 
        public string? Street { get; set; }
    
        public string? City { get; set; }
        [Required(ErrorMessage = "The field is required")]


        public string State { get; set; }
        [Required(ErrorMessage = "The field is required")]

        public string? Zipcode { get; set; }
        public string? RoomNo { get; set; }
       
        public string? Country { get; set; }
        public string ? file{ get; set; }

    }

    public class conciergeReq
    {
        [Required(ErrorMessage = "Please enter Your Firstname")]
        public string? cFirstname { get; set; }
        public IEnumerable<Region> Regions { get; set; }
        [Required(ErrorMessage = "The field is required")]
        public string? cLastname { get; set; }
        [Required(ErrorMessage = "number is required")]
        [RegularExpression(@"^\+\d{1,3}\d{10}$", ErrorMessage = "Phone number must be in the format +[Country Code][10 Digits].")]

        public string? cPhonenumber { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address.")]

        public string? cEmail { get; set; }
        [Required(ErrorMessage = "Please enter Your Hotel/Property Name.")]
        public string? hotelName{ get; set; }
        [Required(ErrorMessage = "Please enter Your Street.")]
        public string? cStreet { get; set; }
        [Required(ErrorMessage = "Please enter Your City:")]
        public string? cCity { get; set; }
        [Required(ErrorMessage = "Please enter Your State.")]
        public string cState { get; set; }
        [Required(ErrorMessage = "Please enter Your ZipCode.")]
        public string? cZipcode { get; set; }

        [Required(ErrorMessage = "Please enter Patient Firstname")]
        public string? Firstname { get; set; }
        [Required(ErrorMessage = "The field is required")]

        public string? Lastname { get; set; }
        [Required(ErrorMessage = "number is required")]
        [RegularExpression(@"^\+\d{1,3}\d{10}$", ErrorMessage = "Phone number must be in the format +[Country Code][10 Digits].")]

        public string? Phonenumber { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "The field is required")]

        public DateTime DOB { get; set; }
        public string? Notes { get; set; }
        public string? pRoomNo { get; set; }
    }
    public class businessReq
    {
        [Required(ErrorMessage = "Please enter Your Firstname")]

        public string? bFirstname { get; set; }
        public IEnumerable<Region> Regions { get; set; }
        [Required(ErrorMessage = "The field is required")]
        public string? bLastname { get; set; }
        [Required(ErrorMessage = "number is required")]
        [RegularExpression(@"^\+\d{1,3}\d{10}$", ErrorMessage = "Phone number must be in the format +[Country Code][10 Digits].")]

        public string? bPhonenumber { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address.")]

        public string? bEmail { get; set; }
        [Required(ErrorMessage = "Please enter Your Business/Property Name")]
        public string? businessName { get; set; }
        public string? caseNo { get; set; }

        [Required(ErrorMessage = "Please enter patient firstname")]

        public string? Firstname { get; set; }
        [Required(ErrorMessage = "Please enter patient lastname")]

        public string? Lastname { get; set; }
        [Required(ErrorMessage = "number is required")]
        [RegularExpression(@"^\+\d{1,3}\d{10}$", ErrorMessage = "Phone number must be in the format +[Country Code][10 Digits].")]

        public string? Phonenumber { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "The field is required")]


        public DateTime DOB { get; set; }
        public string? Notes { get; set; }
        
        public string? Street { get; set; }
        
        public string? City { get; set; }
        [Required(ErrorMessage = "The field is required")]


        public string State { get; set; }
        [Required(ErrorMessage = "The field is required")]



        public string? Zipcode { get; set; }
        public string? RoomNo { get; set; }
        public string? Country { get; set; }
        public string? File { get; set; }
       
    }

    public enum status
    {
        New = 1,
        Pending = 2,
        Active = 3,
        Conclude = 4,
        Close = 5,
        Unpaid = 6

    }
            public enum RequestType
    {
        Friend=1,
        Business=2,
        Patient = 3
    }


    public class Dashboardmodel
    {
        public DateTime createddate { get; set; }
        public status Status { get; set; }  

        public int id { get; set; }

        public int Fcount { get; set; }
        
        
    }
    public class Profilemodel
    {
        [Required(ErrorMessage = "the fileld is required")]
        public string ? FirstName { get; set; }
        [Required(ErrorMessage = "the fileld is required")]

        public string ? LastName { get; set; }
        [Required(ErrorMessage = "the fileld is required")]
        public string ? Email { get; set; }
        [Required(ErrorMessage = "the fileld is required")]
        public DateTime  Dob { get; set; }
        [Required(ErrorMessage = "the fileld is required")]
        public string  ? PhoneNumber { get; set; }
        public string ? Street { get; set; }

        public string ? City { get; set; }
        [Required(ErrorMessage = "the fileld is required")]
        public string ? State { get; set; }
        public string ? Zipcode { get; set; }
    }
    


    public class Dashboardpage
    {
        public List<Dashboardmodel> Dashboard { get; set; }
        public Profilemodel? Profiles { get; set; }
        public List<Documentmodel> models { get; set; }
        public string confirmation { get;set; }
        public IEnumerable<Region> Regions { get;set; }
    }
  
    public class Documentmodel
    {
        public string uploader
        {
            get; set;
        }
        public DateTime uploaddate { get; set; }
        public string Firstname { get; set; }
        public int id { get; set; } 
            public string Filename { get; set; }


    }
    //public class doc
    //{
    //    public List<Documentmodel> docs { get; set; }
    //    public string Filename { get; set; }
    //}

    //public class MyViewModel
    //{
    //    public List<Dashboardmodel> item { get; set; }
    //    public List<Dashboardmodel>
    //}

}
