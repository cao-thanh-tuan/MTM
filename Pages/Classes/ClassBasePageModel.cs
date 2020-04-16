using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MTM.Data;
using MTM.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MTM.Pages.Classes
{
    public class ClassBasePageModel : PageModel
    {
        protected readonly MTMContext _context;

        [BindProperty]
        public Class Class { get; set; }

        public ClassBasePageModel(MTMContext context)
        {
            _context = context;
        }

        public SelectList CitySL { get; set; }

        protected Task<Class> GetClass(int? id)
        {
            return _context.Classes.FirstOrDefaultAsync(m => m.ID == id);
        }

        protected bool ClassExists(int classId, string name)
        {
            var duplicate = _context.Classes.FirstOrDefault(d => d.Name == name && d.ID != classId);

            return duplicate != null;
        }

        protected void PopulateCitiesDropDownList(string selectedCity = null)
        {
            CitySL = new SelectList(new SelectListItem[] {
                new SelectListItem { Value = "TP HCM", Text = "TP HCM" },
                new SelectListItem { Value = "Đà Nẵng", Text = "Đà Nẵng" },
                new SelectListItem { Value = "Hà Nội", Text = "Hà Nội" },
                new SelectListItem { Value = "An Giang", Text = "An Giang" },
                new SelectListItem { Value = "Bà Rịa - Vũng Tàu", Text = "Bà Rịa - Vũng Tàu" },
                new SelectListItem { Value = "Bắc Giang", Text = "Bắc Giang" },
                new SelectListItem { Value = "Bắc Kạn", Text = "Bắc Kạn" },
                new SelectListItem { Value = "Bạc Liêu", Text = "Bạc Liêu" },
                new SelectListItem { Value = "Bắc Ninh", Text = "Bắc Ninh" },
                new SelectListItem { Value = "Bến Tre", Text = "Bến Tre" },
                new SelectListItem { Value = "Bình Định", Text = "Bình Định" },
                new SelectListItem { Value = "Bình Dương", Text = "Bình Dương" },
                new SelectListItem { Value = "Bình Phước", Text = "Bình Phước" },
                new SelectListItem { Value = "Bình Thuận", Text = "Bình Thuận" },
                new SelectListItem { Value = "Cà Mau", Text = "Cà Mau" },
                new SelectListItem { Value = "Cao Bằng", Text = "Cao Bằng" },
                new SelectListItem { Value = "Cần Thơ", Text = "Cần Thơ" },
                new SelectListItem { Value = "Đắk Lắk", Text = "Đắk Lắk" },
                new SelectListItem { Value = "Đắk Nông", Text = "Đắk Nông" },
                new SelectListItem { Value = "Điện Biên", Text = "Điện Biên" },
                new SelectListItem { Value = "Đồng Nai", Text = "Đồng Nai" },
                new SelectListItem { Value = "Đồng Tháp", Text = "Đồng Tháp" },
                new SelectListItem { Value = "Gia Lai", Text = "Gia Lai" },
                new SelectListItem { Value = "Hà Giang", Text = "Hà Giang" },
                new SelectListItem { Value = "Hà Nam", Text = "Hà Nam" },
                new SelectListItem { Value = "Hà Tĩnh", Text = "Hà Tĩnh" },
                new SelectListItem { Value = "Hải Dương", Text = "Hải Dương" },
                new SelectListItem { Value = "Hải Phòng", Text = "Hải Phòng" },
                new SelectListItem { Value = "Hậu Giang", Text = "Hậu Giang" },
                new SelectListItem { Value = "Hòa Bình", Text = "Hòa Bình" },
                new SelectListItem { Value = "Hưng Yên", Text = "Hưng Yên" },
                new SelectListItem { Value = "Khánh Hòa", Text = "Khánh Hòa" },
                new SelectListItem { Value = "Kiên Giang", Text = "Kiên Giang" },
                new SelectListItem { Value = "Kon Tum", Text = "Kon Tum" },
                new SelectListItem { Value = "Lai Châu", Text = "Lai Châu" },
                new SelectListItem { Value = "Lâm Đồng", Text = "Lâm Đồng" },
                new SelectListItem { Value = "Lạng Sơn", Text = "Lạng Sơn" },
                new SelectListItem { Value = "Lào Cai", Text = "Lào Cai" },
                new SelectListItem { Value = "Long An", Text = "Long An" },
                new SelectListItem { Value = "Nam Định", Text = "Nam Định" },
                new SelectListItem { Value = "Nghệ An", Text = "Nghệ An" },
                new SelectListItem { Value = "Ninh Bình", Text = "Ninh Bình" },
                new SelectListItem { Value = "Ninh Thuận", Text = "Ninh Thuận" },
                new SelectListItem { Value = "Phú Thọ", Text = "Phú Thọ" },
                new SelectListItem { Value = "Quảng Bình", Text = "Quảng Bình" },
                new SelectListItem { Value = "Quảng Nam", Text = "Quảng Nam" },
                new SelectListItem { Value = "Quảng Ngãi", Text = "Quảng Ngãi" },
                new SelectListItem { Value = "Quảng Ninh", Text = "Quảng Ninh" },
                new SelectListItem { Value = "Quảng Trị", Text = "Quảng Trị" },
                new SelectListItem { Value = "Sóc Trăng", Text = "Sóc Trăng" },
                new SelectListItem { Value = "Sơn La", Text = "Sơn La" },
                new SelectListItem { Value = "Tây Ninh", Text = "Tây Ninh" },
                new SelectListItem { Value = "Thái Bình", Text = "Thái Bình" },
                new SelectListItem { Value = "Thái Nguyên", Text = "Thái Nguyên" },
                new SelectListItem { Value = "Thanh Hóa", Text = "Thanh Hóa" },
                new SelectListItem { Value = "Thừa Thiên Huế", Text = "Thừa Thiên Huế" },
                new SelectListItem { Value = "Tiền Giang", Text = "Tiền Giang" },
                new SelectListItem { Value = "Trà Vinh", Text = "Trà Vinh" },
                new SelectListItem { Value = "Tuyên Quang", Text = "Tuyên Quang" },
                new SelectListItem { Value = "Vĩnh Long", Text = "Vĩnh Long" },
                new SelectListItem { Value = "Vĩnh Phúc", Text = "Vĩnh Phúc" },
                new SelectListItem { Value = "Yên Bái", Text = "Yên Bái" },
                new SelectListItem { Value = "Phú Yên", Text = "Phú Yên" }
            }, "Value", "Text", selectedCity);
        }
    }
}
