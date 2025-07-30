import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-your-report',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './your-report.component.html',
  styleUrls: ['./your-report.component.css']
})
export class YourReportComponent implements OnInit {
  constructor() {}

  ngOnInit(): void {
    // Future logic for report loading
  }
}
