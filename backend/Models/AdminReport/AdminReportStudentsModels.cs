using System;

namespace ClassroomManagement.Models.AdminReport
{
    public class AdminReportStudentsModels
    {
        public int Id { get; set; }
        public string? StudentId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DOB { get; set; }
        public string? Gender { get; set; }
        public string? BloodGroup { get; set; }
        public string? Aadhar { get; set; }
        public string? PhotoPath { get; set; }
        public string? Mobile { get; set; }
        public string? AltMobile { get; set; }
        public string? Email { get; set; }
        public string? DoorNo { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? State { get; set; }
        public string? Pincode { get; set; }
        public string? Country { get; set; }
        public string? AdmissionNo { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public string? Grade { get; set; }
        public string? Section { get; set; }
        public string? RollNo { get; set; }
        public string? Medium { get; set; }
        public string? AcademicYear { get; set; }
        public string? FatherName { get; set; }
        public string? FatherJob { get; set; }
        public string? FatherMobile { get; set; }
        public string? MotherName { get; set; }
        public string? MotherJob { get; set; }
        public string? MotherMobile { get; set; }
        public string? GuardianName { get; set; }
        public string? GuardianRelation { get; set; }
        public string? GuardianMobile { get; set; }
        public string? PrevSchool { get; set; }
        public string? TCPath { get; set; }
        public string? IDProofPath { get; set; }
        public string? AddressProofPath { get; set; }
        public string? Status { get; set; }
        public string? ReasonLeaving { get; set; }
        public string? EntranceTest { get; set; }
        public string? CommunityType { get; set; }
        public string? CommunityName { get; set; }
        public string? Religion { get; set; }
    }
}
