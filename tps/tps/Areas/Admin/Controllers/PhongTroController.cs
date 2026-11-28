using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tps.Data;
using tps.Services.Interfaces;

namespace tps.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class PhongTroController : Controller
{
    private readonly PhongTroSVContext _db;
    private readonly IFileService _file;

    public PhongTroController(PhongTroSVContext db, IFileService file)
    {
        _db = db;
        _file = file;
    }

    public async Task<IActionResult> Index(string? tuKhoa)
    {
        var q = _db.PhongTros.AsNoTracking().Include(p => p.ChuTro).Include(p => p.LoaiPhong).AsQueryable();
        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            var k = tuKhoa.Trim();
            q = q.Where(p => p.TenNhaTro.Contains(k) || p.DiaChi.Contains(k) || p.ChuTro.HoTen.Contains(k));
        }
        ViewBag.TuKhoa = tuKhoa;
        return View(await q.OrderByDescending(p => p.Id).ToListAsync());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Xoa(int id)
    {
        var p = await _db.PhongTros.FirstOrDefaultAsync(x => x.Id == id);
        if (p == null) return NotFound();

        try
        {
            _db.PhongTros.Remove(p);
            await _db.SaveChangesAsync();
            _file.XoaAnh(p.HinhAnh);
            TempData["ThanhCong"] = "Đã xóa tin phòng trọ.";
        }
        catch (DbUpdateException)
        {
            TempData["Loi"] = "Không thể xóa tin này vì còn dữ liệu liên quan.";
        }
        return RedirectToAction(nameof(Index));
    }
}