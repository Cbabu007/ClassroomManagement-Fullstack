using System.ComponentModel.DataAnnotations.Schema;

namespace ClassroomManagement.Models.Message
{
    [Table("Message")]
    public class MessageModel
    {
        public int Id { get; set; }
        public string? Grade { get; set; }
        public string? Section { get; set; }
        public DateTime? Date { get; set; }
        public string? Topic { get; set; }
        public string? StaffName { get; set; }
        public string? Message { get; set; }
    }
}
