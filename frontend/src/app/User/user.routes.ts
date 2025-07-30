import { Routes } from '@angular/router';
import { UserComponent } from './user.component';
import { UserProfileComponent } from './pages/user-profile/user-profile.component';

export const UserRoutes: Routes = [
  {
    path: '', // ✅ FIXED: path is now '', not 'user'
    component: UserComponent,
    children: [
      {
        path: 'home',
        loadComponent: () => import('./pages/home/home.component').then(m => m.HomeComponent)
      },
      {
        path: 'homework',
        loadComponent: () => import('./pages/homework/homework.component').then(m => m.HomeworkComponent)
      },
      {
        path: 'message',
        loadComponent: () => import('./pages/message/message.component').then(m => m.MessageComponent)
      },
      {
        path: 'tests',
        loadComponent: () => import('./pages/tests/tests.component').then(m => m.TestsComponent)
      },
      {
        path: 'attendance',
        loadComponent: () => import('./pages/attendance/attendance.component').then(m => m.AttendanceComponent)
      },
      {
        path: 'profile',
        component: UserProfileComponent
      },
    {
  path: 'login-activity',
  loadComponent: () =>
    import('./pages/your-activity/your-activity.component').then(m => m.YourActivityStudentComponent)
},
{
  path: 'your-report',
  loadComponent: () =>
    import('./pages/your-report/your-report.component').then(m => m.YourReportComponent)
},
{
        path: 'report', 
        redirectTo: 'your-report',
        pathMatch: 'full'
      },
      {
        path: '',
        redirectTo: 'home',
        pathMatch: 'full'
      },
    ]
  }
];
