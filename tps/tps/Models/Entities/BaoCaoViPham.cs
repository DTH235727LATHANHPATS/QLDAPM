using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tps.Models.Entities;

[Table("bao_cao_vi_pham")]
public partial class BaoCaoViPham
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nguoi_bao_cao_id")]
    [StringLength(450)]
    public string NguoiBaoCaoId { get; set; } = null!;

    [Column("phong_id")]
    public int? PhongId { get; set; }

    [Column("danh_gia_id")]
    public int? DanhGiaId { get; set; }

    [Column("nguoi_bi_bao_cao_id")]
    [StringLength(450)]
    public string? NguoiBiBaoCaoId { get; set; }

    [Column("ly_do")]
    public byte LyDo { get; set; }

    [Column("mo_ta")]
    [StringLength(1000)]
    public string? MoTa { get; set; }

    [Column("trang_thai")]
    public byte TrangThai { get; set; }

    [Column("nguoi_xu_ly_id")]
    [StringLength(450)]
    public string? NguoiXuLyId { get; set; }

    [Column("ngay_xu_ly")]
    public DateTime? NgayXuLy { get; set; }

    [Column("ket_qua_xu_ly")]
    [StringLength(500)]
    public string? KetQuaXuLy { get; set; }

    [Column("ngay_tao")]
    public DateTime NgayTao { get; set; }

    [ForeignKey("DanhGiaId")]
    [InverseProperty("BaoCaoViPhams")]
    public virtual DanhGium? DanhGia { get; set; }

    [ForeignKey("NguoiBaoCaoId")]
    [InverseProperty("BaoCaoViPhamNguoiBaoCaos")]
    public virtual TaiKhoan NguoiBaoCao { get; set; } = null!;

    [ForeignKey("NguoiBiBaoCaoId")]
    [InverseProperty("BaoCaoViPhamNguoiBiBaoCaos")]
    public virtual TaiKhoan? NguoiBiBaoCao { get; set; }

    [ForeignKey("NguoiXuLyId")]
    [InverseProperty("BaoCaoViPhamNguoiXuLies")]
    public virtual TaiKhoan? NguoiXuLy { get; set; }

    [ForeignKey("PhongId")]
    [InverseProperty("BaoCaoViPhams")]
    public virtual PhongTro? Phong { get; set; }
}
