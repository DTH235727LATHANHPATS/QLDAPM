using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tps.Data;
using tps.Models;
using tps.Models.ViewModels;

namespace tps.Controllers;

public class HomeController : Controller
{
    private readonly PhongTroSVContext _db;
    public HomeController(PhongTroSVContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var now = DateTime.Now;

        var vm = new TrangChuViewModel
        {
            PhongMoi = await _db.PhongTros.AsNoTracking()
                .Include(p => p.LoaiPhong)
                .Where(p => p.ConPhong)
                .OrderByDescending(p => p.Id)
                .Take(6).ToListAsync(),

            QuangCaos = await _db.QuangCaos.AsNoTracking()
                .Where(a => a.HienThi && a.ViTri == 1
                         && a.NgayBatDau <= now && (a.NgayKetThuc == null || a.NgayKetThuc >= now))
                .OrderByDescending(a => a.Id)
                .Take(3).ToListAsync(),

            LoaiPhongs = await _db.LoaiPhongs.AsNoTracking().OrderBy(l => l.Id).ToListAsync()
        };
        return View(vm);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
        => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}