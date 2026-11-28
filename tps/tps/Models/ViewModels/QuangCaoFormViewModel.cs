using System.ComponentModel.DataAnnotations;

namespace tps.Models.ViewModels;

public class QuangCaoFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
    [StringLength(200)]
    [Display(Name = "Tiêu đề")]
    public string TieuDe { get; set; } = null!;

    [StringLength(1000)]
    [Display(Name = "Nội dung")]
    public string? NoiDung { get; set; }

    [StringLength(300)]
    [Display(Name = "Đường dẫn khi bấm vào (link)")]
    public string? DuongDan { get; set; }

    [Display(Name = "Vị trí hiển thị")]
    public byte ViTri { get; set; }   // 0 Banner trang chủ, 1 Cột bên, 2 Đầu trang tìm kiếm

    [Display(Name = "Ngày bắt đầu")]
    public DateTime NgayBatDau { get; set; } = DateTime.Today;

    [Display(Name = "Ngày kết thúc")]
    public DateTime? NgayKetThuc { get; set; }

    [Display(Name = "Đang hiển thị")]
    public bool HienThi { get; set; } = true;

    [Display(Name = "Ảnh quảng cáo")]
    public IFormFile? AnhMoi { get; set; }

    public string? HinhAnhHienTai { get; set; }
}