using Microsoft.AspNetCore.Mvc;
using ClassroomManagement.Models.StudentDashboard;
using ClassroomManagement.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagement.Controllers.StudentDashboard
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentDashboardController : ControllerBase
    {
        private readonly ClassroomDbContext _context;

        public StudentDashboardController(ClassroomDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<StudentDashboardModel>> GetStudentByLogin(string email = "", string mobile = "")
        {
            var result = await _context.Students
                .Where(s => s.Email == email && s.Mobile == mobile)
                .Select(s => new StudentDashboardModel
                {
                    StudentId = s.StudentId,
                    RollNo = s.RollNo,
                    Grade = s.Grade,
                    Section = s.Section,
                    PhotoPath = s.PhotoPath,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    Mobile = s.Mobile,
                    Message = _context.ReportTeacherModel
                        .Where(r => r.Grade == s.Grade && r.Section == s.Section)
                        .OrderByDescending(r => r.ReportDate)
                        .Select(r => r.Message).FirstOrDefault()
                }).FirstOrDefaultAsync();

            if (result == null) return NotFound("Invalid login.");

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaderboardModel>>> GetLeaderboard(string grade = "", string section = "")
        {
            var reportMarks = await _context.ReportCard
                .Where(r => r.Grade == grade && r.Section == section)
                .GroupBy(r => r.RollNo)
                .Select(g => new
                {
                    RollNo = g.Key,
                    Total = g.Sum(x => x.AnsweredMark)
                }).ToListAsync();

            var testMarks = await _context.DailyTest
                .Where(d => d.Grade == grade && d.Section == section)
                .GroupBy(d => d.RollNo)
                .Select(g => new
                {
                    RollNo = g.Key,
                    Total = g.Sum(x => x.AnsweredMark)
                }).ToListAsync();

            var totalMarks = reportMarks.Concat(testMarks)
                .GroupBy(x => x.RollNo)
                .Select(g => new
                {
                    RollNo = g.Key,
                    TotalTrophies = g.Sum(x => x.Total)
                }).OrderByDescending(x => x.TotalTrophies).ToList();

            var result = totalMarks.Join(_context.Students,
                mark => mark.RollNo,
                stu => stu.RollNo,
                (mark, stu) => new LeaderboardModel
                {
                    Name = stu.FirstName + " " + stu.LastName,
                    PhotoPath = stu.PhotoPath,
                    Trophies = mark.TotalTrophies
                }).Take(100).ToList();

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaderboardModel>>> GetTopReportCardByTerm(string term)
        {
            var result = await _context.ReportCard
                .Where(r => r.Term == term)
                .GroupBy(r => r.RollNo)
                .Select(g => new {
                    RollNo = g.Key,
                    TotalMarks = g.Sum(x => x.AnsweredMark)
                }).ToListAsync();

            var ranked = result.Join(_context.Students,
                r => r.RollNo,
                s => s.RollNo,
                (r, s) => new LeaderboardModel
                {
                    Name = s.FirstName + " " + s.LastName,
                    PhotoPath = s.PhotoPath,
                    Trophies = r.TotalMarks
                }).OrderByDescending(x => x.Trophies).Take(100).ToList();

            return Ok(ranked);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaderboardModel>>> GetTopDailyTestByTestNo(string testNo)
        {
            var result = await _context.DailyTest
                .Where(r => r.TestNo == testNo)
                .GroupBy(r => r.RollNo)
                .Select(g => new {
                    RollNo = g.Key,
                    TotalMarks = g.Sum(x => x.AnsweredMark)
                }).ToListAsync();

            var ranked = result.Join(_context.Students,
                r => r.RollNo,
                s => s.RollNo,
                (r, s) => new LeaderboardModel
                {
                    Name = s.FirstName + " " + s.LastName,
                    PhotoPath = s.PhotoPath,
                    Trophies = r.TotalMarks
                }).OrderByDescending(x => x.Trophies).Take(100).ToList();

            return Ok(ranked);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReportCardModel>>> GetReportCardByTerm(string term)
        {
            var data = await _context.ReportCard
                .Where(r => r.Term == term)
                .GroupBy(r => r.Subject)
                .Select(g => new ReportCardModel
                {
                    Name = g.Key,
                    Subject = g.Key,
                    Term = term,
                    TotalMark = g.Sum(x => x.TotalMark),
                    AnsweredMark = g.Sum(x => x.AnsweredMark)
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DailyTestModel>>> GetDailyTestByTestNo(string testNo)
        {
            var data = await _context.DailyTest
                .Where(d => d.TestNo == testNo)
                .GroupBy(d => d.Subject)
                .Select(g => new DailyTestModel
                {
                    Name = g.Key,
                    Subject = g.Key,
                    TestNo = testNo,
                    TotalMark = g.Sum(x => x.SelectMark),
                    AnsweredMark = g.Sum(x => x.AnsweredMark)
                }).ToListAsync();

            return Ok(data);
        }
    }
}