using Microsoft.AspNetCore.Mvc;
using ClassroomManagement.Data;
using ClassroomManagement.Models.Staff;

namespace ClassroomManagement.Controllers.Staff
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddStaffController : ControllerBase
    {
        private readonly ClassroomDbContext _context;

        public AddStaffController(ClassroomDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> SaveStaff()
        {
            try
            {
                var form = Request.Form;
                if (form == null || form.Keys.Count == 0)
                    return BadRequest("Form data is empty.");

                DateTime.TryParse(form["dateOfBirth"], out DateTime dob);
                DateTime.TryParse(form["joiningDate"], out DateTime joiningDate);

                var staff = new ClassroomManagement.Models.Staff.Staff
                {
                    StaffId = form["staffId"],
                    FirstName = form["firstName"],
                    LastName = form["lastName"],
                    Gender = form["gender"],
                    DateOfBirth = dob,
                    BloodGroup = form["bloodGroup"],
                    AadharNumber = form["aadharNumber"],
                    Mobile = form["mobile"],
                    AltMobile = form["altMobile"],
                    Email = form["email"],
                    DoorNo = form["doorNo"],
                    Address1 = form["address1"],
                    Address2 = form["address2"],
                    Taluk = form["taluk"],
                    District = form["district"],
                    State = form["state"],
                    Pincode = form["pincode"],
                    Country = form["country"],
                    Department = form["department"],
                    Role = form["role"],
                    JoiningDate = joiningDate,
                    StaffType = form["staffType"],
                    BankName = form["bankName"],
                    AccountNumber = form["accountNumber"],
                    IFSCCode = form["ifscCode"],
                    PANCardNumber = form["panCardNumber"],
                    Username = form["username"],
                    Password = form["password"],
                    LoginRole = form["loginRole"]
                };

                // Save Photo
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

               
                _context.Staffs.Add(staff);
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


                return Ok(new { message = "Staff saved successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error while saving staff: " + ex.Message);
                return StatusCode(500, new { message = "Failed to save staff", error = ex.Message });
            }
        }
    }
}
