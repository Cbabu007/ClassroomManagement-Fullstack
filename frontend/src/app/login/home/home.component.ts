import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  images: string[] = [
    'assets/bus.jpg',
    'assets/playground.jpg',
    'assets/lab.jpg',
    'assets/Schoolbuliding.jpg' // ✅ correct spelling here
  ];

  currentImage = this.images[0];
  currentIndex = 0;

  ngOnInit(): void {
    setInterval(() => {
      this.currentIndex = (this.currentIndex + 1) % this.images.length;
      this.currentImage = this.images[this.currentIndex];
    }, 3000);
  }
}
