using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tps.Data;
using tps.Models.Entities;
using tps.Models.ViewModels;
using tps.Services.Interfaces;

namespace tps.Controllers;

[Authorize(Roles = "ChuTro")]
public class PhongTroController : Controller
{
    private readonly PhongTroSVContext _db;
    private readonly IFileService _file;

    public PhongTroController(PhongTroSVContext db, IFileService file)
    {
        _db = db;
        _file = file;
    }

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    private async Task NapLoaiPhongAsync(int? chon = null)
    {
        var ds = await _db.LoaiPhongs.OrderBy(l => l.Id).ToListAsync();
        ViewBag.LoaiPhongs = new SelectList(ds, "Id", "Ten", chon);
    }

    // ---------- DANH SÁCH (phòng của tôi) ----------
    public async Task<IActionResult> Index()
    {
        var ds = await _db.PhongTros
            .Where(p => p.ChuTroId == UserId)
            .OrderByDescending(p => p.Id)
            .ToListAsync();
        return View(ds);
    }

    // ---------- THÊM ----------
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await NapLoaiPhongAsync();
        return View(new PhongTroFormViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PhongTroFormViewModel model)
    {
        var loi = new ModelStateErrorCollector();
        string? duongDanAnh = null;

        if (model.AnhMoi != null)
        {
            duongDanAnh = await _file.LuuAnhAsync(model.AnhMoi, "phong", loi);
            if (loi.Loi != null) ModelState.AddModelError(nameof(model.AnhMoi), loi.Loi);
        }

        if (!ModelState.IsValid)
        {
            _file.XoaAnh(duongDanAnh);
            await NapLoaiPhongAsync(model.LoaiPhongId);
            return View(model);
        }

        var phong = new PhongTro
        {
            ChuTroId = UserId,
            TenNhaTro = model.TenNhaTro.Trim(),
            DiaChi = model.DiaChi.Trim(),
            LoaiPhongId = model.LoaiPhongId!.Value,
            MoTa = model.MoTa,
            DienTich = model.DienTich!.Value,
            GiaThue = model.GiaThue!.Value,
            GiaDien = model.GiaDien,
            GiaNuoc = model.GiaNuoc,
            TienCoc = model.TienCoc,
            ConPhong = model.ConPhong,
            TimOGhep = model.TimOGhep,
            HinhAnh = duongDanAnh
        };
        _db.PhongTros.Add(phong);
        await _db.SaveChangesAsync();

        TempData["ThanhCong"] = "Đã thêm phòng trọ.";
        return RedirectToAction(nameof(Index));
    }

    // ---------- SỬA ----------
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var p = await _db.PhongTros.FirstOrDefaultAsync(x => x.Id == id && x.ChuTroId == UserId);
        if (p == null) return NotFound();

        await NapLoaiPhongAsync(p.LoaiPhongId);
        return View(new PhongTroFormViewModel
        {
            Id = p.Id,
            TenNhaTro = p.TenNhaTro,
            DiaChi = p.DiaChi,
            LoaiPhongId = p.LoaiPhongId,
            MoTa = p.MoTa,
            DienTich = p.DienTich,
            // Làm tròn để không hiện ",00"
            GiaThue = decimal.Truncate(p.GiaThue),
            GiaDien = p.GiaDien.HasValue ? decimal.Truncate(p.GiaDien.Value) : null,
            GiaNuoc = p.GiaNuoc.HasValue ? decimal.Truncate(p.GiaNuoc.Value) : null,
            TienCoc = p.TienCoc.HasValue ? decimal.Truncate(p.TienCoc.Value) : null,
            ConPhong = p.ConPhong,
            TimOGhep = p.TimOGhep,
            HinhAnhHienTai = p.HinhAnh
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PhongTroFormViewModel model)
    {
        if (id != model.Id) return BadRequest();

        var p = await _db.PhongTros.FirstOrDefaultAsync(x => x.Id == id && x.ChuTroId == UserId);
        if (p == null) return NotFound();

        var loi = new ModelStateErrorCollector();
        string? anhMoi = null;
        if (model.AnhMoi != null)
        {
            anhMoi = await _file.LuuAnhAsync(model.AnhMoi, "phong", loi);
            if (loi.Loi != null) ModelState.AddModelError(nameof(model.AnhMoi), loi.Loi);
        }

        if (!ModelState.IsValid)
        {
            _file.XoaAnh(anhMoi);
            model.HinhAnhHienTai = p.HinhAnh;
            await NapLoaiPhongAsync(model.LoaiPhongId);
            return View(model);
        }

        p.TenNhaTro = model.TenNhaTro.Trim();
        p.DiaChi = model.DiaChi.Trim();
        p.LoaiPhongId = model.LoaiPhongId!.Value;
        p.MoTa = model.MoTa;
        p.DienTich = model.DienTich!.Value;
        p.GiaThue = model.GiaThue!.Value;
        p.GiaDien = model.GiaDien;
        p.GiaNuoc = model.GiaNuoc;
        p.TienCoc = model.TienCoc;
        p.ConPhong = model.ConPhong;
        p.TimOGhep = model.TimOGhep;

        if (anhMoi != null)
        {
            _file.XoaAnh(p.HinhAnh);   // xóa ảnh cũ
            p.HinhAnh = anhMoi;
        }

        await _db.SaveChangesAsync();
        TempData["ThanhCong"] = "Đã cập nhật phòng trọ.";
        return RedirectToAction(nameof(Index));
    }
    // ---------- BẬT/TẮT CÒN PHÒNG NHANH ----------
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DoiTrangThai(int id)
    {
        var p = await _db.PhongTros.FirstOrDefaultAsync(x => x.Id == id && x.ChuTroId == UserId);
        if (p == null) return NotFound();

        p.ConPhong = !p.ConPhong;
        await _db.SaveChangesAsync();

        TempData["ThanhCong"] = p.ConPhong
            ? $"Đã mở lại phòng \"{p.TenNhaTro}\"."
            : $"Đã đánh dấu \"{p.TenNhaTro}\" là hết phòng.";
        return RedirectToAction(nameof(Index));
    }
    // ---------- XÓA ----------
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var p = await _db.PhongTros.FirstOrDefaultAsync(x => x.Id == id && x.ChuTroId == UserId);
        if (p == null) return NotFound();

        try
        {
            _db.PhongTros.Remove(p);
            await _db.SaveChangesAsync();
            _file.XoaAnh(p.HinhAnh);
            TempData["ThanhCong"] = "Đã xóa phòng trọ.";
        }
        catch (DbUpdateException)
        {
            // Phòng đã có đơn ở ghép, đánh giá... không xóa cứng được
            TempData["Loi"] = "Không thể xóa phòng đã có dữ liệu liên quan. Hãy chuyển sang 'Hết phòng' thay vì xóa.";
        }
        return RedirectToAction(nameof(Index));
    }
}