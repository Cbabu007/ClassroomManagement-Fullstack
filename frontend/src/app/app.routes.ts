// src/app/app.routes.ts

import { Routes } from '@angular/router';


export const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('./login/login.routes').then(m => m.loginRoutes)
  }, // ✅ COMMA NEEDED HERE

  {
    path: 'admin',
    loadChildren: () => import('./admin/admin/admin.routes').then(m => m.adminRoutes)
  },
  
  {
    path: 'teacher',
    loadChildren: () => import('./teacher/teacher/teacher.routes').then(m => m.teacherRoutes)
  },
  {
  path: 'user',
  loadChildren: () => import('./User/user.routes').then(m => m.UserRoutes)
}

];
