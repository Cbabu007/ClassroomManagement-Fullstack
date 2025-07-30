using System;

namespace ClassroomManagement.Models.AdminProfile
{
    public class AdminProfileModel
    {
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Role { get; set; }
        public DateTime? JoiningDate { get; set; }
        public string PhotoPath { get; set; }
    }
}
