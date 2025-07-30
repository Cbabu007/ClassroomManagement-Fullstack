using Microsoft.AspNetCore.Mvc;
using ClassroomManagement.Data;
using ClassroomManagement.Models.Staff;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ClassroomManagement.Controllers.Staff
{
    [ApiController]
    [Route("api/[controller]")]
    public class EditStaffController : ControllerBase
    {
        private readonly ClassroomDbContext _context;

        public EditStaffController(ClassroomDbContext context)
        {
            _context = context;
        }

        
        [HttpGet("{staffId}")]
        public async Task<IActionResult> GetStaff(string staffId)
        {
            if (string.IsNullOrEmpty(staffId))
                return BadRequest("StaffId is required.");

            var staff = await _context.Staffs.FirstOrDefaultAsync(x => x.StaffId == staffId);

            if (staff == null)
                return NotFound(new { message = "Staff not found." });

            var qualifications = await _context.StaffQualifications
                                    .Where(q => q.StaffId == staffId)
                                    .ToListAsync();

            return Ok(new { staff, qualifications });
        }

        
        [HttpPut("{staffId}")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UpdateStaff(string staffId)
        {
            try
            {
                var form = Request.Form;
                if (form == null || form.Keys.Count == 0)
                    return BadRequest("Form data is empty.");

                var staff = await _context.Staffs.FirstOrDefaultAsync(x => x.StaffId == staffId);
                if (staff == null)
                    return NotFound(new { message = "Staff not found." });

                
                staff.FirstName = form["firstName"];
                staff.LastName = form["lastName"];
                staff.Gender = form["gender"];
                DateTime.TryParse(form["dateOfBirth"], out DateTime dob);
                staff.DateOfBirth = dob;
                staff.BloodGroup = form["bloodGroup"];
                staff.AadharNumber = form["aadharNumber"];
                staff.Mobile = form["mobile"];
                staff.AltMobile = form["altMobile"];
                staff.Email = form["email"];
                staff.DoorNo = form["doorNo"];
                staff.Address1 = form["address1"];
                staff.Address2 = form["address2"];
                staff.Taluk = form["taluk"];
                staff.District = form["district"];
                staff.State = form["state"];
                staff.Pincode = form["pincode"];
                staff.Country = form["country"];
                staff.Department = form["department"];
                staff.Role = form["role"];
                DateTime.TryParse(form["joiningDate"], out DateTime joiningDate);
                staff.JoiningDate = joiningDate;
                staff.StaffType = form["staffType"];
                staff.BankName = form["bankName"];
                staff.AccountNumber = form["accountNumber"];
                staff.IFSCCode = form["ifscCode"];
                staff.PANCardNumber = form["panCardNumber"];
                staff.Username = form["username"];
                staff.Password = form["password"];
                staff.LoginRole = form["loginRole"];

                
                if (form.Files.Count > 0)
                {
                    var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    Directory.CreateDirectory(uploadsPath);

                    foreach (var file in form.Files)
                    {
                        if (file.Length > 0)
                        {
                            var fileName = $"{staff.StaffId}_{file.Name}_{DateTime.Now.Ticks}{Path.GetExtension(file.FileName)}";
                            var filePath = Path.Combine(uploadsPath, fileName);
                            using var stream = new FileStream(filePath, FileMode.Create);
                            await file.CopyToAsync(stream);

                            if (file.Name == "photo")
                                staff.PhotoPath = Path.Combine("uploads", fileName).Replace("\\", "/");
                        }
                    }
                }

                _context.Staffs.Update(staff);
                await _context.SaveChangesAsync();

                
                var oldQualifications = _context.StaffQualifications.Where(q => q.StaffId == staffId);
                _context.StaffQualifications.RemoveRange(oldQualifications);
                await _context.SaveChangesAsync();

                var qualificationsList = new List<StaffQualification>();
                int index = 0;

                while (form.TryGetValue($"qualifications[{index}].degree", out var degree))
                {
                    var specialization = form[$"qualifications[{index}].specialization"];
                    var institution = form[$"qualifications[{index}].institution"];
                    var university = form[$"qualifications[{index}].university"];
                    var yearOfPassing = form[$"qualifications[{index}].yearOfPassing"];

                    var qualification = new StaffQualification
                    {
                        StaffId = staff.StaffId,
                        Degree = degree,
                        Specialization = specialization,
                        Institution = institution,
                        University = university,
                        YearOfPassing = yearOfPassing
                    };

                    qualificationsList.Add(qualification);
                    index++;
                }

                if (qualificationsList.Count > 0)
                {
                    _context.StaffQualifications.AddRange(qualificationsList);
                    await _context.SaveChangesAsync();
                }

                return Ok(new { message = "Staff updated successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error while updating staff: " + ex.Message);
                return StatusCode(500, new { message = "Failed to update staff", error = ex.Message });
            }
        }
    }
}
