using Microsoft.AspNetCore.Identity;
using tps.Services.Interfaces;

namespace tps.Services.Implementations;

public class MatKhauService : IMatKhauService
{
    private readonly PasswordHasher<object> _hasher = new();

    public string Bam(string matKhau) => _hasher.HashPassword(null!, matKhau);

    public bool KiemTra(string matKhauDaBam, string matKhauNhap)
    {
        var kq = _hasher.VerifyHashedPassword(null!, matKhauDaBam, matKhauNhap);
        return kq != PasswordVerificationResult.Failed;
    }
}