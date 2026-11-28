using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tps.Data;

namespace tps.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class NguoiDungController : Controller
{
    private readonly PhongTroSVContext _db;
    public NguoiDungController(PhongTroSVContext db) => _db = db;

    public async Task<IActionResult> Index(string? tuKhoa)
    {
        var q = _db.TaiKhoans.AsNoTracking().Include(t => t.VaiTros).AsQueryable();
        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            var k = tuKhoa.Trim();
            q = q.Where(t => t.HoTen.Contains(k)
                          || (t.Email != null && t.Email.Contains(k))
                          || (t.SoDienThoai != null && t.SoDienThoai.Contains(k)));
        }
        ViewBag.TuKhoa = tuKhoa;
        return View(await q.OrderBy(t => t.HoTen).ToListAsync());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Xoa(string id)
    {
        if (id == User.FindFirstValue(ClaimTypes.NameIdentifier))
        {
            TempData["Loi"] = "Không thể xóa chính tài khoản đang đăng nhập.";
            return RedirectToAction(nameof(Index));
        }

        var tk = await _db.TaiKhoans.Include(t => t.VaiTros).FirstOrDefaultAsync(t => t.Id == id);
        if (tk == null) return NotFound();
        if (tk.VaiTros.Any(v => v.Ten == "Admin"))
        {
            TempData["Loi"] = "Không thể xóa tài khoản Admin.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            _db.TaiKhoans.Remove(tk);
            await _db.SaveChangesAsync();
            TempData["ThanhCong"] = "Đã xóa tài khoản.";
        }
        catch (DbUpdateException)
        {
            // Chủ trọ còn phòng, hoặc tài khoản có dữ liệu liên quan (khóa ngoại không cascade)
            TempData["Loi"] = "Không thể xóa: tài khoản còn dữ liệu liên quan (phòng trọ, yêu cầu...). Hãy xóa các tin của họ trước.";
        }
        return RedirectToAction(nameof(Index));
    }
}