using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using ClassroomManagement.Models.AttendanceReport;

namespace ClassroomManagement.Controllers.AttendanceReport
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceReportController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AttendanceReportController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("GetByDateRange")]
        public IActionResult GetByDateRange(string studentId, DateTime fromDate, DateTime toDate)
        {
            List<AttendanceReportModel> result = new();

            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("GetStudentFinalAttendance", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudentId", studentId);
                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new AttendanceReportModel
                            {
                                SerialNo = Convert.ToInt32(reader["SerialNo"]),
                                DateOnLeave = reader["DateOnLeave"].ToString(),
                                LeaveType = reader["LeaveType"].ToString()
                            });
                        }
                    }
                }
            }

            return Ok(result);
        }
    }
}
