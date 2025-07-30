import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; // ✅ For ngModel
import { HttpClientModule } from '@angular/common/http'; // ✅ Needed for HTTP requests
import { ReportService } from './report.service';
import Chart from 'chart.js/auto'; // ✅ Make sure this is installed via `npm install chart.js`

@Component({
  selector: 'app-report',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css'],
  providers: [ReportService]
})
export class ReportComponent implements OnInit {
  totalStudents = 0;
  totalClassrooms = 0;
  maleCount = 0;          
  femaleCount = 0;
  classrooms: any[] = [];
  staffSummary: any[] = [];

 grades: string[] = [];
sections: string[] = [];


  studentGrade = '';
  studentSection = '';
  staffGrade = '';
  staffSection = '';

  bottomData: any[] = [];
  bottomColumns: string[] = [];

  constructor(private reportService: ReportService) {}

  ngOnInit(): void {
  this.loadDashboardData();

  this.reportService.getStudentGradeSection().subscribe((data: any) => {
    this.grades = data.grades;
    this.sections = data.sections;
  });
}


  loadDashboardData() {
    this.reportService.getDashboardData().subscribe((data: any) => {
      this.totalStudents = data.totalStudents;
      this.totalClassrooms = data.totalClassrooms;
      this.classrooms = data.classrooms;
      this.staffSummary = data.staffSummary;
      
    // ✅ Set male/female counts first
    this.maleCount = data.maleCount;
    this.femaleCount = data.femaleCount;

    // ✅ Then call the chart render method
    this.renderChart(this.maleCount, this.femaleCount);
    });
  }

  renderChart(male: number, female: number) {
  const existingChart = Chart.getChart('genderChart');
  if (existingChart) {
    existingChart.destroy();
  }

  const ctx = document.getElementById('genderChart') as HTMLCanvasElement;
  new Chart(ctx, {
    type: 'doughnut',
    data: {
      labels: [],
      datasets: [{
        data: [male, female],
        backgroundColor: ['#2196f3', '#e91e63'], // Updated color
        borderWidth: 1
      }]
    },
    options: {
      responsive: true,
      cutout: '65%',
      plugins: {
        legend: { display: false }
      }
    }
  });
}


  viewAllStudents() {
  this.reportService.getAllStudents().subscribe((data: any[]) => {
   console.log('Student Data:', data);
console.log('First object:', data[0]);
 // ✅ Add this for debug
    this.bottomData = data;
    this.bottomColumns = ['studentId', 'name', 'grade', 'section'];
  });
}


  viewAllStaff() {
  this.reportService.getAllStaff().subscribe((data: any[]) => {
    console.log("Staff Data:", data);       // ✅ Check what keys you get
    console.log("First Staff:", data[0]);   // ✅ See exact property names

    this.bottomData = data;
    this.bottomColumns = ['staffId', 'name', 'department']; // adjust if needed
  });
}


  filterStudents() {
  this.reportService.filterStudents(this.studentGrade, this.studentSection).subscribe((data: any[]) => {
    this.bottomData = data;
    this.bottomColumns = [
      'photoPath', 'studentId', 'firstName', 'lastName', 'dob', 'gender', 'bloodGroup',
      'aadhar', 'mobile', 'altMobile', 'email', 'doorNo', 'address1', 'address2', 'city', 'district',
      'state', 'pincode', 'country', 'admissionNo', 'admissionDate', 'grade', 'section',
      'rollNo', 'medium', 'academicYear', 'fatherName', 'fatherJob', 'fatherMobile',
      'motherName', 'motherJob', 'motherMobile', 'guardianName', 'guardianRelation',
      'guardianMobile', 'prevSchool', 'tcPath', 'idProofPath', 'addressProofPath',
      'status', 'reasonLeaving', 'entranceTest', 'communityType', 'communityName', 'religion'
    ];
  });
}

getPhotoUrl(photoPath: string): string {
  if (!photoPath) return '';
  return photoPath.startsWith('http') ? photoPath : `https://localhost:7284/${photoPath}`;
}

  filterStaff() {
    this.reportService.filterStaff(this.staffGrade, this.staffSection).subscribe((data: any[]) => {
      this.bottomData = data;
      this.bottomColumns = ['StaffId', 'Name', 'Department'];
    });
  }
}
