using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MTM.Data;
using MTM.Models;
using Newtonsoft.Json;

namespace MTM.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly MTMContext _context;

        public IndexModel(ILogger<IndexModel> logger, MTMContext context)
        {
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public RegistrationInfo RegistrationInfo { get; set; }

        public string DataPoints { get; set; }

        public bool IsSuccess { get; set; }

        public IActionResult OnGet()
        {
            FillChartData();

            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            FillChartData();

            IsSuccess = false;
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var initiateDate = Convert.ToDateTime(RegistrationInfo.InitiateDate);
            var startTime = Convert.ToDateTime(RegistrationInfo.StartTime);
            var endTime = Convert.ToDateTime(RegistrationInfo.EndTime);

            var disciple = _context
                .Disciples
                .Where(d => d.Phone == RegistrationInfo.Phone && 
                                        d.InitiateDate != null &&
                                        d.InitiateDate.Value.Year == initiateDate.Year &&
                                        d.InitiateDate.Value.Month == initiateDate.Month &&
                                        d.InitiateDate.Value.Day == initiateDate.Day)
                .FirstOrDefault();

            if (disciple != null)
            {
                var registration = new Registration()
                {
                    Disciple = disciple,
                    FromTime = startTime,
                    ToTime = endTime
                };

                disciple.MeditaionRegisters.Add(registration);
                await _context.SaveChangesAsync();

                IsSuccess = true;
            } else
            {
                ModelState.AddModelError(string.Empty, "Không tìm thấy số điện thoại với ngày thọ pháp");
            }

            return Page();
        }

        private void FillChartData()
        {
            var data = _context.DataPoints.FromSqlRaw("CountByWeekReport").ToList();
            DataPoints = JsonConvert.SerializeObject(data);
        }
    }
}
