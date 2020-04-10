using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MTM.Data;
using MTM.Models;

namespace MTM.Pages.Disciples
{
    public class DeleteModel : PageModel
    {
        private readonly MTM.Data.MTMContext _context;

        public DeleteModel(MTM.Data.MTMContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Disciple Disciple { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Disciple = await _context.Disciples.FirstOrDefaultAsync(m => m.ID == id);

            if (Disciple == null)
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

            Disciple = await _context.Disciples.FindAsync(id);

            if (Disciple != null)
            {
                _context.Disciples.Remove(Disciple);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
