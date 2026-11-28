using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tps.Models.Entities;

[Table("tai_khoan")]
public partial class TaiKhoan
{
    [Key]
    [Column("id")]
    public string Id { get; set; } = null!;

    [Column("ten_dang_nhap")]
    [StringLength(256)]
    public string? TenDangNhap { get; set; }

    [Column("email")]
    [StringLength(256)]
    public string? Email { get; set; }

    [Column("mat_khau")]
    [StringLength(200)]
    public string MatKhau { get; set; } = null!;

    [Column("so_dien_thoai")]
    [StringLength(20)]
    public string? SoDienThoai { get; set; }

    [Column("ho_ten")]
    [StringLength(150)]
    public string HoTen { get; set; } = null!;

    [Column("anh_dai_dien")]
    [StringLength(300)]
    public string? AnhDaiDien { get; set; }

    [InverseProperty("NguoiBaoCao")]
    public virtual ICollection<BaoCaoViPham> BaoCaoViPhamNguoiBaoCaos { get; set; } = new List<BaoCaoViPham>();

    [InverseProperty("NguoiBiBaoCao")]
    public virtual ICollection<BaoCaoViPham> BaoCaoViPhamNguoiBiBaoCaos { get; set; } = new List<BaoCaoViPham>();

    [InverseProperty("NguoiXuLy")]
    public virtual ICollection<BaoCaoViPham> BaoCaoViPhamNguoiXuLies { get; set; } = new List<BaoCaoViPham>();

    [InverseProperty("SinhVien")]
    public virtual ICollection<DanhGium> DanhGia { get; set; } = new List<DanhGium>();

    [InverseProperty("TaiKhoan")]
    public virtual HoSoChuTro? HoSoChuTro { get; set; }

    [InverseProperty("TaiKhoan")]
    public virtual HoSoSinhVien? HoSoSinhVien { get; set; }

    [InverseProperty("ChuTro")]
    public virtual ICollection<PhongTro> PhongTros { get; set; } = new List<PhongTro>();

    [InverseProperty("SinhVien")]
    public virtual ICollection<PhongYeuThich> PhongYeuThiches { get; set; } = new List<PhongYeuThich>();

    [InverseProperty("NguoiDang")]
    public virtual ICollection<QuangCao> QuangCaos { get; set; } = new List<QuangCao>();

    [InverseProperty("SinhVien")]
    public virtual ICollection<YeuCauOGhep> YeuCauOGheps { get; set; } = new List<YeuCauOGhep>();

    [InverseProperty("TaiKhoan")]
    public virtual ICollection<YeuCauTaiKhoan> YeuCauTaiKhoans { get; set; } = new List<YeuCauTaiKhoan>();

    [ForeignKey("TaiKhoanId")]
    [InverseProperty("TaiKhoans")]
    public virtual ICollection<VaiTro> VaiTros { get; set; } = new List<VaiTro>();
}
