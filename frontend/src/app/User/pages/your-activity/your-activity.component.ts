import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';

@Component({
  selector: 'app-your-activity',
  standalone: true, // ✅ Add this
  imports: [CommonModule, HttpClientModule, FormsModule], // ✅ Required for standalone
  templateUrl: './your-activity.component.html',
  styleUrls: ['./your-activity.component.css']
})
export class YourActivityStudentComponent implements OnInit {
  activityList: any[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.fetchStudentActivity();
  }

  fetchStudentActivity(): void {
    this.http.get<any[]>('https://localhost:7284/api/LoginActivity/GetAll')
      .subscribe({
        next: (data: any[]) => this.activityList = data,
        error: (err: any) => console.error('Error fetching student activity:', err)
      });
  }

  downloadPDF(): void {
    const doc = new jsPDF();
    doc.text('Student Login Activity Report', 14, 15);

    autoTable(doc, {
      startY: 20,
      head: [['S.No', 'Browse', 'Day', 'Month', 'Year', 'Login Time', 'Logout Time']],
      body: this.activityList.map((item, index) => [
        index + 1,
        item.browse,
        item.day,
        item.month,
        item.year,
        item.loginTime,
        item.logoutTime
      ])
    });

    doc.save('Student_Login_Activity.pdf');
  }
}
