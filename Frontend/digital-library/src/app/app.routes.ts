import { Routes } from '@angular/router';
import { AddBookComponent } from './pages/add-book/add-book.component';
import { AddStudentComponent } from './pages/add-student/add-student.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { DeleteBookComponent } from './pages/delete-book/delete-book.component';
import { IssueBookComponent } from './pages/issue-book/issue-book.component';
import { LoginComponent } from './pages/login/login.component';
import { ReportComponent } from './pages/report/report.component';
import { ReturnBookComponent } from './pages/return-book/return-book.component';
import { ViewBooksComponent } from './pages/view-books/view-books.component';
import { ViewStudentsComponent } from './pages/view-students/view-students.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent, title: 'Login' },
  { path: 'dashboard', component: DashboardComponent, title: 'Dashboard', canActivate: [authGuard] },
  { path: 'books/add', component: AddBookComponent, title: 'Add Book', canActivate: [authGuard] },
  { path: 'books/view', component: ViewBooksComponent, title: 'View Books', canActivate: [authGuard] },
  { path: 'books/delete', component: DeleteBookComponent, title: 'Delete Book', canActivate: [authGuard] },
  { path: 'students/add', component: AddStudentComponent, title: 'Add Student', canActivate: [authGuard] },
  { path: 'students/view', component: ViewStudentsComponent, title: 'View Students', canActivate: [authGuard] },
  { path: 'books/issue', component: IssueBookComponent, title: 'Issue Book', canActivate: [authGuard] },
  { path: 'books/return', component: ReturnBookComponent, title: 'Return Book', canActivate: [authGuard] },
  { path: 'report', component: ReportComponent, title: 'Report', canActivate: [authGuard] },
  { path: '', pathMatch: 'full', redirectTo: 'dashboard' },
  { path: '**', redirectTo: 'dashboard' }
];
