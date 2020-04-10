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
    public class DetailsModel : PageModel
    {
        private readonly MTM.Data.MTMContext _context;

        public DetailsModel(MTM.Data.MTMContext context)
        {
            _context = context;
        }

        public Disciple Disciple { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Disciple = await _context.Disciples
                .Include(m => m.MeditaionRegisters)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.ID == id);

            Disciple.MeditaionRegisters = Disciple.MeditaionRegisters.OrderByDescending(m => m.FromTime).ToList();

            if (Disciple == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
