using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Models;

[Table("Chat")]
public partial class Chat
{
    public int? Physicianid { get; set; }

    public int? Adminid { get; set; }

    [Key]
    public int Chatid { get; set; }

    public int? Patientid { get; set; }

    public int? Createdby { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? Createddate { get; set; }

    [InverseProperty("Chat")]
    public virtual ICollection<Chatdatum> Chatdata { get; set; } = new List<Chatdatum>();
}
