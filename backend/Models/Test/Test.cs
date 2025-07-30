namespace ClassroomManagement.Models.Test
{
    public class ReportCardEntry
    {
        public string Term { get; set; }
        public int TestId { get; set; }
        public string Subject { get; set; }
        public int TotalMark { get; set; }
        public int AnsweredMark { get; set; }
        public string Remark { get; set; }


    }

    public class DailyTestEntry
    {
        public string TestNo { get; set; }
        public int TestId { get; set; }
        public string Subject { get; set; }
        public string Topic { get; set; }
        public int QuestionNo { get; set; }
        public int SelectMark { get; set; }
        public int AnsweredMark { get; set; }
        public string Remark { get; set; }
       

    }

    public class ReportCardRequest
    {
        public string RollNo { get; set; }
        public string Grade { get; set; }
        public string Section { get; set; }
        public DateTime Date { get; set; }
        public List<ReportCardEntry> Entries { get; set; }
    }

    public class DailyTestRequest
    {
        public string RollNo { get; set; }
        public string Grade { get; set; }
        public string Section { get; set; }
        public DateTime Date { get; set; }
        public List<DailyTestEntry> Entries { get; set; }
    }

    public class StudentBasicInfo
    {
        public string FullName { get; set; }
        public string PhotoPath { get; set; }
    }

}
