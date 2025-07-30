using Microsoft.AspNetCore.Mvc;
using ClassroomManagement.Models.YourActivity;
using System.Data.SqlClient;
using System.Data;

namespace ClassroomManagement.Controllers.YourActivity
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginActivityController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LoginActivityController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            List<LoginActivityModel> list = new();

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string query = "SELECT * FROM LoginActivity";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new LoginActivityModel
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        StudentId = reader["StudentId"].ToString(),
                        StudentName = reader["StudentName"].ToString(),
                        Browse = reader["Browse"].ToString(),
                        Day = Convert.ToInt32(reader["Day"]),
                        Month = reader["Month"].ToString(),
                        Year = Convert.ToInt32(reader["Year"]),
                        LoginTime = (TimeSpan)reader["LoginTime"],
                        LogoutTime = (TimeSpan)reader["LogoutTime"]
                    });
                }
            }

            return Ok(list);
        }

        
        [HttpPost("Insert")]
        public IActionResult Insert([FromBody] LoginActivityModel model)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string query = @"INSERT INTO LoginActivity 
                                (StudentId, StudentName, [Browse], Day, Month, Year, LoginTime, LogoutTime)
                                 VALUES
                                (@StudentId, @StudentName, @Browse, @Day, @Month, @Year, @LoginTime, @LogoutTime)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@StudentId", model.StudentId);
                cmd.Parameters.AddWithValue("@StudentName", model.StudentName);
                cmd.Parameters.AddWithValue("@Browse", model.Browse);
                cmd.Parameters.AddWithValue("@Day", model.Day);
                cmd.Parameters.AddWithValue("@Month", model.Month);
                cmd.Parameters.AddWithValue("@Year", model.Year);
                cmd.Parameters.AddWithValue("@LoginTime", model.LoginTime);
                cmd.Parameters.AddWithValue("@LogoutTime", model.LogoutTime);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Ok(new { Message = "Student login activity inserted successfully" });
        }
    }
}
