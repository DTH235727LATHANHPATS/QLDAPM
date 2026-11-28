using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tps.Models.Entities;

[Table("ho_so_sinh_vien")]
public partial class HoSoSinhVien
{
    [Key]
    [Column("tai_khoan_id")]
    public string TaiKhoanId { get; set; } = null!;

    [Column("truong_id")]
    public int? TruongId { get; set; }

    [Column("ma_sinh_vien")]
    [StringLength(30)]
    public string? MaSinhVien { get; set; }

    [Column("gioi_tinh")]
    public byte? GioiTinh { get; set; }

    [Column("ngay_sinh")]
    public DateOnly? NgaySinh { get; set; }

    [Column("nganh_hoc")]
    [StringLength(100)]
    public string? NganhHoc { get; set; }

    [Column("gioi_thieu")]
    [StringLength(500)]
    public string? GioiThieu { get; set; }

    [Column("ngan_sach_tu", TypeName = "decimal(18, 0)")]
    public decimal? NganSachTu { get; set; }

    [Column("ngan_sach_den", TypeName = "decimal(18, 0)")]
    public decimal? NganSachDen { get; set; }

    [Column("dang_tim_o_ghep")]
    public bool DangTimOGhep { get; set; }

    [ForeignKey("TaiKhoanId")]
    [InverseProperty("HoSoSinhVien")]
    public virtual TaiKhoan TaiKhoan { get; set; } = null!;

    [ForeignKey("TruongId")]
    [InverseProperty("HoSoSinhViens")]
    public virtual TruongDaiHoc? Truong { get; set; }

    [ForeignKey("SinhVienId")]
    [InverseProperty("SinhViens")]
    public virtual ICollection<LuaChonThoiQuen> LuaChons { get; set; } = new List<LuaChonThoiQuen>();

    [ForeignKey("SinhVienId")]
    [InverseProperty("SinhViens")]
    public virtual ICollection<SoThich> SoThiches { get; set; } = new List<SoThich>();
}
