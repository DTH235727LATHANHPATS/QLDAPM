using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tps.Models.Entities;

[Table("truong_dai_hoc")]
public partial class TruongDaiHoc
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("ten")]
    [StringLength(200)]
    public string Ten { get; set; } = null!;

    [Column("dia_chi")]
    [StringLength(300)]
    public string? DiaChi { get; set; }

    [Column("vi_do", TypeName = "decimal(9, 6)")]
    public decimal ViDo { get; set; }

    [Column("kinh_do", TypeName = "decimal(9, 6)")]
    public decimal KinhDo { get; set; }

    [InverseProperty("Truong")]
    public virtual ICollection<HoSoSinhVien> HoSoSinhViens { get; set; } = new List<HoSoSinhVien>();
}
