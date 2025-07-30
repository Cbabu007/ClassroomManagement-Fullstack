public class AttendanceSubmissionModel
{
    public List<PresentStudentModel> PresentList { get; set; }
    public List<AbsentStudentModel> AbsentList { get; set; }
}

public class PresentStudentModel
{
    public string StudentId { get; set; }
    public string StudentName { get; set; }
    public string ClassTiming { get; set; }
    public string Date { get; set; }
}

public class AbsentStudentModel
{
    public string StudentId { get; set; }
    public string StudentName { get; set; }
    public string ClassTiming { get; set; }
    public string FatherMobile { get; set; }
    public string MotherMobile { get; set; }
    public string Date { get; set; }
}
