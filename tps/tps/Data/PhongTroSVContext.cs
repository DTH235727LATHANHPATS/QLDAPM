using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using tps.Models.Entities;

namespace tps.Data;

public partial class PhongTroSVContext : DbContext
{
    public PhongTroSVContext(DbContextOptions<PhongTroSVContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BaoCaoViPham> BaoCaoViPhams { get; set; }

    public virtual DbSet<DanhGium> DanhGia { get; set; }

    public virtual DbSet<HoSoChuTro> HoSoChuTros { get; set; }

    public virtual DbSet<HoSoSinhVien> HoSoSinhViens { get; set; }

    public virtual DbSet<LoaiPhong> LoaiPhongs { get; set; }

    public virtual DbSet<LuaChonThoiQuen> LuaChons { get; set; }

    public virtual DbSet<NhomThoiQuen> NhomThoiQuens { get; set; }

    public virtual DbSet<PhongTro> PhongTros { get; set; }

    public virtual DbSet<PhongYeuThich> PhongYeuThiches { get; set; }

    public virtual DbSet<QuangCao> QuangCaos { get; set; }

    public virtual DbSet<SoThich> SoThiches { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<TruongDaiHoc> TruongDaiHocs { get; set; }

    public virtual DbSet<VaiTro> VaiTros { get; set; }

    public virtual DbSet<YeuCauOGhep> YeuCauOGheps { get; set; }

    public virtual DbSet<YeuCauTaiKhoan> YeuCauTaiKhoans { get; set; }

    public virtual DbSet<YeuCauVaiTro> YeuCauVaiTros { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaoCaoViPham>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__bao_cao___3213E83FC5C79C4F");

            entity.Property(e => e.NgayTao).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.DanhGia).WithMany(p => p.BaoCaoViPhams).HasConstraintName("FK__bao_cao_v__danh___01142BA1");

            entity.HasOne(d => d.NguoiBaoCao).WithMany(p => p.BaoCaoViPhamNguoiBaoCaos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__bao_cao_v__nguoi__7F2BE32F");

            entity.HasOne(d => d.NguoiBiBaoCao).WithMany(p => p.BaoCaoViPhamNguoiBiBaoCaos).HasConstraintName("FK__bao_cao_v__nguoi__02084FDA");

            entity.HasOne(d => d.NguoiXuLy).WithMany(p => p.BaoCaoViPhamNguoiXuLies).HasConstraintName("FK__bao_cao_v__nguoi__02FC7413");

            entity.HasOne(d => d.Phong).WithMany(p => p.BaoCaoViPhams)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__bao_cao_v__phong__00200768");
        });

        modelBuilder.Entity<DanhGium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__danh_gia__3213E83F2DE0C21D");

            entity.Property(e => e.HienThi).HasDefaultValue(true);
            entity.Property(e => e.NgayTao).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Phong).WithMany(p => p.DanhGia).HasConstraintName("FK__danh_gia__phong___787EE5A0");

