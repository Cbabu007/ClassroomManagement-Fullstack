using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassroomManagement.Data;
using ClassroomManagement.Models.AdminReport;

namespace ClassroomManagement.Controllers.AdminReport
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminReportStudentController : ControllerBase
    {
        private readonly ClassroomDbContext _context;

        public AdminReportStudentController(ClassroomDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetStudents")]
        public async Task<IActionResult> GetStudents()
        {
            try
            {
                var students = await _context.AdminReportStudents
                    .FromSqlRaw("EXEC GetStudentReport")
                    .ToListAsync();
                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error fetching student data.", error = ex.Message });
            }
        }
    }
}