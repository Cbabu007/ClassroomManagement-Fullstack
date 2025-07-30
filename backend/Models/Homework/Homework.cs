using System;
using System.ComponentModel.DataAnnotations;

namespace ClassroomManagement.Models.Homework
{
    public class Homework
    {
        [Key]
        public int Id { get; set; }

        public string? ClassroomNo { get; set; }
        public string? Grade { get; set; }
        public string? Section { get; set; }

        public string? StaffId { get; set; }
        public string? StaffName { get; set; }

        public string? Subject { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        public string? Topic { get; set; }

        public string? Filepath { get; set; }
    }
}
