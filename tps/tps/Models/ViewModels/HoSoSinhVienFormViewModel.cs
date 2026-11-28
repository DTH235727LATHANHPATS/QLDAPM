using System.ComponentModel.DataAnnotations;

namespace tps.Models.ViewModels;

public class HoSoSinhVienFormViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập họ tên")]
    [StringLength(150)]
    [Display(Name = "Họ tên")]
    public string HoTen { get; set; } = null!;

    [Display(Name = "Trường đại học")]
    public int? TruongId { get; set; }

    [StringLength(30)]
    [Display(Name = "Mã sinh viên")]
    public string? MaSinhVien { get; set; }

    [Display(Name = "Giới tính")]
    public byte? GioiTinh { get; set; }

    [Display(Name = "Ngày sinh")]
    public DateOnly? NgaySinh { get; set; }

    [StringLength(100)]
    [Display(Name = "Ngành học")]
    public string? NganhHoc { get; set; }

    [StringLength(500, ErrorMessage = "Tối đa 500 ký tự")]
    [Display(Name = "Giới thiệu bản thân")]
    public string? GioiThieu { get; set; }

    [Range(0, 100000000, ErrorMessage = "Ngân sách không hợp lệ")]
    [Display(Name = "Ngân sách từ (đ/tháng)")]
    public decimal? NganSachTu { get; set; }

    [Range(0, 100000000, ErrorMessage = "Ngân sách không hợp lệ")]
    [Display(Name = "Ngân sách đến (đ/tháng)")]
    public decimal? NganSachDen { get; set; }

    [Display(Name = "Tôi đang tìm người ở ghép")]
    public bool DangTimOGhep { get; set; }

    public List<int> SoThichIds { get; set; } = new();

    // key = id nhóm thói quen, value = id lựa chọn đã chọn
    public Dictionary<int, int?> ThoiQuen { get; set; } = new();
}