import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-login-page',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css']
})
export class LoginPageComponent {
  username = '';
  password = '';

  constructor(private http: HttpClient) {}

  onLogin() {
    const loginData = {
      username: this.username,
      password: this.password
    };

    this.http.post<any>('https://localhost:7284/api/Login/ValidateLogin', loginData).subscribe({
      next: (res) => {
        if (res.status === 'Success') {
          // ✅ Save user details to sessionStorage
          sessionStorage.setItem('username', this.username);
          sessionStorage.setItem('email', this.username);
          sessionStorage.setItem('mobile', this.password);

          sessionStorage.setItem('firstName', res.firstName);
          sessionStorage.setItem('lastName', res.lastName);
          sessionStorage.setItem('fullName', res.firstName + ' ' + res.lastName);

          // ✅ Build full profile photo URL
          const rawPath = res.photoPath?.startsWith('/') ? res.photoPath : '/' + res.photoPath;
          const fullPhotoUrl = 'https://localhost:7284' + rawPath;
          sessionStorage.setItem('photoPath', fullPhotoUrl);

          // ✅ Optional: Save role
          sessionStorage.setItem('role', res.role);

          const fullName = res.firstName + ' ' + res.lastName;

if (res.role === 'Admin') {
  this.insertAdminActivity(res.staffId, fullName);
} else if (res.role === 'Staff' || res.role === 'Teacher') {
  this.insertTeacherActivity(res.staffId, fullName);
} else if (res.role === 'Student') {
  this.insertStudentActivity(res.studentId, fullName);
}

          // ✅ Redirect based on role
          window.location.href = res.redirectUrl;
        } else {
          alert('Login failed. Please check your credentials.');
        }
      },
      error: (err) => {
        console.error('Login error', err);
        alert('Server error. Please try again later.');
      }
    });
    
  }
  insertStudentActivity(studentId: string, studentName: string) {
  const now = new Date();
  const data = {
    studentId,
    studentName,
    browse: "Chrome",
    day: now.getDate(),
    month: now.toLocaleString('default', { month: 'long' }),
    year: now.getFullYear(),
    loginTime: now.toTimeString().split(' ')[0],
    logoutTime: "00:00:00"
  };

  this.http.post('https://localhost:7284/api/LoginActivity/Insert', data).subscribe();
}
insertTeacherActivity(staffId: string, staffName: string) {
  const now = new Date();
  const data = {
    staffId,
    staffName,
    browse: "Chrome",
    day: now.getDate(),
    month: now.toLocaleString('default', { month: 'long' }),
    year: now.getFullYear(),
    loginTime: now.toTimeString().split(' ')[0],
    logoutTime: "00:00:00"
  };

  this.http.post('https://localhost:7284/api/TeacherActivity/Insert', data).subscribe();
}

insertAdminActivity(staffId: string, staffName: string): void {
  const now = new Date();

  // ✅ Detect browser from userAgent
  const userAgent = navigator.userAgent;
  let browser = 'Unknown';

  if (userAgent.includes('Chrome') && !userAgent.includes('Edg')) {
    browser = 'Chrome';
  } else if (userAgent.includes('Firefox')) {
    browser = 'Firefox';
  } else if (userAgent.includes('Safari') && !userAgent.includes('Chrome')) {
    browser = 'Safari';
  } else if (userAgent.includes('Edg')) {
    browser = 'Edge';
  } else if (userAgent.includes('Opera') || userAgent.includes('OPR')) {
    browser = 'Opera';
  }

  const payload = {
    staffId: staffId,
    staffName: staffName,
    browse: browser,  // ✅ now dynamic
    day: now.getDate(),
    month: now.toLocaleString('default', { month: 'long' }),
    year: now.getFullYear(),
    loginTime: now.toTimeString().split(' ')[0],
    logoutTime: "00:00:00"
  };

  this.http.post('https://localhost:7284/api/AdminActivity/Insert', payload)
    .subscribe({
      next: () => console.log("✅ Admin activity inserted"),
      error: (err) => console.error("❌ Admin insert failed", err)
    });
}

}
