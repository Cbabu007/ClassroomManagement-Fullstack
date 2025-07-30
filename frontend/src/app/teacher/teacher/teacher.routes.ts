import { Routes } from '@angular/router';
import { TeacherComponent } from './teacher.component';

export const teacherRoutes: Routes = [
  {
    path: '',
    component: TeacherComponent,
    children: [
      {
        path: 'attendance',
        loadComponent: () =>
          import('../pages/attendance/attendance.component').then(m => m.AttendanceComponent)
      },
      {
        path: 'class',
        loadComponent: () =>
          import('../pages/class/class.component').then(m => m.ClassComponent)
      },
      {
        path: 'homework',
        loadComponent: () =>
          import('../pages/homework/homework.component').then(m => m.HomeworkComponent)
      },
     
      {
        path: 'test',
        loadComponent: () =>
          import('../pages/test/test.component').then(m => m.TestComponent)
      },
      {
        path: 'message',
        loadComponent: () =>
          import('../pages/message/message.component').then(m => m.MessageComponent)
      },

      // ✅ Added teacher personal pages
      {
        path: 'your-profile',
        loadComponent: () =>
          import('../pages/your-profile/your-profile.component').then(m => m.YourProfileComponent)
      },
    {
  path: 'your-activity',
  loadComponent: () =>
    import('../pages/your-activity/your-activity.component').then(m => m.YourActivityComponent)
},

      {
        path: 'your-report',
        loadComponent: () =>
          import('../pages/your-report/your-report.component').then(m => m.YourReportComponent)
      },

      // Default route
      {
        path: '',
        redirectTo: 'attendance',
        pathMatch: 'full'
      }
    ]
  }
];
