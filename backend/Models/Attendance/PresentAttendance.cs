using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace ClassroomManagement.Models.Attendance
{
    [Table("PresentStudents")] 
    public class PresentStudent
    {
        [Key]
        public int Id { get; set; }

        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string ClassTiming { get; set; }
        public DateTime Date { get; set; }
    }

}
