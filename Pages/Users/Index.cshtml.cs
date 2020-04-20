using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MTM.Data;
using MTM.Models;
using Newtonsoft.Json;

namespace MTM.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly MTM.Data.MTMContext _context;

        public IndexModel(MTM.Data.MTMContext context)
        {
            _context = context;
        }

        public string NameSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public PaginatedList<User> Users { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public async Task OnGetAsync(string sortOrder,
                    string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<User> userIQ = from u in _context.Users select u;
            
            // not allow edit and delete current user
            userIQ = userIQ.Where(u => u.Username != HttpContext.User.Identity.Name);

            // not allow edit and delete admin user
            userIQ = userIQ.Where(u => u.Username != Common.ADMIN_USER);

            if (!String.IsNullOrEmpty(searchString))
            {
                userIQ = userIQ.Where(u => u.Username.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    userIQ = userIQ.OrderByDescending(u => u.Username);
                    break;
                default:
                    userIQ = userIQ.OrderBy(u => u.Username);
                    break;
            }

            Users = await PaginatedList<User>.CreateAsync(userIQ, pageIndex ?? 1, Common.PAGE_SIZE);
        }

        public IActionResult OnPostChangePassword()
        {
            User postData = null;
            MemoryStream stream = new MemoryStream();
            Request.Body.CopyTo(stream);
            stream.Position = 0;

            using (StreamReader reader = new StreamReader(stream))
            {
                string requestBody = reader.ReadToEnd();
                if (requestBody.Length > 0)
                {
                    postData = JsonConvert.DeserializeObject<User>(requestBody);
                    if (postData == null)
                    {
                        return new JsonResult(false);
                    }
                }
            }

            var currentUser = _context.Users.FirstOrDefault(e => e.Username == postData.Username);
            if (currentUser == null ||
               //other user cannot change amdin's password
               (HttpContext.User.Identity.Name != Common.ADMIN_USER && currentUser.Username == Common.ADMIN_USER))
            {
                return new JsonResult(false);
            }

            var passwordHasher = new PasswordHasher<string>();
            _context.Attach(currentUser).State = EntityState.Modified;
            currentUser.Password = passwordHasher.HashPassword(currentUser.Username, postData.Password);

            try
            {
                _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return new JsonResult(false);
            }

            return new JsonResult(true);
        }

        public IActionResult OnPostDelete()
        {
            User postData = null;
            MemoryStream stream = new MemoryStream();
            Request.Body.CopyTo(stream);
            stream.Position = 0;

            using (StreamReader reader = new StreamReader(stream))
            {
                string requestBody = reader.ReadToEnd();
                if (requestBody.Length > 0)
                {
                    postData = JsonConvert.DeserializeObject<User>(requestBody);
                    if (postData == null)
                    {
                        return new JsonResult(false);
                    }
                }
            }

            //Don't allow delete admin and login user
            var currentUser = _context.Users.FirstOrDefault(e => 
                                            e.Username == postData.Username &&
                                            e.Username != Common.ADMIN_USER &&
                                            e.Username != HttpContext.User.Identity.Name);
            if (currentUser == null)
            {
                return new JsonResult(false);
            }

            _context.Users.Remove(currentUser);
            _context.SaveChangesAsync();

            return new JsonResult(true);
        }
    }
}
