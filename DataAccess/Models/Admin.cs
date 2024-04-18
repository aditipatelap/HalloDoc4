using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Models;

[Table("admin")]
public partial class Admin
{
    [Key]
    [Column("adminid")]
    public int Adminid { get; set; }

    [Column("aspnetuserid")]
    [StringLength(128)]
    public string Aspnetuserid { get; set; } = null!;

    [Column("firstname")]
    [StringLength(100)]
    public string Firstname { get; set; } = null!;

    [Column("lastname")]
    [StringLength(100)]
    public string? Lastname { get; set; }

    [Column("email")]
    [StringLength(50)]
    public string Email { get; set; } = null!;

    [Column("mobile")]
    [StringLength(20)]
    public string? Mobile { get; set; }

    [Column("address1")]
    [StringLength(500)]
    public string? Address1 { get; set; }

    [Column("address2")]
    [StringLength(500)]
    public string? Address2 { get; set; }

    [Column("regionid")]
    public int? Regionid { get; set; }

    [Column("zip")]
    [StringLength(10)]
    public string? Zip { get; set; }

    [Column("altphone")]
    [StringLength(20)]
    public string? Altphone { get; set; }

    [Column("createdby")]
    [StringLength(128)]
    public string Createdby { get; set; } = null!;

    [Column("createddate", TypeName = "timestamp without time zone")]
    public DateTime Createddate { get; set; }

    [Column("modifiedby")]
    [StringLength(128)]
    public string? Modifiedby { get; set; }

    [Column("modifieddate", TypeName = "timestamp without time zone")]
    public DateTime? Modifieddate { get; set; }

    [Column("status")]
    public short? Status { get; set; }

    [Column("isdeleted", TypeName = "bit(1)")]
    public BitArray? Isdeleted { get; set; }

    [Column("roleid")]
    public int? Roleid { get; set; }

    [InverseProperty("Admin")]
    public virtual ICollection<Adminregion> Adminregions { get; set; } = new List<Adminregion>();

    [ForeignKey("Aspnetuserid")]
    [InverseProperty("AdminAspnetusers")]
    public virtual Aspnetuser Aspnetuser { get; set; } = null!;

    [ForeignKey("Createdby")]
    [InverseProperty("AdminCreatedbyNavigations")]
    public virtual Aspnetuser CreatedbyNavigation { get; set; } = null!;

    [InverseProperty("Admin")]
    public virtual ICollection<Emaillog> Emaillogs { get; set; } = new List<Emaillog>();

    [InverseProperty("Admin")]
    public virtual ICollection<Encounter> Encounters { get; set; } = new List<Encounter>();

    [ForeignKey("Regionid")]
    [InverseProperty("Admins")]
    public virtual Region? Region { get; set; }

    [InverseProperty("Admin")]
    public virtual ICollection<Requeststatuslog> Requeststatuslogs { get; set; } = new List<Requeststatuslog>();

    [InverseProperty("Admin")]
    public virtual ICollection<Requestwisefile> Requestwisefiles { get; set; } = new List<Requestwisefile>();

    [InverseProperty("Admin")]
    public virtual ICollection<Smslog> Smslogs { get; set; } = new List<Smslog>();
}
