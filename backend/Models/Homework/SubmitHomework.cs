namespace ClassroomManagement.Models.Homework
{
    public class SubmitHomework
    {
        public int Id { get; set; }
        public string Grade { get; set; } = string.Empty;
        public string Section { get; set; } = string.Empty;
        public string RollNo { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Topic { get; set; } = string.Empty;
        public string Mark { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
    }
}
