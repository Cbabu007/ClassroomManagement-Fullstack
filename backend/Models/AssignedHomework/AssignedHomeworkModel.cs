namespace ClassroomManagement.Models.AssignedHomework
{
    public class AssignedHomeworkModel
    {
        public int Id { get; set; }
        public string StaffName { get; set; }
        public string Grade { get; set; }
        public string Section { get; set; }
        public string Subject { get; set; }
        public string Topic { get; set; }
        public DateTime Date { get; set; }
        public string FilePath { get; set; }
    }
}