using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MTM.Data;
using MTM.Models;

namespace MTM.Pages.Disciples
{
    public class EditModel : DiscipleBasePageModel
    {
        public EditModel(MTMContext context) : base(context)
        {
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Disciple = await GetDisciple(id);
            if (Disciple == null)
            {
                return NotFound();
            }

            PopulateGendersDropDownList(Disciple.Gender);
            PopulateClassesDropDownList(_context, Disciple.ClassID);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Disciple).State = EntityState.Modified;

            if (PhoneExists(Disciple.ID, Disciple.Phone))
            {
                ModelState.AddModelError("Disciple.Phone", "Số điện thoại đã tồn tại");
                PopulateClassesDropDownList(_context, Disciple.ClassID);
                PopulateGendersDropDownList(Disciple.Gender);

                return Page();
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await GetDisciple(Disciple.ID) != null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
