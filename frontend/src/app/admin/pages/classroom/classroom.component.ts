import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-classroom',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './classroom.component.html',
  styleUrls: ['./classroom.component.css']
})
export class ClassroomComponent implements OnInit {
  activeTab = 'add';

  // Common Variables
  classroomNumbers = Array.from({ length: 50 }, (_, i) => i + 1);
  classes = ['LKG', 'UKG', '1st', '2nd', '3rd', '4th', '5th', '6th', '7th', '8th', '9th', '10th', '11th', '12th'];
  sections = ['A', 'B', 'C', 'D'];
  subjectOptions: any[] = [];
  staffList: string[] = [];
  availableStudents: any[] = [];
  assignedClassroomsList: any[] = [];
  classTeachersList: any[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.fetchSubjectsAndStaffs();
    this.fetchClassTeachers();
    this.fetchAssignedClassrooms();
    this.fetchAssignedClassroomsForDelete();
  }

  fetchAssignedClassrooms() {
    this.http.get<any[]>('https://localhost:7284/api/EditClassroom/GetAssignedClassrooms')
      .subscribe({
        next: (res) => {
          this.assignedClassroomsList = res;
        },
        error: () => alert('❌ Error fetching assigned classrooms')
      });
  }
  setTab(tabName: string) {
    this.activeTab = tabName;
  }

  // ---------------------- ADD Classroom Section ---------------------- //

  classroom = { classroomNo: '', class: '', section: '', classTeacherId: '' };
  subjectStaffList = [{ subject: '', staff: '' }];
  selectedStudent: any = null; 
 addedStudents: any[] = [];  
  

  
  fetchSubjectsAndStaffs() {
    this.http.get<any[]>('https://localhost:7284/api/AddClassroom/GetSubjectsAndStaffs').subscribe({
      next: (res) => {
        this.subjectOptions = res;
        this.staffList = res.map(x => x.staffName);
      },
      error: () => alert('❌ Error fetching Subjects and Staffs')
    });
  }

  fetchClassTeachers() {
    this.http.get<any[]>('https://localhost:7284/api/AddClassroom/GetClassTeachers').subscribe({
      next: (res) => {
        this.classTeachersList = res;
      },
      error: () => alert('❌ Error fetching Class Teachers')
    });
  }

  fetchStudentsByClass(className: string) {
    if (!className) return;
    this.http.get<any[]>(`https://localhost:7284/api/AddClassroom/GetStudentsByClass/${encodeURIComponent(className)}`).subscribe({
      next: (res) => {
        this.availableStudents = res.map(x => ({
  studentId: x.studentId,
  studentName: x.studentName
}));

      },
      error: () => alert('❌ Error fetching Students')
    });
  }

  addSubjectStaff() {
    this.subjectStaffList.push({ subject: '', staff: '' });
  }

  removeSubjectStaff(index: number) {
    this.subjectStaffList.splice(index, 1);
  }

  onSubjectChange(index: number) {
    const selectedSubject = this.subjectStaffList[index].subject;
    const staff = this.subjectOptions.find(x => x.subject === selectedSubject);
    if (staff) {
      this.subjectStaffList[index].staff = staff.staffName;
    } else {
      this.subjectStaffList[index].staff = '';
    }
  }

 addStudent() {
  if (
    this.selectedStudent &&
    !this.addedStudents.some(s => s.studentId === this.selectedStudent.studentId)
  ) {
    this.addedStudents.push({
      studentId: this.selectedStudent.studentId,
      studentName: this.selectedStudent.studentName
    });
    this.selectedStudent = null;
  }
}


  removeStudent(index: number) {
    this.addedStudents.splice(index, 1);
  }

  saveAssignedClassroom() {
    const payload = {
      ClassroomNo: this.classroom.classroomNo,
      Class: this.classroom.class,
      Section: this.classroom.section,
      ClassTeacherId: this.classroom.classTeacherId,
      ClassTeacherName: this.classTeachersList.find(x => x.staffId === this.classroom.classTeacherId)?.staffName || ''
      ,
      SubjectStaffList: this.subjectStaffList.map(x => {
        const staff = this.subjectOptions.find(y => y.subject === x.subject && y.staffName === x.staff);
        return {
          Subject: x.subject,
          StaffId: staff ? staff.staffId : 'UNKNOWN', // ✅ Real StaffId
          StaffName: x.staff
        };
      }),
      
      Students: this.addedStudents.map(s => ({
  StudentId: s.studentId,
  StudentName: s.studentName
}))

    };

    this.http.post('https://localhost:7284/api/AddClassroom/AddClassroom', payload).subscribe({
      next: () => {
        alert('✅ Classroom Assigned Successfully!');
        this.resetForm();
      },
      error: () => alert('❌ Failed to assign Classroom')
    });
  }

