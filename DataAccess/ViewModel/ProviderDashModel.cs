using DataAccess.Models;
using Microsoft.AspNetCore.Http;
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
    public class ProviderInfoModel
    {
        public int? PhysicianId { get; set; }
        [Required(ErrorMessage = "please enter your username")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "please enter your password")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "please enter your firstname")]
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required(ErrorMessage = "please enter your email")]
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? MedicalLicenseNumber { get; set; }
        public string? NPINumber { get; set; }
        public string? SyncEmail { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Phonenumber format is not valid.")]
        [Required(ErrorMessage = "please enter your phonenumber")]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "please enter your businessname")]
        public string? BusinessName { get; set; }
        [Required(ErrorMessage = "please enter your businesswebsite")]
        public string? BusinessWebsite { get; set; }
        public string? AdminNote { get; set; }
        public int? formid { get; set; }
        public string? viewid { get; set; }
        public string? Photo1 { get; set; }
        public string PhotoName { get; set; }
        public string? Signature1 { get; set; }
        public string? Altphone { get; set; }
        public List<Region> RegionList { get; set; }
        public IFormFile? Photo { get; set; }
        public IFormFile? Signature { get; set; }
        public IFormFile? ICA { get; set; }
        public IFormFile? BGCheck { get; set; }
        public IFormFile? HIPAACompliance { get; set; }
        public IFormFile? NDA { get; set; }
        public IFormFile? LicenseDoc { get; set; }
        public string? Role { get; set; }
        public int? RoleId { get; set; }
        public string? OnCallStatus { get; set; }
        //public UserStatus? Status { get; set; }
        public int ProviderID { get; set; }
        public string? aspId { get; set; }

        public int? regionId { get; set; }
        public BitArray IsPhoto { get; set; }

        public int? statusId { get; set; }
    }
    public class PhysicianProfileModel
    {
        public List<Physicianregion> physicianregion { get; set; }

        public List<int>? SelectedReg { get; set; }
        public List<Physician>? physician { get; set; }
        public string? Firstname { get; set; } = null!;
        public string? Lastname { get; set; }
        public string? Email { get; set; } = null!;
        public string? Mobile { get; set; }
        public string? Medicallicense { get; set; }
        public string? Photo { get; set; }
        public string? Adminnotes { get; set; }
        public BitArray? Isagreementdoc { get; set; }
        public BitArray? Isbackgrounddoc { get; set; }
        public BitArray? Istrainingdoc { get; set; }
        public BitArray? Isnondisclosuredoc { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public int? Regionid { get; set; }
        public int PhysicianId { get; set; }
        public string? Zip { get; set; }
        public string? Altphone { get; set; }
        public string? Createdby { get; set; } = null!;
        public DateTime Createddate { get; set; }
        public string? Modifiedby { get; set; }
        public DateTime? Modifieddate { get; set; }
        public short? Status { get; set; }
        public string Businessname { get; set; } = null!;
        public string Businesswebsite { get; set; } = null!;
        public BitArray? Isdeleted { get; set; }
        public string? Npinumber { get; set; }
        public string? Signature { get; set; }
        public string? Syncemailaddress { get; set; }
    }
    public class EncounterModel
    {
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
    }

    public class ProviderDashModel
    {
        public string? firstName { get; set; }

        public string? lastName { get; set; }

        public string strMonth { get; set; }

        public int? Month { get; set; }
        public int? intYear { get; set; }
        public int? intDate { get; set; }

        public DateTime? DOB { get; set; }
        public string? requestorFname { get; set; }

        public string? requestorLname { get; set; }
        public string requestorPhonenumber { get; set; }

        public DateTime createdDate { get; set; }

        public string? mobileNo { get; set; }

        public string? Email { get; set; }

        public string? city { get; set; }

        public string? street { get; set; }

        public string? zipCode { get; set; }

        public long? RoomNo { get; set; }

        public string? state { get; set; }

        [Required]
        public string? notes { get; set; }

        public int? requestTypeId { get; set; }

        public int? status { get; set; }

        public int? ReqCount { get; set; }

        public int? StatusId { get; set; }

        [Required]
        public string? Region { get; set; }

        public string? Address { get; set; }

        public DateTime? DateOfService { get; set; }

        [Required]
        public string? PhysicianName { get; set; }

        public int? RequestId { get; set; }

    }

    public class ProviderDash
    {
        public List<ProviderDashModel>? requestByStatus { get; set; }
        public int? statusId { get; set; }
        public Status? Status { get; set; }
        public int? CurrentPage { get; set; }
        public int? TotalPages { get; set; }
        public int? PageSize { get; set; }
        public List<Region> regions { get; set; }
        //public ReqCount reqCount { get; set; }
        public ProviderInfoModel? ProviderInfomodel { get; set; }
        public int newcount { get; set; }
        public int pendingcount { get; set; }
        public int activecount { get; set; }
        public int conclude { get; set; }
    }
}
