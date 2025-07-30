namespace ClassroomManagement.Models.StudentDashboard
{
    public class StudentDashboardModel
    {
        public string StudentId { get; set; }
        public string RollNo { get; set; }
        public string Grade { get; set; }
        public string Section { get; set; }
        public string PhotoPath { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Message { get; set; }
    }

    public class LeaderboardModel
    {
        public string Name { get; set; }
        public string PhotoPath { get; set; }
        public int Trophies { get; set; }
    }

    public class ReportCardModel
    {
        public string Name { get; set; } 
        public string Subject { get; set; }
        public string Term { get; set; }
        public int TotalMark { get; set; }
        public int AnsweredMark { get; set; }
    }

    public class DailyTestModel
    {
        public string Name { get; set; } 
        public string Subject { get; set; }
        public string TestNo { get; set; }
        public int TotalMark { get; set; }
        public int AnsweredMark { get; set; }
    }
}
