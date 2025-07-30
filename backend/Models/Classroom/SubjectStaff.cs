namespace ClassroomManagement.Models.Classroom
{
    public class SubjectStaff
    {
        public int Id { get; set; }
        public int ClassroomId { get; set; }
        public string? Subject { get; set; }
        public string? StaffId { get; set; }
        public string? StaffName { get; set; }

        public string? ClassroomNo { get; set; }
        public string? Class { get; set; }
        public string? Section { get; set; }
    }
}
