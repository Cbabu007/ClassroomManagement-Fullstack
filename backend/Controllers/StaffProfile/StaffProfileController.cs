using ClassroomManagement.Models.StaffProfile;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace ClassroomManagement.Controllers.StaffProfile
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffProfileController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public StaffProfileController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("GetByUsername")]
        public IActionResult GetByUsername(string username)
        {
            StaffProfileModel profile = new StaffProfileModel();

            try
            {
                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Staffs WHERE Username = @Username", conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            profile.StaffId = reader["StaffId"]?.ToString();
                            profile.FirstName = reader["FirstName"]?.ToString();
                            profile.LastName = reader["LastName"]?.ToString();
                            profile.Gender = reader["Gender"]?.ToString();
                            profile.DateOfBirth = reader["DateOfBirth"] as DateTime?;
                            profile.BloodGroup = reader["BloodGroup"]?.ToString();
                            profile.AadharNumber = reader["AadharNumber"]?.ToString();

                            profile.Mobile = reader["Mobile"]?.ToString();
                            profile.AltMobile = reader["AltMobile"]?.ToString();
                            profile.Email = reader["Email"]?.ToString();

                            profile.DoorNo = reader["DoorNo"]?.ToString();
                            profile.Address1 = reader["Address1"]?.ToString();
                            profile.Address2 = reader["Address2"]?.ToString();
                            profile.Taluk = reader["Taluk"]?.ToString();
                            profile.District = reader["District"]?.ToString();
                            profile.State = reader["State"]?.ToString();
                            profile.Pincode = reader["Pincode"]?.ToString();
                            profile.Country = reader["Country"]?.ToString();

                            profile.Department = reader["Department"]?.ToString();
                            profile.Role = reader["Role"]?.ToString();
                            profile.JoiningDate = reader["JoiningDate"] as DateTime?;
                            profile.StaffType = reader["StaffType"]?.ToString();

                            profile.PhotoPath = reader["PhotoPath"]?.ToString();
                        }
                        else
                        {
                            return NotFound(new { message = "Profile not found." });
                        }
                    }
                }

                return Ok(profile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Server error", error = ex.Message });
            }
        }
    }
}
