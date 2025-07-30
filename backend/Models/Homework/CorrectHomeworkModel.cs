namespace ClassroomManagement.Models.Homework
{
    public class CorrectHomeworkModel
    {
        public string Grade { get; set; }
        public string Section { get; set; }
        public string RollNo { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Topic { get; set; }
        public string Date { get; set; }
        public string FilePath { get; set; }

        public int Mark { get; set; }          
        public string Action { get; set; }
    }
}
