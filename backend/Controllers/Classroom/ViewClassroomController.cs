using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassroomManagement.Data;
using ClassroomManagement.Models.Classroom;

namespace ClassroomManagement.Controllers.Classroom
{
    [ApiController]
    [Route("api/[controller]")]
    public class ViewClassroomController : ControllerBase
    {
        private readonly ClassroomDbContext _context;

        public ViewClassroomController(ClassroomDbContext context)
        {
            _context = context;
        }

        
        [HttpGet("GetAssignedClassrooms")]
        public async Task<IActionResult> GetAssignedClassrooms()
        {
            var classrooms = await _context.Classrooms
                .Select(c => new
                {
                    Id = c.Id,
                    ClassroomNo = c.ClassroomNo,
                    Class = c.Class,
                    Section = c.Section
                })
                .ToListAsync();

            return Ok(classrooms);
        }

        
        [HttpGet("GetClassroomById/{id}")]
        public async Task<IActionResult> GetClassroomById(int id)
        {
            var classroom = await _context.Classrooms.FirstOrDefaultAsync(c => c.Id == id);

            if (classroom == null)
                return NotFound("Classroom not found");

            var subjectStaffs = await _context.SubjectStaffs
                .Where(s => s.ClassroomId == id)
                .Select(s => new
                {
                    Subject = s.Subject,
                    StaffName = s.StaffName
                })
                .ToListAsync();

            var students = await _context.ClassroomStudents
                .Where(s => s.ClassroomId == id)
                .Select(s => new
                {
                    StudentName = s.StudentName
                })
                .ToListAsync();

            return Ok(new
            {
                Classroom = new
                {
                    classroom.Id,
                    classroom.ClassroomNo,
                    classroom.Class,
                    classroom.Section,
                    classroom.ClassTeacherId,
                    classroom.ClassTeacherName
                },
                SubjectStaffList = subjectStaffs,
                Students = students
            });
        }
    }
}
