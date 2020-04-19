using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MTM.Data;
using MTM.Models;

namespace MTM.Pages
{
    public class LoginModel : PageModel
    {
        private readonly MTMContext _context;

        public LoginModel(MTMContext context)
        {
            _context = context;
        }

        [BindProperty]
        [Required(ErrorMessage = "Vui lòng nhập Tên Đăng Nhập")]
        [Display(Name = "Tên Đăng Nhập")]
        public string Username { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Vui lòng nhập Mật Khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật Khẩu")]
        public string Password { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            User user = _context.Users.Where(u => u.Username == Username).FirstOrDefault();
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Tên đăng nhập không tồn tại");
                return Page();
            }

            var passwordHasher = new PasswordHasher<string>();
            if (passwordHasher.VerifyHashedPassword(user.Username, user.Password, Password) == PasswordVerificationResult.Success)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username)
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToPage("/Disciples/index");
            }

            ModelState.AddModelError(string.Empty, "Mật khẩu không chính xác");
            return Page();
        }
    }
}
