using Microsoft.AspNetCore.Http;

namespace ClassroomManagement.Models.SubmitHomework
{
    public class SubmitHomeworkModel
    {
        public string StudentId { get; set; }
        public string Name { get; set; }
        public string RollNo { get; set; }
        public string Grade { get; set; }
        public string Section { get; set; }
        public string Subject { get; set; }
        public string Topic { get; set; }
        public IFormFile File { get; set; }
    }
}
