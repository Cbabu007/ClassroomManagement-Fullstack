using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ClassroomManagement.Data;

namespace ClassroomManagement.Controllers.StudentTestReport
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentTestReportController : ControllerBase
    {
        private readonly ClassroomDbContext _context;

        public StudentTestReportController(ClassroomDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetStudentFullReport")]
        public async Task<IActionResult> GetStudentFullReport(string email, string mobile)
        {
            var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = "GetStudentFullReportByRollNo";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@Email", email));
            command.Parameters.Add(new SqlParameter("@Mobile", mobile));

            var result = new List<object>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new
                {
                    studentId = reader["StudentId"],
                    email = reader["Email"],
                    mobile = reader["Mobile"],
                    date = reader["Date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["Date"]),
                    testId = reader["TestId"],
                    term = reader["Term"],
                    subject = reader["Subject"],
                    totalMark = Convert.ToInt32(reader["TotalMark"]),
                    answeredMark = Convert.ToInt32(reader["AnsweredMark"]),
                    remark = reader["Remark"]?.ToString(),
                    source = reader["Source"].ToString()
                });
            }

            return Ok(result);
        }

        [HttpGet("GetAllTerms")]
        public IActionResult GetAllTerms(string email, string mobile)
        {
            var rollNo = _context.Students
                .Where(s => s.Email == email && s.Mobile == mobile)
                .Select(s => s.RollNo)
                .FirstOrDefault();

            if (rollNo == null)
                return NotFound("Student not found.");

            var terms = _context.ReportCard
                .Where(r => r.RollNo == rollNo)
                .Select(r => r.Term)
                .Distinct()
                .ToList();

            return Ok(terms);
        }

        [HttpGet("GetAllTestNos")]
        public IActionResult GetAllTestNos(string email, string mobile)
        {
            var rollNo = _context.Students
                .Where(s => s.Email == email && s.Mobile == mobile)
                .Select(s => s.RollNo)
                .FirstOrDefault();

            if (rollNo == null)
                return NotFound("Student not found.");

            var testNos = _context.DailyTest
                .Where(t => t.RollNo == rollNo)
                .Select(t => t.TestNo) 
                .Distinct()
                .ToList();

            return Ok(testNos);
        }

    }
}