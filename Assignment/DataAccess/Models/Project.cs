using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Models;

[Table("Project")]
public partial class Project
{
    [Key]
    public int Id { get; set; }

    [StringLength(200)]
    public string? ProjectName { get; set; }

    [StringLength(100)]
    public string? Assignee { get; set; }

    [Column("DomainID")]
    public int? DomainId { get; set; }

    [StringLength(200)]
    public string? Description { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? DueDate { get; set; }

    [StringLength(100)]
    public string? Domain { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [ForeignKey("DomainId")]
    [InverseProperty("Projects")]
    public virtual Domain? DomainNavigation { get; set; }
}
