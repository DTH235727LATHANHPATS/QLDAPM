using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tps.Models.Entities;

[Table("yeu_cau_o_ghep")]
[Index("PhongId", "SinhVienId", Name = "UQ_ycog", IsUnique = true)]
public partial class YeuCauOGhep
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("phong_id")]
    public int PhongId { get; set; }

    [Column("sinh_vien_id")]
    public string SinhVienId { get; set; } = null!;

    [Column("loi_nhan")]
    [StringLength(500)]
    public string? LoiNhan { get; set; }

    [Column("trang_thai")]
    public byte TrangThai { get; set; }

    [Column("ngay_tao")]
    public DateTime NgayTao { get; set; }

    [Column("ngay_phan_hoi")]
    public DateTime? NgayPhanHoi { get; set; }

    [ForeignKey("PhongId")]
    [InverseProperty("YeuCauOGheps")]
    public virtual PhongTro Phong { get; set; } = null!;

    [ForeignKey("SinhVienId")]
    [InverseProperty("YeuCauOGheps")]
    public virtual TaiKhoan SinhVien { get; set; } = null!;
}
