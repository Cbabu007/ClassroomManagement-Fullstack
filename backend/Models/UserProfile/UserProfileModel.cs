namespace ClassroomManagement.Models.UserProfile
{
    public class UserProfileModel
    {
        public string StudentId { get; set; }
        public string PhotoPath { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Grade { get; set; }
        public string Section { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string AltMobile { get; set; }
        public string RollNo { get; set; }
        public DateTime DOB { get; set; }
        public string DoorNo { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
    }
}
