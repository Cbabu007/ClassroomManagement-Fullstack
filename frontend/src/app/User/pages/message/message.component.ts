import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';  // ✅ Import CommonModule
import { FormsModule } from '@angular/forms';     // (Optional if needed for [(ngModel)])
import { HttpClientModule } from '@angular/common/http';
import { MessageService } from './message.service';

@Component({
  selector: 'app-message',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule], // ✅ Include CommonModule here
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css'],
  providers: [MessageService]
})
export class MessageComponent implements OnInit {
  messages: any[] = [];

  constructor(private messageService: MessageService) {}

  ngOnInit(): void {
    const email = sessionStorage.getItem('email');
    const mobile = sessionStorage.getItem('mobile');

    if (email && mobile) {
      this.messageService.getMessagesByLogin(email, mobile).subscribe({
        next: (data) => {
          this.messages = data;
        },
        error: (err) => {
          console.error('Failed to load messages:', err);
        }
      });
    }
  }
}
