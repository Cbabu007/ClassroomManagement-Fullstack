using Microsoft.AspNetCore.Mvc;
using ClassroomManagement.Models.Admin;
using ClassroomManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagement.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class ViewStudentController : ControllerBase
    {
        private readonly ClassroomDbContext _context;

        public ViewStudentController(ClassroomDbContext context)
        {
            _context = context;
        }

        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetStudent(string studentId)
        {
            var student = await _context.Students
        .FirstOrDefaultAsync(s => s.StudentId == studentId);
            if (student == null)
                return NotFound(new { message = "Student not found" });

            return Ok(student);
        }
    }
}
