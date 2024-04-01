using DataAccess.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAccess.ViewModel.Constant;

namespace DataAccess.ViewModel
{
    public class AdminDash
    {
        public string? Name { get; set; }
        public int? IntDate { get; set; }
        public int? IntYear { get; set; }
        public string? StrMonth { get; set; }
        public string? Requestor { get; set; }
        public string? PhysicianName { get; set; }
        public DateTime? RequestedDate { get; set; }
        public DateTime? DateofService { get; set; }
        public string? PatientPhone { get; set; }
        public string? RequestorPhone { get; set; }
        public string? Address { get; set; }
        public string? Notes { get; set; }
        public int RequestTypeid { get; set; }
        public string Region { get; set; }
        public int requestid { get; set; }
        public RequestType Status { get; set; }
        public int PageSize { get; set; }




    }
    public class ViewCase
    {
        public int RequestTypeId { get; set; }
        public int RequestId { get; set; }
        public string ConfNo { get; set; }
        public string Symptoms { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Phone number must be of 10 digit")]
        public string? Mobile { get; set; }
        [Required(ErrorMessage = "required")]
        public string Email { get; set; }
        public string? Region { get; set; }
        public string Address { get; set; }
        public string? Room { get; set; }
    }
    public class BlockReq
    {
        [Required(ErrorMessage = "requirwed")]
        public string Blockreason { get; set; }

    }

    public class AssignReq
    {
        [Required(ErrorMessage = "The field is required")]

        public string Description { get; set; }
        [Required(ErrorMessage = "Enter physician name")]
        public string physicianname { get; set; }
        [Required(ErrorMessage = "The field is required")]
        public string RegionItems { get; set; }

    }
    public class CancelReq
    {
        [Required(ErrorMessage = "Please select a reason")]
        public string reason
        {
            get; set;
        }
        public string description
        {
            get; set;
        }
    }
    public class TranserReq
    {
        [Required(ErrorMessage = "required field ")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please select a physician")]
        public string physicianname { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        public string Region { get; set; }

    }
    public class SendOrders
    {
        public string Description { get; set; }
        public string Business { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        public string Email { get; set; }
        public string BusinessContact { get; set; }
        public string FaxNumber { get; set; }
        public string Prescription { get; set; }
        public string Profession { get; set; }
        public int NUmberofrefill { get; set; }
        public int VendorId { get; set; }

    }
    public class ViewNotesModel
    {
        public string TransferNotes { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public DateTime CreatedDate { get; set; }

    }
    public class ViewUpload
    {
        public DateTime CreatedDate
        {
            get; set;
        }
        public Month Month { get; set; }
        public DateTime uploaddate { get; set; }
        public string UploaderName { get; set; }
        public int? Adminid { get; set; }
        public string FileName { get; set; }
        public int Requestid { get; set; }
    }
    public class AgreementReq
    {
        public string? PhoneNumber { get; set; }
        public string? EmailID
        {
            get; set;
        }
        public int Requestid { get; set; }
        public string Notes { get; set; }
    }
    public class EncounterForm
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public DateTime Dob { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

    }
    public class SendLink
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage = "email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "number is required")]
        public string PhoneNumber { get; set; }

    }
    public class Profile
    {
        public int? Aspnetid { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        [Required(ErrorMessage = "fname is required")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "lname is required")]
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }

        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zipcode { get; set; }
        public string Mobile { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }

        public Status Status { get; set; }
        public string? Role { get; set; }
        public int? regionid { get; set; }
        public int? roleid { get; set; }
        public int adminid { get; set; }
        public string ConfirmEmail { get; set; }
        public Region? Region { get; set; }
        public string? ViewId { get; set; }
        /******for porvider profile info*/
        public string Businessname { get; set; }
        public string? BusinessWebsite { get; set; }
        public PhysicianStatus status { get; set; }
    }
    public class RegionCheckbox {
        public int RegionId
        { get; set;}
        public string? Regionname { get; set;}
}
    public class CreateReqquestModel
    {
        [Required(ErrorMessage = "this field is required")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "this field is required")]
        public string? LastName { get; set; }
        public DateTime DOB { get; set; }
        [Required(ErrorMessage = "this field is required")]
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "this field is required")]
        public string? Street { get; set; }
        [Required(ErrorMessage = "this field is required")]
        public string? City { get; set; }
        [Required(ErrorMessage = "this field is required")]
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Notes { get; set;}


    }
    public class CloseCaseModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DOB { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string ConfirmationNo { get; set; }
        public DateTime Dob { get; set; }
        public List<Requestwisefile> documents{get;set;}
    }



