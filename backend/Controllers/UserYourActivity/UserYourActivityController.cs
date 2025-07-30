using ClassroomManagement.Data;
using ClassroomManagement.Models.UserYourActivity;
using Microsoft.AspNetCore.Mvc;

namespace ClassroomManagement.Controllers.UserYourActivity
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserYourActivityController : ControllerBase
    {
        private readonly ClassroomDbContext _context;

        public UserYourActivityController(ClassroomDbContext context)
        {
            _context = context;
        }

        
        [HttpPost("LogLoginActivity")]
        public async Task<IActionResult> LogLoginActivity([FromBody] UserYourActivityModel model)
        {
            if (model == null) return BadRequest("Invalid data");

            model.LoginTime = DateTime.Now;
            model.IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            _context.UserYourActivity.Add(model);
            await _context.SaveChangesAsync();

            return Ok(new { status = "Saved" });
        }

       
        [HttpGet("GetAllActivity")]
        public IActionResult GetAllActivity()
        {
            var activityList = _context.UserYourActivity.OrderByDescending(x => x.LoginTime).ToList();
            return Ok(activityList);
        }

        
        [HttpGet("FilterByEmailAndMonth")]
        public IActionResult FilterByEmailAndMonth(string email, int month, int year)
        {
            var result = _context.UserYourActivity
                .Where(x => x.Email == email &&
                            x.LoginTime.Month == month &&
                            x.LoginTime.Year == year)
                .OrderByDescending(x => x.LoginTime)
                .ToList();

            return Ok(result);
        }
    }
}
