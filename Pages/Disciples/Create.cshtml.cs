using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MTM.Data;
using MTM.Models;

namespace MTM.Pages.Disciples
{
    public class CreateModel : DiscipleBasePageModel
    {
        public CreateModel(MTMContext context): base(context)
        {
        }

        public IActionResult OnGet()
        {
            PopulateGendersDropDownList();
            PopulateClassesDropDownList(_context);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (PhoneExists(Disciple.ID, Disciple.Phone))
            {
                ModelState.AddModelError("Disciple.Phone", "Số điện thoại đã tồn tại");
                PopulateClassesDropDownList(_context, Disciple.ClassID);
                PopulateGendersDropDownList(Disciple.Gender);

                return Page();
            }

            _context.Disciples.Add(Disciple);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
