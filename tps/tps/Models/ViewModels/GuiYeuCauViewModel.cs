using System.ComponentModel.DataAnnotations;

namespace tps.Models.ViewModels;

public class GuiYeuCauViewModel
{
    public int PhongId { get; set; }
    public string TenPhong { get; set; } = "";

    [StringLength(500, ErrorMessage = "Tối đa 500 ký tự")]
    [Display(Name = "Lời nhắn cho chủ trọ")]
    public string? LoiNhan { get; set; }
}