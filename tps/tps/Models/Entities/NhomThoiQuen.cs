using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tps.Models.Entities;

[Table("nhom_thoi_quen")]
[Index("Ma", Name = "UQ__nhom_tho__3213C8B61D3C51DD", IsUnique = true)]
public partial class NhomThoiQuen
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("ma")]
    [StringLength(30)]
    [Unicode(false)]
    public string Ma { get; set; } = null!;

    [Column("ten")]
    [StringLength(80)]
    public string Ten { get; set; } = null!;

    [Column("trong_so", TypeName = "decimal(4, 2)")]
    public decimal TrongSo { get; set; }

    [InverseProperty("Nhom")]
    public virtual ICollection<LuaChonThoiQuen> LuaChons { get; set; } = new List<LuaChonThoiQuen>();
}
