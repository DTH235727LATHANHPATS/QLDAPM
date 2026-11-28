using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tps.Models.Entities;

[Table("quang_cao")]
public partial class QuangCao
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nguoi_dang_id")]
    [StringLength(450)]
    public string NguoiDangId { get; set; } = null!;

    [Column("tieu_de")]
    [StringLength(200)]
    public string TieuDe { get; set; } = null!;

    [Column("noi_dung")]
    [StringLength(1000)]
    public string? NoiDung { get; set; }

    [Column("hinh_anh")]
    [StringLength(300)]
    public string? HinhAnh { get; set; }

    [Column("duong_dan")]
    [StringLength(300)]
    public string? DuongDan { get; set; }

    [Column("vi_tri")]
    public byte ViTri { get; set; }

    [Column("ngay_bat_dau")]
    public DateTime NgayBatDau { get; set; }

    [Column("ngay_ket_thuc")]
    public DateTime? NgayKetThuc { get; set; }

    [Column("hien_thi")]
    public bool HienThi { get; set; }

    [Column("ngay_tao")]
    public DateTime NgayTao { get; set; }

    [ForeignKey("NguoiDangId")]
    [InverseProperty("QuangCaos")]
    public virtual TaiKhoan NguoiDang { get; set; } = null!;
}
