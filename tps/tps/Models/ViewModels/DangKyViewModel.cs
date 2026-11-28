using System.ComponentModel.DataAnnotations;

namespace tps.Models.ViewModels;

public class DangKyViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập họ tên"), StringLength(150)]
    [Display(Name = "Họ tên")]
    public string HoTen { get; set; } = null!;

    [Required(ErrorMessage = "Vui lòng nhập email hoặc số điện thoại")]
    [Display(Name = "Email hoặc số điện thoại")]
    public string TaiKhoan { get; set; } = null!;

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "Mật khẩu từ 6 đến 50 ký tự")]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu")]
    public string MatKhau { get; set; } = null!;

    [Compare("MatKhau", ErrorMessage = "Mật khẩu nhập lại không khớp")]
    [DataType(DataType.Password)]
    [Display(Name = "Nhập lại mật khẩu")]
    public string NhapLaiMatKhau { get; set; } = null!;

    [Required(ErrorMessage = "Vui lòng chọn vai trò")]
    [Display(Name = "Bạn là")]
    public string VaiTro { get; set; } = null!;   // "ChuTro" hoặc "SinhVien"
}