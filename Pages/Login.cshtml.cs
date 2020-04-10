using System.Collections.Generic;
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
        public User Login { get; set; }

        [BindProperty]
        public string Message { get; set; }

        public void OnGet()
        {
            Message = "";
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            User user = _context.Users.Where(u => u.Username == Login.Username).FirstOrDefault();
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Không tìm thấy tên đăng nhập này");
                return Page();
            }

            var passwordHasher = new PasswordHasher<string>();
            if (passwordHasher.VerifyHashedPassword(null, user.Password, Login.Password) == PasswordVerificationResult.Success)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, Login.Username)
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
