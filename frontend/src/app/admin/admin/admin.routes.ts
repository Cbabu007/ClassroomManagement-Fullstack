import { Routes } from '@angular/router';
import { AdminComponent } from './admin.component';

export const adminRoutes: Routes = [
  {
    path: '',
    component: AdminComponent,
    children: [
      { path: 'staff', loadComponent: () => import('../pages/staff/staff.component').then(m => m.StaffComponent) },
      { path: 'student', loadComponent: () => import('../pages/student/student.component').then(m => m.StudentComponent) },
      { path: 'classroom', loadComponent: () => import('../pages/classroom/classroom.component').then(m => m.ClassroomComponent) },
      { path: 'event', loadComponent: () => import('../pages/event/event.component').then(m => m.EventComponent) },
      { path: 'report', loadComponent: () => import('../pages/report/report.component').then(m => m.ReportComponent) },

      // ✅ New routes for YourProfile, YourActivity, YourReport
      { path: 'your-profile', loadComponent: () => import('../pages/your-profile/your-profile.component').then(m => m.YourProfileComponent) },
      { path: 'your-activity', loadComponent: () => import('../pages/your-activity/your-activity.component').then(m => m.YourActivityComponent) },
      { path: 'your-report', loadComponent: () => import('../pages/your-report/your-report.component').then(m => m.YourReportComponent) },

      // Default redirect
      { path: '', redirectTo: 'staff', pathMatch: 'full' }
    ]
  }
];
