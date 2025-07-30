import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AttendanceService {
  private apiUrl = 'https://localhost:7284/api/Attendance';

  constructor(private http: HttpClient) {}

  getStudents(grade: string, section: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/GetStudents?grade=${grade}&section=${section}`);
  }

  submitAttendance(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/SubmitAttendance`, data);
  }
  

  getGradesAndSections(): Observable<any> {
    return this.http.get(`${this.apiUrl}/GetGradesAndSections`);
  }
}
