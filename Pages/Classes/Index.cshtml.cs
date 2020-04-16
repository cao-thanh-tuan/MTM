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
    public class IndexModel : PageModel
    {
        private readonly MTM.Data.MTMContext _context;

        public IndexModel(MTM.Data.MTMContext context)
        {
            _context = context;
        }

        public IList<Class> Classes { get;set; }

        public async Task OnGetAsync()
        {
            Classes = await _context.Classes.ToListAsync();
        }
    }
}
