using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using ClassroomManagement.Models.SubmitHomework;

namespace ClassroomManagement.Controllers.SubmitHomework
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmitHomeworkController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public SubmitHomeworkController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        [HttpGet("GetStudentByLogin")]
        public IActionResult GetStudentByLogin(string email, string mobile)
        {
            using SqlConnection conn = new(_configuration.GetConnectionString("DefaultConnection"));
            conn.Open();
            string query = "SELECT TOP 1 StudentId, FirstName, LastName, RollNo, Grade, Section FROM Students WHERE Email = @Email AND Mobile = @Mobile";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Mobile", mobile);

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return Ok(new
                {
                    StudentId = reader["StudentId"].ToString(),
                    FirstName = reader["FirstName"].ToString(),
                    LastName = reader["LastName"].ToString(),
                    RollNo = reader["RollNo"].ToString(),
                    Grade = reader["Grade"].ToString(),
                    Section = reader["Section"].ToString()
                });
            }
            return NotFound();
        }

        [HttpGet("GetSubjectsAndTopics")]
        public IActionResult GetSubjectsAndTopics(string grade, string section)
        {
            List<string> subjects = new();
            List<string> topics = new();

            using SqlConnection conn = new(_configuration.GetConnectionString("DefaultConnection"));
            conn.Open();
            string query = "SELECT DISTINCT Subject, Topic FROM Homework WHERE Grade = @Grade AND Section = @Section";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@Grade", grade);
            cmd.Parameters.AddWithValue("@Section", section);

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (!subjects.Contains(reader["Subject"].ToString()))
                    subjects.Add(reader["Subject"].ToString());
                if (!topics.Contains(reader["Topic"].ToString()))
                    topics.Add(reader["Topic"].ToString());
            }

            return Ok(new { subjects, topics });
        }

        [HttpPost("Submit")]
        public IActionResult Submit([FromForm] SubmitHomeworkModel model)
        {
            if (model.File == null || model.File.Length == 0)
                return BadRequest("File is required.");

            string folder = Path.Combine(_environment.WebRootPath, "uploads", "uploadhomework");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.File.FileName);
            string filePath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                model.File.CopyTo(stream);
            }

            string dbPath = "/uploads/uploadhomework/" + fileName;

            using SqlConnection conn = new(_configuration.GetConnectionString("DefaultConnection"));
            conn.Open();

            string insertQuery = @"
                INSERT INTO UploadHomework (StudentId, Name, RollNo, Grade, Section, Subject, Topic, FilePath)
                VALUES (@StudentId, @Name, @RollNo, @Grade, @Section, @Subject, @Topic, @FilePath)";

            using SqlCommand cmd = new(insertQuery, conn);
            cmd.Parameters.AddWithValue("@StudentId", model.StudentId);
            cmd.Parameters.AddWithValue("@Name", model.Name);
            cmd.Parameters.AddWithValue("@RollNo", model.RollNo);
            cmd.Parameters.AddWithValue("@Grade", model.Grade);
            cmd.Parameters.AddWithValue("@Section", model.Section);
            cmd.Parameters.AddWithValue("@Subject", model.Subject);
            cmd.Parameters.AddWithValue("@Topic", model.Topic);
            cmd.Parameters.AddWithValue("@FilePath", dbPath);

            cmd.ExecuteNonQuery();
            return Ok(new { status = "success", message = "Homework submitted successfully." });
        }
    }
}
