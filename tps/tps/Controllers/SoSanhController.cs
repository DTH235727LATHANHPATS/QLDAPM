using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tps.Data;

namespace tps.Controllers;

public class SoSanhController : Controller
{
    private const string Khoa = "SoSanhIds";
    private const int ToiDa = 3;
    private readonly PhongTroSVContext _db;

    public SoSanhController(PhongTroSVContext db) => _db = db;

    private List<int> LayDanhSach()
    {
        var json = HttpContext.Session.GetString(Khoa);
        return string.IsNullOrEmpty(json)
            ? new List<int>()
            : JsonSerializer.Deserialize<List<int>>(json) ?? new List<int>();
    }

    private void LuuDanhSach(List<int> ds)
        => HttpContext.Session.SetString(Khoa, JsonSerializer.Serialize(ds));

    // Trang so sánh
    public async Task<IActionResult> Index()
    {
        var ids = LayDanhSach();
        var ds = await _db.PhongTros.AsNoTracking()
            .Include(p => p.LoaiPhong)
            .Include(p => p.ChuTro)
            .Where(p => ids.Contains(p.Id))
            .ToListAsync();

        // Giữ đúng thứ tự người dùng đã thêm
        ds = ds.OrderBy(p => ids.IndexOf(p.Id)).ToList();
        return View(ds);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Them(int id, string? quayLai)
    {
        var ds = LayDanhSach();
        if (ds.Contains(id))
        {
            TempData["ThongBaoSoSanh"] = "Phòng này đã có trong danh sách so sánh.";
        }
        else if (ds.Count >= ToiDa)
        {
            TempData["ThongBaoSoSanh"] = $"Chỉ so sánh tối đa {ToiDa} phòng. Hãy bỏ bớt một phòng.";
        }
        else if (!_db.PhongTros.Any(p => p.Id == id))
        {
            return NotFound();
        }
        else
        {
            ds.Add(id);
            LuuDanhSach(ds);
            TempData["ThongBaoSoSanh"] = $"Đã thêm vào so sánh ({ds.Count}/{ToiDa}).";
        }
        return QuayLai(quayLai);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Bo(int id, string? quayLai)
    {
        var ds = LayDanhSach();
        ds.Remove(id);
        LuuDanhSach(ds);
        return QuayLai(quayLai);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult XoaTatCa()
    {
        LuuDanhSach(new List<int>());
        return RedirectToAction(nameof(Index));
    }

    // Chỉ quay về địa chỉ nội bộ, chống chuyển hướng ra ngoài
    private IActionResult QuayLai(string? url)
        => !string.IsNullOrEmpty(url) && Url.IsLocalUrl(url)
            ? LocalRedirect(url)
            : RedirectToAction(nameof(Index));
}