using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MTM.Data;
using MTM.Models;

namespace MTM.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly MTM.Data.MTMContext _context;

        public CreateModel(MTM.Data.MTMContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public User LoginUser { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            User user = _context.Users.Where(u => u.Username == LoginUser.Username).FirstOrDefault();
            if (user != null)
            {
                ModelState.AddModelError(string.Empty, "Tên Đăng Nhập đã tồn tại");
                return Page();
            }

            var passwordHasher = new PasswordHasher<string>();
            LoginUser.Password = passwordHasher.HashPassword(LoginUser.Username, LoginUser.Password);

            _context.Users.Add(LoginUser);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
