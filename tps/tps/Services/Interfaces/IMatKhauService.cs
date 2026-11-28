namespace tps.Services.Interfaces;

public interface IMatKhauService
{
    string Bam(string matKhau);
    bool KiemTra(string matKhauDaBam, string matKhauNhap);
}