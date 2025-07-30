import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgIf, NgFor, CommonModule } from '@angular/common';
import { AttendanceService } from './attendance.service';

@Component({
  selector: 'app-attendance',
  standalone: true,
  imports: [CommonModule, NgIf, NgFor, FormsModule],
  templateUrl: './attendance.component.html',
  styleUrls: ['./attendance.component.css']
})
export class AttendanceComponent implements OnInit {
  grades: string[] = [];
  sections: string[] = [];
  classTimings: string[] = [
    'Class 1', 'Class 2', 'Class 3', 'Class 4',
    'Class 5', 'Class 6', 'Class 7', 'Class 8'
  ];

  selectedGrade: string = '';
  selectedSection: string = '';
  selectedClassTime: string = '';

  students: any[] = [];

  constructor(private attendanceService: AttendanceService) {}

  ngOnInit(): void {
    this.loadGradesAndSections();
  }

  loadGradesAndSections(): void {
    this.attendanceService.getGradesAndSections().subscribe({
      next: (res: any) => {
        this.grades = [...new Set(res.map((s: any) => s.grade))] as string[];
        this.sections = [...new Set(res.map((s: any) => s.section))] as string[];
      },
      error: () => alert('❌ Failed to load Grades/Sections')
    });
  }

  fetchStudents(): void {
    if (!this.selectedGrade || !this.selectedSection) {
      alert('Please select grade and section');
      return;
    }

    this.attendanceService.getStudents(this.selectedGrade, this.selectedSection).subscribe({
      next: (res: any) => {
        this.students = res.map((s: any) => ({
          studentId: s.studentId,
          name: s.name,
          fatherMobile: s.fatherMobile,
          motherMobile: s.motherMobile,
          present: false
        }));
      },
      error: () => alert('❌ Failed to fetch student list')
    });
  }

  submitAttendance(): void {
    const today = new Date().toISOString().split('T')[0];
  
    const presentList = this.students
      .filter(s => s.present)
      .map(s => ({
        studentId: s.studentId,
        studentName: s.name,
        classTiming: this.selectedClassTime,
        date: today
      }));
  
    const absentList = this.students
      .filter(s => !s.present)
      .map(s => ({
        studentId: s.studentId,
        studentName: s.name,
        classTiming: this.selectedClassTime,
        fatherMobile: s.fatherMobile,
        motherMobile: s.motherMobile,
        date: today
      }));
  
    const payload = { presentList, absentList };
  
    this.attendanceService.submitAttendance(payload).subscribe({
      next: (res: any) => {
        alert(res.message || '✅ Attendance submitted!');
      },
      error: (err) => {
        console.error('❌ Error submitting attendance:', err);
        alert('❌ Error submitting attendance');
      }
    });
  }  
}
