using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassroomManagement.Data;
using ClassroomManagement.Models.AdminReport;

namespace ClassroomManagement.Controllers.AdminReport
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminReportDashboardController : ControllerBase
    {
        private readonly ClassroomDbContext _context;

        public AdminReportDashboardController(ClassroomDbContext context)
        {
            _context = context;
        }

        
        [HttpGet("GetDashboardData")]
        public async Task<IActionResult> GetDashboardData()
        {
            try
            {
                
                var students = await _context.AdminReportStudents.FromSqlRaw("EXEC GetStudentReport").ToListAsync();
                var staffs = await _context.AdminReportStaffs.FromSqlRaw("EXEC GetStaffReport").ToListAsync();
                var classrooms = await _context.AdminReportClassrooms.FromSqlRaw("EXEC GetClassroomReport").ToListAsync();

               
                int totalStudents = students.Count;
                int totalClassrooms = classrooms.Count;
                int maleCount = students.Count(s => s.Gender?.ToLower() == "male");
                int femaleCount = students.Count(s => s.Gender?.ToLower() == "female");

                
                var staffSummary = staffs
                    .GroupBy(s => s.Role)
                    .Select(g => new {
                        staff = g.Key,
                        count = g.Count()
                    }).ToList();

                return Ok(new
                {
                    totalStudents,
                    totalClassrooms,
                    maleCount,
                    femaleCount,
                    classrooms,
                    staffSummary
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Dashboard error", error = ex.Message });
            }
        }

       
        [HttpGet("GetAllStudents")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _context.AdminReportStudents
                .FromSqlRaw("EXEC GetStudentReport").ToListAsync();

            var result = students.Select(s => new StudentSummary
            {
                StudentId = s.StudentId,
                Name = $"{s.FirstName} {s.LastName}",
                Grade = s.Grade,
                Section = s.Section
            }).ToList();

            return Ok(result);
        }



        
        [HttpGet("GetAllStaff")]
        public async Task<IActionResult> GetAllStaff()
        {
            var staffs = await _context.AdminReportStaffs
                .FromSqlRaw("EXEC GetStaffReport").ToListAsync();

            var result = staffs.Select(s => new StaffSummary
            {
                StaffId = s.StaffId,
                Name = $"{s.FirstName} {s.LastName}",
                Department = s.Department
            }).ToList();

            return Ok(result);
        }


       
        [HttpGet("FilterStudents")]
        public async Task<IActionResult> FilterStudents(string grade, string section)
        {
            try
            {
                var students = await _context.AdminReportStudents
                    .FromSqlRaw("EXEC GetStudentReport")
                    .ToListAsync();

                var filtered = students
                    .Where(s => s.Grade == grade && s.Section == section)
                    .Select(s => new
                    {
                        studentId = s.StudentId,
                        firstName = s.FirstName,
                        lastName = s.LastName,
                        dob = s.DOB,
                        gender = s.Gender,
                        bloodGroup = s.BloodGroup,
                        aadhar = s.Aadhar,
                        photoPath = s.PhotoPath,
                        mobile = s.Mobile,
                        altMobile = s.AltMobile,
                        email = s.Email,
                        doorNo = s.DoorNo,
                        address1 = s.Address1,
                        address2 = s.Address2,
                        city = s.City,
                        district = s.District,
                        state = s.State,
                        pincode = s.Pincode,
                        country = s.Country,
                        admissionNo = s.AdmissionNo,
                        admissionDate = s.AdmissionDate,
                        grade = s.Grade,
                        section = s.Section,
                        rollNo = s.RollNo,
                        medium = s.Medium,
                        academicYear = s.AcademicYear,
                        fatherName = s.FatherName,
                        fatherJob = s.FatherJob,
                        fatherMobile = s.FatherMobile,
                        motherName = s.MotherName,
                        motherJob = s.MotherJob,
                        motherMobile = s.MotherMobile,
                        guardianName = s.GuardianName,
                        guardianRelation = s.GuardianRelation,
                        guardianMobile = s.GuardianMobile,
                        prevSchool = s.PrevSchool,
                        tcPath = s.TCPath,
                        idProofPath = s.IDProofPath,
                        addressProofPath = s.AddressProofPath,
                        status = s.Status,
                        reasonLeaving = s.ReasonLeaving,
                        entranceTest = s.EntranceTest,
                        communityType = s.CommunityType,
                        communityName = s.CommunityName,
                        religion = s.Religion
                    }).ToList();

                return Ok(filtered);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error filtering students.", error = ex.Message });
            }
        }


        
        [HttpGet("FilterStaff")]
        public async Task<IActionResult> FilterStaff(string grade, string section)
        {
            try
            {
                var staffs = await _context.AdminReportStaffs.FromSqlRaw("EXEC GetStaffReport").ToListAsync();
                var filtered = staffs
                    .Where(s => s.Department == grade && s.Role == section)
                    .Select(s => new {
                        StaffId = s.StaffId,
                        Name = $"{s.FirstName} {s.LastName}",
                        Department = s.Department
                    }).ToList();

                return Ok(filtered);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error filtering staff.", error = ex.Message });
            }
        }
        [HttpGet("GetStudentGradeSection")]
        public async Task<IActionResult> GetStudentGradeSection()
        {
            try
            {
                var students = await _context.AdminReportStudents
                    .FromSqlRaw("EXEC GetStudentReport")
                    .ToListAsync();

                var gradeList = students.Select(s => s.Grade).Distinct().Where(g => !string.IsNullOrEmpty(g)).ToList();
                var sectionList = students.Select(s => s.Section).Distinct().Where(s => !string.IsNullOrEmpty(s)).ToList();

                return Ok(new { grades = gradeList, sections = sectionList });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error loading grade/section data", error = ex.Message });
            }
        }

    }
}
