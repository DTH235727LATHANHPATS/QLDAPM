using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tps.Models.Entities;

[Table("loai_phong")]
public partial class LoaiPhong
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("ten")]
    [StringLength(80)]
    public string Ten { get; set; } = null!;

    [InverseProperty("LoaiPhong")]
    public virtual ICollection<PhongTro> PhongTros { get; set; } = new List<PhongTro>();
}
