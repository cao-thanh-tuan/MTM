using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MTM.Models
{
    public class RegistrationInfo
    {
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^[0-9]{9,12}", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Phone { get; set; }

        public string InitiateDate { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }
    }
}
