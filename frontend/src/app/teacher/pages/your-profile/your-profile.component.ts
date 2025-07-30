import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common'; // ✅ Import CommonModule for date pipe

@Component({
  selector: 'app-your-profile',
  standalone: true,
  imports: [CommonModule, HttpClientModule], // ✅ Add CommonModule here
  templateUrl: './your-profile.component.html',
  styleUrls: ['./your-profile.component.css']
})
export class YourProfileComponent implements OnInit {
  staff: any = {};

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    const username = sessionStorage.getItem('username');
    if (!username) {
      alert('❌ Username not found in session.');
      return;
    }

    this.http.get<any>('https://localhost:7284/api/StaffProfile/GetByUsername?username=' + username)
      .subscribe({
        next: (res) => {
          const baseUrl = 'https://localhost:7284/';
          if (res.photoPath && !res.photoPath.startsWith('http')) {
            res.photoPath = baseUrl + res.photoPath;
          }
          this.staff = res;
        },
        error: (err) => {
          console.error('❌ Failed to load staff profile', err);
          alert('❌ Failed to load staff profile.');
        }
      });
  }
}
