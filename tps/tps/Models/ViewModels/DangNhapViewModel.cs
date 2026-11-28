using System.ComponentModel.DataAnnotations;

namespace tps.Models.ViewModels;

public class DangNhapViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập email hoặc số điện thoại")]
    [Display(Name = "Email hoặc số điện thoại")]
    public string TaiKhoan { get; set; } = null!;

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu")]
    public string MatKhau { get; set; } = null!;

    [Display(Name = "Ghi nhớ đăng nhập")]
    public bool GhiNho { get; set; }
}