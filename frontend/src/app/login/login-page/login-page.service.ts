import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  status: string;
  role: string;
  redirectUrl: string;
}

@Injectable({
  providedIn: 'root'
})
export class LoginPageService {
  private apiUrl = 'http://localhost:7284/api/Login/ValidateLogin'; // 🔁 Use your correct API port here

  constructor(private http: HttpClient) {}

  validateLogin(data: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(this.apiUrl, data);
  }
}
