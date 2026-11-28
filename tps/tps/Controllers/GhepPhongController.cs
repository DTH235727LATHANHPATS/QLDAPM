using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tps.Data;
using tps.Models.Entities;
using tps.Models.ViewModels;
using tps.Services.Interfaces;

namespace tps.Controllers;

[Authorize]
public class GhepPhongController : Controller
{
    private readonly PhongTroSVContext _db;
    private readonly IGoiYService _goiY;

    public GhepPhongController(PhongTroSVContext db, IGoiYService goiY)
    {
        _db = db;
        _goiY = goiY;
    }

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    // Trạng thái: 0 Chờ, 1 Chấp nhận, 2 Từ chối, 3 Hủy

    // ---------- GỢI Ý BẠN Ở GHÉP (sinh viên) ----------
    [Authorize(Roles = "SinhVien")]
    public async Task<IActionResult> GoiY()
    {
        var coHoSo = await _db.HoSoSinhViens.AnyAsync(h => h.TaiKhoanId == UserId);
        if (!coHoSo)
        {
            TempData["Loi"] = "Hãy hoàn thiện hồ sơ (thói quen, sở thích) để nhận gợi ý.";
            return RedirectToAction("SinhVien", "HoSo");
        }
        return View(await _goiY.GoiYBanOGhepAsync(UserId));
    }

    // ---------- GỬI YÊU CẦU ----------
    [Authorize(Roles = "SinhVien")]
    [HttpGet]
    public async Task<IActionResult> GuiYeuCau(int phongId)
    {
        var p = await _db.PhongTros.AsNoTracking().FirstOrDefaultAsync(x => x.Id == phongId);
        if (p == null) return NotFound();
        if (!p.TimOGhep || !p.ConPhong)
        {
            TempData["Loi"] = "Phòng này hiện không nhận người ở ghép.";
            return RedirectToAction("ChiTiet", "TimKiem", new { id = phongId });
        }
        return View(new GuiYeuCauViewModel { PhongId = p.Id, TenPhong = p.TenNhaTro });
    }

    [Authorize(Roles = "SinhVien")]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> GuiYeuCau(GuiYeuCauViewModel model)
    {
        var p = await _db.PhongTros.FirstOrDefaultAsync(x => x.Id == model.PhongId);
        if (p == null) return NotFound();
        if (!p.TimOGhep || !p.ConPhong)
        {
            TempData["Loi"] = "Phòng này hiện không nhận người ở ghép.";
            return RedirectToAction("ChiTiet", "TimKiem", new { id = p.Id });
        }
        model.TenPhong = p.TenNhaTro;
        if (!ModelState.IsValid) return View(model);

        // Mỗi sinh viên chỉ có 1 yêu cầu / phòng (ràng buộc UNIQUE trong DB)
        var cu = await _db.YeuCauOGheps.FirstOrDefaultAsync(y => y.PhongId == p.Id && y.SinhVienId == UserId);
        if (cu != null)
        {
            if (cu.TrangThai == 0 || cu.TrangThai == 1)
            {
                TempData["Loi"] = "Bạn đã gửi yêu cầu cho phòng này rồi.";
                return RedirectToAction(nameof(CuaToi));
            }
            // Đã bị từ chối hoặc đã hủy: cho gửi lại bằng cách làm mới yêu cầu cũ
            cu.TrangThai = 0;
            cu.LoiNhan = model.LoiNhan?.Trim();
            cu.NgayTao = DateTime.UtcNow;
            cu.NgayPhanHoi = null;
        }
        else
        {
            _db.YeuCauOGheps.Add(new YeuCauOGhep
            {
                PhongId = p.Id,
                SinhVienId = UserId,
                LoiNhan = model.LoiNhan?.Trim(),
                TrangThai = 0
            });
        }
        await _db.SaveChangesAsync();
        TempData["ThanhCong"] = "Đã gửi yêu cầu ở ghép.";
        return RedirectToAction(nameof(CuaToi));
    }

    // ---------- YÊU CẦU CỦA TÔI (sinh viên) ----------
    [Authorize(Roles = "SinhVien")]
    public async Task<IActionResult> CuaToi()
    {
        var ds = await _db.YeuCauOGheps.AsNoTracking()
            .Include(y => y.Phong)
            .Where(y => y.SinhVienId == UserId)
            .OrderByDescending(y => y.NgayTao)
            .ToListAsync();
        return View(ds);
    }

    [Authorize(Roles = "SinhVien")]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Huy(int id)
    {
        var y = await _db.YeuCauOGheps.FirstOrDefaultAsync(x => x.Id == id && x.SinhVienId == UserId);
        if (y == null) return NotFound();
        if (y.TrangThai == 0)
        {
            y.TrangThai = 3;
            y.NgayPhanHoi = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            TempData["ThanhCong"] = "Đã hủy yêu cầu.";
        }
        return RedirectToAction(nameof(CuaToi));
    }

    // ---------- YÊU CẦU GỬI TỚI PHÒNG CỦA TÔI (chủ trọ) ----------
    [Authorize(Roles = "ChuTro")]
    public async Task<IActionResult> NhanDuoc()
    {
        var ds = await _db.YeuCauOGheps.AsNoTracking()
            .Include(y => y.Phong)
            .Include(y => y.SinhVien)
            .Where(y => y.Phong.ChuTroId == UserId)
            .OrderBy(y => y.TrangThai).ThenByDescending(y => y.NgayTao)
            .ToListAsync();
        return View(ds);
    }

    [Authorize(Roles = "ChuTro")]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> PhanHoi(int id, bool chapNhan)
    {
        var y = await _db.YeuCauOGheps.Include(x => x.Phong)
            .FirstOrDefaultAsync(x => x.Id == id && x.Phong.ChuTroId == UserId);
        if (y == null) return NotFound();

        if (y.TrangThai == 0)
        {
            y.TrangThai = (byte)(chapNhan ? 1 : 2);
            y.NgayPhanHoi = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            TempData["ThanhCong"] = chapNhan ? "Đã chấp nhận yêu cầu." : "Đã từ chối yêu cầu.";
        }
        return RedirectToAction(nameof(NhanDuoc));
    }
}