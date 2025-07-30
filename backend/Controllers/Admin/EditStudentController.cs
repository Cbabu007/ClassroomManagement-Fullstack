using Microsoft.AspNetCore.Mvc;
using ClassroomManagement.Models.Admin;
using ClassroomManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagement.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class EditStudentController : ControllerBase
    {
        private readonly ClassroomDbContext _context;

        public EditStudentController(ClassroomDbContext context)
        {
            _context = context;
        }

        [HttpPut("{studentId}")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UpdateStudent(string studentId)
        {
            var form = Request.Form;
            var files = form.Files;
            var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentId == studentId);

            if (student == null)
                return NotFound(new { message = "Student not found" });

            // Update normal fields
            student.FirstName = form["firstName"];
            student.LastName = form["lastName"];
            DateTime.TryParse(form["dob"], out DateTime dob);
            student.DOB = dob;
            student.Gender = form["gender"];
            student.BloodGroup = form["bloodGroup"];
            student.Aadhar = form["aadhar"];
            student.Mobile = form["mobile"];
            student.AltMobile = form["altMobile"];
            student.Email = form["email"];
            student.DoorNo = form["doorNo"];
            student.Address1 = form["address1"];
            student.Address2 = form["address2"];
            student.City = form["city"];
            student.District = form["district"];
            student.State = form["state"];
            student.Pincode = form["pincode"];
            student.Country = form["country"];
            student.AdmissionNo = form["admissionNo"];
            DateTime.TryParse(form["admissionDate"], out DateTime admissionDate);
            student.AdmissionDate = admissionDate;
            student.Grade = form["grade"];
            student.Section = form["section"];
            student.RollNo = form["rollNo"];
            student.Medium = form["medium"];
            student.AcademicYear = form["academicYear"];
            student.FatherName = form["fatherName"];
            student.FatherJob = form["fatherJob"];
            student.FatherMobile = form["fatherMobile"];
            student.MotherName = form["motherName"];
            student.MotherJob = form["motherJob"];
            student.MotherMobile = form["motherMobile"];
            student.GuardianName = form["guardianName"];
            student.GuardianRelation = form["guardianRelation"];
            student.GuardianMobile = form["guardianMobile"];
            student.PrevSchool = form["prevSchool"];
            student.CommunityType = form["communityType"];
            student.CommunityName = form["communityName"];
            student.Religion = form["religion"];
            student.Status = form["status"];
            student.ReasonLeaving = form["reasonLeaving"];

            //  Save uploaded files
            if (files.Any())
            {
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var fileName = $"{studentId}_{file.Name}_{DateTime.Now.Ticks}{Path.GetExtension(file.FileName)}";
                        var filePath = Path.Combine(uploadPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Set path in database
                        switch (file.Name)
                        {
                            case "tc":
                                student.TCPath = "uploads/" + fileName;
                                break;
                            case "idProof":
                                student.IDProofPath = "uploads/" + fileName;
                                break;
                            case "addressProof":
                                student.AddressProofPath = "uploads/" + fileName;
                                break;
                            case "photo":
                                student.PhotoPath = "uploads/" + fileName;
                                break;
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Student updated successfully" });
        }
    }
}
