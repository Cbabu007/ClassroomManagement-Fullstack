using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ClassroomManagement.Models.StudentAttendance;
using System.Data;

namespace ClassroomManagement.Controllers.StudentAttendance
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentAttendanceController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public StudentAttendanceController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("GetFinalAttendance")]
        public async Task<IActionResult> GetFinalAttendance(string studentId, DateTime fromDate, DateTime toDate)
        {
            var attendanceList = new List<StudentAttendanceModel>();

            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            using (SqlCommand cmd = new SqlCommand("GetAllFinalAttendanceReport", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", fromDate);
                cmd.Parameters.AddWithValue("@ToDate", toDate);

                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        if (reader["StudentId"].ToString() == studentId)
                        {
                            attendanceList.Add(new StudentAttendanceModel
                            {
                                SerialNo = Convert.ToInt32(reader["SerialNo"]),
                                Date = reader["Date"].ToString(),
                                LeaveType = reader["LeaveType"].ToString()
                            });
                        }
                    }
                }
            }

            return Ok(attendanceList);
        }
    }
}
