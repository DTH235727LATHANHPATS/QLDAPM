namespace tps.Services.Interfaces;

public interface IFileService
{
    Task<string?> LuuAnhAsync(IFormFile file, string thuMuc, ModelStateErrorCollector? loi = null);
    void XoaAnh(string? duongDan);
}

public class ModelStateErrorCollector
{
    public string? Loi { get; set; }
}