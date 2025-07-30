using ClassroomManagement.Models.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace ClassroomManagement.Controllers.Login
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("ValidateLogin")]
        public async Task<IActionResult> ValidateLogin([FromBody] LoginModel model)
        {
            var response = new LoginResponseModel();

            try
            {
                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                using (SqlCommand cmd = new SqlCommand("ValidateLogin", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", model.Username);
                    cmd.Parameters.AddWithValue("@Password", model.Password);

                    conn.Open();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            response.Status = reader["Status"].ToString();
                            response.Role = reader["Role"].ToString();
                            response.RedirectUrl = reader["RedirectUrl"].ToString();
                            response.FirstName = reader["FirstName"].ToString();
                            response.LastName = reader["LastName"].ToString();
                            response.PhotoPath = reader["PhotoPath"].ToString();

                            
                            if (response.Role == "Student" && reader["StudentId"] != DBNull.Value)
                                response.StudentId = reader["StudentId"].ToString();
                            else if ((response.Role == "Staff" || response.Role == "Admin") && reader["StaffId"] != DBNull.Value)
                                response.StaffId = reader["StaffId"].ToString();

                            
                            string fullName = response.FirstName + " " + response.LastName;
                            string id = response.Role == "Student" ? response.StudentId : response.StaffId;
                            InsertLoginActivity(response.Role, id, fullName);
                        }
                        else
                        {
                            response.Status = "Failed";
                        }
                    }
                }

                if (response.Status == "Success")
                    return Ok(response);
                else
                    return Unauthorized(new { message = "Invalid credentials." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Server error", error = ex.Message });
            }
        }

        private void InsertLoginActivity(string role, string id, string name)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string tableName = role switch
                {
                    "Student" => "LoginActivity",
                    "Teacher" or "Staff" => "TeacherActivity",
                    "Admin" => "AdminActivity",
                    _ => null
                };

                if (tableName == null) return;

                DateTime now = DateTime.Now;
                string columnId = role == "Student" ? "StudentId" : "StaffId";
                string columnName = role == "Student" ? "StudentName" : "StaffName";

                string query = $@"
                    INSERT INTO {tableName}
                    ({columnId}, {columnName}, [Browse], Day, Month, Year, LoginTime, LogoutTime)
                    VALUES
                    (@Id, @Name, @Browse, @Day, @Month, @Year, @LoginTime, @LogoutTime)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Browse", "API");
                cmd.Parameters.AddWithValue("@Day", now.Day);
                cmd.Parameters.AddWithValue("@Month", now.ToString("MMMM"));
                cmd.Parameters.AddWithValue("@Year", now.Year);
                cmd.Parameters.AddWithValue("@LoginTime", now.TimeOfDay);
                cmd.Parameters.AddWithValue("@LogoutTime", TimeSpan.Zero);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
