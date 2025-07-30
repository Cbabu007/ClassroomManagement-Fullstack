import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HomeworkService {
  private baseUrl = 'https://localhost:7284/api';

  constructor(private http: HttpClient) {}

  // 🔹 Get Grades
  getGrades(): Observable<string[]> {
    return this.http.get<string[]>(`${this.baseUrl}/AssignedHomework/GetAllGrades`);
  }

  // 🔹 Get Sections
  getSections(grade: string): Observable<string[]> {
    const params = new HttpParams().set('grade', grade);
    return this.http.get<string[]>(`${this.baseUrl}/AssignedHomework/GetSectionsByGrade`, { params });
  }

  // 🔹 View Homework by Grade and Section
  getHomeworkByGradeSection(grade: string, section: string): Observable<any[]> {
    const params = new HttpParams()
      .set('grade', grade)
      .set('section', section);
    return this.http.get<any[]>(`${this.baseUrl}/AssignedHomework/GetByGradeSection`, { params });
  }

  // 🔹 Get Student Info from Login
  getStudentByLogin(email: string, mobile: string): Observable<any> {
    const params = new HttpParams()
      .set('email', email)
      .set('mobile', mobile);
    return this.http.get<any>(`${this.baseUrl}/SubmitHomework/GetStudentByLogin`, { params });
  }

  // 🔹 Get Subjects and Topics
  getSubjectsAndTopics(grade: string, section: string): Observable<any> {
    const params = new HttpParams()
      .set('grade', grade)
      .set('section', section);
    return this.http.get<any>(`${this.baseUrl}/SubmitHomework/GetSubjectsAndTopics`, { params });
  }

  // 🔹 Submit Uploaded Homework
  uploadHomework(formData: FormData): Observable<any> {
    return this.http.post(`${this.baseUrl}/SubmitHomework/Submit`, formData);
  }
}