  resetForm() {
    this.classroom = { classroomNo: '', class: '', section: '', classTeacherId: '' };
    this.subjectStaffList = [{ subject: '', staff: '' }];
    this.addedStudents = [];
    this.selectedStudent = '';
  }

  // ---------------------- EDIT Classroom Section ---------------------- //

  selectedEditClassroomId: number = 0;
  editClassroom: any;
  editSubjectStaffList: any[] = [];
  editStudentList: any[] = [];
  selectedStudentToAdd: string = '';

  fetchClassroomToEdit() {
    if (!this.selectedEditClassroomId) return;
    this.http.get<any>(`https://localhost:7284/api/EditClassroom/GetClassroomById/${this.selectedEditClassroomId}`).subscribe({
      next: (res) => {
        this.editClassroom = res.classroom;
        this.editSubjectStaffList = res.subjectStaffList;
        this.editStudentList = res.students;
      },
      error: () => alert('❌ Failed to fetch Classroom')
    });
  }

  addSubjectEdit() {
    this.editSubjectStaffList.push({ subject: '', staffName: '' });
  }

  removeSubjectEdit(index: number) {
    this.editSubjectStaffList.splice(index, 1);
  }

  addStudentEdit() {
    if (this.selectedStudentToAdd && !this.editStudentList.some(x => x.studentName === this.selectedStudentToAdd)) {
      this.editStudentList.push({ studentName: this.selectedStudentToAdd });
      this.selectedStudentToAdd = '';
    }
  }

  removeStudentEdit(index: number) {
    this.editStudentList.splice(index, 1);
  }

  updateAssignedClassroom() {
    const payload = {
      ClassroomNo: this.editClassroom.classroomNo,
      Class: this.editClassroom.class,
      Section: this.editClassroom.section,
      ClassTeacherId: this.editClassroom.classTeacherId || 'TEMP_ID',
      ClassTeacherName: this.editClassroom.classTeacherName,
      SubjectStaffList: this.editSubjectStaffList,
      Students: this.editStudentList
    };

    this.http.put(`https://localhost:7284/api/EditClassroom/UpdateClassroom/${this.selectedEditClassroomId}`, payload).subscribe({
      next: () => {
        alert('✅ Classroom Updated Successfully!');
      },
      error: () => alert('❌ Failed to update Classroom')
    });
  }

  // ---------------------- DELETE Classroom Section ---------------------- //


// 🔵 Used in DELETE TAB
fetchAssignedClassroomsForDelete() {
  this.http.get<any[]>('https://localhost:7284/api/DeleteClassroom/GetAssignedClassrooms')
    .subscribe({
      next: (res) => {
        this.assignedClassroomsList = res;
      },
      error: () => alert('❌ Error fetching classrooms for Delete')
    });
}

  selectedDeleteClassroomId: number | null = null;
  selectedClassroomDetails: any = null;
  
  fetchSelectedClassroomDetails() {
    if (!this.selectedDeleteClassroomId) {
      return;
    }
  
    this.http.get<any>(`https://localhost:7284/api/DeleteClassroom/GetClassroomById/${this.selectedDeleteClassroomId}`)
      .subscribe({
        next: (res) => {
          this.selectedClassroomDetails = res;
        },
        error: () => {
          alert('❌ Error fetching selected classroom details');
        }
      });
  }
  

  deleteClassroom() {
    if (!this.selectedDeleteClassroomId) {
      alert('❌ Please select a classroom to delete.');
      return;
    }
  
    if (!confirm('⚠️ Are you sure you want to delete this classroom?')) {
      return;
    }
  
    this.http.delete(`https://localhost:7284/api/DeleteClassroom/DeleteClassroom/${this.selectedDeleteClassroomId}`)
      .subscribe({
        next: (res) => {
          alert('✅ Classroom deleted successfully!');
          this.selectedDeleteClassroomId = null;
          this.selectedClassroomDetails = null;
          this.fetchAssignedClassrooms(); // Refresh the dropdown after delete
        },
        error: (err) => {
          alert('❌ Failed to delete classroom');
        }
      });
  }
  

  // ---------------------- VIEW Classroom Section ---------------------- //

  selectedViewClassroomId: number = 0;
  viewClassroom: any = null;
  viewSubjects: any[] = [];
  viewStudents: any[] = [];

 

  fetchClassroomToView() {
    if (!this.selectedViewClassroomId) return;

    this.http.get<any>(`https://localhost:7284/api/ViewClassroom/GetClassroomById/${this.selectedViewClassroomId}`)
      .subscribe({
        next: (res) => {
          this.viewClassroom = res.classroom;
          this.viewSubjects = res.subjectStaffList;
          this.viewStudents = res.students;
        },
        error: () => {
          alert('❌ Failed to load Classroom details');
        }
      });
  }

}
