using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MTM.Data;
using MTM.Models;

namespace MTM.Pages.Users
{
    public class ChangePasswordModel : PageModel
    {
        private readonly MTM.Data.MTMContext _context;

        public ChangePasswordModel(MTM.Data.MTMContext context)
        {
            _context = context;
        }

        [BindProperty]
        [Required(ErrorMessage = "Vui lòng nhập Mật Khẩu Cũ")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật Khẩu Cũ")]
        public string OldPassword { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Vui lòng nhập Mật Khẩu Mới")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^[a-zA-Z0-9]{4,15}$", ErrorMessage = "Mật Khẩu chỉ chứa số hoặc ký tự và dài từ 4 đến 15 ký tự")]
        [Display(Name = "Mật Khẩu Mới")]
        public string NewPassword { get; set; }

        public IActionResult OnGet()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var currentUser = await GetUser(HttpContext.User.Identity.Name);
            if (currentUser == null)
            {
                return NotFound();
            }
            
            var passwordHasher = new PasswordHasher<string>();
            if (passwordHasher.VerifyHashedPassword(currentUser.Username, currentUser.Password, OldPassword) != PasswordVerificationResult.Success)
            {
                ModelState.AddModelError("OldPassword", "Mật khẩu cũ không chính xác");
                return Page();
            }

            _context.Attach(currentUser).State = EntityState.Modified;
            currentUser.Password = passwordHasher.HashPassword(currentUser.Username, NewPassword) ;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return RedirectToPage("./Index");
        }

        private Task<User> GetUser(string username)
        {
            return _context.Users.FirstOrDefaultAsync(e => e.Username == username);
        }
    }
}
