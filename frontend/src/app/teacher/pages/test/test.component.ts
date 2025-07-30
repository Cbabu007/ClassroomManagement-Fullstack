import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; 
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { TestService } from './test.service';

@Component({
  selector: 'app-test',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule
  ],
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.css']
})
export class TestComponent implements OnInit {
  activeTab = 'testMark';

  // Common fields
  rollNo = '';
  grade = '';
  section = '';
  date = '';
  testType = '';

  // Report Card Fields
  term = '';
  subject = '';
  totalMark = 0;
  answeredMark = 0;
  remark = '';
  reportCardList: any[] = [];
  reportCardTestId = 1;

  // Daily Test Fields
  testNo = '';
  topic = '';
  questionNo = 1;
  selectMark = 0;
  dailyTestList: any[] = [];
  dailyTestTestId = 1;

  viewStudent: any = null;

  // View Fields
  viewRollNo = '';
  viewGrade = '';
  viewSection = '';
  viewDate = '';
  selectedTestNo = '';
  availableTestNos: string[] = [];
  reportCardViewList: any[] = [];
  dailyTestViewList: any[] = [];

  // Dropdown options
  terms = ['Unit Test', 'Cycle Test', 'Quarterly Exam', 'Half-Yearly Exam', 'Revision Exam', 'Public Exam', 'Practical Exam', 'Supplementary Exam', 'Internal Assessment', 'First Term', 'Second Term', 'Third Term'];
  markOptions = Array.from({ length: 20 }, (_, i) => (i + 1) * 5);
  questionOptions = Array.from({ length: 100 }, (_, i) => i + 1);
  smallMarkOptions = Array.from({ length: 11 }, (_, i) => i);

  constructor(private testService: TestService, private http: HttpClient) {}

  newTestInitialized = false;

initializeNewTest() {
  this.newTestInitialized = true;

  if (this.testType === 'Report Card') {
    this.reportCardTestId += 1;
  } else if (this.testType === 'Daily Test') {
    this.dailyTestTestId += 1;
  }
}
  ngOnInit(): void {}

  setTab(tab: string) {
    this.activeTab = tab;
  }

  resetForm() {
    this.term = '';
    this.subject = '';
    this.totalMark = 0;
    this.answeredMark = 0;
    this.remark = '';
    this.testNo = '';
    this.topic = '';
    this.questionNo = 1;
    this.selectMark = 0;
  }

  fetchStudentDetails() {
    this.testService.getStudentDetails(this.rollNo).subscribe((data: any) => {
      this.grade = data.grade;
      this.section = data.section;
    });
  }

  fetchViewStudentDetails() {
  if (!this.viewRollNo) return;

  this.http.get<any>(`http://localhost:7284/api/Test/GetStudentByRollNo?rollNo=${this.viewRollNo}`).subscribe({
    next: (res) => {
      this.viewStudent = {
        fullName: res.fullName,
        photoPath: res.photoPath
      };

      // 🔁 Get test numbers here
      this.testService.getAvailableTestNos(this.viewRollNo).subscribe({
        next: (data) => {
          console.log('✅ Test Numbers:', data); // Debug log
          this.availableTestNos = data;

          // Auto-select first
          if (this.availableTestNos.length > 0) {
            this.selectedTestNo = this.availableTestNos[0];
            this.filterDailyTest();
          }
        },
        error: (err) => {
          console.error('❌ Failed to load test numbers', err);
        }
      });
    },
    error: (err) => {
      console.error('Failed to load student', err);
      this.viewStudent = null;
    }
  });
}



  getImageUrl(path: string): string {
    return path ? `/uploads/photos/${path}` : '/assets/default-photo.png'; // optional
  }
  
  setDefaultPhoto(event: any) {
    event.target.src = '/assets/default-photo.png';
  }
  
  
  removeReportCard(index: number) {
    this.reportCardList.splice(index, 1);
  }
  removeDailyTest(index: number) {
    this.dailyTestList.splice(index, 1);
  }
  
  addReportCard() {
    const entry = {
      term: this.term,
      testId: this.reportCardTestId,
      subject: this.subject,
      totalMark: this.totalMark,
      answeredMark: this.answeredMark,
      remark: this.remark
    };
    this.reportCardList.push(entry);
  }

  addDailyTest() {
    const entry = {
      testNo: this.testNo,
      testId: this.dailyTestTestId,
      subject: this.subject,
      topic: this.topic,
      questionNo: this.questionNo,
      selectMark: this.selectMark,
      answeredMark: this.answeredMark,
      remark: this.remark
    };
    this.dailyTestList.push(entry);

    if (!this.availableTestNos.includes(this.testNo)) {
      this.availableTestNos.push(this.testNo);
    }
  }

  submitTestMarks() {
    if (this.testType === 'Report Card') {
      const payload = {
        rollNo: this.rollNo,
        grade: this.grade,
        section: this.section,
        date: this.date,
        entries: this.reportCardList
      };
  
      this.testService.submitReportCard(payload).subscribe({
        next: (res: any) => {
          alert('✅ Report Card saved successfully!');
          this.reportCardList = [];
          this.newTestInitialized = false;
        },
        error: (err: any) => {
          console.error(err);
          alert('❌ Failed to save Report Card!');
        }
      });
      
    }
  
    if (this.testType === 'Daily Test') {
      const payload = {
        rollNo: this.rollNo,
        grade: this.grade,
        section: this.section,
        date: this.date,
        entries: this.dailyTestList
      };
  
      this.testService.submitDailyTest(payload).subscribe({
        next: (res: any) => {
          alert('✅ Daily Test saved successfully!');
          this.dailyTestList = [];
          this.newTestInitialized = false;
        },
        error: (err: any) => {
          console.error(err);
          alert('❌ Failed to save Daily Test!');
        }
      });
      
    }
  }
  

  loadViewMarks() {
    this.testService.getReportCard(this.viewRollNo, this.viewDate).subscribe((data: any[]) => {
      this.reportCardViewList = data;
    });

    this.testService.getDailyTest(this.viewRollNo, this.viewDate).subscribe((data: any[]) => {
      this.dailyTestViewList = data;
    });
  }

filterDailyTest() {
  if (!this.selectedTestNo || !this.viewRollNo || !this.viewDate) return;

  this.testService
    .getDailyTestByTestNo(this.viewRollNo, this.viewDate, this.selectedTestNo)
    .subscribe((data: any[]) => {
      this.dailyTestViewList = data;
    });
}

}
