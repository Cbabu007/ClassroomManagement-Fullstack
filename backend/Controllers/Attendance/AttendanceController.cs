using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using ClassroomManagement.Data;
using ClassroomManagement.Models.Attendance;

namespace ClassroomManagement.Controllers.Attendance
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly ClassroomDbContext _context;
        private readonly IConfiguration _configuration;

        public AttendanceController(ClassroomDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        
        [HttpGet("GetGradesAndSections")]
        public async Task<IActionResult> GetGradesAndSections()
        {
            var data = await _context.Students
                .Select(s => new { Grade = s.Grade, Section = s.Section })
                .Distinct()
                .ToListAsync();

            return Ok(data);
        }

        
        [HttpGet("GetStudents")]
        public async Task<IActionResult> GetStudents(string grade, string section)
        {
            var students = await _context.Students
                .Where(s => s.Grade == grade && s.Section == section)
                .Select(s => new
                {
                    StudentId = s.StudentId,
                    Name = s.FirstName + " " + s.LastName,
                    FatherMobile = s.FatherMobile,
                    MotherMobile = s.MotherMobile
                })
                .ToListAsync();

            return Ok(students);
        }

       
        [HttpPost("SubmitAttendance")]
        public async Task<IActionResult> SubmitAttendance([FromBody] AttendanceSubmissionModel model)
        {
            try
            {
                if (model == null)
                    return BadRequest("Invalid data");

                if (model.PresentList != null)
                {
                    foreach (var p in model.PresentList)
                    {
                        _context.PresentStudents.Add(new PresentStudent
                        {
                            StudentId = p.StudentId,
                            StudentName = p.StudentName,
                            ClassTiming = p.ClassTiming,
                            Date = DateTime.TryParse(p.Date, out var parsedDate) ? parsedDate : DateTime.Today
                        });
                    }
                }

                if (model.AbsentList != null)
                {
                    foreach (var a in model.AbsentList)
                    {
                        _context.AbsentStudents.Add(new AbsentStudent
                        {
                            StudentId = a.StudentId,
                            StudentName = a.StudentName,
                            ClassTiming = a.ClassTiming,
                            FatherMobile = a.FatherMobile,
                            MotherMobile = a.MotherMobile,
                            Date = DateTime.TryParse(a.Date, out var parsedDate) ? parsedDate : DateTime.Today
                        });

                        await SendAbsenceSms(a.FatherMobile, a.StudentName);
                        await SendAbsenceSms(a.MotherMobile, a.StudentName);
                    }
                }

                await _context.SaveChangesAsync();
                return Ok(new { message = "✅ Attendance saved" });

            }
            catch (Exception ex)
            {
                Console.WriteLine("⚠️ Attendance submission failed:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, $"❌ Server error: {ex.Message}");
            }
        }


       
        private async Task SendAbsenceSms(string phoneNumber, string studentName)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return;

            var accountSid = _configuration["Twilio:AccountSid"];
            var authToken = _configuration["Twilio:AuthToken"];
            var fromPhone = _configuration["Twilio:FromPhone"];

            TwilioClient.Init(accountSid, authToken);

            var messageBody = $"Your child {studentName} was absent today. Please contact the school.";

            try
            {
                var message = await MessageResource.CreateAsync(
                    to: new PhoneNumber(phoneNumber),
                    from: new PhoneNumber(fromPhone),
                    body: messageBody
                );

                Console.WriteLine($"✅ SMS sent to {phoneNumber}: SID = {message.Sid}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to send SMS to {phoneNumber}: {ex.Message}");
            }
        }

    }
}
