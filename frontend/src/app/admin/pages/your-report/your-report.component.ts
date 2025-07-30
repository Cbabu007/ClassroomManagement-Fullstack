import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-your-report',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './your-report.component.html',
  styleUrls: ['./your-report.component.css']
})
export class YourReportComponent {
  contact: any = {
    schoolName: '',
    address: '',
    officePhone: '',
    mobile: '',
    email: '',
    website: '',
    workingHours: '',
    facebook: '',
    instagram: '',
    twitter: ''
  };

  constructor(private http: HttpClient) {}

  submitContact() {
    this.http.post('https://localhost:7284/api/Contact/Insert', this.contact).subscribe({
      next: () => {
        alert("✅ Contact details saved to Event table!");
        this.contact = {}; // Clear form
      },
      error: (err) => {
        console.error("❌ Error saving contact:", err);
        alert("Something went wrong!");
      }
    });
  }
}
