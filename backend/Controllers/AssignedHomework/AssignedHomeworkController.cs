using ClassroomManagement.Models.AssignedHomework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace ClassroomManagement.Controllers.AssignedHomework
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignedHomeworkController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AssignedHomeworkController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("GetByGradeSection")]
        public IActionResult GetByGradeSection(string grade, string section)
        {
            List<AssignedHomeworkModel> result = new();
            using SqlConnection conn = new(_configuration.GetConnectionString("DefaultConnection"));
            conn.Open();

            string query = @"SELECT Id, StaffName, Grade, Section, Subject, Topic, Date, FilePath
                             FROM Homework
                             WHERE Grade = @Grade AND Section = @Section
                             ORDER BY Date DESC";

            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@Grade", grade);
            cmd.Parameters.AddWithValue("@Section", section);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new AssignedHomeworkModel
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    StaffName = reader["StaffName"].ToString(),
                    Grade = reader["Grade"].ToString(),
                    Section = reader["Section"].ToString(),
                    Subject = reader["Subject"].ToString(),
                    Topic = reader["Topic"].ToString(),
                    Date = Convert.ToDateTime(reader["Date"]),
                    FilePath = reader["FilePath"].ToString()
                });
            }

            return Ok(result);
        }

        [HttpGet("GetAllGrades")]
        public IActionResult GetAllGrades()
        {
            List<string> grades = new();
            using SqlConnection conn = new(_configuration.GetConnectionString("DefaultConnection"));
            conn.Open();
            string query = "SELECT DISTINCT Grade FROM Homework";
            using SqlCommand cmd = new(query, conn);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                grades.Add(reader["Grade"].ToString());
            return Ok(grades);
        }

        [HttpGet("GetSectionsByGrade")]
        public IActionResult GetSectionsByGrade(string grade)
        {
            List<string> sections = new();
            using SqlConnection conn = new(_configuration.GetConnectionString("DefaultConnection"));
            conn.Open();
            string query = "SELECT DISTINCT Section FROM Homework WHERE Grade = @Grade";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@Grade", grade);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                sections.Add(reader["Section"].ToString());
            return Ok(sections);
        }
    }
}
