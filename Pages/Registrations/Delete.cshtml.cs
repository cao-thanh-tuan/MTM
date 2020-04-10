using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MTM.Data;
using MTM.Models;

namespace MTM.Pages.Registrations
{
    public class DeleteModel : PageModel
    {
        private readonly MTM.Data.MTMContext _context;

        public DeleteModel(MTM.Data.MTMContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Registration Registration { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Registration = await _context.Registrations
                .Include(r => r.Disciple)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Registration == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Registration = await _context.Registrations
                .Include(r => r.Disciple)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Registration != null)
            {
                _context.Registrations.Remove(Registration);
                await _context.SaveChangesAsync();
         
                return RedirectToPage("../Disciples/Details", new { id = Registration.Disciple.ID });
            }
            else
            {
                return RedirectToPage("../Disciples/Index");
            }
        }
    }
}
