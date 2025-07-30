using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using ClassroomManagement.Models.ViewMessage;

namespace ClassroomManagement.Controllers.ViewMessage
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewMessageController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ViewMessageController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("GetMessages")]
        public async Task<IActionResult> GetMessages([FromBody] LoginRequest request)
        {
            List<ViewMessageModel> messages = new();
            using (SqlConnection con = new(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new("GetStudentMessagesByLogin", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", request.Email);
                    cmd.Parameters.AddWithValue("@Mobile", request.Mobile);
                    con.Open();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        messages.Add(new ViewMessageModel
                        {
                            Date = reader["Date"].ToString(),
                            Topic = reader["Topic"].ToString(),
                            StaffName = reader["StaffName"].ToString(),
                            Message = reader["Message"].ToString()
                        });
                    }
                    con.Close();
                }
            }
            return Ok(messages);
        }

        public class LoginRequest
        {
            public string Email { get; set; }
            public string Mobile { get; set; }
        }
    }
}
