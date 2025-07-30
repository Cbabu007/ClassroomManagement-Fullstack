import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class StudentService {
  constructor(private http: HttpClient) {}

  addStudent(formData: FormData): Observable<any> {
    return this.http.post('https://localhost:7284/api/AddStudent', formData);
  }

  getStudentById(studentId: string) {
    return this.http.get<any>(`https://localhost:7284/api/ViewStudent/${studentId}`);
  }

  updateStudent(studentId: string, formData: FormData) {
    return this.http.put(`https://localhost:7284/api/EditStudent/${studentId}`, formData);
  }

  deleteStudent(studentId: string) {
    return this.http.delete(`https://localhost:7284/api/DeleteStudent/${studentId}`);
  }
}
