using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Models;

[Table("invoicing")]
public partial class Invoicing
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("startdate", TypeName = "timestamp without time zone")]
    public DateTime Startdate { get; set; }

    [Column("enddate", TypeName = "timestamp without time zone")]
    public DateTime Enddate { get; set; }

    [Column("physicianid")]
    public int Physicianid { get; set; }

    [Column("invoicetotal")]
    public int? Invoicetotal { get; set; }

    [Column("isfinalize")]
    public bool Isfinalize { get; set; }

    [Column("isapproved")]
    public bool Isapproved { get; set; }

    [Column("createdby")]
    public int Createdby { get; set; }

    [Column("createddate", TypeName = "timestamp without time zone")]
    public DateTime Createddate { get; set; }

    [Column("modifiedby")]
    public int Modifiedby { get; set; }

    [Column("modifieddate", TypeName = "timestamp without time zone")]
    public DateTime Modifieddate { get; set; }

    [ForeignKey("Physicianid")]
    [InverseProperty("Invoicings")]
    public virtual Physician Physician { get; set; } = null!;

    [InverseProperty("Invoice")]
    public virtual ICollection<Timesheet> Timesheets { get; set; } = new List<Timesheet>();
}
