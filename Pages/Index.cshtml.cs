using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
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

        public IActionResult OnGet()
        {
            var data = _context.DataPoints.FromSqlRaw("dbo.CountByWeekReport").ToList();
            DataPoints = JsonConvert.SerializeObject(data);

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { Success = false, Message = "Thông tin đăng ký không hợp lệ!" });
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
                _context.SaveChanges();

                return new JsonResult(new { Success = true, Message = "Đăng ký thiền tại gia thành công!" });
            }

            return new JsonResult(new { Success = false, Message = "Không tìm thấy số điện thoại với ngày thọ pháp!" });
        }
    }
}
