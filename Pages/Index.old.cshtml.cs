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

        public DoWDataPointClient Chart { get; set; }

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
                .Where(d => d.IdentitcationNumber == RegistrationInfo.IdentitcationNumber && 
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
                ModelState.AddModelError(string.Empty, "Không tìm thấy CMND với năm và tháng thọ pháp");
            }

            return Page();
        }

        private void FillChartData()
        {
            Chart = new DoWDataPointClient();
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.Connection.Open();
                command.CommandType = CommandType.Text;
                command.CommandText = @"
select DayOfWeek, DayPart, count(DayOfWeek) Number 
from
(
	select --FromTime,
		CASE
			WHEN DATEPART(weekday, FromTime) = 2 THEN 'Th 2'
			WHEN DATEPART(weekday, FromTime) = 3 THEN 'Th 3'
			WHEN DATEPART(weekday, FromTime) = 4 THEN 'Th 4'
			WHEN DATEPART(weekday, FromTime) = 5 THEN 'Th 5'
			WHEN DATEPART(weekday, FromTime) = 6 THEN 'Th 6'
			WHEN DATEPART(weekday, FromTime) = 7 THEN 'Th 7'
			ELSE 'CN'
		END DayOfWeek,
		CASE
			WHEN DATEPART(hour, FromTime) between 0 and 5 THEN 'MidNight'
			WHEN DATEPART(hour, FromTime) between 6 and 11 THEN 'Morning'
			WHEN DATEPART(hour, FromTime) between 12 and 18 THEN 'AfterNoon'
			ELSE 'Night'
		END DayPart
	from [dbo].[Registration]
) tmp
group by DayOfWeek, DayPart
order by DayOfWeek";

                var dowDataPoint = new DoWDataPoint();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var DayOfWeek = reader.GetString("DayOfWeek");
                        var DayPart = reader.GetString("DayPart");
                        var Number = reader.GetInt32("Number");
                        switch (DayPart)
                        {
                            case "MidNight":
                                dowDataPoint.MidNight.Add(new DataPoint(DayOfWeek, Number));
                                break;
                            case "Morning":
                                dowDataPoint.Morning.Add(new DataPoint(DayOfWeek, Number));
                                break;
                            case "AfterNoon":
                                dowDataPoint.AfterNoon.Add(new DataPoint(DayOfWeek, Number));
                                break;
                            default:
                                dowDataPoint.Night.Add(new DataPoint(DayOfWeek, Number));
                                break;
                        }
                    }
                }

                Chart.MidNight = JsonConvert.SerializeObject(dowDataPoint.MidNight);
                Chart.Morning = JsonConvert.SerializeObject(dowDataPoint.Morning);
                Chart.AfterNoon = JsonConvert.SerializeObject(dowDataPoint.AfterNoon);
                Chart.Night = JsonConvert.SerializeObject(dowDataPoint.Night);

                command.Connection.Close();
            }
        }
    }
}
