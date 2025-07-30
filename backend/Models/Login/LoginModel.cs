namespace ClassroomManagement.Models.Login
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponseModel
    {
        public string Status { get; set; }
        public string Role { get; set; }
        public string RedirectUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhotoPath { get; set; }
        public string StaffId { get; set; }     
        public string StudentId { get; set; }

    }
}
