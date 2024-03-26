using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.Models;

[Table("aspnetuserroles")]
public partial class Aspnetuserrole
{
    [Column("userid", TypeName = "character varying")]
    public string Userid { get; set; } = null!;

    [Column("roleid")]
    public int Roleid { get; set; }

    [Key]
    [Column("aspnetuserroleid")]
    public int Aspnetuserroleid { get; set; }
}
