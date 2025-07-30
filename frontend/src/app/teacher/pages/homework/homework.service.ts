import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HomeworkService {
  private apiBase = 'https://localhost:7284/api';

  constructor(private http: HttpClient) {}

  createHomework(formData: FormData): Observable<any> {
    return this.http.post(`${this.apiBase}/AddHomework/Create`, formData);
  }

  updateHomework(formData: FormData): Observable<any> {
    return this.http.post(`${this.apiBase}/EditHomework/Update`, formData);
  }

  getHomework(grade: string, section: string, subject: string, date: string): Observable<any> {
    const params = new HttpParams()
      .set('grade', grade)
      .set('section', section)
      .set('subject', subject)
      .set('date', date);
    return this.http.get(`${this.apiBase}/ViewHomework/Get`, { params });
  }

  deleteHomework(grade: string, section: string, subject: string, date: string): Observable<any> {
    const params = new HttpParams()
      .set('grade', grade)
      .set('section', section)
      .set('subject', subject)
      .set('date', date);
    return this.http.delete(`${this.apiBase}/DeleteHomework/Delete`, { params });
  }

  getTopics(grade: string, section: string, subject: string): Observable<string[]> {
    const params = new HttpParams()
      .set('grade', grade)
      .set('section', section)
      .set('subject', subject);
    return this.http.get<string[]>(`${this.apiBase}/CorrectedHomeworkControllers/GetTopics`, { params });
  }
getCompletedHomework(grade: string, section: string, subject: string, topic: string, date: string): Observable<any[]> {
  const params = new HttpParams()
    .set('grade', grade)
    .set('section', section)
    .set('subject', subject)
    .set('topic', topic)
    .set('date', date);
  return this.http.get<any[]>(`${this.apiBase}/CompletedHomework/GetCompleted`, { params });
}

  getHomeworkForCorrection(grade: string, section: string, subject: string, topic: string, date: string): Observable<any[]> {
    const params = new HttpParams()
      .set('grade', grade)
      .set('section', section)
      .set('subject', subject)
      .set('topic', topic)
      .set('date', date);
    return this.http.get<any[]>(`${this.apiBase}/CorrectedHomeworkControllers/GetHomeworkForCorrection`, { params });
  }

  submitCorrectedHomework(batchData: any[]): Observable<any> {
    return this.http.post(`${this.apiBase}/CorrectedHomeworkControllers/InsertCorrected`, batchData);
  }
}
