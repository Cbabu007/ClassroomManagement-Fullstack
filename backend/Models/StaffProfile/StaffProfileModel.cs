using System;

namespace ClassroomManagement.Models.StaffProfile
{
    public class StaffProfileModel
    {
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string BloodGroup { get; set; }
        public string AadharNumber { get; set; }

       
        public string Mobile { get; set; }
        public string AltMobile { get; set; }
        public string Email { get; set; }

      
        public string DoorNo { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Taluk { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string Country { get; set; }

        
        public string Department { get; set; }
        public string Role { get; set; }
        public DateTime? JoiningDate { get; set; }
        public string StaffType { get; set; }

        public string PhotoPath { get; set; }
    }
}
