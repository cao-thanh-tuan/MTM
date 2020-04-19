using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTM.Models
{
    public class User
    {
        [Key]
        [Required(ErrorMessage = "Vui lòng nhập Tên Đăng Nhập")]
        [StringLength(15)]
        [RegularExpression(@"^[a-zA-Z0-9]{4,15}$", ErrorMessage = "Tên Đăng Nhập chỉ chứa số hoặc ký tự và dài từ 4 đến 15 ký tự")]
        [Display(Name = "Tên Đăng Nhập")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Mật Khẩu")]
        [StringLength(100)]
        [RegularExpression(@"^[a-zA-Z0-9]{4,15}$", ErrorMessage = "Mật Khẩu chỉ chứa số hoặc ký tự và dài từ 4 đến 15 ký tự")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật Khẩu")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Xác Nhận Mật Khẩu")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Xác Nhận Mật Khẩu và Mật Khẩu không giống nhau")]
        [NotMapped]
        [Display(Name = "Xác Nhận Mật Khẩu")]
        public string ConfirmPassword { get; set; }
    }
}
