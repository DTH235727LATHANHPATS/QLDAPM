using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tps.Data;
using tps.Models.Entities;
using tps.Models.ViewModels;
using tps.Services.Interfaces;

namespace tps.Controllers;

public class TaiKhoanController : Controller
{
    private readonly PhongTroSVContext _db;
    private readonly IMatKhauService _matKhau;

    public TaiKhoanController(PhongTroSVContext db, IMatKhauService matKhau)
    {
        _db = db;
        _matKhau = matKhau;
    }

    // ---------- ĐĂNG KÝ ----------
    [HttpGet]
    public IActionResult DangKy() => View(new DangKyViewModel());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DangKy(DangKyViewModel model)
    {
        if (model.VaiTro != "ChuTro" && model.VaiTro != "SinhVien")
            ModelState.AddModelError(nameof(model.VaiTro), "Vai trò không hợp lệ");

        var input = model.TaiKhoan.Trim();
        bool laEmail = input.Contains('@');

        if (laEmail)
        {
            if (!new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(input))
                ModelState.AddModelError(nameof(model.TaiKhoan), "Email không hợp lệ");
        }
        else if (!System.Text.RegularExpressions.Regex.IsMatch(input, @"^(0|\+84)\d{9}$"))
        {
            ModelState.AddModelError(nameof(model.TaiKhoan), "Số điện thoại không hợp lệ");
        }

        if (ModelState.IsValid)
        {
            bool daTonTai = laEmail
                ? await _db.TaiKhoans.AnyAsync(t => t.Email == input)
                : await _db.TaiKhoans.AnyAsync(t => t.SoDienThoai == input);
            if (daTonTai)
                ModelState.AddModelError(nameof(model.TaiKhoan), "Email hoặc số điện thoại đã được đăng ký");
        }

        if (!ModelState.IsValid) return View(model);

        var vaiTro = await _db.VaiTros.FirstAsync(v => v.Ten == model.VaiTro);
        var tk = new TaiKhoan
        {
            Id = Guid.NewGuid().ToString(),
            HoTen = model.HoTen.Trim(),
            Email = laEmail ? input : null,
            SoDienThoai = laEmail ? null : input,
            TenDangNhap = input,
            MatKhau = _matKhau.Bam(model.MatKhau)
        };
        tk.VaiTros.Add(vaiTro);
        _db.TaiKhoans.Add(tk);
        await _db.SaveChangesAsync();

        TempData["ThanhCong"] = "Đăng ký thành công, vui lòng đăng nhập.";
        return RedirectToAction(nameof(DangNhap));
    }

    // ---------- ĐĂNG NHẬP ----------
    [HttpGet]
    public IActionResult DangNhap(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View(new DangNhapViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DangNhap(DangNhapViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (!ModelState.IsValid) return View(model);

        var input = model.TaiKhoan.Trim();
        var tk = await _db.TaiKhoans
            .Include(t => t.VaiTros)
            .FirstOrDefaultAsync(t => t.Email == input || t.SoDienThoai == input);

        if (tk == null || !_matKhau.KiemTra(tk.MatKhau, model.MatKhau))
        {
            ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, tk.Id),
            new(ClaimTypes.Name, tk.HoTen)
        };
        claims.AddRange(tk.VaiTros.Select(v => new Claim(ClaimTypes.Role, v.Ten)));

        var identity = new ClaimsIdentity(claims, "Cookies");
        await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(identity),
            new AuthenticationProperties { IsPersistent = model.GhiNho });

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);
        return RedirectToAction("Index", "Home");
    }

    // ---------- ĐĂNG XUẤT ----------
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DangXuat()
    {
        await HttpContext.SignOutAsync("Cookies");
        return RedirectToAction("Index", "Home");
    }

    public IActionResult TuChoiTruyCap() => View();
}