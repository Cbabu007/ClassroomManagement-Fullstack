import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HomeworkService } from './homework.service';

@Component({
  selector: 'app-homework',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './homework.component.html',
  styleUrls: ['./homework.component.css']
})
export class HomeworkComponent implements OnInit {
  activeTab = 'assign';
  mode = 'Create';

  grades: string[] = [];
  sections: string[] = [];
  staffList: any[] = [];
  subjects: string[] = [];

  grade = '';
  section = '';
  classroomNo = '';
  subject = '';
  date = '';
  topic = '';
  staffId = '';
  file: File | null = null;
  fileName = '';
  previewUrl = '';

  correctGrade = '';
  correctSection = '';
  correctSubject = '';
  correctDate = '';
  correctTopic = '';
  topics: string[] = [];
  correctHomeworkList: any[] = [];

  viewData: any = null;
  editHomeworkId: number = 0;

  completedGrade = '';
  completedSection = '';
  completedSubject = '';
  completedTopic = '';
  completedDate = '';
  completedList: any[] = [];

  constructor(private http: HttpClient, public hwService: HomeworkService) {}

  ngOnInit(): void {
    this.fetchGradesAndSections();
    this.fetchStaffs();
    this.fetchSubjects();
  }

  fetchGradesAndSections() {
    this.http.get<any>('https://localhost:7284/api/AddHomework/GetGradesAndSections')
      .subscribe(res => {
        this.grades = res.grades;
        this.sections = res.sections;
      });
  }

  fetchStaffs() {
    this.http.get<any[]>('https://localhost:7284/api/AddHomework/GetStaffList')
      .subscribe(res => {
        this.staffList = res.map(s => ({
          staffId: s.staffId,
          name: s.name,
          subject: s.department
        }));
      });
  }

  fetchSubjects() {
    this.http.get<string[]>('https://localhost:7284/api/EditHomework/GetSubjects')
      .subscribe(res => this.subjects = res);
  }

  onGradeOrSectionChange() {
    if (this.grade && this.section) {
      const params = { className: this.grade, section: this.section };
      this.http.get<number>('https://localhost:7284/api/AddHomework/GetClassroomNo', { params })
        .subscribe({
          next: (res) => this.classroomNo = res.toString(),
          error: () => this.classroomNo = ''
        });
    }
  }

  onStaffChange() {
    const selected = this.staffList.find(s => s.staffId === this.staffId);
    this.subject = selected ? selected.subject : '';
  }

  onFileChange(event: any) {
    const file = event.target.files[0];
    this.file = file;
    this.fileName = file?.name;
    if (file) {
      const reader = new FileReader();
      reader.onload = () => this.previewUrl = reader.result as string;
      reader.readAsDataURL(file);
    }
  }

  submitHomework() {
    if (!this.grade || !this.section || !this.classroomNo || !this.subject || !this.topic || !this.date || !this.staffId) {
      alert('❌ Please fill all required fields!');
      return;
    }

    const formData = new FormData();
    formData.append('grade', this.grade);
    formData.append('section', this.section);
    formData.append('classroomNo', this.classroomNo);
    formData.append('subject', this.subject);
    formData.append('date', this.date);
    formData.append('staffId', this.staffId);
    formData.append('staffName', this.staffList.find(s => s.staffId === this.staffId)?.name || '');
    formData.append('topic', this.topic);
    formData.append('filepath', '');
    if (this.file) formData.append('file', this.file);

    this.hwService.createHomework(formData).subscribe({
      next: () => {
        alert('✅ Homework Created!');
        this.resetForm();
      },
      error: () => alert('❌ Failed to create homework.')
    });
  }

  submitEdit() {
    if (!this.editHomeworkId) {
      alert("❌ No homework selected to update.");
      return;
    }

    const formData = new FormData();
    formData.append("id", this.editHomeworkId.toString());
    formData.append("grade", this.grade);
    formData.append("section", this.section);
    formData.append("subject", this.subject);
    formData.append("date", this.date);
    formData.append("topic", this.topic);
    if (this.file) formData.append("file", this.file);

    this.http.put(`https://localhost:7284/api/EditHomework/Update/${this.editHomeworkId}`, formData, { responseType: 'text' })
      .subscribe({
        next: () => alert("✅ Homework updated successfully!"),
        error: () => alert("❌ Update failed.")
      });
  }

