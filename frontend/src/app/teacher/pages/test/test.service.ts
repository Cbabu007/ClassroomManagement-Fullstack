import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TestService {
  private baseUrl = 'https://localhost:7284/api/Test';

  constructor(private http: HttpClient) {}

  getStudentDetails(rollNo: string): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/GetStudentByRollNo?rollNo=${rollNo}`);
  }

  submitReportCard(data: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/SubmitReportCard`, data);
  }

  submitDailyTest(data: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/SubmitDailyTest`, data);
  }

  getReportCard(rollNo: string, date: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/GetReportCard?rollNo=${rollNo}&date=${date}`);
  }

  getDailyTest(rollNo: string, date: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/GetDailyTest?rollNo=${rollNo}&date=${date}`);
  }
  getStudentByRollNo(rollNo: string) {
    return this.http.get<any>(`http://localhost:7284/api/Test/GetStudentByRollNo?rollNo=${rollNo}`);
  }
  
  getDailyTestByTestNo(rollNo: string, date: string, testNo: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/GetDailyTestByTestNo?rollNo=${rollNo}&date=${date}&testNo=${testNo}`);
  }
  getAvailableTestNos(rollNo: string) {
  return this.http.get<string[]>(`http://localhost:7284/api/Test/GetAvailableTestNos?rollNo=${rollNo}`);
}


}
