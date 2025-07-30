// src/app/report/report.service.ts

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  private baseUrl = 'https://localhost:7284/api/AdminReportDashboard'; // ✅ Make sure this matches controller route

  constructor(private http: HttpClient) {}

  // 1. Dashboard Summary: Total Students, Gender, Classrooms, Staff Summary
  getDashboardData(): Observable<any> {
    return this.http.get(`${this.baseUrl}/GetDashboardData`);
  }

  // 2. View All Students
  getAllStudents(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/GetAllStudents`);
  }

  // 3. View All Staff
  getAllStaff(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/GetAllStaff`);
  }

  // 4. Filter Students by Grade and Section
  filterStudents(grade: string, section: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/FilterStudents?grade=${grade}&section=${section}`);
  }

  // 5. Filter Staff by Grade and Section
  filterStaff(grade: string, section: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/FilterStaff?grade=${grade}&section=${section}`);
  }
  getStudentGradeSection(): Observable<any> {
  return this.http.get(`${this.baseUrl}/GetStudentGradeSection`);
}

}
