import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-posts',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './posts.component.html',
  styleUrls: ['./posts.component.css']
})
export class PostsComponent implements OnInit {
  events: any[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.fetchEvents();
  }

  fetchEvents(): void {
    this.http.get<any[]>('https://localhost:7284/api/Event').subscribe({
      next: (res) => {
        this.events = res;
      },
      error: (err) => {
        console.error('Failed to load events:', err);
      }
    });
  }
}
