using Microsoft.AspNetCore.Mvc;
using ClassroomManagement.Models.YourActivity;
using System.Data.SqlClient;

namespace ClassroomManagement.Controllers.YourActivity
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminActivityController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AdminActivityController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("Insert")]
        public IActionResult Insert([FromBody] AdminActivityModel model)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string query = @"INSERT INTO AdminActivity 
                                (StaffId, StaffName, [Browse], Day, Month, Year, LoginTime, LogoutTime)
                                 VALUES
                                (@StaffId, @StaffName, @Browse, @Day, @Month, @Year, @LoginTime, @LogoutTime)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@StaffId", model.StaffId);
                cmd.Parameters.AddWithValue("@StaffName", model.StaffName);
                cmd.Parameters.AddWithValue("@Browse", model.Browse);
                cmd.Parameters.AddWithValue("@Day", model.Day);
                cmd.Parameters.AddWithValue("@Month", model.Month);
                cmd.Parameters.AddWithValue("@Year", model.Year);
                cmd.Parameters.AddWithValue("@LoginTime", model.LoginTime);
                cmd.Parameters.AddWithValue("@LogoutTime", model.LogoutTime);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Ok(new { Message = "✅ Admin login activity inserted successfully" });
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            List<AdminActivityModel> list = new();

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM AdminActivity", con);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new AdminActivityModel
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        StaffId = reader["StaffId"].ToString(),
                        StaffName = reader["StaffName"].ToString(),
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
    }
}