  loadHomeworkForEdit() {
    const params = new HttpParams()
      .set('grade', this.grade)
      .set('section', this.section)
      .set('subject', this.subject)
      .set('date', this.date);

    this.http.get<any>('https://localhost:7284/api/EditHomework', { params })
      .subscribe({
        next: (data) => {
          this.editHomeworkId = data.id;
          this.topic = data.topic;
          this.previewUrl = 'https://localhost:7284' + data.filepath;
        },
        error: () => alert("❌ No homework found.")
      });
  }

  loadHomeworkForView() {
    const params = new HttpParams()
      .set('grade', this.grade)
      .set('section', this.section)
      .set('subject', this.subject)
      .set('date', this.date);

    this.http.get<any>('https://localhost:7284/api/ViewHomework', { params })
      .subscribe({
        next: (res) => {
          if (res.filepath) res.filepath = 'https://localhost:7284' + res.filepath;
          this.viewData = res;
        },
        error: () => {
          alert('❌ No homework found.');
          this.viewData = null;
        }
      });
  }

  deleteHomework() {
    if (!this.grade || !this.section || !this.subject || !this.date) {
      alert('❌ Please fill all fields');
      return;
    }

    const params = new HttpParams()
      .set('grade', this.grade)
      .set('section', this.section)
      .set('subject', this.subject)
      .set('date', this.date);

    this.http.delete('https://localhost:7284/api/DeleteHomework', { params, responseType: 'text' })
      .subscribe({
        next: () => alert('🗑️ Homework deleted successfully'),
        error: () => alert('❌ Failed to delete homework.')
      });
  }

  fetchTopics() {
    if (this.correctGrade && this.correctSection && this.correctSubject) {
      const params = new HttpParams()
        .set('grade', this.correctGrade)
        .set('section', this.correctSection)
        .set('subject', this.correctSubject);

      this.http.get<string[]>('https://localhost:7284/api/CorrectedHomeworkControllers/GetTopics', { params })
        .subscribe({
          next: (res) => this.topics = res,
          error: () => this.topics = []
        });
    }
  }

  loadCorrectHomeworkData() {
    if (!this.correctGrade || !this.correctSection || !this.correctSubject || !this.correctTopic || !this.correctDate) {
      alert("❌ Please fill all fields.");
      return;
    }

    const params = new HttpParams()
      .set('grade', this.correctGrade)
      .set('section', this.correctSection)
      .set('subject', this.correctSubject)
      .set('topic', this.correctTopic)
      .set('date', this.correctDate);

    this.http.get<any[]>('https://localhost:7284/api/CorrectedHomeworkControllers/GetHomeworkForCorrection', { params })
      .subscribe({
        next: (res) => this.correctHomeworkList = res,
        error: () => alert('❌ Failed to load homework data')
      });
  }

  loadCompletedHomework() {
    if (!this.completedGrade || !this.completedSection || !this.completedSubject || !this.completedTopic || !this.completedDate) {
      alert('❌ Please fill all completed filters');
      return;
    }

    this.hwService.getCompletedHomework(
      this.completedGrade,
      this.completedSection,
      this.completedSubject,
      this.completedTopic,
      this.completedDate
    ).subscribe({
      next: (res) => this.completedList = res,
      error: () => alert('❌ Failed to load completed homework data')
    });
  }

  // ✅ Submit Corrected Homework
  submitCorrectedHomework() {
    if (!this.correctGrade || !this.correctSection || !this.correctSubject || !this.correctTopic || !this.correctDate) {
      alert("❌ Please fill all fields.");
      return;
    }

    const correctedList = this.correctHomeworkList.map(hw => ({
      grade: this.correctGrade,
      section: this.correctSection,
      rollNo: hw.rollNo,
      name: hw.name,
      subject: this.correctSubject,
      topic: this.correctTopic,
      date: this.correctDate,
      filePath: hw.filePath || '', // ✅ Required by backend
      mark: hw.marked ? Number(hw.mark || 0) : 0,
      action: hw.marked ? "Correct" : "Not Corrected"
    }));

    this.hwService.submitCorrectedHomework(correctedList).subscribe({
      next: (res) => alert(res.message || "✔️ Corrections submitted successfully."),
      error: (err) => alert("❌ Submission failed: " + (err.error?.error || "Unknown server error"))
    });
  }

  setTab(tab: string) {
    this.activeTab = tab;
    this.mode = 'Create';
    this.resetForm();
  }

  setMode(mode: string) {
    this.mode = mode;
    this.resetForm();
  }

  resetForm() {
    this.grade = '';
    this.section = '';
    this.classroomNo = '';
    this.subject = '';
    this.date = '';
    this.topic = '';
    this.staffId = '';
    this.file = null;
    this.fileName = '';
    this.previewUrl = '';
    this.viewData = null;
  }
}
