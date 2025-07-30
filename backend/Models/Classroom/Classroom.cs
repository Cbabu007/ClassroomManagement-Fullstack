using System;
using System.Collections.Generic;

namespace ClassroomManagement.Models.Classroom
{
    public class Classroom
    {
        public int Id { get; set; }
        public string? ClassroomNo { get; set; }
        public string? Class { get; set; }
        public string? Section { get; set; }
        public string? ClassTeacherId { get; set; }
        public string? ClassTeacherName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
