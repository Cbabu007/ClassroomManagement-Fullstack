namespace ClassroomManagement.Models.Classroom
{
    public class ClassroomStudent
    {
        public int Id { get; set; }
        public int ClassroomId { get; set; }
        public string? StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? ClassroomNo { get; set; }
        public string? Class { get; set; }
        public string? Section { get; set; }
    }
}
