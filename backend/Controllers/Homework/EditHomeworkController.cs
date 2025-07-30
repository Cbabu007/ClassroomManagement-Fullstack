using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassroomManagement.Data;
using HomeworkModel = ClassroomManagement.Models.Homework.Homework;

namespace ClassroomManagement.Controllers.Homework
{
    [ApiController]
    [Route("api/[controller]")]
    public class EditHomeworkController : ControllerBase
    {
        private readonly ClassroomDbContext _context;

        public EditHomeworkController(ClassroomDbContext context)
        {
            _context = context;
        }

       
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetHomeworkById(int id)
        {
            var homework = await _context.Homework.FindAsync(id);
            return homework == null ? NotFound("Not found") : Ok(homework);
        }

        
        [HttpGet]
        public async Task<IActionResult> GetHomework([FromQuery] string grade, [FromQuery] string section, [FromQuery] string subject, [FromQuery] DateTime date)
        {
            var homework = await _context.Homework
                .FirstOrDefaultAsync(h =>
                    h.Grade == grade &&
                    h.Section == section &&
                    h.Subject == subject &&
                    h.Date.HasValue &&
                    h.Date.Value.Date == date.Date);

            if (homework == null) return NotFound("No homework found.");
            return Ok(homework);
        }

        
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] HomeworkModel updated, IFormFile? file)
        {
            var existing = await _context.Homework.FindAsync(id);
            if (existing == null) return NotFound("Homework not found");

            existing.Grade = updated.Grade;
            existing.Section = updated.Section;
            existing.ClassroomNo = updated.ClassroomNo;
            existing.StaffId = updated.StaffId;
            existing.StaffName = updated.StaffName;
            existing.Subject = updated.Subject;
            existing.Date = updated.Date;
            existing.Topic = updated.Topic;

            if (file != null)
            {
                var folderPath = Path.Combine("wwwroot", "uploads", "homework");
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var path = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                    await file.CopyToAsync(stream);

                existing.Filepath = "/uploads/homework/" + fileName;
            }

            await _context.SaveChangesAsync();
            return Ok("✅ Homework updated");
        }

        
        [HttpGet("GetSubjects")]
        public async Task<IActionResult> GetSubjects()
        {
            var subjects = await _context.Homework
                .Select(h => h.Subject)
                .Distinct()
                .ToListAsync();

            return Ok(subjects);
        }
    }
}
