using System.ComponentModel.DataAnnotations.Schema;

namespace ClassroomManagement.Models.ReportTeacher
{
    [Table("TeacherReport")] 
    public class ReportTeacherModel
    {
        public int Id { get; set; }
        public string Grade { get; set; }
        public string Section { get; set; }
        public DateTime ReportDate { get; set; }
        public string Message { get; set; }
    }
}