using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassroomManagement.Data;
using ClassroomManagement.Models.Staff;

namespace ClassroomManagement.Controllers.Staff
{
    [ApiController]
    [Route("api/[controller]")]
    public class ViewStaffController : ControllerBase
    {
        private readonly ClassroomDbContext _context;

        public ViewStaffController(ClassroomDbContext context)
        {
            _context = context;
        }

        [HttpGet("{staffId}")]
        public async Task<IActionResult> GetStaff(string staffId)
        {
            if (string.IsNullOrEmpty(staffId))
                return BadRequest("StaffId is required.");

            var staff = await _context.Staffs
                .FirstOrDefaultAsync(x => x.StaffId == staffId);

            if (staff == null)
                return NotFound(new { message = "Staff not found." });

            var qualifications = await _context.StaffQualifications
                .Where(q => q.StaffId == staffId)
                .ToListAsync();

            return Ok(new
            {
                staff,
                qualifications
            });
        }
    }
}
