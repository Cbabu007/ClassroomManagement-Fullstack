using Microsoft.AspNetCore.Mvc;
using ClassroomManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagement.Controllers.Homework
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeleteHomeworkController : ControllerBase
    {
        private readonly ClassroomDbContext _context;

        public DeleteHomeworkController(ClassroomDbContext context)
        {
            _context = context;
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteHomework(string grade, string section, string subject, DateTime date)
        {
            var item = await _context.Homework
                .FirstOrDefaultAsync(h =>
                    h.Grade == grade &&
                    h.Section == section &&
                    h.Subject == subject &&
                    h.Date.HasValue && h.Date.Value.Date == date.Date);

            if (item == null)
                return NotFound("❌ No homework found to delete.");

            _context.Homework.Remove(item);
            await _context.SaveChangesAsync();

            return Ok("🗑️ Homework deleted.");
        }
    }
}
