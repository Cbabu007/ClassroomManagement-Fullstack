using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassroomManagement.Data;
using ClassroomManagement.Models.AdminReport;

namespace ClassroomManagement.Controllers.AdminReport
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminReportStaffController : ControllerBase
    {
        private readonly ClassroomDbContext _context;

        public AdminReportStaffController(ClassroomDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetStaffs")]
        public async Task<IActionResult> GetStaffs()
        {
            try
            {
                var staffs = await _context.AdminReportStaffs
                    .FromSqlRaw("EXEC GetStaffReport")
                    .ToListAsync();
                return Ok(staffs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error fetching staff data.", error = ex.Message });
            }
        }
    }
}