using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tps.Data;
using tps.Models.Entities;
using tps.Models.ViewModels;

namespace tps.Controllers;

[Authorize]
public class HoSoController : Controller
{
    private readonly PhongTroSVContext _db;
    public HoSoController(PhongTroSVContext db) => _db = db;

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    // Chuyển tới đúng hồ sơ theo vai trò
    public IActionResult Index()
    {
        if (User.IsInRole("SinhVien")) return RedirectToAction(nameof(SinhVien));
        if (User.IsInRole("ChuTro")) return RedirectToAction(nameof(ChuTro));
        return RedirectToAction("Index", "TimKiem");
    }

    // Cập nhật tên hiển thị trong cookie, giữ nguyên vai trò và "ghi nhớ"
    private async Task LamMoiCookieAsync(string hoTen)
    {
        var kq = await HttpContext.AuthenticateAsync("Cookies");
        var claims = User.Claims.Where(c => c.Type != ClaimTypes.Name).ToList();
        claims.Add(new Claim(ClaimTypes.Name, hoTen));
        await HttpContext.SignInAsync("Cookies",
            new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies")), kq.Properties);
    }

    // ================= SINH VIÊN =================
    private async Task NapDanhMucSinhVienAsync(int? truongId)
    {
        var truongs = await _db.TruongDaiHocs.AsNoTracking().OrderBy(t => t.Ten).ToListAsync();
        ViewBag.Truongs = new SelectList(truongs, "Id", "Ten", truongId);
        ViewBag.SoThiches = await _db.SoThiches.AsNoTracking().OrderBy(s => s.Id).ToListAsync();
        ViewBag.Nhoms = await _db.NhomThoiQuens.AsNoTracking()
            .Include(n => n.LuaChons).OrderBy(n => n.Id).ToListAsync();
    }

    [Authorize(Roles = "SinhVien")]
    [HttpGet]
    public async Task<IActionResult> SinhVien()
    {
        var tk = await _db.TaiKhoans.AsNoTracking().FirstAsync(t => t.Id == UserId);
        var hs = await _db.HoSoSinhViens.AsNoTracking()
            .Include(h => h.SoThiches)
            .Include(h => h.LuaChons)
            .FirstOrDefaultAsync(h => h.TaiKhoanId == UserId);

        var model = new HoSoSinhVienFormViewModel { HoTen = tk.HoTen };
        if (hs != null)
        {
            model.TruongId = hs.TruongId;
            model.MaSinhVien = hs.MaSinhVien;
            model.GioiTinh = hs.GioiTinh;
            model.NgaySinh = hs.NgaySinh;
            model.NganhHoc = hs.NganhHoc;
            model.GioiThieu = hs.GioiThieu;
            model.NganSachTu = hs.NganSachTu;
            model.NganSachDen = hs.NganSachDen;
            model.DangTimOGhep = hs.DangTimOGhep;
            model.SoThichIds = hs.SoThiches.Select(s => s.Id).ToList();
            model.ThoiQuen = hs.LuaChons.ToDictionary(o => o.NhomId, o => (int?)o.Id);
        }
        await NapDanhMucSinhVienAsync(model.TruongId);
        return View(model);
    }

    [Authorize(Roles = "SinhVien")]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> SinhVien(HoSoSinhVienFormViewModel model)
    {
        if (model.NganSachTu.HasValue && model.NganSachDen.HasValue
            && model.NganSachTu > model.NganSachDen)
            ModelState.AddModelError(nameof(model.NganSachDen), "Ngân sách đến phải lớn hơn hoặc bằng ngân sách từ");

        if (model.GioiTinh.HasValue && model.GioiTinh > 2)
            ModelState.AddModelError(nameof(model.GioiTinh), "Giới tính không hợp lệ");

        if (model.TruongId.HasValue && !await _db.TruongDaiHocs.AnyAsync(t => t.Id == model.TruongId))
            ModelState.AddModelError(nameof(model.TruongId), "Trường không hợp lệ");

        // Kiểm tra thói quen: mỗi lựa chọn phải thuộc đúng nhóm gửi lên
        var chon = model.ThoiQuen.Where(kv => kv.Value.HasValue)
                                 .ToDictionary(kv => kv.Key, kv => kv.Value!.Value);
        var ids = chon.Values.ToList();
        var luaChons = await _db.LuaChons.Where(o => ids.Contains(o.Id)).ToListAsync();
        foreach (var kv in chon)
        {
            if (!luaChons.Any(o => o.Id == kv.Value && o.NhomId == kv.Key))
            {
                ModelState.AddModelError("", "Lựa chọn thói quen không hợp lệ");
                break;
            }
        }

        if (!ModelState.IsValid)
        {
            await NapDanhMucSinhVienAsync(model.TruongId);
            return View(model);
        }

        var tk = await _db.TaiKhoans.FirstAsync(t => t.Id == UserId);
        tk.HoTen = model.HoTen.Trim();

        var hs = await _db.HoSoSinhViens
            .Include(h => h.SoThiches)
            .Include(h => h.LuaChons)
            .FirstOrDefaultAsync(h => h.TaiKhoanId == UserId);
        if (hs == null)
        {
            hs = new HoSoSinhVien { TaiKhoanId = UserId };
            _db.HoSoSinhViens.Add(hs);
        }

        hs.TruongId = model.TruongId;
        hs.MaSinhVien = model.MaSinhVien?.Trim();
        hs.GioiTinh = model.GioiTinh;
        hs.NgaySinh = model.NgaySinh;
        hs.NganhHoc = model.NganhHoc?.Trim();
        hs.GioiThieu = model.GioiThieu?.Trim();
        hs.NganSachTu = model.NganSachTu;
        hs.NganSachDen = model.NganSachDen;
        hs.DangTimOGhep = model.DangTimOGhep;

        // Sở thích: chỉ nhận id có thật trong bảng
        var soThichs = await _db.SoThiches.Where(s => model.SoThichIds.Contains(s.Id)).ToListAsync();
        hs.SoThiches.Clear();
        foreach (var s in soThichs) hs.SoThiches.Add(s);

        hs.LuaChons.Clear();
        foreach (var o in luaChons) hs.LuaChons.Add(o);

        await _db.SaveChangesAsync();
        await LamMoiCookieAsync(tk.HoTen);

        TempData["ThanhCong"] = "Đã cập nhật hồ sơ.";
        return RedirectToAction(nameof(SinhVien));
    }

    // ================= CHỦ TRỌ =================
    [Authorize(Roles = "ChuTro")]
    [HttpGet]
    public async Task<IActionResult> ChuTro()
    {
        var tk = await _db.TaiKhoans.AsNoTracking().FirstAsync(t => t.Id == UserId);
        var hs = await _db.HoSoChuTros.AsNoTracking().FirstOrDefaultAsync(h => h.TaiKhoanId == UserId);

        return View(new HoSoChuTroFormViewModel
        {
            HoTen = tk.HoTen,
            SdtLienHe = hs?.SdtLienHe ?? tk.SoDienThoai,
            EmailLienHe = hs?.EmailLienHe ?? tk.Email,
            DiaChi = hs?.DiaChi
        });
    }

    [Authorize(Roles = "ChuTro")]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ChuTro(HoSoChuTroFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var tk = await _db.TaiKhoans.FirstAsync(t => t.Id == UserId);
        tk.HoTen = model.HoTen.Trim();

        var hs = await _db.HoSoChuTros.FirstOrDefaultAsync(h => h.TaiKhoanId == UserId);
        if (hs == null)
        {
            hs = new HoSoChuTro { TaiKhoanId = UserId };
            _db.HoSoChuTros.Add(hs);
        }
        hs.SdtLienHe = model.SdtLienHe?.Trim();
        hs.EmailLienHe = model.EmailLienHe?.Trim();
        hs.DiaChi = model.DiaChi?.Trim();

        await _db.SaveChangesAsync();
        await LamMoiCookieAsync(tk.HoTen);

        TempData["ThanhCong"] = "Đã cập nhật hồ sơ.";
        return RedirectToAction(nameof(ChuTro));
    }
}