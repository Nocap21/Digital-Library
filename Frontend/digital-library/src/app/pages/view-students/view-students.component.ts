import { Component, OnInit } from '@angular/core';
import { finalize } from 'rxjs';
import { apiErrorMessage } from '../../core/utils/api-error';
import { Student, StudentService } from '../../services/student.service';

@Component({
  selector: 'app-view-students',
  standalone: true,
  templateUrl: './view-students.component.html',
  styleUrl: './view-students.component.css'
})
export class ViewStudentsComponent implements OnInit {
  students: Student[] = [];
  isLoading = true;
  errorMessage = '';

  constructor(private readonly studentService: StudentService) {}

  ngOnInit(): void {
    this.isLoading = true;
    this.studentService.getStudents().pipe(finalize(() => this.isLoading = false)).subscribe({
      next: (students) => this.students = students,
      error: (error: unknown) => this.errorMessage = apiErrorMessage(error, 'Unable to load students.')
    });
  }
}
