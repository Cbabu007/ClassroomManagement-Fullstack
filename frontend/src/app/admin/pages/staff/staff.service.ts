// 🛡️ staff.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Staff } from './staff.component'; // ✅ Import Staff class

@Injectable({
  providedIn: 'root'
})
export class StaffService {
  private apiUrl = 'https://localhost:7284/api';

  constructor(private http: HttpClient) { }

  addStaff(formData: FormData): Observable<any> {
    return this.http.post(`${this.apiUrl}/AddStaff`, formData);
  }

  getStaffById(staffId: string) {
    return this.http.get<any>(`https://localhost:7284/api/EditStaff/${staffId}`);
  }
  
  
  updateStaff(staffId: string, formData: FormData) {
    return this.http.put(`https://localhost:7284/api/EditStaff/${staffId}`, formData);
  }
  
  getStaffForDelete(staffId: string) {
    return this.http.get<any>(`https://localhost:7284/api/DeleteStaff/${staffId}`);
  }
  
  deleteStaff(staffId: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/DeleteStaff/${staffId}`);
  }

  viewStaff(staffId: string): Observable<Staff> {
    return this.http.get<Staff>(`${this.apiUrl}/ViewStaff/${staffId}`);
  }
}
