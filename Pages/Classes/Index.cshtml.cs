using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MTM.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MTM.Pages.Classes
{
    public class IndexModel : PageModel
    {
        private readonly MTM.Data.MTMContext _context;

        public IndexModel(MTM.Data.MTMContext context)
        {
            _context = context;
        }

        public string NameSort { get; set; }
        public string CitySort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<Class> Classes { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public async Task OnGetAsync(string sortOrder,
            string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            CitySort = sortOrder == "city" ? "city_desc" : "city";
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;


            IQueryable<Class> classIQ = from d in _context.Classes select d;
            if (!String.IsNullOrEmpty(searchString))
            {
                classIQ = classIQ.Where(c => c.Name.Contains(searchString)
                                       || c.City.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    classIQ = classIQ.OrderByDescending(c => c.Name);
                    break;
                case "city":
                    classIQ = classIQ.OrderBy(c => c.City);
                    break;
                case "city_desc":
                    classIQ = classIQ.OrderByDescending(c => c.City);
                    break;
                default:
                    classIQ = classIQ.OrderBy(c => c.Name);
                    break;
            }

            Classes = await PaginatedList<Class>.CreateAsync(classIQ, pageIndex ?? 1, Common.PAGE_SIZE);
        }

        public IActionResult OnGetCanDelete(int id)
        {
            return new JsonResult(!_context.Disciples.Any(d => d.ClassID == id));
        }

        public IActionResult OnPostDelete()
        {
            Class postData = null;
            MemoryStream stream = new MemoryStream();
            Request.Body.CopyTo(stream);
            stream.Position = 0;

            using (StreamReader reader = new StreamReader(stream))
            {
                string requestBody = reader.ReadToEnd();
                if (requestBody.Length > 0)
                {
                    postData = JsonConvert.DeserializeObject<Class>(requestBody);
                    if (postData == null)
                    {
                        return new JsonResult(false);
                    }
                }
            }

            var currentClass = _context.Classes.FirstOrDefault(c => c.ID == postData.ID);
            if (currentClass == null)
            {
                return new JsonResult(false);
            }

            _context.Classes.Remove(currentClass);
            _context.SaveChanges();

            return new JsonResult(true);
        }
    }
}
