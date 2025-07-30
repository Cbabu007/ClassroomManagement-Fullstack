import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AttendanceService {
  private apiUrl = 'https://localhost:7284/api/StudentAttendance';

  constructor(private http: HttpClient) {}

  getFinalAttendance(studentId: string, from: string, to: string) {
    return this.http.get<any[]>(`${this.apiUrl}/GetFinalAttendance?studentId=${studentId}&fromDate=${from}&toDate=${to}`);
  }
}
