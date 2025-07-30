// ✅ FULL UPDATED FILE (homework.component.ts) with UPLOAD HOMEWORK section integration

import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HttpClient, HttpHeaders } from '@angular/common/http';
import { HomeworkService } from './homework.service';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';


@Component({
  selector: 'app-homework',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './homework.component.html',
  styleUrls: ['./homework.component.css'],
  providers: [HomeworkService]
})
export class HomeworkComponent implements OnInit {
  activeTab: string = 'view';
  grades: string[] = [];
  sections: string[] = [];
  subjects: string[] = [];
  topics: string[] = [];

  viewGrade: string = '';
  viewSection: string = '';
  viewData: any[] = [];

  fileBaseUrl: string = 'https://localhost:7284';

  // ⬇️ Upload Homework Form Fields
  studentId: string = '';
  studentName: string = '';
  rollNo: string = '';
  uploadGrade: string = '';
  uploadSection: string = '';
  subject: string = '';
  topic: string = '';
  homeworkSubjects: string[] = [];
  homeworkTopics: string[] = [];
  uploadPreviewUrl: SafeResourceUrl | null = null;
uploadFile: File | null = null;

  

  constructor(private service: HomeworkService, private http: HttpClient, private sanitizer: DomSanitizer) {}

  ngOnInit(): void {
    this.service.getGrades().subscribe(res => this.grades = res);
    this.getStudentInfo();
  }

  switchTab(tab: string) {
    this.activeTab = tab;
  }

  onGradeChange(): void {
    this.viewSection = '';
    this.sections = [];
    if (this.viewGrade) {
      this.service.getSections(this.viewGrade).subscribe(res => this.sections = res);
    }
  }

  fetchHomework(): void {
    if (!this.viewGrade || !this.viewSection) {
      alert('Please select both Grade and Section.');
      return;
    }

    this.service.getHomeworkByGradeSection(this.viewGrade, this.viewSection)
      .subscribe(res => this.viewData = res);
  }

  isImage(path: string): boolean {
    return path ? /\.(jpg|jpeg|png|gif)$/i.test(path) : false;
  }

  isPDF(path: string): boolean {
    return path?.toLowerCase().endsWith('.pdf') || false;
  }

  getFileName(path: string): string {
    return path.split('/').pop() || 'file.pdf';
  }

  openDirectPdf(path: string): void {
    const fullUrl = this.fileBaseUrl + path;
    window.open(fullUrl, '_blank');
  }

  downloadFile(path: string, subject: string) {
    const fullUrl = this.fileBaseUrl + path;
    const extension = path.split('.').pop();
    const fileName = `${subject}.${extension}`;

    fetch(fullUrl, {
      method: 'GET',
      headers: { 'Content-Type': 'application/octet-stream' }
    })
    .then(res => {
      if (!res.ok) throw new Error('Download failed');
      return res.blob();
    })
    .then(blob => {
      const blobUrl = URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = blobUrl;
      a.download = fileName;
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
    })
    .catch(err => {
      console.error('❌ Fetch error:', err);
      alert('Download failed: ' + err.message);
    });
  }

 onUploadFileChange(event: any) {
  const file = event.target.files[0];
  this.uploadFile = file;

  if (file && file.type === 'application/pdf') {
    const blobUrl = URL.createObjectURL(file);
    this.uploadPreviewUrl = this.sanitizer.bypassSecurityTrustResourceUrl(blobUrl);
  } else {
    this.uploadPreviewUrl = null;
  }
}


  getStudentInfo() {
    const email = sessionStorage.getItem('email') || '';
    const mobile = sessionStorage.getItem('mobile') || '';

    this.service.getStudentByLogin(email, mobile).subscribe(data => {
      this.studentId = data.studentId;
      this.studentName = `${data.firstName} ${data.lastName}`;
      this.rollNo = data.rollNo;
      this.uploadGrade = data.grade;
      this.uploadSection = data.section;
      this.getSubjectsAndTopics();
    });
  }

  getSubjectsAndTopics() {
    this.service.getSubjectsAndTopics(this.uploadGrade, this.uploadSection).subscribe(data => {
      this.homeworkSubjects = data.subjects;
      this.homeworkTopics = data.topics;
    });
  }

  submitHomeworkUpload() {
    if (!this.subject || !this.topic || !this.uploadFile) {
      alert('❌ Please select subject, topic, and file.');
      return;
    }

    const formData = new FormData();
    formData.append('studentId', this.studentId);
    formData.append('name', this.studentName);
    formData.append('rollNo', this.rollNo);
    formData.append('grade', this.uploadGrade);
    formData.append('section', this.uploadSection);
    formData.append('subject', this.subject);
    formData.append('topic', this.topic);
    formData.append('file', this.uploadFile);

   this.service.uploadHomework(formData).subscribe({
  next: (res: any) => {
    if (res?.status === 'success') {
      alert('✅ Homework submitted successfully!');
    } else {
      alert('⚠️ Submission response not understood.');
    }
  },
  error: (err) => {
    console.error('Upload error:', err);
    alert('❌ Failed to submit homework.');
  }
});

  }
}
