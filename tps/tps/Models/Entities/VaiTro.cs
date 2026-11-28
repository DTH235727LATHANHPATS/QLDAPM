using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tps.Models.Entities;

[Table("vai_tro")]
[Index("Ten", Name = "UX_vai_tro_ten", IsUnique = true)]
public partial class VaiTro
{
    [Key]
    [Column("id")]
    public string Id { get; set; } = null!;

    [Column("ten")]
    [StringLength(256)]
    public string Ten { get; set; } = null!;

    [Column("dau_an_dong_thoi")]
    public string? DauAnDongThoi { get; set; }

    [InverseProperty("VaiTro")]
    public virtual ICollection<YeuCauVaiTro> YeuCauVaiTros { get; set; } = new List<YeuCauVaiTro>();

    [ForeignKey("VaiTroId")]
    [InverseProperty("VaiTros")]
    public virtual ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();
}
