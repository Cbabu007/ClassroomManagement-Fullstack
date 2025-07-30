import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { MessageService } from './message.service';

@Component({
  selector: 'app-message',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css'],
  providers: [MessageService]
})
export class MessageComponent implements OnInit {
  grade = '';
  section = '';
  date = new Date().toISOString().substring(0, 10);  // yyyy-MM-dd
  topic = '';
  staffName = '';
  message = '';
  messages: any[] = [];

  constructor(private messageService: MessageService) {}

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages() {
    this.messageService.getMessages().subscribe(data => {
      this.messages = data;
    });
  }

  addMessage() {
    if (!this.message.trim()) return;

    const newMessage = {
      grade: this.grade,
      section: this.section,
      date: this.date,
      topic: this.topic,
      staffName: this.staffName,
      message: this.message
    };

    this.messageService.addMessage(newMessage).subscribe(() => {
      this.resetForm();
      this.loadMessages();
    });
  }

  deleteMessage(id: number) {
    this.messageService.deleteMessage(id).subscribe(() => {
      this.loadMessages();
    });
  }

  resetForm() {
    this.grade = '';
    this.section = '';
    this.date = new Date().toISOString().substring(0, 10);
    this.topic = '';
    this.staffName = '';
    this.message = '';
  }
}
