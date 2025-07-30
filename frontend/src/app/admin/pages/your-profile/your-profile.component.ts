import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule, DatePipe } from '@angular/common';

@Component({
  selector: 'app-your-profile',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './your-profile.component.html',
  styleUrls: ['./your-profile.component.css']
})
export class YourProfileComponent implements OnInit {
  staff: any = {};
  photoUrl: string = '';
  fullName: string = '';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    const username = sessionStorage.getItem('username');
    if (!username) {
      alert('❌ Username not found in session.');
      return;
    }

    this.http.get<any>('https://localhost:7284/api/AdminProfile/GetByUsername?username=' + username)
      .subscribe({
        next: (res) => {
          this.staff = res;
          this.fullName = res.firstName + ' ' + res.lastName;
          this.photoUrl = 'https://localhost:7284/' + res.photoPath;
        },
        error: () => alert('❌ Failed to load staff profile.')
      });
  }
}
