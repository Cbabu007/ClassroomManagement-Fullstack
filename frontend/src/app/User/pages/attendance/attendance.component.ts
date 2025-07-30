import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import Chart from 'chart.js/auto';
import { AttendanceService } from './attendance.service';

@Component({
  selector: 'app-attendance',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './attendance.component.html',
  styleUrls: ['./attendance.component.css'],
  providers: [AttendanceService]
})
export class AttendanceComponent implements OnInit {
  fromDate = '';
  toDate = '';
  studentId = '';
  attendanceData: any[] = [];
  chart: any;

  email = sessionStorage.getItem('email') || '';
  mobile = sessionStorage.getItem('mobile') || '';

  constructor(
    private attendanceService: AttendanceService,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
    // 🧠 Get studentId based on login
    if (!this.email || !this.mobile) {
      alert('Login required');
      return;
    }

    this.http
      .get<any>(
        `https://localhost:7284/api/UserProfile/GetByLogin?email=${this.email}&mobile=${this.mobile}`
      )
      .subscribe({
        next: (res) => {
          this.studentId = res.studentId;
        },
        error: () => {
          alert('Failed to load student profile');
        }
      });
  }

  fetchAttendance() {
    if (!this.fromDate || !this.toDate || !this.studentId) {
      alert('Please select both dates and ensure login');
      return;
    }

    this.attendanceService
      .getFinalAttendance(this.studentId, this.fromDate, this.toDate)
      .subscribe((res) => {
        this.attendanceData = res;
        this.renderChart();
      });
  }

  renderChart() {
    const summary = { present: 0, absent: 0, halfPresent: 0, halfAbsent: 0 };

    this.attendanceData.forEach((item) => {
      const type = item.leaveType;
      if (type === 'Full Day Present') summary.present++;
      else if (type === 'Full Day Absent') summary.absent++;
      else if (type === 'Half Day Present') summary.halfPresent++;
      else summary.halfAbsent++;
    });

    if (this.chart) this.chart.destroy();

    this.chart = new Chart('attendanceChart', {
      type: 'doughnut',
      data: {
        labels: ['Present', 'Absent', 'Half Present', 'Half Absent'],
        datasets: [
          {
            data: [
              summary.present,
              summary.absent,
              summary.halfPresent,
              summary.halfAbsent
            ],
            backgroundColor: ['#00FF00', '#FF0000', '#00008B', '#8B4513']
          }
        ]
      }
    });
  }
}
