using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Models;

[Table("timesheet")]
public partial class Timesheet
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("invoiceid")]
    public int Invoiceid { get; set; }

    [Column("date", TypeName = "timestamp without time zone")]
    public DateTime Date { get; set; }

    [Column("totalhours")]
    public int? Totalhours { get; set; }

    [Column("weekend")]
    public bool? Weekend { get; set; }

    [Column("housecall")]
    public int? Housecall { get; set; }

    [Column("consult")]
    public int? Consult { get; set; }

    [Column("item")]
    [StringLength(250)]
    public string? Item { get; set; }

    [Column("amount")]
    public int? Amount { get; set; }

    [Column("billname")]
    [StringLength(250)]
    public string? Billname { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [ForeignKey("Invoiceid")]
    [InverseProperty("Timesheets")]
    public virtual Invoicing Invoice { get; set; } = null!;
}
