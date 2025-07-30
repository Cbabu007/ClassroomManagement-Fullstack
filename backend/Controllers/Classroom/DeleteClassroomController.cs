using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassroomManagement.Data; 

namespace ClassroomManagement.Controllers.Classroom
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeleteClassroomController : ControllerBase
    {
        private readonly ClassroomDbContext _context;

        public DeleteClassroomController(ClassroomDbContext context)
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

            return Ok(classroom);
        }

        [HttpDelete("DeleteClassroom/{id}")]
        public async Task<IActionResult> DeleteClassroom(int id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var classroom = await _context.Classrooms.FindAsync(id);
                    if (classroom == null)
                        return NotFound("Classroom not found");

                    var relatedSubjects = await _context.SubjectStaffs.Where(s => s.ClassroomId == id).ToListAsync();
                    if (relatedSubjects.Any())
                    {
                        _context.SubjectStaffs.RemoveRange(relatedSubjects);
                        await _context.SaveChangesAsync(); 
                    }

                    var relatedStudents = await _context.ClassroomStudents.Where(s => s.ClassroomId == id).ToListAsync();
                    if (relatedStudents.Any())
                    {
                        _context.ClassroomStudents.RemoveRange(relatedStudents);
                        await _context.SaveChangesAsync(); 
                    }

                    _context.Classrooms.Remove(classroom);
                    await _context.SaveChangesAsync(); 

                    await transaction.CommitAsync();

                    return Ok(new { message = "✅ Classroom and related records deleted successfully!" });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, $"❌ Failed to delete Classroom: {ex.Message}");
                }
            }
        }


    }
}
