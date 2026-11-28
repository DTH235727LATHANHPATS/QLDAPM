using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tps.Models.Entities;

[Table("yeu_cau_tai_khoan")]
public partial class YeuCauTaiKhoan
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("tai_khoan_id")]
    [StringLength(450)]
    public string TaiKhoanId { get; set; } = null!;

    [Column("loai_yeu_cau")]
    public string? LoaiYeuCau { get; set; }

    [Column("gia_tri")]
    public string? GiaTri { get; set; }

    [ForeignKey("TaiKhoanId")]
    [InverseProperty("YeuCauTaiKhoans")]
    public virtual TaiKhoan TaiKhoan { get; set; } = null!;
}
