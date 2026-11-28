using tps.Models.Entities;

namespace tps.Services.Interfaces;

public class GoiYKetQua
{
    public HoSoSinhVien HoSo { get; set; } = null!;
    public string HoTen { get; set; } = null!;
    public int Diem { get; set; }   // 0 - 100
}

public interface IGoiYService
{
    Task<List<GoiYKetQua>> GoiYBanOGhepAsync(string sinhVienId, int top = 10);
}