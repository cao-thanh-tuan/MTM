using System.ComponentModel.DataAnnotations;

namespace MTM.Models
{
    public class User
    {
        [Key]
        [Required]
        [StringLength(15)]
        [RegularExpression(@"^[a-z0-9_-]{4,15}$")]
        [Display(Name = "Tên Đăng Nhập")]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật Khẩu")]
        public string Password { get; set; }
    }
}
