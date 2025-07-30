using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ClassroomManagement.Data;
using ClassroomManagement.Models.VideoClass;
using System.Data;

namespace ClassroomManagement.Controllers.AssignedVideoClass
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignedVideoClassController : ControllerBase
    {
        private readonly ClassroomDbContext _context;
        private readonly IConfiguration _config;

        public AssignedVideoClassController(ClassroomDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

       
        [HttpGet("GetDropdowns")]
        public async Task<IActionResult> GetDropdowns()
        {
            var result = new
            {
                Grades = new List<string>(),
                Sections = new List<string>(),
                Subjects = new List<string>(),
                Staffs = new List<object>(),
                Emails = new List<string>()
            };

            var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var cmd = new SqlCommand("sp_GetVideoClassDropdowns", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            await conn.OpenAsync();
            var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
                result.Grades.Add(reader.GetString(0));

            await reader.NextResultAsync();
            while (await reader.ReadAsync())
                result.Sections.Add(reader.GetString(0));

            await reader.NextResultAsync();
            while (await reader.ReadAsync())
                result.Subjects.Add(reader.GetString(0));

            await reader.NextResultAsync();
            while (await reader.ReadAsync())
            {
                result.Staffs.Add(new
                {
                    StaffId = reader["StaffId"].ToString(),
                    StaffName = reader["StaffName"].ToString()
                });
            }

            await reader.NextResultAsync();
            while (await reader.ReadAsync())
                result.Emails.Add(reader.GetString(0));

            await conn.CloseAsync();
            return Ok(result);
        }

        
        [HttpPost("Assign")]
        public async Task<IActionResult> AssignVideo([FromBody] ClassroomManagement.Models.VideoClass.AssignedVideoClass model)
        {
            if (ModelState.IsValid)
            {
                _context.AssignedVideoClass.Add(model);
                await _context.SaveChangesAsync();
                return Ok(new { message = "✅ Video assigned successfully" });
            }
            return BadRequest("❌ Invalid data");
        }

        
        [HttpGet("GetVideoDetailsForEmail")]
        public async Task<IActionResult> GetVideoDetailsForEmail(string grade, string section, DateTime date)
        {
            var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var cmd = new SqlCommand("sp_GetAssignedVideoForEmail", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@Grade", grade);
            cmd.Parameters.AddWithValue("@Section", section);
            cmd.Parameters.AddWithValue("@Date", date);

            await conn.OpenAsync();
            var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var videoDetails = new
                {
                    Grade = reader["Grade"].ToString(),
                    Section = reader["Section"].ToString(),
                    Date = Convert.ToDateTime(reader["Date"]).ToString("yyyy-MM-dd"),
                    Subject = reader["Subject"].ToString(),
                    SubjectTopic = reader["SubjectTopic"].ToString(),
                    Message = reader["Message"].ToString(),
                    Url = reader["Url"].ToString()
                };

                await conn.CloseAsync();
                return Ok(videoDetails);
            }

            await conn.CloseAsync();
            return NotFound("❌ No video details found");
        }
    }
}
