using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MTM.Data;
using MTM.Models;

namespace MTM.Pages.Classes
{
    public class DetailsModel : ClassBasePageModel
    {
        public PaginatedList<Disciple> Disciples { get; set; }

        public DetailsModel(MTMContext context) : base(context)
        {
        }

        public async Task<IActionResult> OnGetAsync(int? id, int? pageIndex)
        {
            if (id == null)
            {
                return NotFound();
            }

            Class = await _context.Classes
                .Include(c => c.Disciples)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            Disciples = PaginatedList<Disciple>.Create(
                Class.Disciples.OrderBy(d => d.FirstName).AsQueryable<Disciple>(),
                pageIndex ?? 1, Common.PAGE_SIZE);

            if (Class == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
