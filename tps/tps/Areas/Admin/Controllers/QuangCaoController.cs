using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tps.Data;
using tps.Models.Entities;
using tps.Models.ViewModels;
using tps.Services.Interfaces;

namespace tps.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class QuangCaoController : Controller
{
    private readonly PhongTroSVContext _db;
    private readonly IFileService _file;

    public QuangCaoController(PhongTroSVContext db, IFileService file)
    {
        _db = db;
        _file = file;
    }

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    public async Task<IActionResult> Index()
        => View(await _db.QuangCaos.AsNoTracking().OrderByDescending(q => q.Id).ToListAsync());

    [HttpGet]
    public IActionResult Create() => View(new QuangCaoFormViewModel());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(QuangCaoFormViewModel model)
    {
        if (model.NgayKetThuc.HasValue && model.NgayKetThuc < model.NgayBatDau)
            ModelState.AddModelError(nameof(model.NgayKetThuc), "Ngày kết thúc phải sau ngày bắt đầu");

        var loi = new ModelStateErrorCollector();
        string? anh = null;
        if (model.AnhMoi != null)
        {
            anh = await _file.LuuAnhAsync(model.AnhMoi, "quangcao", loi);
            if (loi.Loi != null) ModelState.AddModelError(nameof(model.AnhMoi), loi.Loi);
        }
        if (!ModelState.IsValid)
        {
            _file.XoaAnh(anh);
            return View(model);
        }

        _db.QuangCaos.Add(new QuangCao
        {
            NguoiDangId = UserId,
            TieuDe = model.TieuDe.Trim(),
            NoiDung = model.NoiDung?.Trim(),
            DuongDan = model.DuongDan?.Trim(),
            ViTri = model.ViTri,
            NgayBatDau = model.NgayBatDau,
            NgayKetThuc = model.NgayKetThuc,
            HienThi = model.HienThi,
            HinhAnh = anh
        });
        await _db.SaveChangesAsync();
        TempData["ThanhCong"] = "Đã thêm quảng cáo.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var q = await _db.QuangCaos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (q == null) return NotFound();
        return View(new QuangCaoFormViewModel
        {
            Id = q.Id,
            TieuDe = q.TieuDe,
            NoiDung = q.NoiDung,
            DuongDan = q.DuongDan,
            ViTri = q.ViTri,
            NgayBatDau = q.NgayBatDau,
            NgayKetThuc = q.NgayKetThuc,
            HienThi = q.HienThi,
            HinhAnhHienTai = q.HinhAnh
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, QuangCaoFormViewModel model)
    {
        if (id != model.Id) return BadRequest();
        var q = await _db.QuangCaos.FirstOrDefaultAsync(x => x.Id == id);
        if (q == null) return NotFound();

        if (model.NgayKetThuc.HasValue && model.NgayKetThuc < model.NgayBatDau)
            ModelState.AddModelError(nameof(model.NgayKetThuc), "Ngày kết thúc phải sau ngày bắt đầu");

        var loi = new ModelStateErrorCollector();
        string? anhMoi = null;
        if (model.AnhMoi != null)
        {
            anhMoi = await _file.LuuAnhAsync(model.AnhMoi, "quangcao", loi);
            if (loi.Loi != null) ModelState.AddModelError(nameof(model.AnhMoi), loi.Loi);
        }
        if (!ModelState.IsValid)
        {
            _file.XoaAnh(anhMoi);
            model.HinhAnhHienTai = q.HinhAnh;
            return View(model);
        }

        q.TieuDe = model.TieuDe.Trim();
        q.NoiDung = model.NoiDung?.Trim();
        q.DuongDan = model.DuongDan?.Trim();
        q.ViTri = model.ViTri;
        q.NgayBatDau = model.NgayBatDau;
        q.NgayKetThuc = model.NgayKetThuc;
        q.HienThi = model.HienThi;
        if (anhMoi != null)
        {
            _file.XoaAnh(q.HinhAnh);
            q.HinhAnh = anhMoi;
        }
        await _db.SaveChangesAsync();
        TempData["ThanhCong"] = "Đã cập nhật quảng cáo.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var q = await _db.QuangCaos.FirstOrDefaultAsync(x => x.Id == id);
        if (q == null) return NotFound();
        _db.QuangCaos.Remove(q);
        await _db.SaveChangesAsync();
        _file.XoaAnh(q.HinhAnh);
        TempData["ThanhCong"] = "Đã xóa quảng cáo.";
        return RedirectToAction(nameof(Index));
    }
}