using Microsoft.AspNetCore.Mvc;
using ClassroomManagement.Models.Admin;
using ClassroomManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagement.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeleteStudentController : ControllerBase
    {
        private readonly ClassroomDbContext _context;

        public DeleteStudentController(ClassroomDbContext context)
        {
            _context = context;
        }

        [HttpDelete("{studentId}")]
        public async Task<IActionResult> DeleteStudent(string studentId)
        {
            var student = await _context.Students
    .FirstOrDefaultAsync(s => s.StudentId == studentId);
            if (student == null)
                return NotFound(new { message = "Student not found" });

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Student deleted successfully" });
        }
    }
}
