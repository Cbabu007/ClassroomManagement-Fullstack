using System;
using System.ComponentModel.DataAnnotations;

namespace ClassroomManagement.Models.Class
{
    public class FinalAttendance
    {
        [Key]
        public int Id { get; set; }

        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string Grade { get; set; }
        public string Section { get; set; }

        public string Class1 { get; set; }
        public string Class2 { get; set; }
        public string Class3 { get; set; }
        public string Class4 { get; set; }
        public string Class5 { get; set; }
        public string Class6 { get; set; }
        public string Class7 { get; set; }
        public string Class8 { get; set; }

        public DateTime Date { get; set; }
    }
}
