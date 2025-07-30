import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  standalone: true,
  selector: 'app-user',
  imports: [CommonModule, RouterModule, HttpClientModule],
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  dropdownOpen = false;
  menuOpen = false;
  fullName = 'Student';
  profilePhoto = 'https://cdn-icons-png.flaticon.com/512/3177/3177440.png'; // Default

  constructor(private router: Router, private http: HttpClient) {}

  ngOnInit(): void {
    const storedName = sessionStorage.getItem('fullName');
    const storedPhoto = sessionStorage.getItem('photoPath');

    if (storedName) this.fullName = storedName;
    if (storedPhoto) this.profilePhoto = storedPhoto;

    const email = sessionStorage.getItem('email');
    const mobile = sessionStorage.getItem('mobile');

    if (email && mobile) {
      this.http.get<any>(`https://localhost:7284/api/UserProfile/GetByLogin?email=${email}&mobile=${mobile}`)
        .subscribe({
          next: (res) => {
            this.fullName = res.firstName + ' ' + res.lastName;
           const rawPath = res.photoPath?.startsWith('/') ? res.photoPath : '/' + res.photoPath;
this.profilePhoto = 'https://localhost:7284' + rawPath;
            sessionStorage.setItem('fullName', this.fullName);
            sessionStorage.setItem('photoPath', this.profilePhoto);
          },
          error: (err) => {
            console.error('❌ Failed to load profile name/photo', err);
          }
        });
    }
  }

  toggleDropdown() {
    this.dropdownOpen = !this.dropdownOpen;
  }

  toggleMenu() {
    this.menuOpen = !this.menuOpen;
  }

  logout() {
    sessionStorage.clear();
    this.router.navigate(['/login']);
  }
}
