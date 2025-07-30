namespace ClassroomManagement.Models.StudentTestReport
{
    public class StudentTestReportModel
    {
        public string StudentId { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public DateTime? Date { get; set; }
        public string TestId { get; set; }
        public string Term { get; set; }
        public string Subject { get; set; }
        public int TotalMark { get; set; }
        public int AnsweredMark { get; set; }
        public string Remark { get; set; }
        public string Source { get; set; }
    }
}