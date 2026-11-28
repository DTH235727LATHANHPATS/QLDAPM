using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tps.Models.Entities;

[Table("danh_gia")]
[Index("PhongId", "SinhVienId", Name = "UQ_danhgia", IsUnique = true)]
public partial class DanhGium
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("phong_id")]
    public int PhongId { get; set; }

    [Column("sinh_vien_id")]
    public string SinhVienId { get; set; } = null!;

    [Column("so_sao")]
    public byte SoSao { get; set; }

    [Column("binh_luan")]
    [StringLength(1000)]
    public string? BinhLuan { get; set; }

    [Column("hien_thi")]
    public bool HienThi { get; set; }

    [Column("ngay_tao")]
    public DateTime NgayTao { get; set; }

    [InverseProperty("DanhGia")]
    public virtual ICollection<BaoCaoViPham> BaoCaoViPhams { get; set; } = new List<BaoCaoViPham>();

    [ForeignKey("PhongId")]
    [InverseProperty("DanhGia")]
    public virtual PhongTro Phong { get; set; } = null!;

    [ForeignKey("SinhVienId")]
    [InverseProperty("DanhGia")]
    public virtual TaiKhoan SinhVien { get; set; } = null!;
}
