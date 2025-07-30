import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { TestService } from './test.service';

@Component({
  selector: 'app-tests',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './tests.component.html',
  styleUrls: ['./tests.component.css']
})
export class TestsComponent implements OnInit {
  email = sessionStorage.getItem('email') || '';
  mobile = sessionStorage.getItem('mobile') || '';

  fullReportData: any[] = [];
  reportCardData: any[] = [];
  dailyTestData: any[] = [];

  terms: string[] = [];
  testNumbers: string[] = [];

  selectedTerm: string = '';
  selectedTestNo: string = '';

  constructor(private testService: TestService) {}

  ngOnInit(): void {
    this.loadFullReport();
    this.loadDropdowns();
  }

  loadFullReport() {
    this.testService.getFullReport(this.email, this.mobile).subscribe((res: any[]) => {
      this.fullReportData = res;
    });
  }

  loadDropdowns() {
    this.testService.getAllTerms(this.email, this.mobile).subscribe((res: any[]) => {
      this.terms = [...new Set(res.map(term => term?.trim()))].sort();
    });

    this.testService.getAllTestNos(this.email, this.mobile).subscribe((res: any[]) => {
      this.testNumbers = [...new Set(res.map(test => test?.trim()))].sort();
    });
  }

  fetchReportCard() {
    this.reportCardData = this.fullReportData.filter(
      x => x.source === 'ReportCard' && x.term === this.selectedTerm
    );
  }

  fetchDailyTest() {
    this.dailyTestData = this.fullReportData.filter(
      x => x.source === 'DailyTest' && x.testId === this.selectedTestNo
    );
  }

  getTotal(section: any[]) {
    return {
      total: section.reduce((sum, item) => sum + item.totalMark, 0),
      answered: section.reduce((sum, item) => sum + item.answeredMark, 0)
    };
  }
}
