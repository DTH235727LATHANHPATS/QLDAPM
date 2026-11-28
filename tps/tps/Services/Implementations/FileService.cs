using tps.Services.Interfaces;

namespace tps.Services.Implementations;

public class FileService : IFileService
{
    private static readonly string[] DuoiChoPhep = { ".jpg", ".jpeg", ".png", ".webp" };
    private const long ToiDa = 2 * 1024 * 1024; // 2 MB
    private readonly IWebHostEnvironment _env;

    public FileService(IWebHostEnvironment env) => _env = env;

    public async Task<string?> LuuAnhAsync(IFormFile file, string thuMuc, ModelStateErrorCollector? loi = null)
    {
        if (file == null || file.Length == 0) return null;

        var duoi = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!DuoiChoPhep.Contains(duoi))
        {
            if (loi != null) loi.Loi = "Chỉ chấp nhận ảnh .jpg, .jpeg, .png, .webp";
            return null;
        }
        if (file.Length > ToiDa)
        {
            if (loi != null) loi.Loi = "Ảnh tối đa 2 MB";
            return null;
        }

        var thuMucVatLy = Path.Combine(_env.WebRootPath, "uploads", thuMuc);
        Directory.CreateDirectory(thuMucVatLy);

        var tenFile = $"{Guid.NewGuid():N}{duoi}";
        var duongDanVatLy = Path.Combine(thuMucVatLy, tenFile);
        using (var stream = new FileStream(duongDanVatLy, FileMode.Create))
            await file.CopyToAsync(stream);

        return $"/uploads/{thuMuc}/{tenFile}";
    }

    public void XoaAnh(string? duongDan)
    {
        if (string.IsNullOrEmpty(duongDan)) return;
        var vatLy = Path.Combine(_env.WebRootPath, duongDan.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
        if (File.Exists(vatLy)) File.Delete(vatLy);
    }
}