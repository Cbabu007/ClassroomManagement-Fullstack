using Microsoft.AspNetCore.Mvc;
using ClassroomManagement.Models.AdminProfile;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ClassroomManagement.Controllers.AdminProfile
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminProfileController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AdminProfileController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("GetByUsername")]
        public IActionResult GetByUsername(string username)
        {
            var staff = new AdminProfileModel();

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string query = @"SELECT StaffId, FirstName, LastName, Gender, DateOfBirth, 
                                        Mobile, Email, Department, Role, JoiningDate, PhotoPath 
                                 FROM Staffs 
                                 WHERE Username = @Username";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            staff.StaffId = reader["StaffId"].ToString();
                            staff.FirstName = reader["FirstName"].ToString();
                            staff.LastName = reader["LastName"].ToString();
                            staff.Gender = reader["Gender"].ToString();
                            staff.DateOfBirth = reader["DateOfBirth"] == DBNull.Value ? null : Convert.ToDateTime(reader["DateOfBirth"]);
                            staff.Mobile = reader["Mobile"].ToString();
                            staff.Email = reader["Email"].ToString();
                            staff.Department = reader["Department"].ToString();
                            staff.Role = reader["Role"].ToString();
                            staff.JoiningDate = reader["JoiningDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["JoiningDate"]);
                            staff.PhotoPath = reader["PhotoPath"].ToString();
                        }
                        else
                        {
                            return NotFound("Staff not found.");
                        }
                    }
                }
            }

            return Ok(staff);
        }
    }
}
