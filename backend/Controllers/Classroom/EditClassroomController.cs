using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassroomManagement.Data;
using ClassroomManagement.Models.Classroom;

namespace ClassroomManagement.Controllers.Classroom
{
    [ApiController]
    [Route("api/[controller]")]
    public class EditClassroomController : ControllerBase
    {
        private readonly ClassroomDbContext _context;

        public EditClassroomController(ClassroomDbContext context)
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
                    Id = s.Id,
                    Subject = s.Subject,
                    StaffId = s.StaffId,
                    StaffName = s.StaffName
                })
                .ToListAsync();

            var classroomStudents = await _context.ClassroomStudents
                .Where(s => s.ClassroomId == id)
                .Select(s => new
                {
                    StudentId = s.StudentId,
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
                Students = classroomStudents
            });
        }

        [HttpGet("GetStudentsByGrade/{grade}")]
        public async Task<IActionResult> GetStudentsByGrade(string grade)
        {
            var students = await _context.Students
                 .Where(s => s.Grade == grade)
                .Select(s => new {
                    StudentId = s.StudentId,
                    StudentName = s.FirstName + " " + s.LastName
                })
                .ToListAsync();

            return Ok(students);
        }


        [HttpPut("UpdateClassroom/{id}")]
        public async Task<IActionResult> UpdateClassroom(int id, [FromBody] ClassroomSaveRequest request)
        {
            if (request == null)
                return BadRequest("Invalid Request. Request body is empty.");

            if (string.IsNullOrEmpty(request.ClassroomNo) || string.IsNullOrEmpty(request.Class))
                return BadRequest("Missing ClassroomNo or Class fields");

            var classroom = await _context.Classrooms.FindAsync(id);
            if (classroom == null)
                return NotFound("Classroom Not Found");

            
            classroom.ClassroomNo = request.ClassroomNo;
            classroom.Class = request.Class;
            classroom.Section = request.Section;
            classroom.ClassTeacherId = request.ClassTeacherId;
            classroom.ClassTeacherName = request.ClassTeacherName;

            
            var oldSubjects = _context.SubjectStaffs.Where(x => x.ClassroomId == id);
            _context.SubjectStaffs.RemoveRange(oldSubjects);

            
            if (request.SubjectStaffList != null && request.SubjectStaffList.Any())
            {
                foreach (var subj in request.SubjectStaffList)
                {
                    var subjectStaff = new SubjectStaff
                    {
                        ClassroomId = id,
                        Subject = subj.Subject ?? "",
                        StaffId = subj.StaffId ?? "TEMP",
                        StaffName = subj.StaffName ?? ""
                    };
                    _context.SubjectStaffs.Add(subjectStaff);
                }
            }

            
            var oldStudents = _context.ClassroomStudents.Where(x => x.ClassroomId == id);
            _context.ClassroomStudents.RemoveRange(oldStudents);

           
            if (request.Students != null && request.Students.Any())
            {
                foreach (var stud in request.Students)
                {
                    var student = new ClassroomStudent
                    {
                        ClassroomId = id,
                        StudentId = stud.StudentId ?? "TEMP",
                        StudentName = stud.StudentName ?? stud.StudentId ?? ""
                    };
                    _context.ClassroomStudents.Add(student);
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "✅ Classroom Updated Successfully!" });
        }

    }
}
