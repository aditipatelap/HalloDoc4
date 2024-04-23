using DataAccess.Models;
using Microsoft.AspNetCore.Http;
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
        public Month month { get; set; }
        public int RequestClientid { get; set; }
        public string ConfirmationNo { get; set; }
        public string Email { get; set; }
        public int regionid { get; set; }
        public string? Requestor { get; set; }
        public string? PhysicianName { get; set; }
        public DateTime RequestedDate { get; set; }
        public DateTime? DateofService { get; set; }
        public string? PatientPhone { get; set; }
        public string? RequestorPhone { get; set; }
        public string? Address { get; set; }
        public string? Notes { get; set; }
        public int RequestTypeid { get; set; }
        public string Region { get; set; }
        public int requestid { get; set; }
        public RequestType Status { get; set; }
        public int status { get; set; }
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
        [Required(ErrorMessage = "number is required")]
        [RegularExpression(@"^\+\d{1,3}\d{10}$", ErrorMessage = "Phone number must be in the format +[Country Code][10 Digits].")]
        public string? Mobile { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address.")]
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
        [Required(ErrorMessage = "Please Select Another Region")]
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
        [Required(ErrorMessage = "Please Select Another Region")]
        public string physicianname { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        public string Region { get; set; }

    }
    public class SendOrders
    {
        public string Description { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        public string Business { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        public string Email { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        public string BusinessContact { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        public string FaxNumber { get; set; }
        public string Prescription { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        public string Profession { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        public int NUmberofrefill { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        public int Healthprofessionalid { get; set; }
        [Required(ErrorMessage = "This is a required field")]
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
       
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? Location { get; set; }
            public string? Address { get; set; }
            public string? BirthDate { get; set; }
            public string? Number { get; set; }
            public DateTime? Date { get; set; }
            public string? fullDate { get; set; }
            public string? Email { get; set; }
            public int Requestid { get; set; }
            public bool? IsFinalized { get; set; }
            public string? HistoryOfPresentIllness { get; set; }
            public string? MedicalHistory { get; set; }
            public string? Medications { get; set; }
            public string? Allergies { get; set; }
            public string? Temperature { get; set; }
            public string? HR { get; set; }
            public string? RR { get; set; }
            public string? BPSystolic { get; set; }
            public string? BPDiastolic { get; set; }
            public string? O2 { get; set; }
            public string? Pain { get; set; }
            public string? Heent { get; set; }
            public string? CV { get; set; }
            public string? Chest { get; set; }
            public string? ABD { get; set; }
            public string? Extr { get; set; }
            public string? Skin { get; set; }
            public string? Neuro { get; set; }
            public string? Other { get; set; }
            public string? Diagnosis { get; set; }
            public string? TreatmentPlan { get; set; }
            public string? MedicationDispensed { get; set; }
            public string? Procedures { get; set; }
            public string? FollowUp { get; set; }
            public bool IsFinalize { get; set; }
        

    }
    public class SendLink
    {
        //[Required(ErrorMessage = "email is required")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
            [Required(ErrorMessage = "Email is required.")]
            [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "number is required")]
        [RegularExpression(@"^\+\d{1,3}\d{10}$", ErrorMessage = "Phone number must be in the format +[Country Code][10 Digits].")]

        public string PhoneNumber { get; set; }

    }
    public class Profile
    {
        public string? Aspnetid { get; set; }
        public string? UserName { get; set; }
        [Required(ErrorMessage = "fname is required")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "fname is required")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "lname is required")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "fname is required")]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "fname is required")]
        public string? Email { get; set; }
        public int physicianid { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string ConfirmationNo { get; set; }
        public string? State { get; set; }
        public string? Zipcode { get; set; }
        public string Mobile { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        public PhysicianStatus Status { get; set; }
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
    public class ViewDoc
    {
        public int? Id { get; set; }
        public string? uploader
        {
            get; set;
        }
        public DateTime uploaddate { get; set; }

        public string Name { get; set; }
        public int? reqId { get; set; }
    }
    public class RegionCheckbox {
        public int RegionId
        { get; set; }
        public string? Regionname { get; set; }
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
        public string? Notes { get; set; }


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
        public List<Requestwisefile> documents { get; set; }
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
        public BitArray notification { get; set; }

        public BitArray checkbox { get; set; }
        public string aspnetuserid { get;set; }


    }
    // public class checkbox { }
    public class checkboxmodel
    {
        public int physicianid { get; set; }
        public bool checkbox { get; set; }
    }
    public class AccountAccess
    {
        public string Name { get; set; }
        public Roles AccountType { get; set; }
        public int roleid { get; set; }
    }
    public class EditAccountAccess
    {
        public string? rolename { get; set; }
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
        public Roles? AccountType { get; set; }
        public string PhoneNumber { get; set; }
        public PhysicianStatus? status { get; set; }

        public int? accounttypeid { get; set; }
        public string aspnetuserid { get; set; }
        public int physicianid { get; set; }
        public PhysicianStatus AdminStatus { get; set; }
    }
    public class PhysicianProfile
    {
        public string? UserName { get; set; }
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Password must contain at least one uppercase letter and one digit")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 20 characters long")]
        public string? Password { get; set; }
        public short? status { get; set; }
        public string? role { get; set; }
        [Required(ErrorMessage = "The field is required")]
        public string? firstName { get; set; }
        public string aspnetid { get; set; }
        [Required(ErrorMessage = "The field is required")]
        public string? lastName { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public string LicenseNo { get; set; }
        public string? NIPNo { get; set; }
        public string SignatureName { get; set; }
        public string? SynchronizationEmailAddress { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? Zip { get; set; }
        public int? Regionid { get; set; }
        public string? phonenumber { get; set; }
        public string? BusinessName { get; set; }
        public string? BusinessWebsite { get; set; }
        public IFormFile Photo { get; set; }
        public IFormFile signature { get; set; }
        public string? notes { get; set; }
        public int? roleid { get; set; }
        public int physicianid { get; set; }
        public BitArray Isagreementdoc { get; set; }
        public IFormFile? AgreementDocument { get; set; }
        public BitArray Isbackgrounddoc { get; set; }
        public IFormFile? BackgroundDocument { get; set; }
        public BitArray Istrainingdoc { get; set; }
        public IFormFile? TraningDocument { get; set; }
        public BitArray Isnondisclosuredoc { get; set; }
        public IFormFile? NonDisclosureDocument { get; set; }
        public BitArray Islicensedoc { get; set; }
        public IFormFile? LicenseDocument { get; set; }
        public BitArray Iscredentialdoc { get; set; }
        public IFormFile? CredentialDocument { get; set; }
        public BitArray IsPhoto { get; set ;}
            public string PhotoName { get; set; }
        public int radioSMSEmail { get; set; }
        public string NotificationMassage { get; set; }
        public TimeOnly Starttime { get; set; }
        public TimeOnly Endtime { get; set; }
    }
    /**partner**/
    public class AddBusiness
    {
        [Required(ErrorMessage = "The field is required")]
        public string? BusinessName { get; set; }
        public string? FAXNumber { get; set; }
        [Required(ErrorMessage = "The field is required")]
        public int? ProfessionID { get; set; }
        [Required(ErrorMessage = "number is required")]
        [RegularExpression(@"^\+\d{1,3}\d{10}$", ErrorMessage = "Phone number must be in the format +[Country Code][10 Digits].")]
        public string? PHoneNumber { get; set; }
        [Required(ErrorMessage = "The field is required")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "The field is required")]
        public string? BusinessContanct { get; set; }
        public string? street { get; set; }
        [Required(ErrorMessage = "The field is required")]
        public string? city { get; set; }
        [Required(ErrorMessage = "The field is required")]
        public string? state { get; set; }

        public int? regionID { get; set; }
        [Required(ErrorMessage = "The field is required")]
        public string? zip { get; set; }
        public int? vendorID { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
    public class PartnerModel
    {
        public string? Profession { get; set; }
        public string? BusinessName { get; set; }
        public string? email { get; set; }
        public string? faxnumber { get; set; }
        public string? phonenumber { get; set; }
        public string? businessContact { get; set; }
        public int? businessId { get; set; }
    }
    //public class ScheduleModel
    //{
    //    public int? checkWeekday { get; set; }
    //    public DateTime Endtime { get; set; }
    //    public DateTime Starttime { get; set; }
    //    public DateTime? Startdate { get; set; }
    //    public DateTime Shiftdate { get; set; }
    //    public int? repeatcount { get; set; }

    //    public bool? Isrepeat { get; set; }
    //    public int Regionid { get; set; }
    //    public int Shiftid { get; set; }
    //    public int Physicianid { get; set; }
    //    public int? Repeatupto { get; set; }

    //}
   
       
    ///************* Scheduling************************* /

    public class scheduleModel
    {
        public int Regionid { get; set; }

        public int Physicianid { get; set; }

        [Required(ErrorMessage = "The Field is required")]
        public DateTime ShiftDate { get; set; }
        [Required(ErrorMessage = "The Field is required")]

        public TimeOnly StartTime { get; set; }
        [Required(ErrorMessage = "The Field is required")]
        public TimeOnly EndTime { get; set; }

        public bool isRepeat { get; set; }

        public string SelectedDayIds { get; set; }

        public int RepeatEnd { get; set; }
    }

    public class EventModel
    {
        public int Shiftdetailid { get; set; }

        public int Shiftid { get; set; }

        public int Physicianid { get; set; }

        public string? PhysicianName { get; set; }

        public DateTime Shiftdate { get; set; }

        public int? Regionid { get; set; }

        public string? Regionname { get; set; }

        public string Starttime { get; set; }

        public string Endtime { get; set; }

        public short Status { get; set; }

        public BitArray Isdeleted { get; set; }

        public string color { get; set; }
    }


/***********records****************/
public class Records
    {
        public IEnumerable<Requeststatus> requeststatuses { get; set; }
        public IEnumerable<Requesttype> requesttypes { get; set; }
        public int RequestId { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public string Requestor { get; set; }
        public DateTime DateOfService { get; set; }
        public DateTime CloseCaseDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public string RequestStatus { get; set; }
        public string Physician { get; set; }
        public string PhysicianNote { get; set; }
        public string CancelledByPhysicianNote { get; set; }
        public string AdminNote { get; set; }
        public string PatientNote { get; set; }

    }
    public class searchstream
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }

        public int Status { get; set; }
        public int reqType { get; set; }
        public string number { get; set; }
        public DateTime dateofservice { get; set; }
        public DateTime lastdateofservice { get; set; }
        public string providername { get; set; }
        public string email { get; set; }
        public DateTime sentdate { get; set; }
        public DateTime createdDate { get; set; }

    }

    //EmailLog
    public class EmailLogModel
    {
        public string Receipient { get; set; }
        public string Action { get; set; }
        public Roles RoleName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SentDate { get; set; }
        public string Sent { get; set; }
        public int? SentTries { get; set; }
        public string ConfirmationNumber { get; set; }
    }

    public class EmailLogList
    {
        public List<Aspnetrole> Roles { get; set; }
        public int RoleId { get; set; }
        public string ReceiverName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? SentDate { get; set; }
    }
    //SMSLogList
    public class SMSLogModel
    {
        public string Receipient { get; set; }
        public string Action { get; set; }
        public Role RoleName { get; set; }
        public string number { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SentDate { get; set; }
        public string Sent { get; set; }
        public int? SentTries { get; set; }
        public string ConfirmationNumber { get; set; }
    }
    public class SMSLogList
    {
        public List<Aspnetrole> Roles { get; set; }
        public int RoleId { get; set; }
        public string ReceiverName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? SentDate { get; set; }
    }
    //BlockedHistory

    public class BlockedHistory
    {
        public int BlockedRequestID { get; set; }
        public int RequestId { get; set; }
        public string PatientName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Notes { get; set; }
        public BitArray IsActive { get; set; }

    }
    //Patient history
    public class PatientHistoryModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int? UserId { get; set; }
    }
    //patient RecordsModel
    public class PatientRecordModel
    {
        public int RequestId { get; set; }
        public int RequestClientId { get; set; }
        public string Name { get; set; }
        public string ConfirmationNumber { get; set; }
        public string ProviderName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ConcludedDate { get; set; }
        public string Status { get; set; }
        public string FinalReport { get; set; }
        public int DocumentCount { get; set; }
    }   

    public class AdminDashboard
    {
        public string email { get; set; }
        public List<ViewDoc> doc { get; set; }
        public string UserName { get; set; }
       
        public IEnumerable<Casetag> Caserequest { get; set; }
        public IEnumerable<Physician> Physicians { get; set; }
        [Required(ErrorMessage = "the field is required")]
        public string SendYourOrderBusiness { get; set; }
        [Required(ErrorMessage = "the field is required")]
        public IEnumerable<Healthprofessional> Healthprofessionals { get; set; }
        [Required(ErrorMessage = "the field is required")]
        public IEnumerable<Healthprofessionaltype> healthprofessionaltypes { get; set; }
        public List<AdminDash> Dashboards { get; set; }
        public Profile myProfile { get; set; }
        public CreateReqquestModel CreateReqquestModel { get; set; }
        public CloseCaseModel CloseCaseModel { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<Adminregion> adminregions { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int ToatCount { get; set; }

        public List<ViewUpload> ViewUpload { get; set; }
        public BlockReq blockreq { get; set; }
        public CancelReq cancelreq { get; set; }
        public AssignReq assignreq { get; set; }
        public TranserReq transferreq { get; set; }
        public SendOrders sendorder { get; set; }
        public AgreementReq agreementReq { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        public IEnumerable<Region> Regions { get; set; }
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
        public string patientname { get; set; }
        public string Adminname { get; set; }
        public string ConfirmationNo { get; set; }
        public string Email { get; set; }
        public int statusid { get; set; }
        public string btnname { get; set; }
        public int accounttype { get; set; }
        public int roleid { get; set; }
        public string aspnetuserid { get; set; }

        public int adminaccountfilter { get; set; }
        public IEnumerable<AdminDash> adminDashes { get; set; }
        public IEnumerable<Role> RoleModel { get; set; }

        //public PayLoad payLoad { get; set; }
        public string tabid { get; set; }
        public int regionid { get; set; }
        public int physicianid { get; set; }
        public List<int> Physicianids { get; set; }
        public List<BitArray> checkbox { get; set; }

        [Required(ErrorMessage = "please select atleast one checkbox")]
        public List<RegionCheckbox> regionCheckbox { get; set; }
        public GetTabParameter GetTabParameter { get; set; }


        /*******providers********/
        public List<int> RegionArray{get;set;}
        public IEnumerable<Physicianlocation> physicianlocations { get; set; }
        public IEnumerable<Rolemenu> rolemenus { get; set; }
        public List<AccountAccess> accountAccess { get; set; }
        public List<MenuCheckbox> menuCheckboxes { get; set; }
        public EditAccountAccess EditaccountAccess { get; set; }
        public List<ProviderInfo> ProviderInfo { get; set; }
        public IEnumerable<Menu> ? menu { get; set; }
       public  List<UserAccessModel> userAccessModels { get; set; } 
        public PhysicianProfile PhysicianProfile { get; set; }  
        public AddBusiness AddBusinessModel { get;set; }
        public List<PartnerModel> PartnerModel { get; set; }
        public int VendorId { get;set; }
        //public List<checkboxmodel> checkboxmodel { get; set; }
        //public List<PhysicancheckboxModel> physicancheckboxModels { get; set; }

        /****payload*////
        //public class PayLoad
        //{
        //    public string tabid { get; set; }
        //    public int requestid { get; set; }
        //    public string physicianid { get; set; }
        //}
        //public ScheduleModel ScheduleModel { get;set; }
        /************scheduling********/
        public List<Physician> oncall { get; set; } = new List<Physician>();
        public List<Physician> offduty { get; set; } = new List<Physician>();
        public scheduleModel scheduleModel { get; set; }
        public List<EventModel> eventModel { get; set; }
        public List<PhysicianProfile> PhysicianProfilList { get; set; }
        public string currentView { get; set; }
        /***********************records**************/
        //search records

        public Records Records { get; set; }
        public List<Records> RecordsList { get; set; }
        public searchstream searchstream { get; set; }
        //EmailLog
        public EmailLogList EmailLog { get; set; }
        public List<EmailLogModel> emailLogModel { get; set; }

        //SMSLog
        public SMSLogList SMSLog { get; set; }
        public List<SMSLogModel> SMSLogModel { get; set; }

        //BlockHistory
        public List<BlockedHistory> BlockedHistory { get; set; }

        //PatientHistory
        public List<PatientHistoryModel>? PatientHistory { get; set; }

        public List<PatientRecordModel>? PatientsRecord { get; set; }

    }
    public class GetTabParameter
    {
        //public string tabid { get; set; }
        public int regionid { get; set; }
    }
}