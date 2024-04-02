using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Models;

[Table("aspnetuserroles")]
public partial class Aspnetuserrole
{
    [Column("userid", TypeName = "character varying")]
    public string Userid { get; set; } = null!;

    [Key]
    [Column("aspnetuserroleid")]
    public int Aspnetuserroleid { get; set; }

    [Column("roleid")]
    public int? Roleid { get; set; }

    [ForeignKey("Roleid")]
    [InverseProperty("Aspnetuserroles")]
    public virtual Aspnetrole? Role { get; set; }

    [ForeignKey("Userid")]
    [InverseProperty("Aspnetuserroles")]
    public virtual Aspnetuser User { get; set; } = null!;
}
