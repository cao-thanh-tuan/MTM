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
    public class CreateModel : ClassBasePageModel
    {
        public CreateModel(MTMContext context) : base(context)
        {
        }

        public IActionResult OnGet()
        {
            PopulateCitiesDropDownList();
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

            if (ClassExists(Class.ID, Class.Name))
            {
                ModelState.AddModelError("Class.Name", "Tên lớp đã tồn tại");
                PopulateCitiesDropDownList(Class.City);
                return Page();
            }

            _context.Classes.Add(Class);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
