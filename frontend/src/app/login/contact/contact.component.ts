import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common'; // ✅ Add this

@Component({
  selector: 'app-contact',
  standalone: true,
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css'],
  imports: [CommonModule] // ✅ Fix here
})
export class ContactComponent implements OnInit {
  contact: any = null;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get<any[]>('https://localhost:7284/api/Contact/GetAll').subscribe({
      next: (res) => {
        if (res.length > 0) {
          this.contact = res[res.length - 1]; // ✅ latest
        }
      },
      error: (err) => {
        console.error('❌ Error loading contact:', err);
      }
    });
  }
}
