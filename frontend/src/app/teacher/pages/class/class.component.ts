import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ClassService } from './class.service';

@Component({
  selector: 'app-class',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './class.component.html',
  styleUrls: ['./class.component.css']
})
export class ClassComponent implements OnInit {
  activeTab: 'attendance' | 'video' | 'students' = 'attendance';

  // Shared Dropdowns
  grades: string[] = [];
  sections: string[] = [];
  subjects: string[] = [];
  staffs: any[] = [];
  mediums: string[] = [];

  // Class Timings (used in Attendance table header)
  classTimings: string[] = [
    'Class 1', 'Class 2', 'Class 3', 'Class 4',
    'Class 5', 'Class 6', 'Class 7', 'Class 8'
  ];

  // Attendance tab
  selectedGrade = '';
  selectedSection = '';
  selectedClassTime = ''; // Not used in logic directly, placeholder if needed
  selectedDate = '';
  allAttendance: any[] = [];
  attendanceSubmitted = false;

  // Assign Video tab
  video = {
    grade: '', section: '', subject: '', staff: '',
    subjectTopic: '', message: '', url: '', date: ''
  };
  assignedList: any[] = [];

  // View Students tab
  studentGrade = '';
  studentSection = '';
  studentMedium = '';
  studentList: any[] = [];

  constructor(private classService: ClassService) {}

  ngOnInit(): void {
    this.loadDropdowns();
    this.loadMediums();
  }

  loadDropdowns(): void {
    this.classService.getDropdowns().subscribe({
      next: (res: any) => {
        this.grades = res.grades || [];
        this.sections = res.sections || [];
        this.subjects = res.subjects || [];
        this.staffs = res.staffs || [];
      },
      error: () => alert('❌ Failed to load dropdowns')
    });
  }

  loadMediums(): void {
    this.classService.getMediums().subscribe({
      next: (res: string[]) => this.mediums = res,
      error: () => alert('❌ Failed to load mediums')
    });
  }

  loadAttendance(): void {
    if (!this.selectedGrade || !this.selectedSection || !this.selectedDate) {
      alert('⚠️ Please select Grade, Section and Date');
      return;
    }

    this.classService.getAttendanceStatus(this.selectedGrade, this.selectedSection, this.selectedDate)
      .subscribe({
        next: (res: any[]) => this.allAttendance = res,
        error: () => alert('❌ Failed to load attendance status')
      });
  }

  getStatusClass(status: string): string {
    if (!status) return '';
    const normalized = status.toLowerCase();
    return normalized === 'present' ? 'present' :
           normalized === 'absent' ? 'absent' : '';
  }

  assignVideo(): void {
    const payload = { ...this.video };
    if (!payload.grade || !payload.section || !payload.subject || !payload.staff || !payload.url || !payload.date) {
      alert('⚠️ Fill all required fields');
      return;
    }

    this.classService.assignVideo(payload).subscribe({
      next: () => {
        alert('✅ Video Assigned Successfully!');
        this.assignedList.push({ ...payload });
        this.video = {
          grade: '', section: '', subject: '', staff: '',
          subjectTopic: '', message: '', url: '', date: ''
        };
      },
      error: () => alert('❌ Failed to assign video')
    });
  }

  submitFinalAttendance(): void {
    if (this.allAttendance.length === 0) {
      alert('⚠️ No attendance data to submit.');
      return;
    }

    this.classService.submitFinalAttendance(this.allAttendance).subscribe({
      next: () => {
        alert('✅ Attendance submitted!');
        this.attendanceSubmitted = true;
      },
      error: () => alert('❌ Submission failed.')
    });
  }

  loadClassStudents(): void {
    if (!this.studentGrade || !this.studentSection || !this.studentMedium) {
      alert('⚠️ Please select Grade, Section and Medium');
      return;
    }

    this.classService.getClassStudents(this.studentGrade, this.studentSection, this.studentMedium).subscribe({
      next: (res: any[]) => this.studentList = res,
      error: () => alert('❌ Failed to load student details')
    });
  }

  getFullImageUrl(path: string): string {
    if (!path) return '';
    return path.startsWith('http') ? path : `https://localhost:7284/${path}`;
  }
  
}
