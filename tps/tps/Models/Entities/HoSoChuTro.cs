using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tps.Models.Entities;

[Table("ho_so_chu_tro")]
public partial class HoSoChuTro
{
    [Key]
    [Column("tai_khoan_id")]
    public string TaiKhoanId { get; set; } = null!;

    [Column("sdt_lien_he")]
    [StringLength(20)]
    public string? SdtLienHe { get; set; }

    [Column("email_lien_he")]
    [StringLength(100)]
    public string? EmailLienHe { get; set; }

    [Column("dia_chi")]
    [StringLength(300)]
    public string? DiaChi { get; set; }

    [ForeignKey("TaiKhoanId")]
    [InverseProperty("HoSoChuTro")]
    public virtual TaiKhoan TaiKhoan { get; set; } = null!;
}
