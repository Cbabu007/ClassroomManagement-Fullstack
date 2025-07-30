// src/app/login/login.component.ts

import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  standalone: true,
  selector: 'app-login-layout',
  imports: [RouterModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent { menuOpen = false;

  toggleMenu() {
    this.menuOpen = !this.menuOpen;
  }}
