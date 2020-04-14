using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MTM.Data;
using MTM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTM.Pages.Disciples
{
    public class DiscipleBasePageModel : PageModel
    {
        protected readonly MTMContext _context;

        public DiscipleBasePageModel(MTMContext context)
        {
            _context = context;
        }

        public SelectList ClassNameSL { get; set; }
        public SelectList GenderSL { get; set; }

        protected Disciple GetDisciple(int id)
        {
            return _context.Disciples.FirstOrDefault(m => m.ID == id);
        }

        protected bool PhoneExists(int discipleId, string phone)
        {
            var duplicate = _context.Disciples.FirstOrDefault(d => d.Phone == phone && d.ID != discipleId);

            return duplicate != null;
        }

        protected void PopulateClassesDropDownList(MTMContext _context, object selectedClass = null)
        {
            var classesQuery = from d in _context.Classes
                                   orderby d.Name // Sort by name.
                                   select d;

            ClassNameSL = new SelectList(
                classesQuery.AsNoTracking(), "ID", "Name", selectedClass);
        }

        protected void PopulateGendersDropDownList(string selectedGender = null)
        {
            GenderSL = new SelectList(new SelectListItem[] {
                new SelectListItem { Value = Gender.UNKNOWN, Text = Gender.UNKNOWN },
                new SelectListItem { Value = Gender.FEMALE, Text = Gender.FEMALE },
                new SelectListItem { Value = Gender.MALE, Text = Gender.MALE },
            }, "Value", "Text", selectedGender);
        }
    }
}
