using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Models;

public partial class Chatdatum
{
    [Key]
    public int Chatdataid { get; set; }

    [Column(TypeName = "character varying")]    
    public string? Message { get; set; }

    [Column(TypeName = "character varying")]
    public string? Messageby { get; set; }

    [Column(TypeName = "character varying")]
    public string? Sendername { get; set; }

    public bool? Isdeleted { get; set; }

    public int? Chatid { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? Date { get; set; }

    [ForeignKey("Chatid")]
    [InverseProperty("Chatdata")]
    public virtual Chat? Chat { get; set; }
}
