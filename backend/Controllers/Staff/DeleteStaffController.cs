using Microsoft.AspNetCore.Mvc;
using ClassroomManagement.Data;
using ClassroomManagement.Models.Staff;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagement.Controllers.Staff
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeleteStaffController : ControllerBase
    {
        private readonly ClassroomDbContext _context;

        public DeleteStaffController(ClassroomDbContext context)
        {
            _context = context;
        }

        [HttpDelete("{staffId}")]
        public async Task<IActionResult> DeleteStaff(string staffId)
        {
            if (string.IsNullOrEmpty(staffId))
                return BadRequest("Staff ID is required.");

            
            var staff = await _context.Staffs.FirstOrDefaultAsync(s => s.StaffId == staffId);

            if (staff == null)
                return NotFound(new { message = "Staff not found." });

            
            var qualifications = _context.StaffQualifications.Where(q => q.StaffId == staffId);
            _context.StaffQualifications.RemoveRange(qualifications);

           
            if (!string.IsNullOrEmpty(staff.PhotoPath))
            {
                var photoFullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", staff.PhotoPath);
                if (System.IO.File.Exists(photoFullPath))
                {
                    System.IO.File.Delete(photoFullPath);
                }
            }

           
            _context.Staffs.Remove(staff);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Staff deleted successfully!" });
        }
    }
}
