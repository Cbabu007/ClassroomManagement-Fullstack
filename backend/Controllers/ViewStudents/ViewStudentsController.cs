using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using ClassroomManagement.Models.ViewStudents;
using Microsoft.EntityFrameworkCore;
using ClassroomManagement.Data; 
using ClassroomManagement.Models.Admin; 

namespace ClassroomManagement.Controllers.ViewStudents
{
    [ApiController]
    [Route("api/[controller]")]
    public class ViewStudentsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ClassroomDbContext _context;

        public ViewStudentsController(IConfiguration configuration, ClassroomDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        
        [HttpGet("GetStudents")]
        public async Task<IActionResult> GetStudents(string grade, string section, string medium)
        {
            List<ViewStudentModel> students = new List<ViewStudentModel>();

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetStudentsByGradeSectionMedium", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Grade", grade);
                    cmd.Parameters.AddWithValue("@Section", section);
                    cmd.Parameters.AddWithValue("@Medium", medium);

                    await con.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            students.Add(new ViewStudentModel
                            {
                                StudentId = reader["StudentId"].ToString(),
                                PhotoPath = reader["PhotoPath"].ToString(),
                                RollNo = reader["RollNo"].ToString(),
                                Name = reader["Name"].ToString(),
                                DOB = Convert.ToDateTime(reader["DOB"]),
                                Gender = reader["Gender"].ToString(),
                                BloodGroup = reader["BloodGroup"].ToString(),
                                Mobile = reader["Mobile"].ToString(),
                                AltMobile = reader["AltMobile"].ToString(),
                                Email = reader["Email"].ToString(),
                                Address = $"{reader["DoorNo"]}, {reader["Address1"]}, {reader["Address2"]}, {reader["City"]}, {reader["District"]}, {reader["State"]} - {reader["Pincode"]}, {reader["Country"]}",
                                FatherName = reader["FatherName"].ToString(),
                                FatherMobile = reader["FatherMobile"].ToString(),
                                MotherName = reader["MotherName"].ToString(),
                                MotherMobile = reader["MotherMobile"].ToString(),
                                GuardianName = reader["GuardianName"].ToString(),
                                GuardianMobile = reader["GuardianMobile"].ToString()
                            });
                        }
                    }
                }
            }

            return Ok(students);
        }

        
        [HttpGet("GetMediums")]
        public async Task<IActionResult> GetMediums()
        {
            var mediums = await _context.Students
                .Where(s => !string.IsNullOrEmpty(s.Medium))
                .Select(s => s.Medium)
                .Distinct()
                .ToListAsync();

            return Ok(mediums);
        }
    }
}
