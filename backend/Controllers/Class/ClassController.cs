using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ClassroomManagement.Data;
using ClassroomManagement.Models.Class;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Controllers.Class
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassController : ControllerBase
    {
        private readonly ClassroomDbContext _context;

        public ClassController(ClassroomDbContext context)
        {
            _context = context;
        }

        
        [HttpGet("GetGradesAndSections")]
        public async Task<IActionResult> GetGradesAndSections()
        {
            var result = await _context.Students
                .Select(s => new { s.Grade, s.Section })
                .Distinct()
                .ToListAsync();

            return Ok(result);
        }

        
        [HttpGet("GetAttendanceStatus")]
        public async Task<IActionResult> GetAttendanceStatus(string grade, string section, DateTime date)
        {
            try
            {
                var result = await _context.AttendanceStatusResult
                    .FromSqlRaw("EXEC sp_GetAttendanceStatus @Grade, @Section, @Date",
                        new SqlParameter("@Grade", grade),
                        new SqlParameter("@Section", section),
                        new SqlParameter("@Date", date))
                    .ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ Error fetching attendance: {ex.Message}");
            }
        }

        [HttpPost("SubmitFinalAttendance")]
        public async Task<IActionResult> SubmitFinalAttendance([FromBody] List<AttendanceStatusResult> data)
        {
            if (data == null || !data.Any())
                return BadRequest("No attendance data received.");

            foreach (var record in data)
            {
                var final = new FinalAttendance
                {
                    StudentId = record.StudentId,
                    StudentName = record.StudentName,
                    Grade = record.Grade,
                    Section = record.Section,
                    Date = DateTime.Today,
                    Class1 = record.Class1,
                    Class2 = record.Class2,
                    Class3 = record.Class3,
                    Class4 = record.Class4,
                    Class5 = record.Class5,
                    Class6 = record.Class6,
                    Class7 = record.Class7,
                    Class8 = record.Class8
                };

                _context.FinalAttendance.Add(final);
            }

            await _context.SaveChangesAsync();
            return Ok("Attendance submitted.");
        }

    }
}
