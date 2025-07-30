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
    public class CompletedHomeworkController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly SqlConnection _con;

        public CompletedHomeworkController(IConfiguration config)
        {
            _config = config;
            _con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        }

       
        [HttpGet]
        public IActionResult GetCompleted(string grade, string section, string subject, string topic, string date)
        {
            List<CompletedHomeworkModel> list = new List<CompletedHomeworkModel>();

            using (SqlCommand cmd = new SqlCommand("GetHomeworkSubmissionStatus", _con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Grade", grade);
                cmd.Parameters.AddWithValue("@Section", section);
                cmd.Parameters.AddWithValue("@Subject", subject);
                cmd.Parameters.AddWithValue("@Topic", topic);
                cmd.Parameters.AddWithValue("@Date", date);

                _con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new CompletedHomeworkModel
                    {
                        RollNo = reader["RollNo"].ToString(),
                        Name = reader["Name"].ToString(),
                        Date = reader["Date"].ToString(),
                        Action = reader["Action"].ToString()
                    });
                }
                _con.Close();
            }

            return Ok(list);
        }
    }
}