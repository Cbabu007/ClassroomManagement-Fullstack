namespace ClassroomManagement.Models.YourActivity
{
    public class AdminActivityModel
    {
        public int Id { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string Browse { get; set; }
        public int Day { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public TimeSpan LoginTime { get; set; }
        public TimeSpan LogoutTime { get; set; }
    }
}
