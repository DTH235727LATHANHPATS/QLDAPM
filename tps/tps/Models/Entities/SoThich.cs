using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tps.Models.Entities;

[Table("so_thich")]
[Index("Ten", Name = "UQ__so_thich__DC107AB1E0F48A36", IsUnique = true)]
public partial class SoThich
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("ten")]
    [StringLength(80)]
    public string Ten { get; set; } = null!;

    [ForeignKey("SoThichId")]
    [InverseProperty("SoThiches")]
    public virtual ICollection<HoSoSinhVien> SinhViens { get; set; } = new List<HoSoSinhVien>();
}
