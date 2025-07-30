// src/app/login/login.routes.ts
import { Routes } from '@angular/router';
import { LoginComponent } from './login.component';
import { HomeComponent } from './home/home.component';
import { LoginPageComponent } from './login-page/login-page.component';
import { PostsComponent } from './posts/posts.component';
import { ContactComponent } from './contact/contact.component';

export const loginRoutes: Routes = [
  {
    path: '',
    component: LoginComponent,
    children: [
      { path: 'home', component: HomeComponent },
      { path: 'login', component: LoginPageComponent },
      { path: 'posts', component: PostsComponent },
      { path: 'contact', component: ContactComponent },
      { path: '', redirectTo: 'home', pathMatch: 'full' }
    ]
  }
];
