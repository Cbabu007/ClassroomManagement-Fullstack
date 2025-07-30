using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassroomManagement.Data;
using ClassroomManagement.Models.Staff;
using ClassroomManagement.Models.Admin;
using ClassroomManagement.Models.Classroom;

namespace ClassroomManagement.Controllers.Classroom
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddClassroomController : ControllerBase
    {
        private readonly ClassroomDbContext _context;

        public AddClassroomController(ClassroomDbContext context)
        {
            _context = context;
        }

        
        [HttpGet("GetSubjectsAndStaffs")]
        public async Task<IActionResult> GetSubjectsAndStaffs()
        {
            var subjectStaffs = await _context.Staffs
                .Where(s => s.Department != null && s.Role != "Class Teacher") // Exclude Class Teachers
                .Select(s => new
                {
                    Subject = s.Department,
                    StaffId = s.StaffId,
                    StaffName = s.FirstName + " " + s.LastName
                })
                .ToListAsync();

            return Ok(subjectStaffs);
        }

       
        [HttpGet("GetClassTeachers")]
        public async Task<IActionResult> GetClassTeachers()
        {
            var allStaffs = await _context.Staffs
                .Select(s => new
                {
                    StaffId = s.StaffId,
                    StaffName = s.FirstName + " " + s.LastName
                })
                .ToListAsync();

            return Ok(allStaffs);
        }

        
        [HttpGet("GetStudentsByClass/{className}")]
        public async Task<IActionResult> GetStudentsByClass(string className)
        {
            var students = await _context.Students
                .Where(s => s.Grade == className)
                .Select(s => new
                {
                    StudentId = s.StudentId,
                    StudentName = s.FirstName + " " + s.LastName + " " + s.EntranceTest
                })
                .ToListAsync();

            return Ok(students);
        }

       
        [HttpPost("AddClassroom")]
        public async Task<IActionResult> AddClassroom([FromBody] ClassroomSaveRequest request)
        {
            if (request == null)
                return BadRequest("Invalid classroom data.");

           
            var classroom = new ClassroomManagement.Models.Classroom.Classroom
            {
                ClassroomNo = request.ClassroomNo,
                Class = request.Class,
                Section = request.Section,
                ClassTeacherId = request.ClassTeacherId,
                ClassTeacherName = request.ClassTeacherName
            };

            _context.Classrooms.Add(classroom);
            await _context.SaveChangesAsync(); 

          
            if (request.SubjectStaffList != null)
            {
                foreach (var subjectStaff in request.SubjectStaffList)
                {
                    var staff = new SubjectStaff
                    {
                        ClassroomId = classroom.Id,
                        Subject = subjectStaff.Subject,
                        StaffId = subjectStaff.StaffId,
                        StaffName = subjectStaff.StaffName,
                        ClassroomNo = classroom.ClassroomNo,
                        Class = classroom.Class,
                        Section = classroom.Section
                    };
                    _context.SubjectStaffs.Add(staff);
                }
            }

            
            if (request.Students != null)
            {
                foreach (var student in request.Students)
                {
                    var classroomStudent = new ClassroomStudent
                    {
                        ClassroomId = classroom.Id,
                        StudentId = student.StudentId,
                        StudentName = student.StudentName,
                        ClassroomNo = classroom.ClassroomNo,
                        Class = classroom.Class,
                        Section = classroom.Section
                    };
                    _context.ClassroomStudents.Add(classroomStudent);
                }
            }

            await _context.SaveChangesAsync(); 

            return Ok(new { message = "✅ Classroom assigned successfully!" });
        }
    }
}
