using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tps.Data;
using tps.Models.ViewModels;

namespace tps.Controllers;

public class TimKiemController : Controller
{
    private const int KichThuocTrang = 9;
    private readonly PhongTroSVContext _db;

    public TimKiemController(PhongTroSVContext db) => _db = db;

    public async Task<IActionResult> Index([FromQuery] TimKiemViewModel q)
    {
        var query = _db.PhongTros
            .AsNoTracking()
            .Include(p => p.LoaiPhong)
            .AsQueryable();

        // Từ khóa: tìm không phân biệt hoa thường và không dấu
        if (!string.IsNullOrWhiteSpace(q.TuKhoa))
        {
            var k = q.TuKhoa.Trim();
            query = query.Where(p =>
                EF.Functions.Collate(p.TenNhaTro, "Vietnamese_CI_AI").Contains(k) ||
                EF.Functions.Collate(p.DiaChi, "Vietnamese_CI_AI").Contains(k) ||
                (p.MoTa != null && EF.Functions.Collate(p.MoTa, "Vietnamese_CI_AI").Contains(k)));
        }

        if (q.GiaTu.HasValue) query = query.Where(p => p.GiaThue >= q.GiaTu.Value);
        if (q.GiaDen.HasValue) query = query.Where(p => p.GiaThue <= q.GiaDen.Value);
        if (q.DienTichTu.HasValue) query = query.Where(p => p.DienTich >= q.DienTichTu.Value);
        if (q.DienTichDen.HasValue) query = query.Where(p => p.DienTich <= q.DienTichDen.Value);
        if (q.LoaiPhongId.HasValue) query = query.Where(p => p.LoaiPhongId == q.LoaiPhongId.Value);
        if (q.ChiConPhong) query = query.Where(p => p.ConPhong);
        if (q.TimOGhep) query = query.Where(p => p.TimOGhep);

        query = q.Sort switch
        {
            "giatang" => query.OrderBy(p => p.GiaThue),
            "giagiam" => query.OrderByDescending(p => p.GiaThue),
            "dientich" => query.OrderByDescending(p => p.DienTich),
            _ => query.OrderByDescending(p => p.Id)
        };

        // Phân trang
        q.TongSo = await query.CountAsync();
        q.TongTrang = (int)Math.Ceiling(q.TongSo / (double)KichThuocTrang);
        if (q.Trang < 1) q.Trang = 1;
        if (q.TongTrang > 0 && q.Trang > q.TongTrang) q.Trang = q.TongTrang;

        q.KetQua = await query
            .Skip((q.Trang - 1) * KichThuocTrang)
            .Take(KichThuocTrang)
            .ToListAsync();

        var loai = await _db.LoaiPhongs.AsNoTracking().OrderBy(l => l.Id).ToListAsync();
        ViewBag.LoaiPhongs = new SelectList(loai, "Id", "Ten", q.LoaiPhongId);
        var now = DateTime.Now;
        ViewBag.QuangCaos = await _db.QuangCaos.AsNoTracking()
            .Where(a => a.HienThi && a.NgayBatDau <= now && (a.NgayKetThuc == null || a.NgayKetThuc >= now))
            .OrderByDescending(a => a.Id)
            .Take(3)
            .ToListAsync();
        return View(q);
    }

    public async Task<IActionResult> ChiTiet(int id)
    {
        var p = await _db.PhongTros
            .AsNoTracking()
            .Include(x => x.LoaiPhong)
            .Include(x => x.ChuTro)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (p == null) return NotFound();
        return View(p);
    }
}