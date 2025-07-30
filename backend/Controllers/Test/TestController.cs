using ClassroomManagement.Models.Test;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace ClassroomManagement.Controllers.Test
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("SubmitReportCard")]
        public IActionResult SubmitReportCard([FromBody] ReportCardRequest request)
        {
            var dt = new DataTable();
            dt.Columns.Add("Term", typeof(string));
            dt.Columns.Add("TestId", typeof(int));
            dt.Columns.Add("Subject", typeof(string));
            dt.Columns.Add("TotalMark", typeof(int));
            dt.Columns.Add("AnsweredMark", typeof(int));
            dt.Columns.Add("Remark", typeof(string));

            foreach (var entry in request.Entries)
            {
                dt.Rows.Add(entry.Term, entry.TestId, entry.Subject, entry.TotalMark, entry.AnsweredMark, entry.Remark);
            }

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                SqlCommand cmd = new SqlCommand("testtable_InsertReportCard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RollNo", request.RollNo);
                cmd.Parameters.AddWithValue("@Grade", request.Grade);
                cmd.Parameters.AddWithValue("@Section", request.Section);
                cmd.Parameters.AddWithValue("@Date", request.Date);
                cmd.Parameters.AddWithValue("@Entries", dt);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            return Ok(new { message = "Report Card saved successfully" });
        }

        [HttpPost("SubmitDailyTest")]
        public IActionResult SubmitDailyTest([FromBody] DailyTestRequest request)
        {
            var dt = new DataTable();
            dt.Columns.Add("TestNo", typeof(string));
            dt.Columns.Add("TestId", typeof(int));
            dt.Columns.Add("Subject", typeof(string));
            dt.Columns.Add("Topic", typeof(string));
            dt.Columns.Add("QuestionNo", typeof(int));
            dt.Columns.Add("SelectMark", typeof(int));
            dt.Columns.Add("AnsweredMark", typeof(int));
            dt.Columns.Add("Remark", typeof(string));

            foreach (var entry in request.Entries)
            {
                dt.Rows.Add(entry.TestNo, entry.TestId, entry.Subject, entry.Topic, entry.QuestionNo, entry.SelectMark, entry.AnsweredMark, entry.Remark);
            }

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                SqlCommand cmd = new SqlCommand("testtable_InsertDailyTest", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RollNo", request.RollNo);
                cmd.Parameters.AddWithValue("@Grade", request.Grade);
                cmd.Parameters.AddWithValue("@Section", request.Section);
                cmd.Parameters.AddWithValue("@Date", request.Date);
                cmd.Parameters.AddWithValue("@Entries", dt);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            return Ok(new { message = "Daily Test saved successfully" });
        }

        [HttpGet("GetReportCard")]
        public IActionResult GetReportCard(string rollNo, DateTime date)
        {
            var result = new List<object>();

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                SqlCommand cmd = new SqlCommand("testtable_GetReportCard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RollNo", rollNo);
                cmd.Parameters.AddWithValue("@Date", date);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new
                        {
                            Term = reader["Term"],
                            TestId = reader["TestId"],
                            Subject = reader["Subject"],
                            TotalMark = reader["TotalMark"],
                            AnsweredMark = reader["AnsweredMark"],
                            Remark = reader["Remark"]
                        });
                    }
                }
                con.Close();
            }

            return Ok(result);
        }

        [HttpGet("GetDailyTest")]
        public IActionResult GetDailyTest(string rollNo, DateTime date)
        {
            var result = new List<object>();

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                SqlCommand cmd = new SqlCommand("testtable_GetDailyTest", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RollNo", rollNo);
                cmd.Parameters.AddWithValue("@Date", date);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new
                        {
                            TestNo = reader["TestNo"],
                            TestId = reader["TestId"],
                            Subject = reader["Subject"],
                            Topic = reader["Topic"],
                            QuestionNo = reader["QuestionNo"],
                            SelectMark = reader["SelectMark"],
                            AnsweredMark = reader["AnsweredMark"],
                            Remark = reader["Remark"]
                        });
                    }
                }
                con.Close();
            }

            return Ok(result);
        }

        [HttpGet("GetDailyTestByTestNo")]
        public IActionResult GetDailyTestByTestNo(string rollNo, DateTime date, string testNo)
        {
            var result = new List<object>();

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                SqlCommand cmd = new SqlCommand("testtable_GetDailyTestByTestNo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RollNo", rollNo);
                cmd.Parameters.AddWithValue("@Date", date);
                cmd.Parameters.AddWithValue("@TestNo", testNo);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new
                        {
                            TestNo = reader["TestNo"],
                            TestId = reader["TestId"],
                            Subject = reader["Subject"],
                            Topic = reader["Topic"],
                            QuestionNo = reader["QuestionNo"],
                            SelectMark = reader["SelectMark"],
                            AnsweredMark = reader["AnsweredMark"],
                            Remark = reader["Remark"]
                        });
                    }
                }
                con.Close();
            }

            return Ok(result);
        }
        [HttpGet("GetStudentByRollNo")]
        public IActionResult GetStudentByRollNo(string rollNo)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                SqlCommand cmd = new SqlCommand("SELECT FirstName, LastName, PhotoPath FROM Students WHERE RollNo = @RollNo", con);
                cmd.Parameters.AddWithValue("@RollNo", rollNo);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    var student = new
                    {
                        FullName = reader["FirstName"].ToString() + " " + reader["LastName"].ToString(),
                        PhotoPath = reader["PhotoPath"].ToString()
                    };
                    con.Close();
                    return Ok(student);
                }
                con.Close();
                return NotFound(new { message = "Student not found" });
            }
        }

        [HttpGet("GetAvailableTestNos")]
        public IActionResult GetAvailableTestNos(string rollNo)
        {
            var testNos = new List<string>();

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                SqlCommand cmd = new SqlCommand("SELECT DISTINCT TestNo FROM DailyTest WHERE RollNo = @RollNo", con);
                cmd.Parameters.AddWithValue("@RollNo", rollNo);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    testNos.Add(reader["TestNo"].ToString());
                }
                con.Close();
            }

            return Ok(testNos);
        }

    }
}
