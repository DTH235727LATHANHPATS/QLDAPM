using tps.Models.Entities;

namespace tps.Models.ViewModels;

public class TimKiemViewModel
{
    // Điều kiện lọc
    public string? TuKhoa { get; set; }
    public string? KhuVuc { get; set; }
    public decimal? GiaTu { get; set; }
    public decimal? GiaDen { get; set; }
    public decimal? DienTichTu { get; set; }
    public decimal? DienTichDen { get; set; }
    public int? LoaiPhongId { get; set; }
    public bool ChiConPhong { get; set; } = true;
    public bool TimOGhep { get; set; }
    public string Sort { get; set; } = "moi";   // moi | giatang | giagiam | dientich

    // Phân trang
    public int Trang { get; set; } = 1;
    public int TongSo { get; set; }
    public int TongTrang { get; set; }

    // Kết quả
    public List<PhongTro> KetQua { get; set; } = new();
}