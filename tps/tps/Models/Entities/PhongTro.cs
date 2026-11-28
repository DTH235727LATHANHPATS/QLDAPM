using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tps.Models.Entities;

[Table("phong_tro")]
[Index("ChuTroId", Name = "IX_phong_chu_tro")]
[Index("ConPhong", "GiaThue", Name = "IX_phong_gia")]
public partial class PhongTro
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("chu_tro_id")]
    public string ChuTroId { get; set; } = null!;

    [Column("ten_nha_tro")]
    [StringLength(150)]
    public string TenNhaTro { get; set; } = null!;

    [Column("dia_chi")]
    [StringLength(300)]
    public string DiaChi { get; set; } = null!;

    [Column("loai_phong_id")]
    public int LoaiPhongId { get; set; }

    [Column("mo_ta")]
    public string? MoTa { get; set; }

    [Column("dien_tich", TypeName = "decimal(6, 1)")]
    public decimal DienTich { get; set; }

    [Column("gia_thue", TypeName = "decimal(18, 0)")]
    public decimal GiaThue { get; set; }

    [Column("gia_dien", TypeName = "decimal(18, 0)")]
    public decimal? GiaDien { get; set; }

    [Column("gia_nuoc", TypeName = "decimal(18, 0)")]
    public decimal? GiaNuoc { get; set; }

    [Column("tien_coc", TypeName = "decimal(18, 0)")]
    public decimal? TienCoc { get; set; }

    [Column("con_phong")]
    public bool ConPhong { get; set; }

    [Column("tim_o_ghep")]
    public bool TimOGhep { get; set; }

    [Column("hinh_anh")]
    [StringLength(300)]
    public string? HinhAnh { get; set; }

    [InverseProperty("Phong")]
    public virtual ICollection<BaoCaoViPham> BaoCaoViPhams { get; set; } = new List<BaoCaoViPham>();

    [ForeignKey("ChuTroId")]
    [InverseProperty("PhongTros")]
    public virtual TaiKhoan ChuTro { get; set; } = null!;

    [InverseProperty("Phong")]
    public virtual ICollection<DanhGium> DanhGia { get; set; } = new List<DanhGium>();

    [ForeignKey("LoaiPhongId")]
    [InverseProperty("PhongTros")]
    public virtual LoaiPhong LoaiPhong { get; set; } = null!;

    [InverseProperty("Phong")]
    public virtual ICollection<PhongYeuThich> PhongYeuThiches { get; set; } = new List<PhongYeuThich>();

    [InverseProperty("Phong")]
    public virtual ICollection<YeuCauOGhep> YeuCauOGheps { get; set; } = new List<YeuCauOGhep>();
}
