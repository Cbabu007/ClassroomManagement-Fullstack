using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using ClassroomManagement.Models.Homework;

namespace ClassroomManagement.Controllers.Homework
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CorrectedHomeworkControllers : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly SqlConnection _con;

        public CorrectedHomeworkControllers(IConfiguration config)
        {
            _config = config;
            _con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        }

        // ✅ 1. Get Topics
        [HttpGet]
        public IActionResult GetTopics(string grade, string section, string subject)
        {
            List<string> topics = new List<string>();

            // 🔄 FIXED: Changed FROM UploadHomework ➝ Homework
            string query = @"SELECT DISTINCT Topic 
                     FROM Homework 
                     WHERE Grade = @Grade AND Section = @Section AND Subject = @Subject";

            using (SqlCommand cmd = new SqlCommand(query, _con))
            {
                cmd.Parameters.AddWithValue("@Grade", grade);
                cmd.Parameters.AddWithValue("@Section", section);
                cmd.Parameters.AddWithValue("@Subject", subject);

                _con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    topics.Add(reader["Topic"].ToString());
                }
                _con.Close();
            }

            return Ok(topics);
        }


        // ✅ 2. Get Homework List for Correction
        [HttpGet]
        public IActionResult GetHomeworkForCorrection(string grade, string section, string subject, string topic, string date)
        {
            List<CorrectHomeworkModel> list = new List<CorrectHomeworkModel>();

            string query = @"SELECT RollNo, Name, FilePath 
                             FROM UploadHomework 
                             WHERE Grade = @Grade AND Section = @Section AND Subject = @Subject 
                             AND Topic = @Topic AND CAST(SubmittedAt AS DATE) = @SubmittedAt";

            using (SqlCommand cmd = new SqlCommand(query, _con))
            {
                cmd.Parameters.AddWithValue("@Grade", grade);
                cmd.Parameters.AddWithValue("@Section", section);
                cmd.Parameters.AddWithValue("@Subject", subject);
                cmd.Parameters.AddWithValue("@Topic", topic);
                cmd.Parameters.AddWithValue("@SubmittedAt", date);

                _con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CorrectHomeworkModel model = new CorrectHomeworkModel
                    {
                        Grade = grade,
                        Section = section,
                        Subject = subject,
                        Topic = topic,
                        Date = date,
                        RollNo = reader["RollNo"].ToString(),
                        Name = reader["Name"].ToString(),
                        FilePath = reader["FilePath"].ToString()
                    };
                    list.Add(model);
                }
                _con.Close();
            }

            return Ok(list);
        }

        // ✅ 3. Insert Corrected Homework
        [HttpPost]
        public IActionResult InsertCorrected([FromBody] List<CorrectHomeworkModel> correctedList)
        {
            if (correctedList == null || correctedList.Count == 0)
                return BadRequest(new { message = "❌ No data received." });

            try
            {
                foreach (var item in correctedList)
                {
                    string query = @"INSERT INTO SubmitHomework 
                                     (Grade, Section, RollNo, Name, Subject, Topic, Date, Mark, Action) 
                                     VALUES 
                                     (@Grade, @Section, @RollNo, @Name, @Subject, @Topic, @Date, @Mark, @Action)";

                    using (SqlCommand cmd = new SqlCommand(query, _con))
                    {
                        cmd.Parameters.AddWithValue("@Grade", item.Grade ?? "");
                        cmd.Parameters.AddWithValue("@Section", item.Section ?? "");
                        cmd.Parameters.AddWithValue("@RollNo", item.RollNo ?? "");
                        cmd.Parameters.AddWithValue("@Name", item.Name ?? "");
                        cmd.Parameters.AddWithValue("@Subject", item.Subject ?? "");
                        cmd.Parameters.AddWithValue("@Topic", item.Topic ?? "");
                        cmd.Parameters.AddWithValue("@Date", item.Date ?? "");
                        cmd.Parameters.AddWithValue("@Mark", item.Mark); // ✅ int
                        cmd.Parameters.AddWithValue("@Action", item.Action ?? "Not Corrected");

                        _con.Open();
                        cmd.ExecuteNonQuery();
                        _con.Close();
                    }
                }

                return Ok(new { message = "✔️ Homework corrections submitted successfully." });
            }
            catch (Exception ex)
            {
                _con.Close();
                return StatusCode(500, new
                {
                    message = "❌ Server-side error occurred",
                    error = ex.Message
                });
            }
        }

    }
}
