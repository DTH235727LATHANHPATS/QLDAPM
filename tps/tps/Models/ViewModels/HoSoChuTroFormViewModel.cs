using System.ComponentModel.DataAnnotations;

namespace tps.Models.ViewModels;

public class HoSoChuTroFormViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập họ tên")]
    [StringLength(150)]
    [Display(Name = "Họ tên")]
    public string HoTen { get; set; } = null!;

    [RegularExpression(@"^(0|\+84)\d{9}$", ErrorMessage = "Số điện thoại không hợp lệ")]
    [Display(Name = "SĐT liên hệ")]
    public string? SdtLienHe { get; set; }

    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    [StringLength(100)]
    [Display(Name = "Email liên hệ")]
    public string? EmailLienHe { get; set; }

    [StringLength(300)]
    [Display(Name = "Địa chỉ")]
    public string? DiaChi { get; set; }
}