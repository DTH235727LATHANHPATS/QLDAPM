using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tps.Models.Entities;

[PrimaryKey("SinhVienId", "PhongId")]
[Table("phong_yeu_thich")]
public partial class PhongYeuThich
{
    [Key]
    [Column("sinh_vien_id")]
    public string SinhVienId { get; set; } = null!;

    [Key]
    [Column("phong_id")]
    public int PhongId { get; set; }

    [Column("ngay_luu")]
    public DateTime NgayLuu { get; set; }

    [ForeignKey("PhongId")]
    [InverseProperty("PhongYeuThiches")]
    public virtual PhongTro Phong { get; set; } = null!;

    [ForeignKey("SinhVienId")]
    [InverseProperty("PhongYeuThiches")]
    public virtual TaiKhoan SinhVien { get; set; } = null!;
}
