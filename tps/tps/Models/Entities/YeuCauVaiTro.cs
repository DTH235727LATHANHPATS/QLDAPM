using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tps.Models.Entities;

[Table("yeu_cau_vai_tro")]
public partial class YeuCauVaiTro
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("vai_tro_id")]
    [StringLength(450)]
    public string VaiTroId { get; set; } = null!;

    [Column("loai_yeu_cau")]
    public string? LoaiYeuCau { get; set; }

    [Column("gia_tri")]
    public string? GiaTri { get; set; }

    [ForeignKey("VaiTroId")]
    [InverseProperty("YeuCauVaiTros")]
    public virtual VaiTro VaiTro { get; set; } = null!;
}
