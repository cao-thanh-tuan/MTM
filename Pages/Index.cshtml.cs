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
                                        d.InitiateDate.Value.Month == initiateDate.Month)
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
                ModelState.AddModelError(string.Empty, "Không tìm thấy số điện thoại với năm và tháng thọ pháp");
            }

            return Page();
        }

        private void FillChartData()
        {
            var data = _context.DataPoints.FromSqlRaw(@"
DECLARE @today DATETIME
DECLARE @MondayThisWeek DATETIME
DECLARE @SundayThisWeek DATETIME
DECLARE @MondayNextWeek DATETIME
DECLARE @SundayNextWeek DATETIME
DECLARE @MondayPrevWeek DATETIME
DECLARE @SundayPrevWeek DATETIME

SET @today = GETDATE()
SET @MondayThisWeek = CONVERT(date, DATEADD(dd, 0 - (@@DATEFIRST + 5 + DATEPART(dw, @today)) % 7, @today))
SET @SundayThisWeek = CONVERT(date, DATEADD(dd, 6 - (@@DATEFIRST + 5 + DATEPART(dw, @today)) % 7, @today))
SET @SundayThisWeek = DATEADD(HOUR, 23, @SundayThisWeek)
SET @SundayThisWeek = DATEADD(MINUTE, 59, @SundayThisWeek)
SET @SundayThisWeek = DATEADD(SECOND, 59, @SundayThisWeek)

SET @MondayNextWeek = DATEADD(dd, 7, @MondayThisWeek)
SET @SundayNextWeek = DATEADD(dd, 7, @SundayThisWeek)
SET @MondayPrevWeek = DATEADD(dd, -7, @MondayThisWeek)
SET @SundayPrevWeek = DATEADD(dd, -7, @SundayThisWeek)

Select 1 ID, FORMAT(@MondayPrevWeek, 'dd/MM') + ' - ' + FORMAT(@SundayPrevWeek, 'dd/MM') Label, 
    (select count(*) from [dbo].[Registration]
    where FromTime between @MondayPrevWeek and @SundayPrevWeek) Y
UNION
Select 2 ID, FORMAT(@MondayThisWeek, 'dd/MM') + ' - ' + FORMAT(@SundayThisWeek, 'dd/MM') Label, 
    (select count(*) ThisWeek from [dbo].[Registration]
    where FromTime between @MondayThisWeek and @SundayThisWeek) Y
UNION
Select 3 ID, FORMAT(@MondayNextWeek, 'dd/MM') + ' - ' + FORMAT(@SundayNextWeek, 'dd/MM') Label, 
    (select count(*) from [dbo].[Registration]
    where FromTime between @MondayNextWeek and @SundayNextWeek) Y")
                .ToList();

                DataPoints = JsonConvert.SerializeObject(data);
        }
    }
}
