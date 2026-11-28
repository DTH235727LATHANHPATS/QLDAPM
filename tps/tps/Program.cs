using Microsoft.EntityFrameworkCore;
using tps.Data;
using tps.Services.Implementations;
using tps.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// 1. DbContext
builder.Services.AddDbContext<PhongTroSVContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Cookie Authentication (thay cho Identity đầy đủ)
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/TaiKhoan/DangNhap";
        options.AccessDeniedPath = "/TaiKhoan/TuChoiTruyCap";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.Cookie.HttpOnly = true;
    });
builder.Services.AddAuthorization();

// 3. Service
builder.Services.AddScoped<IMatKhauService, MatKhauService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IGoiYService, GoiYService>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o =>
{
    o.IdleTimeout = TimeSpan.FromHours(2);
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
});
var app = builder.Build();

// Seed tài khoản Admin lần đầu
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PhongTroSVContext>();
    var hasher = scope.ServiceProvider.GetRequiredService<IMatKhauService>();

    if (!db.TaiKhoans.Any(t => t.Email == "admin@phongtro.vn"))
    {
        var vaiTroAdmin = db.VaiTros.First(v => v.Ten == "Admin");
        var admin = new tps.Models.Entities.TaiKhoan
        {
            Id = Guid.NewGuid().ToString(),
            TenDangNhap = "admin",
            Email = "admin@phongtro.vn",
            MatKhau = hasher.Bam("Admin@123"),
            HoTen = "Quản trị viên"
        };
        admin.VaiTros.Add(vaiTroAdmin);
        db.TaiKhoans.Add(admin);
        db.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();   // luôn đứng TRƯỚC UseAuthorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();