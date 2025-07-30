import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { EventService } from './event.service';

@Component({
  selector: 'app-event',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './event.component.html',
  styleUrls: ['./event.component.css']
})
export class EventComponent implements OnInit {
  newMessage: string = '';
  tempEvents: { message: string, date: string }[] = [];
  savedEvents: any[] = [];

  days = Array.from({ length: 31 }, (_, i) => i + 1);
  months = [
    'January', 'February', 'March', 'April', 'May', 'June',
    'July', 'August', 'September', 'October', 'November', 'December'
  ];
  years = Array.from({ length: new Date().getFullYear() - 2023 + 1 }, (_, i) => 2024 + i);

  selectedDay: number = new Date().getDate();
  selectedMonth: string = this.months[new Date().getMonth()];
  selectedYear: number = new Date().getFullYear();

  constructor(private eventService: EventService) {}

  ngOnInit(): void {
    this.loadSavedEvents();
  }

  loadSavedEvents() {
    this.eventService.getEvents().subscribe(res => {
      this.savedEvents = res;
    });
  }

  addToTable() {
    if (!this.newMessage.trim()) return;

    const monthIndex = this.months.indexOf(this.selectedMonth) + 1;
    const formattedDate = `${this.selectedYear}-${String(monthIndex).padStart(2, '0')}-${String(this.selectedDay).padStart(2, '0')}`;

    this.tempEvents.push({
      message: this.newMessage,
      date: formattedDate
    });

    this.newMessage = '';
  }

  saveToDatabase(index: number) {
    const msg = this.tempEvents[index];
    console.log("Sending to API: ", { message: msg.message });
    this.eventService.addEvent({
      message: msg.message,
      date: msg.date  // "2025-05-14" format
    })
    .subscribe({
      next: () => {
        this.tempEvents.splice(index, 1);
        this.loadSavedEvents();
      },
      error: (err) => {
        console.error(err);
        alert("❌ Failed to save to database");
      }
    });
  }

  deleteFromDatabase(id: number) {
    this.eventService.deleteEvent(id).subscribe(() => {
      this.loadSavedEvents();
    });
  }
}
