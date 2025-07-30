using System.ComponentModel.DataAnnotations.Schema;

namespace ClassroomManagement.Models.Events
{
    [Table("Event")]  
    public class EventModel
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public DateTime? Date { get; set; }  
    }
}
