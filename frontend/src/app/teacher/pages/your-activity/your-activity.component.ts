import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';

@Component({
  selector: 'app-your-activity',
  standalone: true,
  imports: [CommonModule, HttpClientModule, FormsModule],
  templateUrl: './your-activity.component.html',
  styleUrls: ['./your-activity.component.css']
})
export class YourActivityComponent implements OnInit {
  activityList: any[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.fetchTeacherActivity();
  }

  fetchTeacherActivity(): void {
    this.http.get<any[]>('https://localhost:7284/api/TeacherActivity/GetAll')
      .subscribe({
        next: (data: any[]) => this.activityList = data,
        error: (err: any) => console.error('Error fetching teacher activity:', err)
      });
  }

  downloadPDF(): void {
    const doc = new jsPDF();
    doc.text('Teacher Login Activity Report', 14, 15);

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

    doc.save('Teacher_Login_Activity.pdf');
  }
}
