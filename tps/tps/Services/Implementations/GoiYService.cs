using Microsoft.EntityFrameworkCore;
using tps.Data;
using tps.Models.Entities;
using tps.Services.Interfaces;

namespace tps.Services.Implementations;

public class GoiYService : IGoiYService
{
    private readonly PhongTroSVContext _db;
    public GoiYService(PhongTroSVContext db) => _db = db;

    public async Task<List<GoiYKetQua>> GoiYBanOGhepAsync(string sinhVienId, int top = 10)
    {
        var toi = await _db.HoSoSinhViens.AsNoTracking()
            .Include(h => h.SoThiches).Include(h => h.LuaChons)
            .FirstOrDefaultAsync(h => h.TaiKhoanId == sinhVienId);
        if (toi == null) return new();

        var nhoms = await _db.NhomThoiQuens.AsNoTracking().Include(n => n.LuaChons).ToListAsync();

        var ungViens = await _db.HoSoSinhViens.AsNoTracking()
            .Include(h => h.SoThiches).Include(h => h.LuaChons)
            .Where(h => h.DangTimOGhep && h.TaiKhoanId != sinhVienId)
            .ToListAsync();

        var ids = ungViens.Select(u => u.TaiKhoanId).ToList();
        var ten = await _db.TaiKhoans.AsNoTracking()
            .Where(t => ids.Contains(t.Id)).ToDictionaryAsync(t => t.Id, t => t.HoTen);

        var kq = new List<GoiYKetQua>();
        foreach (var u in ungViens)
        {
            double diem = TinhThoiQuen(toi, u, nhoms) * 70
                        + TinhSoThich(toi, u) * 20
                        + (NganSachGiao(toi, u) ? 10 : 0);
            kq.Add(new GoiYKetQua
            {
                HoSo = u,
                HoTen = ten.GetValueOrDefault(u.TaiKhoanId, "(không tên)"),
                Diem = (int)Math.Round(diem)
            });
        }
        return kq.OrderByDescending(x => x.Diem).Take(top).ToList();
    }

    // trả về 0..1
    private static double TinhThoiQuen(HoSoSinhVien a, HoSoSinhVien b, List<NhomThoiQuen> nhoms)
    {
        double tong = 0, tongTrongSo = 0;
        foreach (var n in nhoms)
        {
            var oa = a.LuaChons.FirstOrDefault(o => o.NhomId == n.Id);
            var ob = b.LuaChons.FirstOrDefault(o => o.NhomId == n.Id);
            if (oa == null || ob == null) continue;

            var gt = n.LuaChons.Select(o => o.GiaTri).ToList();
            double khoang = gt.Count > 0 ? gt.Max() - gt.Min() : 0;
            double giong = khoang == 0 ? 1 : 1 - Math.Abs(oa.GiaTri - ob.GiaTri) / khoang;

            double w = (double)n.TrongSo;
            tong += giong * w;
            tongTrongSo += w;
        }
        return tongTrongSo == 0 ? 0 : tong / tongTrongSo;
    }

    private static double TinhSoThich(HoSoSinhVien a, HoSoSinhVien b)
    {
        var sa = a.SoThiches.Select(s => s.Id).ToHashSet();
        var sb = b.SoThiches.Select(s => s.Id).ToHashSet();
        int hop = sa.Union(sb).Count();
        return hop == 0 ? 0 : (double)sa.Intersect(sb).Count() / hop;
    }

    private static bool NganSachGiao(HoSoSinhVien a, HoSoSinhVien b)
    {
        if (a.NganSachTu == null || a.NganSachDen == null || b.NganSachTu == null || b.NganSachDen == null)
            return false;
        return a.NganSachTu <= b.NganSachDen && b.NganSachTu <= a.NganSachDen;
    }
}