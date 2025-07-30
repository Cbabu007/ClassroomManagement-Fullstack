using ClassroomManagement.Models.Admin;
using ClassroomManagement.Models.AdminReport;
using ClassroomManagement.Models.Attendance;
using ClassroomManagement.Models.Class;
using ClassroomManagement.Models.Classroom;
using ClassroomManagement.Models.Contact; 
using ClassroomManagement.Models.Events;
using ClassroomManagement.Models.Homework;
using ClassroomManagement.Models.Message;
using ClassroomManagement.Models.ReportTeacher;
using ClassroomManagement.Models.Staff;
using ClassroomManagement.Models.StudentTestReport;
using ClassroomManagement.Models.Test;
using ClassroomManagement.Models.UserYourActivity;
using ClassroomManagement.Models.VideoClass;
using ClassroomManagement.Models.ViewStudents;
using ClassroomManagement.Models.YourActivity;
using Microsoft.EntityFrameworkCore;


namespace ClassroomManagement.Data
{
    public class ClassroomDbContext : DbContext
    {
        public ClassroomDbContext(DbContextOptions<ClassroomDbContext> options)
            : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<StaffQualification> StaffQualifications { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<SubjectStaff> SubjectStaffs { get; set; }
        public DbSet<ClassroomStudent> ClassroomStudents { get; set; }
        public DbSet<EventModel> Events { get; set; }
        public DbSet<PresentStudent> PresentStudents { get; set; }
        public DbSet<AbsentStudent> AbsentStudents { get; set; }
        public DbSet<AttendanceStatusResult> AttendanceStatusResult { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AttendanceStatusResult>().HasNoKey(); 
            modelBuilder.Entity<ViewStudentModel>().HasNoKey();
            modelBuilder.Entity<AdminReportStudentsModels>().HasNoKey();
            modelBuilder.Entity<AdminReportStaffModels>().HasNoKey();
            modelBuilder.Entity<AdminReportClassroomModels>().HasNoKey();

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<AssignedVideoClass> AssignedVideoClass { get; set; }

        public DbSet<FinalAttendance> FinalAttendance { get; set; }
        public DbSet<Homework> Homework { get; set; }
        public DbSet<AdminReportStudentsModels> AdminReportStudents { get; set; }
        public DbSet<AdminReportStaffModels> AdminReportStaffs { get; set; }

        public DbSet<AdminReportClassroomModels> AdminReportClassrooms { get; set; }
        public DbSet<MessageModel> Messages { get; set; }

        public DbSet<DailyTest> DailyTest { get; set; } 

        public DbSet<ReportCard> ReportCard { get; set; }
        public DbSet<UserYourActivityModel> UserYourActivity { get; set; }

        public DbSet<LoginActivityModel> LoginActivity { get; set; }
        public DbSet<TeacherActivityModel> TeacherActivity { get; set; }
        public DbSet<AdminActivityModel> AdminActivity { get; set; }
        public DbSet<ContactModel> Contact { get; set; }
        public DbSet<ReportTeacherModel> ReportTeacherModel { get; set; }


    }
}

