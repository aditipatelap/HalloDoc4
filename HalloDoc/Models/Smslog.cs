using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.Models;

[Table("smslog")]
public partial class Smslog
{
    [Key]
    [Column("smslogid")]
    public int Smslogid { get; set; }

    [Column("smstemplate", TypeName = "character varying")]
    public string Smstemplate { get; set; } = null!;

    [Column("mobilenumber")]
    [StringLength(50)]
    public string Mobilenumber { get; set; } = null!;

    [Column("confirmationnumber")]
    [StringLength(50)]
    public string? Confirmationnumber { get; set; }

    [Column("roleid")]
    public int? Roleid { get; set; }

    [Column("adminid")]
    public int? Adminid { get; set; }

    [Column("requestid")]
    public int? Requestid { get; set; }

    [Column("physicianid")]
    public int? Physicianid { get; set; }

    [Column("createdate", TypeName = "timestamp without time zone")]
    public DateTime Createdate { get; set; }

    [Column("sentdate", TypeName = "timestamp without time zone")]
    public DateTime? Sentdate { get; set; }

    [Column("issmssent", TypeName = "bit(1)")]
    public BitArray? Issmssent { get; set; }

    [Column("senttries")]
    public int Senttries { get; set; }

    [Column("action")]
    public int? Action { get; set; }

    [ForeignKey("Adminid")]
    [InverseProperty("Smslogs")]
    public virtual Admin? Admin { get; set; }

    [ForeignKey("Physicianid")]
    [InverseProperty("Smslogs")]
    public virtual Physician? Physician { get; set; }

    [ForeignKey("Requestid")]
    [InverseProperty("Smslogs")]
    public virtual Request? Request { get; set; }
}
