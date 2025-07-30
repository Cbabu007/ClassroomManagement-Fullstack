using Microsoft.AspNetCore.Mvc;
using ClassroomManagement.Data;
using ClassroomManagement.Models.Events;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagement.Controllers.Events
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly ClassroomDbContext _context;

        public EventController(ClassroomDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventModel>>> GetEvents()
        {
            return await _context.Events.OrderBy(e => e.Id).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<EventModel>> PostEvent([FromBody] EventModel eventItem)
        {
            if (string.IsNullOrWhiteSpace(eventItem.Message))
                return BadRequest("Message is required.");

            var newEvent = new EventModel
            {
                Message = eventItem.Message,
                Date = eventItem.Date
            };

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return Ok(newEvent);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
                return NotFound();

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
