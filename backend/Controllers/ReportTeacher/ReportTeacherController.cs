using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using ClassroomManagement.Models.ReportTeacher;

namespace ClassroomManagement.Controllers.ReportTeacher
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportTeacherController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ReportTeacherController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public IActionResult SubmitReport([FromBody] ReportTeacherModel model)
        {
            if (model == null)
                return BadRequest("Invalid data");

            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                string query = @"INSERT INTO TeacherReport (Grade, Section, ReportDate, Message)
                                 VALUES (@Grade, @Section, @ReportDate, @Message)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Grade", model.Grade ?? "");
                    cmd.Parameters.AddWithValue("@Section", model.Section ?? "");
                    cmd.Parameters.AddWithValue("@ReportDate", model.ReportDate);
                    cmd.Parameters.AddWithValue("@Message", model.Message ?? "");

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            return Ok(new { message = "✅ Report submitted successfully." });
        }
    }
}
