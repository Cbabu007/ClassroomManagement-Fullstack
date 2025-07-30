import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  private apiUrl = 'https://localhost:7284/api/Message';

  constructor(private http: HttpClient) {}

  getMessages(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  addMessage(msg: any): Observable<any> {
    return this.http.post(this.apiUrl, msg);
  }

  deleteMessage(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
