using Microsoft.EntityFrameworkCore;

namespace ClassroomManagement.Models.ViewStudents
{
    [Keyless]
    public class ViewStudentModel
    {
        public string StudentId { get; set; }
        public string PhotoPath { get; set; }
        public string RollNo { get; set; }
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string BloodGroup { get; set; }
        public string Mobile { get; set; }
        public string AltMobile { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string FatherName { get; set; }
        public string FatherMobile { get; set; }
        public string MotherName { get; set; }
        public string MotherMobile { get; set; }
        public string GuardianName { get; set; }
        public string GuardianMobile { get; set; }
    }
}
