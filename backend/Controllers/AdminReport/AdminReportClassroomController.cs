using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassroomManagement.Data;
using ClassroomManagement.Models.AdminReport;

namespace ClassroomManagement.Controllers.AdminReport
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminReportClassroomController : ControllerBase
    {
        private readonly ClassroomDbContext _context;

        public AdminReportClassroomController(ClassroomDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetClassrooms")]
        public async Task<IActionResult> GetClassrooms()
        {
            try
            {
                var classrooms = await _context.AdminReportClassrooms
                    .FromSqlRaw("EXEC GetClassroomReport")
                    .ToListAsync();
                return Ok(classrooms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error fetching classroom data.", error = ex.Message });
            }
        }
    }
}
