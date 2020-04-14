using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTM.Models
{
    public class Class
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Tên Lớp")]
        [StringLength(15)]
        [Display(Name = "Tên Lớp")]
        public string Name { get; set; }

        [StringLength(15)]
        [Display(Name = "Thành Phố")]
        public string City { get; set; }
    }
}
