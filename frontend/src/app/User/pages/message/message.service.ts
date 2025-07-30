import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  private apiUrl = 'https://localhost:7284/api/ViewMessage/GetMessages';

  constructor(private http: HttpClient) {}

  getMessagesByLogin(email: string, mobile: string): Observable<any[]> {
    return this.http.post<any[]>(this.apiUrl, { email, mobile });
  }
}
