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
    public class DetailsModel : DiscipleBasePageModel
    {
        public PaginatedList<Registration> MeditaionRegisters { get; set; }

        public DetailsModel(MTMContext context) : base(context)
        {
        }
        
        public async Task<IActionResult> OnGetAsync(int? id, int? pageIndex)
        {
            if (id == null)
            {
                return NotFound();
            }

            Disciple = await _context.Disciples
                .Include(c => c.Class)
                .Include(m => m.MeditaionRegisters)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.ID == id);

            MeditaionRegisters = PaginatedList<Registration>.Create(
                Disciple.MeditaionRegisters.OrderByDescending(m => m.FromTime).AsQueryable<Registration>(),
                pageIndex ?? 1, Common.PAGE_SIZE);

            if (Disciple == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
