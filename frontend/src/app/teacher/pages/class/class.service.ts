import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ClassService {
  private baseUrl = 'https://localhost:7284/api';

  constructor(private http: HttpClient) {}

  getDropdowns(): Observable<any> {
    return this.http.get(`${this.baseUrl}/AssignedVideoClass/GetDropdowns`);
  }

  assignVideo(payload: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/AssignedVideoClass/Assign`, payload);
  }

  getAttendanceStatus(grade: string, section: string, date: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/Class/GetAttendanceStatus?grade=${grade}&section=${section}&date=${date}`);
  }
  

  getGradesAndSections(): Observable<any> {
    return this.http.get(`${this.baseUrl}/Class/GetGradesAndSections`);
  }

  submitFinalAttendance(data: any[]): Observable<any> {
    return this.http.post(`${this.baseUrl}/Class/SubmitFinalAttendance`, data, { responseType: 'text' });
  }
 
  getClassStudents(grade: string, section: string, medium: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/ViewStudents/GetStudents?grade=${grade}&section=${section}&medium=${medium}`);
  }
  getMediums(): Observable<string[]> {
    return this.http.get<string[]>(`${this.baseUrl}/ViewStudents/GetMediums`);
  }
  
 
  
}