using Microsoft.AspNetCore.Mvc;
using ClassroomManagement.Data;
using ClassroomManagement.Models.Message;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagement.Controllers.Message
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly ClassroomDbContext _context;

        public MessageController(ClassroomDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageModel>>> GetMessages()
        {
            return await _context.Messages.OrderBy(m => m.Id).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> PostMessage([FromBody] MessageInputModel input)
        {
            if (string.IsNullOrWhiteSpace(input.Message)) return BadRequest("Message required");

            var newMsg = new MessageModel
            {
                Grade = input.Grade,
                Section = input.Section,
                Topic = input.Topic,
                StaffName = input.StaffName,
                Message = input.Message,
                Date = DateTime.Parse(input.Date ?? DateTime.Now.ToString("yyyy-MM-dd"))
            };

            _context.Messages.Add(newMsg);
            await _context.SaveChangesAsync();
            return Ok(newMsg);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var msg = await _context.Messages.FindAsync(id);
            if (msg == null) return NotFound();

            _context.Messages.Remove(msg);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
