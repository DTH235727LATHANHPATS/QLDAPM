using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tps.Data;

namespace tps.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class HomeController : Controller
{
    private readonly PhongTroSVContext _db;
    public HomeController(PhongTroSVContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        ViewBag.SoSinhVien = await _db.TaiKhoans.CountAsync(t => t.VaiTros.Any(v => v.Ten == "SinhVien"));
        ViewBag.SoChuTro = await _db.TaiKhoans.CountAsync(t => t.VaiTros.Any(v => v.Ten == "ChuTro"));
        ViewBag.SoPhong = await _db.PhongTros.CountAsync();
        ViewBag.SoPhongTrong = await _db.PhongTros.CountAsync(p => p.ConPhong);
        ViewBag.SoYeuCau = await _db.YeuCauOGheps.CountAsync();
        ViewBag.SoGhepThanhCong = await _db.YeuCauOGheps.CountAsync(y => y.TrangThai == 1);

        // Số phòng theo loại (cho biểu đồ)
        ViewBag.TheoLoai = await _db.PhongTros
            .GroupBy(p => p.LoaiPhong.Ten)
            .Select(g => new { Ten = g.Key, SoLuong = g.Count() })
            .ToListAsync();

        return View();
    }
}