            entity.HasOne(d => d.SinhVien).WithMany(p => p.DanhGia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__danh_gia__sinh_v__797309D9");
        });

        modelBuilder.Entity<HoSoChuTro>(entity =>
        {
            entity.HasKey(e => e.TaiKhoanId).HasName("PK__ho_so_ch__1284327C74C8E20D");

            entity.HasOne(d => d.TaiKhoan).WithOne(p => p.HoSoChuTro).HasConstraintName("FK_hsct_tai_khoan");
        });

        modelBuilder.Entity<HoSoSinhVien>(entity =>
        {
            entity.HasKey(e => e.TaiKhoanId).HasName("PK__ho_so_si__1284327CD6A4EC5B");

            entity.HasOne(d => d.TaiKhoan).WithOne(p => p.HoSoSinhVien).HasConstraintName("FK_hssv_tai_khoan");

            entity.HasOne(d => d.Truong).WithMany(p => p.HoSoSinhViens).HasConstraintName("FK_hssv_truong");

            entity.HasMany(d => d.LuaChons).WithMany(p => p.SinhViens)
                .UsingEntity<Dictionary<string, object>>(
                    "SinhVienThoiQuen",
                    r => r.HasOne<LuaChonThoiQuen>().WithMany()
                        .HasForeignKey("LuaChonId")
                        .HasConstraintName("FK__sinh_vien__lua_c__5EBF139D"),
                    l => l.HasOne<HoSoSinhVien>().WithMany()
                        .HasForeignKey("SinhVienId")
                        .HasConstraintName("FK__sinh_vien__sinh___5DCAEF64"),
                    j =>
                    {
                        j.HasKey("SinhVienId", "LuaChonId").HasName("PK__sinh_vie__C115AA9428ACEB3E");
                        j.ToTable("sinh_vien_thoi_quen");
                        j.IndexerProperty<string>("SinhVienId").HasColumnName("sinh_vien_id");
                        j.IndexerProperty<int>("LuaChonId").HasColumnName("lua_chon_id");
                    });

            entity.HasMany(d => d.SoThiches).WithMany(p => p.SinhViens)
                .UsingEntity<Dictionary<string, object>>(
                    "SinhVienSoThich",
                    r => r.HasOne<SoThich>().WithMany()
                        .HasForeignKey("SoThichId")
                        .HasConstraintName("FK__sinh_vien__so_th__534D60F1"),
                    l => l.HasOne<HoSoSinhVien>().WithMany()
                        .HasForeignKey("SinhVienId")
                        .HasConstraintName("FK__sinh_vien__sinh___52593CB8"),
                    j =>
                    {
                        j.HasKey("SinhVienId", "SoThichId").HasName("PK__sinh_vie__EB35ABD5FB2A06DF");
                        j.ToTable("sinh_vien_so_thich");
                        j.IndexerProperty<string>("SinhVienId").HasColumnName("sinh_vien_id");
                        j.IndexerProperty<int>("SoThichId").HasColumnName("so_thich_id");
                    });
        });

        modelBuilder.Entity<LoaiPhong>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__loai_pho__3213E83F3ECB1BC1");
        });

        modelBuilder.Entity<LuaChonThoiQuen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__lua_chon__3213E83FADAC1354");

            entity.HasOne(d => d.Nhom).WithMany(p => p.LuaChons).HasConstraintName("FK__lua_chon___nhom___5AEE82B9");
        });

        modelBuilder.Entity<NhomThoiQuen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__nhom_tho__3213E83FAD750E7A");

            entity.Property(e => e.TrongSo).HasDefaultValue(10m);
        });

        modelBuilder.Entity<PhongTro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__phong_tr__3213E83F6F0381ED");

            entity.Property(e => e.ConPhong).HasDefaultValue(true);

            entity.HasOne(d => d.ChuTro).WithMany(p => p.PhongTros)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_phong_chu_tro");

            entity.HasOne(d => d.LoaiPhong).WithMany(p => p.PhongTros)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_phong_loai");
        });

        modelBuilder.Entity<PhongYeuThich>(entity =>
        {
            entity.HasKey(e => new { e.SinhVienId, e.PhongId }).HasName("PK__phong_ye__DB284A233CA3BCAA");

            entity.Property(e => e.NgayLuu).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Phong).WithMany(p => p.PhongYeuThiches).HasConstraintName("FK__phong_yeu__phong__6C190EBB");

            entity.HasOne(d => d.SinhVien).WithMany(p => p.PhongYeuThiches).HasConstraintName("FK__phong_yeu__sinh___6B24EA82");
        });

        modelBuilder.Entity<QuangCao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__quang_ca__3213E83F0ED3F83B");

            entity.Property(e => e.HienThi).HasDefaultValue(true);
            entity.Property(e => e.NgayBatDau).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.NgayTao).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.NguoiDang).WithMany(p => p.QuangCaos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__quang_cao__nguoi__09A971A2");
        });

        modelBuilder.Entity<SoThich>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__so_thich__3213E83F78A32E63");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tai_khoa__3213E83F1A6B8D06");

            entity.HasIndex(e => e.Email, "UX_tai_khoan_email")
                .IsUnique()
                .HasFilter("([email] IS NOT NULL)");

            entity.HasIndex(e => e.SoDienThoai, "UX_tai_khoan_sdt")
                .IsUnique()
                .HasFilter("([so_dien_thoai] IS NOT NULL)");

            entity.HasIndex(e => e.TenDangNhap, "UX_tai_khoan_ten_dang_nhap")
                .IsUnique()
                .HasFilter("([ten_dang_nhap] IS NOT NULL)");

            entity.Property(e => e.HoTen).HasDefaultValue("");

            entity.HasMany(d => d.VaiTros).WithMany(p => p.TaiKhoans)
                .UsingEntity<Dictionary<string, object>>(
                    "VaiTroNguoiDung",
                    r => r.HasOne<VaiTro>().WithMany()
                        .HasForeignKey("VaiTroId")
                        .HasConstraintName("FK_vtnd_vai_tro"),
                    l => l.HasOne<TaiKhoan>().WithMany()
                        .HasForeignKey("TaiKhoanId")
                        .HasConstraintName("FK_vtnd_tai_khoan"),
                    j =>
                    {
                        j.HasKey("TaiKhoanId", "VaiTroId");
                        j.ToTable("vai_tro_nguoi_dung");
                        j.HasIndex(new[] { "VaiTroId" }, "IX_vtnd_vai_tro");
                        j.IndexerProperty<string>("TaiKhoanId").HasColumnName("tai_khoan_id");
                        j.IndexerProperty<string>("VaiTroId").HasColumnName("vai_tro_id");
                    });
        });

        modelBuilder.Entity<TruongDaiHoc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__truong_d__3213E83FEBF98A2C");
        });

        modelBuilder.Entity<VaiTro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__vai_tro__3213E83F57ED1E97");
        });

        modelBuilder.Entity<YeuCauOGhep>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__yeu_cau___3213E83F1C0EE4D9");

            entity.Property(e => e.NgayTao).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Phong).WithMany(p => p.YeuCauOGheps).HasConstraintName("FK__yeu_cau_o__phong__71D1E811");

            entity.HasOne(d => d.SinhVien).WithMany(p => p.YeuCauOGheps)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__yeu_cau_o__sinh___72C60C4A");
        });

        modelBuilder.Entity<YeuCauTaiKhoan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__yeu_cau___3213E83F7A34ED28");

            entity.HasOne(d => d.TaiKhoan).WithMany(p => p.YeuCauTaiKhoans).HasConstraintName("FK_yctk_tai_khoan");
        });

        modelBuilder.Entity<YeuCauVaiTro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__yeu_cau___3213E83F01BDEF20");

            entity.HasOne(d => d.VaiTro).WithMany(p => p.YeuCauVaiTros).HasConstraintName("FK_ycvt_vai_tro");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
