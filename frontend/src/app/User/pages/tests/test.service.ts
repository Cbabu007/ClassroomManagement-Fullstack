import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TestService {
  private baseUrl = 'https://localhost:7284/api/StudentTestReport';

  constructor(private http: HttpClient) {}
// 🔹 Full report from both ReportCard and DailyTest
  getFullReport(email: string, mobile: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/GetStudentFullReport?email=${email}&mobile=${mobile}`);
  }

  // 🔹 Terms dropdown (unique terms from ReportCard)
  getAllTerms(email: string, mobile: string): Observable<string[]> {
    return this.http.get<string[]>(`${this.baseUrl}/GetAllTerms?email=${email}&mobile=${mobile}`);
  }

  // 🔹 Test No dropdown (unique testIds from DailyTest)
  getAllTestNos(email: string, mobile: string): Observable<string[]> {
    return this.http.get<string[]>(`${this.baseUrl}/GetAllTestNos?email=${email}&mobile=${mobile}`);
  }
}
