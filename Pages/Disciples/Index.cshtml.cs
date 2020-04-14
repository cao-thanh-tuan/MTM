using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MTM.Data;
using MTM.Models;

namespace MTM.Pages.Disciples
{
    public class IndexModel : PageModel
    {
        private readonly MTMContext _context;

        public IndexModel(MTMContext context)
        {
            _context = context;
        }

        public string NameSort { get; set; }
        public string ClassSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public PaginatedList<Disciple> Disciples { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public async Task OnGetAsync(string sortOrder,
            string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ClassSort = sortOrder == "class" ? "class_desc" : "class";
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<Disciple> discipleIQ = from d in _context.Disciples select d;
            if (!String.IsNullOrEmpty(searchString))
            {
                discipleIQ = discipleIQ.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstName.Contains(searchString)
                                       || s.Phone.Contains(searchString)
                                       || s.Class.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    discipleIQ = discipleIQ.OrderByDescending(s => s.FirstName);
                    break;
                case "class":
                    discipleIQ = discipleIQ.OrderBy(s => s.Class.Name);
                    break;
                case "class_desc":
                    discipleIQ = discipleIQ.OrderByDescending(s => s.Class.Name);
                    break;
                default:
                    discipleIQ = discipleIQ.OrderBy(s => s.FirstName);
                    break;
            }

            int pageSize = 10;
            Disciples = await PaginatedList<Disciple>.CreateAsync(
                discipleIQ.Include(d => d.Class).AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
