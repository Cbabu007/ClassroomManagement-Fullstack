namespace ClassroomManagement.Models.UserYourActivity
{
    public class UserYourActivityModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime LoginTime { get; set; }
        public string Browser { get; set; }
        public string Platform { get; set; }
        public string IPAddress { get; set; }
    }
}
