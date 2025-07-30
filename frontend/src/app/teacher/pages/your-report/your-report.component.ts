import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms'; // ✅ Required for ngModel
import { CommonModule } from '@angular/common';
import { HttpClientModule, HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-your-report',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule], // ✅ Add FormsModule here
  templateUrl: './your-report.component.html',
  styleUrls: ['./your-report.component.css']
})
export class YourReportComponent {
  grades = ['1st', '2nd', '3rd', '4th', '5th', '6th', '7th', '8th', '9th', '10th', '11th', '12th'];
  sections = ['A', 'B', 'C'];

  report = {
    grade: '',
    section: '',
    reportDate: '',
    message: ''
  };

  constructor(private http: HttpClient) {}

  submitReport() {
    this.http.post('https://localhost:7284/api/ReportTeacher/SubmitReport', this.report)
      .subscribe({
        next: (res: any) => alert(res.message || '✅ Submitted'),
        error: () => alert('❌ Submission failed')
      });
  }
}
