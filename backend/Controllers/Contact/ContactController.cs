using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using ClassroomManagement.Models.Contact;

namespace ClassroomManagement.Controllers.Contact
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ContactController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        
        [HttpPost("Insert")]
        public IActionResult Insert([FromBody] ContactModel model)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string query = @"INSERT INTO Contact 
                (SchoolName, Address, OfficePhone, Mobile, Email, Website, WorkingHours, Facebook, Instagram, Twitter)
                VALUES 
                (@SchoolName, @Address, @OfficePhone, @Mobile, @Email, @Website, @WorkingHours, @Facebook, @Instagram, @Twitter)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@SchoolName", model.SchoolName);
                cmd.Parameters.AddWithValue("@Address", model.Address);
                cmd.Parameters.AddWithValue("@OfficePhone", model.OfficePhone);
                cmd.Parameters.AddWithValue("@Mobile", model.Mobile);
                cmd.Parameters.AddWithValue("@Email", model.Email);
                cmd.Parameters.AddWithValue("@Website", model.Website);
                cmd.Parameters.AddWithValue("@WorkingHours", model.WorkingHours);
                cmd.Parameters.AddWithValue("@Facebook", model.Facebook);
                cmd.Parameters.AddWithValue("@Instagram", model.Instagram);
                cmd.Parameters.AddWithValue("@Twitter", model.Twitter);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Ok(new { Message = "✅ Contact inserted successfully into Contact table" });
        }

       
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            List<ContactModel> list = new();

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string query = "SELECT * FROM Contact";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new ContactModel
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        SchoolName = reader["SchoolName"].ToString(),
                        Address = reader["Address"].ToString(),
                        OfficePhone = reader["OfficePhone"].ToString(),
                        Mobile = reader["Mobile"].ToString(),
                        Email = reader["Email"].ToString(),
                        Website = reader["Website"].ToString(),
                        WorkingHours = reader["WorkingHours"].ToString(),
                        Facebook = reader["Facebook"].ToString(),
                        Instagram = reader["Instagram"].ToString(),
                        Twitter = reader["Twitter"].ToString()
                    });
                }
            }

            return Ok(list);
        }
    }
}
