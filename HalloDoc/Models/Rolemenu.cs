﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.Models;

[Table("rolemenu")]
public partial class Rolemenu
{
    [Key]
    [Column("rolemenuid")]
    public int Rolemenuid { get; set; }

    [Column("roleid")]
    public int Roleid { get; set; }

    [Column("menuid")]
    public int Menuid { get; set; }

    [ForeignKey("Menuid")]
    [InverseProperty("Rolemenus")]
    public virtual Menu Menu { get; set; } = null!;
}
