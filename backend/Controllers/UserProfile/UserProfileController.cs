using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using ClassroomManagement.Models.UserProfile;

namespace ClassroomManagement.Controllers.UserProfile
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UserProfileController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("GetByLogin")]
        public IActionResult GetByLogin(string email, string mobile)
        {
            UserProfileModel profile = null;

            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM Students WHERE Email = @Email AND Mobile = @Mobile", conn))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Mobile", mobile);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        profile = new UserProfileModel
                        {
                            StudentId = reader["StudentId"].ToString(),
                            PhotoPath = reader["PhotoPath"].ToString().Replace("\\", "/"),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Grade = reader["Grade"].ToString(),
                            Section = reader["Section"].ToString(),
                            Email = reader["Email"].ToString(),
                            Mobile = reader["Mobile"].ToString(),
                            AltMobile = reader["AltMobile"].ToString(),
                            RollNo = reader["RollNo"].ToString(),
                            DOB = Convert.ToDateTime(reader["DOB"]),
                            DoorNo = reader["DoorNo"].ToString(),
                            Address1 = reader["Address1"].ToString(),
                            Address2 = reader["Address2"].ToString(),
                            City = reader["City"].ToString(),
                            District = reader["District"].ToString(),
                            State = reader["State"].ToString(),
                            Pincode = reader["Pincode"].ToString()
                        };
                    }
                }
            }

            return profile != null ? Ok(profile) : NotFound(new { message = "Profile not found." });
        }
    }
}