    /****************provider*************************/
    public class ProviderInfo
    {
        public string ProviderName { get; set; }
        public string Role { get; set; }
        public BitArray OnCall { get; set; }
        public PhysicianStatus ProviderStatus { get; set; }
        public int physicianid { get; set; }
        public string Username { get; set; }
        public BitArray notification { get;set; }
       
        public BitArray checkbox { get; set; }


    }
   // public class checkbox { }
    public class checkboxmodel
    {
        public int physicianid { get;set; }
        public bool checkbox { get;set; }
    }
    public class AccountAccess
    {
        public string Name { get; set; }    
        public Roles AccountType { get;set; }
        public int roleid { get; set; }
    }
    public class EditAccountAccess
    {
        public string? rolename { get; set;}
        public short AccountType { get; set; }

    }
    public class MenuCheckbox
    {
        public int menuid
        { get; set; }
        public string? menuname { get; set; }
    }
    public class UserAccessModel
    {
        public string AccountPOC { get; set; }
        public string AccountType { get; set; }
        public int PhoneNumber { get; set; }
        public string status { get; set; }
    }

    public class AdminDashboard
    {
        public string UserName { get;set; }
        public IEnumerable<Casetag> Caserequest { get; set; }
        public IEnumerable<Physician> Physicians { get; set; }
        [Required(ErrorMessage = "fname is required")]
        public IEnumerable<Healthprofessional> Healthprofessionals  { get; set; }
        public IEnumerable<Healthprofessionaltype> healthprofessionaltypes { get; set; }
        public List<AdminDash> Dashboards { get; set; }
        public Profile myProfile { get; set; }
        public CreateReqquestModel CreateReqquestModel { get; set; }
        public CloseCaseModel CloseCaseModel { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public List<ViewUpload> ViewUpload { get; set; }
        public BlockReq blockreq { get; set;}
        public CancelReq cancelreq { get; set; }    
        public AssignReq assignreq { get; set; }
        public TranserReq transferreq { get; set; }
        public SendOrders sendorder { get; set; }
        public AgreementReq agreementReq { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        public IEnumerable<Region> Regions {get; set;}
        public ViewCase viewcase { get; set; }
        public List<ViewNotesModel> viewnotes { get; set; }
        public string AdminNoes { get; set; }
        public string PhysicianNotes { get; set; }
      
        public string AdditionalNotes { get; set; }
        public EncounterForm encounterform { get; set; }
        public SendLink sendLink { get; set; }  
        public int newcount { get; set; }
        public int pendingcount { get; set; }
        public int activecount { get; set; }
        public int toclosecount { get; set; }
        public int concludecount { get; set; }
        public int unpaidcount { get; set; }
       public status Status { get; set; }
        public int requestid { get; set; }
        public string patientname { get;set; }
        public string Adminname  { get; set; }
        public string ConfirmationNo { get; set; }
        public string Email { get; set; }
        public int statusid { get;set; }
        public string btnname { get;set; }
        public int accounttype { get; set; }
        public int roleid { get;set; }  
        public string searchuseraccess { get;set; }
        public IEnumerable<AdminDash> adminDashes { get; set; }
        

        //public PayLoad payLoad { get; set; }
        public string tabid { get; set; }
        public int regionid { get; set; }
        public int physicianid { get; set; }
        public List<int> Physicianids { get; set; }
        public List<BitArray> checkbox { get; set; }

        public List<RegionCheckbox> regionCheckbox { get; set; }
        public GetTabParameter GetTabParameter { get; set; }    

       
        /*******providers********/
        public IEnumerable<Rolemenu> rolemenus { get; set; }
        public List<AccountAccess> accountAccess { get; set; }
        public List<MenuCheckbox> menuCheckboxes { get; set; }
        public EditAccountAccess EditaccountAccess { get; set; }
        public List<ProviderInfo> ProviderInfo { get; set; }
        public IEnumerable<Menu> ? menu { get; set; }
       public  List<UserAccessModel> userAccessModels { get; set; } 
        //public List<checkboxmodel> checkboxmodel { get; set; }
        //public List<PhysicancheckboxModel> physicancheckboxModels { get; set; }

        /****payload*////
        //public class PayLoad
        //{
        //    public string tabid { get; set; }
        //    public int requestid { get; set; }
        //    public string physicianid { get; set; }
        //}

    }
    public class GetTabParameter
    {
        //public string tabid { get; set; }
        public int regionid { get; set; }
    }
}