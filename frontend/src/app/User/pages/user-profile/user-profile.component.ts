import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  profile: any = {};
  errorMessage = '';
  photoPreviewUrl: string = ''; // ✅ Add this line to fix the error
  imgBaseUrl = 'https://localhost:7284/';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    const email = sessionStorage.getItem('email');
    const mobile = sessionStorage.getItem('mobile');

    if (!email || !mobile) {
      this.errorMessage = 'Login required to view profile.';
      return;
    }

    this.http.get<any>(`https://localhost:7284/api/UserProfile/GetByLogin?email=${email}&mobile=${mobile}`)
      .subscribe({
        next: (res) => {
          // ✅ Replace any backslashes in photoPath
        if (res.photoPath) {
  const cleanPath = res.photoPath.replace(/\\/g, '/');
  this.photoPreviewUrl = cleanPath.startsWith('http')
    ? cleanPath
    : 'https://localhost:7284/' + cleanPath;
}
       this.profile = res;
        },
        error: (err) => {
          console.error('❌ Error fetching profile:', err);
          this.errorMessage = 'Failed to load profile. Please try again.';
        }
      });
  }
}
