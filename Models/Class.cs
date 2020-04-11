using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTM.Models
{
    public class Class
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        [Required]
        [StringLength(15)]
        [Display(Name = "Tên Lớp")]
        public string Name { get; set; }

        [StringLength(15)]
        [Display(Name = "Thành Phố")]
        public string City { get; set; }
    }
}
