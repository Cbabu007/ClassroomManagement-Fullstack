import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EventService {
  private apiUrl = 'https://localhost:7284/api/Event';

  constructor(private http: HttpClient) {}

  getEvents(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  addEvent(event: any): Observable<any> {
    return this.http.post(this.apiUrl, event);
  }

  deleteEvent(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
