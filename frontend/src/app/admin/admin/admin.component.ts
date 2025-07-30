import { Component, OnInit } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  standalone: true,
  selector: 'app-admin-layout',
  imports: [RouterModule, CommonModule, HttpClientModule],
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  dropdownOpen = false;
  menuOpen = false;
  fullName = 'Admin';
  photoPath = 'https://cdn-icons-png.flaticon.com/512/3177/3177440.png'; // Default admin avatar

  constructor(private router: Router, private http: HttpClient) {}

  ngOnInit(): void {
    // Load stored values from sessionStorage
    const storedName = sessionStorage.getItem('fullName');
    const storedPhoto = sessionStorage.getItem('photoPath');

    if (storedName) this.fullName = storedName;
    if (storedPhoto) this.photoPath = storedPhoto;

    // Try to update with fresh profile from backend
    const email = sessionStorage.getItem('email');
    const mobile = sessionStorage.getItem('mobile');

    if (email && mobile) {
      this.http.get<any>(`https://localhost:7284/api/UserProfile/GetByLogin?email=${email}&mobile=${mobile}`)
        .subscribe({
          next: (res) => {
            this.fullName = res.firstName + ' ' + res.lastName;

            const rawPath = res.photoPath?.startsWith('/') ? res.photoPath : '/' + res.photoPath;
            this.photoPath = 'https://localhost:7284' + rawPath;

            sessionStorage.setItem('fullName', this.fullName);
            sessionStorage.setItem('photoPath', this.photoPath);
          },
          error: (err) => {
            console.error('❌ Failed to load admin profile', err);
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
