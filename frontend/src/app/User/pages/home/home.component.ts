import { Component, OnInit, NgZone } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import Chart from 'chart.js/auto';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  student: any = {};
  nextAction: string = '';
  leaderboard: any[] = [];

  selectedTab = 'reportcard';
  terms: string[] = ['First Term', 'Quarterly Exam'];
  selectedTerm = this.terms[0];
  subjectStats: any[] = [];

  testNumbers: string[] = ['Test No 1', 'Test No 2'];
  selectedTestNo = this.testNumbers[0];
  testSubjectStats: any[] = [];

  baseUrl: string = 'https://localhost:7284';
  chart: any;
  showReport = true;

  constructor(private http: HttpClient, private ngZone: NgZone) {}

  ngOnInit(): void {
    this.loadStudentInfo();
    this.loadLeaderboard();
    this.fetchReportCard();
  }

  selectTab(tab: string) {
    this.selectedTab = tab;
    if (tab === 'reportcard') this.fetchReportCard();
    else this.fetchDailyTest();
  }

  getImage(photoPath: string): string {
    return photoPath ? `${this.baseUrl}/${photoPath}` : 'https://cdn-icons-png.flaticon.com/512/847/847969.png';
  }

  loadStudentInfo() {
    this.http.get<any>(`${this.baseUrl}/api/StudentDashboard/GetStudentByLogin?email=babuvenkatesan7@gmail.com&mobile=9998887776`)
      .subscribe(data => {
        this.student = data;
        this.nextAction = data.message;
      });
  }

  loadLeaderboard() {
    this.http.get<any[]>(`${this.baseUrl}/api/StudentDashboard/GetLeaderboard?grade=2nd&section=A`)
      .subscribe(data => this.leaderboard = data);
  }

  fetchReportCard() {
  this.showReport = false;

  this.http.get<any[]>(`${this.baseUrl}/api/StudentDashboard/GetTopReportCardByTerm?term=${encodeURIComponent(this.selectedTerm)}`)
    .subscribe(data => {
      this.subjectStats = data;

      // Use Angular's zone to ensure async render completes before chart/table generation
      this.ngZone.runOutsideAngular(() => {
        requestAnimationFrame(() => {
          this.showReport = true;

          // Run after DOM refresh
          setTimeout(() => {
            this.generateBubbleChart(data, 'reportCardChart');
            this.renderPieCharts('pie', data);
          }, 50); // Small delay ensures canvas is in DOM
        });
      });
    });
}


  fetchDailyTest() {
    this.http.get<any[]>(`${this.baseUrl}/api/StudentDashboard/GetTopDailyTestByTestNo?testNo=${encodeURIComponent(this.selectedTestNo)}`)
      .subscribe(data => {
        this.testSubjectStats = data;
        this.generateBubbleChart(data, 'dailyTestChart');
        this.renderPieCharts('pie', data);
      });
  }

  generateBubbleChart(data: any[], canvasId: string) {
    const container = document.getElementById(canvasId + 'Wrapper');

    if (this.chart) this.chart.destroy();
    if (container) container.innerHTML = '';

    const table = document.createElement('table');
    table.style.width = '100%';
    table.style.borderCollapse = 'collapse';
    table.innerHTML = `
      <thead>
        <tr>
          <th style="border: 1px solid #ccc; padding: 8px;">S.No</th>
          <th style="border: 1px solid #ccc; padding: 8px;">Name</th>
          <th style="border: 1px solid #ccc; padding: 8px;">Rank</th>
        </tr>
      </thead>
      <tbody>
        ${data.map((student, i) => `
          <tr>
            <td style="border: 1px solid #ccc; padding: 8px; text-align:center;">${i + 1}</td>
            <td style="border: 1px solid #ccc; padding: 8px;">${student.name}</td>
            <td style="border: 1px solid #ccc; padding: 8px; text-align:center;">Rank ${i + 1}</td>
          </tr>
        `).join('')}
      </tbody>
    `;

    container?.appendChild(table);
  }

  renderPieCharts(prefix: string, data: any[]) {
    data.forEach(subj => {
      const canvasId = `${prefix}-${subj.subject}`;
      const canvasElem = document.getElementById(canvasId) as HTMLCanvasElement;
      if (!canvasElem) return;

      new Chart(canvasElem, {
        type: 'pie',
        data: {
          datasets: [{
            data: [subj.answeredMark, subj.totalMark - subj.answeredMark],
            backgroundColor: [subj.answeredMark >= (subj.totalMark * 0.35) ? 'green' : 'red', '#eee']
          }]
        },
        options: {
          plugins: {
            legend: { display: false }
          }
        }
      });
    });
  }
}
