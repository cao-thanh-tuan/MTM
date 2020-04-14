using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MTM.Data;
using MTM.Models;

namespace MTM.Pages.Classes
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
        public Class Class { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var duplicate = _context.Classes.FirstOrDefault(c => c.Name == Class.Name);

            if (duplicate != null)
            {
                ModelState.AddModelError("Class.Name", "Tên lớp đã tồn tại");
                return Page();
            }

            _context.Classes.Add(Class);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
