using System.Collections.Generic;

namespace ClassroomManagement.Models.Classroom
{
    public class ClassroomSaveRequest
    {
        public string? ClassroomNo { get; set; }
        public string? Class { get; set; }
        public string? Section { get; set; }
        public string? ClassTeacherId { get; set; }
        public string? ClassTeacherName { get; set; }
        public List<SubjectStaffRequest>? SubjectStaffList { get; set; }
        public List<ClassroomStudentRequest>? Students { get; set; }
    }

    public class SubjectStaffRequest
    {
        public string? Subject { get; set; }
        public string? StaffId { get; set; }
        public string? StaffName { get; set; }
    }

    public class ClassroomStudentRequest
    {
        public string? StudentId { get; set; }
        public string? StudentName { get; set; }
    }
}
