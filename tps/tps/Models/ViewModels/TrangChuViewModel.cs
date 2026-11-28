using tps.Models.Entities;

namespace tps.Models.ViewModels;

public class TrangChuViewModel
{
    public List<PhongTro> PhongMoi { get; set; } = new();
    public List<PhongTro> PhongOGhep { get; set; } = new();
    public List<QuangCao> QuangCaos { get; set; } = new();
    public List<LoaiPhong> LoaiPhongs { get; set; } = new();
    public int TongPhong { get; set; }
    public int TongChuTro { get; set; }
    public int TongSinhVien { get; set; }
}