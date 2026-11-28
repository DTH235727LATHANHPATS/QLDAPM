using System.ComponentModel.DataAnnotations;

namespace tps.Models.ViewModels;

public class PhongTroFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên nhà trọ")]
    [StringLength(150)]
    [Display(Name = "Tên nhà trọ")]
    public string TenNhaTro { get; set; } = null!;

    [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
    [StringLength(300)]
    [Display(Name = "Địa chỉ")]
    public string DiaChi { get; set; } = null!;

    [Required(ErrorMessage = "Vui lòng chọn loại phòng")]
    [Display(Name = "Loại phòng")]
    public int? LoaiPhongId { get; set; }

    [Display(Name = "Mô tả")]
    public string? MoTa { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập diện tích")]
    [Range(1, 500, ErrorMessage = "Diện tích từ 1 đến 500 m²")]
    [Display(Name = "Diện tích (m²)")]
    public decimal? DienTich { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập giá thuê")]
    [Range(0, 100000000, ErrorMessage = "Giá thuê không hợp lệ")]
    [Display(Name = "Giá thuê (đ/tháng)")]
    public decimal? GiaThue { get; set; }

    [Range(0, 100000, ErrorMessage = "Giá điện không hợp lệ")]
    [Display(Name = "Giá điện (đ/kWh)")]
    public decimal? GiaDien { get; set; }

    [Range(0, 1000000, ErrorMessage = "Giá nước không hợp lệ")]
    [Display(Name = "Giá nước")]
    public decimal? GiaNuoc { get; set; }

    [Range(0, 100000000, ErrorMessage = "Tiền cọc không hợp lệ")]
    [Display(Name = "Tiền cọc")]
    public decimal? TienCoc { get; set; }

    [Display(Name = "Còn phòng")]
    public bool ConPhong { get; set; } = true;

    [Display(Name = "Tìm người ở ghép")]
    public bool TimOGhep { get; set; }

    [Display(Name = "Ảnh phòng")]
    public IFormFile? AnhMoi { get; set; }

    public string? HinhAnhHienTai { get; set; }
}