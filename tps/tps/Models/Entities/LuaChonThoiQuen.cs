using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tps.Models.Entities;

[Table("lua_chon_thoi_quen")]
public partial class LuaChonThoiQuen
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nhom_id")]
    public int NhomId { get; set; }

    [Column("nhan")]
    [StringLength(100)]
    public string Nhan { get; set; } = null!;

    [Column("gia_tri")]
    public int GiaTri { get; set; }

    [ForeignKey("NhomId")]
    [InverseProperty("LuaChons")]
    public virtual NhomThoiQuen Nhom { get; set; } = null!;

    [ForeignKey("LuaChonId")]
    [InverseProperty("LuaChons")]
    public virtual ICollection<HoSoSinhVien> SinhViens { get; set; } = new List<HoSoSinhVien>();
}
