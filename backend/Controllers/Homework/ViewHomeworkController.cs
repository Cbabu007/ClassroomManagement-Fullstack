using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassroomManagement.Data;

namespace ClassroomManagement.Controllers.Homework
{
    [ApiController]
    [Route("api/[controller]")]
    public class ViewHomeworkController : ControllerBase
    {
        private readonly ClassroomDbContext _context;

        public ViewHomeworkController(ClassroomDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetHomework([FromQuery] string grade, [FromQuery] string section, [FromQuery] string subject, [FromQuery] DateTime date)
        {
            if (string.IsNullOrWhiteSpace(grade) || string.IsNullOrWhiteSpace(section) || string.IsNullOrWhiteSpace(subject))
                return BadRequest("Missing parameters.");

            var homework = await _context.Homework
                .Where(h => h.Grade == grade &&
                            h.Section == section &&
                            h.Subject == subject &&
                            h.Date.HasValue && h.Date.Value.Date == date.Date)
                .FirstOrDefaultAsync();

            if (homework == null)
                return NotFound("❌ No homework found for the given filters.");

            return Ok(new
            {
                homework.Topic,
                homework.Date,
                homework.Filepath
            });
        }

    }
}
