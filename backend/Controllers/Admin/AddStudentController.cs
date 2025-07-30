using Microsoft.AspNetCore.Mvc;
using ClassroomManagement.Models.Admin;
using ClassroomManagement.Data;

namespace ClassroomManagement.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddStudentController : ControllerBase
    {
        private readonly ClassroomDbContext _context;

        public AddStudentController(ClassroomDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> SaveStudent()
        {
            try
            {
                var form = Request.Form;
                if (form == null || form.Keys.Count == 0)
                    return BadRequest("Form data is empty.");

                // Parse dates
                DateTime.TryParse(form["dob"], out DateTime dob);
                DateTime.TryParse(form["admissionDate"], out DateTime admissionDate);

                // Fill student model
                var student = new Student
                {
                    StudentId = form["studentId"],
                    FirstName = form["firstName"],
                    LastName = form["lastName"],
                    DOB = dob,
                    Gender = form["gender"],
                    BloodGroup = form["bloodGroup"],
                    Aadhar = form["aadhar"],
                    Mobile = form["mobile"],
                    AltMobile = form["altMobile"],
                    Email = form["email"],
                    DoorNo = form["doorNo"],
                    Address1 = form["address1"],
                    Address2 = form["address2"],
                    City = form["city"],
                    District = form["district"],
                    State = form["state"],
                    Pincode = form["pincode"],
                    Country = form["country"],
                    AdmissionNo = form["admissionNo"],
                    AdmissionDate = admissionDate,
                    Grade = form["grade"],
                    Section = form["section"],
                    RollNo = form["rollNo"],
                    Medium = form["medium"],
                    AcademicYear = form["academicYear"],
                    EntranceTest = form["entranceTest"],
                    FatherName = form["fatherName"],
                    FatherJob = form["fatherJob"],
                    FatherMobile = form["fatherMobile"],
                    MotherName = form["motherName"],
                    MotherJob = form["motherJob"],
                    MotherMobile = form["motherMobile"],
                    GuardianName = form["guardianName"],
                    GuardianRelation = form["guardianRelation"],
                    GuardianMobile = form["guardianMobile"],
                    PrevSchool = form["prevSchool"],
                    CommunityType = form["communityType"],       
                    CommunityName = form["communityName"],      
                    Religion = form["religion"],
                    Status = form["status"],
                    ReasonLeaving = form["reasonLeaving"]
                };

                // File saving path
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(uploadsPath);

                // File uploads
                foreach (var file in form.Files)
                {
                    if (file.Length > 0)
                    {
                        var filePath = Path.Combine(uploadsPath, file.FileName);
                        using var stream = new FileStream(filePath, FileMode.Create);
                        await file.CopyToAsync(stream);

                        if (file.Name == "photo") student.PhotoPath = "uploads/" + file.FileName;
                        if (file.Name == "tc") student.TCPath = "uploads/" + file.FileName;
                        if (file.Name == "idProof") student.IDProofPath = "uploads/" + file.FileName;
                        if (file.Name == "addressProof") student.AddressProofPath = "uploads/" + file.FileName;
                    }
                }

                _context.Students.Add(student);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Student saved successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Entity Save Error: " + ex.InnerException?.Message ?? ex.Message);
                return StatusCode(500, new
                {
                    message = "Failed to save student",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }

        }
    }
}
