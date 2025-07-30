using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassroomManagement.Data;
using HomeworkModel = ClassroomManagement.Models.Homework.Homework;

namespace ClassroomManagement.Controllers.Homework
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddHomeworkController : ControllerBase
    {
        private readonly ClassroomDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AddHomeworkController(ClassroomDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

       
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] HomeworkModel model, IFormFile? file)
        {
            try
            {
                model.Id = 0;

               
                var staff = await _context.Staffs.FirstOrDefaultAsync(s => s.StaffId == model.StaffId);
                if (staff != null)
                {
                    model.StaffName = $"{staff.FirstName} {staff.LastName}";
                    model.Subject = staff.Department;
                }

                
                if (file != null)
                {
                    var folder = Path.Combine("wwwroot", "uploads", "homework");
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var path = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    model.Filepath = "/uploads/homework/" + fileName;
                }

                
                _context.Homework.Add(model);
                await _context.SaveChangesAsync();

                return Ok(new { message = "✅ Homework added successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ Error: {ex.Message}");
            }
        }

        
        [HttpGet("GetGradesAndSections")]
        public async Task<IActionResult> GetGradesAndSections()
        {
            var grades = await _context.Classrooms.Select(c => c.Class).Distinct().ToListAsync();
            var sections = await _context.Classrooms.Select(c => c.Section).Distinct().ToListAsync();
            return Ok(new { grades, sections });
        }

        
        [HttpGet("GetClassroomNo")]
        public async Task<IActionResult> GetClassroomNo([FromQuery] string className, [FromQuery] string section)
        {
            var classroomNo = await _context.Classrooms
                .Where(c => c.Class == className && c.Section == section)
                .Select(c => c.ClassroomNo)
                .FirstOrDefaultAsync();

            return classroomNo == null
                ? NotFound("Classroom not found")
                : Ok(classroomNo);
        }

        
        [HttpGet("GetStaffList")]
        public async Task<IActionResult> GetStaffList()
        {
            var staffList = await _context.Staffs
                .Where(s => !string.IsNullOrEmpty(s.Department))
                .Select(s => new
                {
                    staffId = s.StaffId,
                    firstName = s.FirstName,
                    lastName = s.LastName,
                    name = s.FirstName + " " + s.LastName,
                    department = s.Department
                }).ToListAsync();

            return Ok(staffList);
        }
    }
}
