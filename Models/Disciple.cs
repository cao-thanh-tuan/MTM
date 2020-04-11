using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MTM.Models
{
    public class Disciple
    {
        public Disciple()
        {
            MeditaionRegisters = new List<Registration>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [StringLength(15)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số Điện Thoại")]
        public string Phone { get; set; }
        [StringLength(20)]
        [Display(Name = "Tên")]
        public string FirstName { get; set; }
        [StringLength(30)]
        [Display(Name = "Tên Lót")]
        public string MiddleName { get; set; }
        [StringLength(20)]
        [Display(Name = "Họ")]
        public string LastName { get; set; }
        [Display(Name = "Họ Tên")]
        public string FullName
        {
            get
            {
                string[] arr = { LastName, MiddleName, FirstName };
                return String.Join(" ", arr.Where(s => !String.IsNullOrEmpty(s)));
            }
        }
        [StringLength(50)]
        [Display(Name = "Địa Chỉ")]
        public string Address { get; set; }
        [StringLength(10)]
        [Display(Name = "Giới Tính")]
        public string Gender { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Ngày Sinh")]
        public Nullable<DateTime> DateOfBirth { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Ngày Thọ")]
        public Nullable<DateTime> InitiateDate { get; set; }
        [Display(Name = "Đăng Ký Thiền")]
        public ICollection<Registration> MeditaionRegisters { get; set; }
    }

    public class Gender
    {
        public const string MALE = "Nam";
        public const string FEMALE = "Nữ";
        public const string UNKNOWN = "";
    }
}
