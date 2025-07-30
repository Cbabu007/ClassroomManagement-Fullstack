using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace ClassroomManagement.Models.Attendance
{
    [Table("AbsentStudents")] 
    public class AbsentStudent
    {
        [Key]
        public int Id { get; set; }

        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string ClassTiming { get; set; }
        public string FatherMobile { get; set; }
        public string MotherMobile { get; set; }
        public DateTime Date { get; set; }
    }

}
