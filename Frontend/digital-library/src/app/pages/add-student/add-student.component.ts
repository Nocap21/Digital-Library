import { Component } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { finalize } from 'rxjs';
import { apiErrorMessage } from '../../core/utils/api-error';
import { StudentRequest, StudentService } from '../../services/student.service';

@Component({
  selector: 'app-add-student',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './add-student.component.html',
  styleUrl: './add-student.component.css'
})
export class AddStudentComponent {
  student: StudentRequest = this.createEmptyStudent();
  readonly branches = ['IT', 'MECH', 'CIVIL'];
  successMessage = '';
  errorMessage = '';
  isSubmitting = false;

  constructor(private readonly studentService: StudentService) {}

  submitStudent(form: NgForm): void {
    this.successMessage = '';
    this.errorMessage = '';
    if (form.invalid || this.isSubmitting) {
      form.control.markAllAsTouched();
      return;
    }

    const request: StudentRequest = {
      rollNo: this.student.rollNo.trim(),
      name: this.student.name.trim(),
      branch: this.student.branch.trim()
    };
    this.isSubmitting = true;
    this.studentService.addStudent(request)
      .pipe(finalize(() => this.isSubmitting = false))
      .subscribe({
        next: () => {
          this.successMessage = 'Student added successfully.';
          form.resetForm();
          this.student = this.createEmptyStudent();
        },
        error: (error: unknown) => this.errorMessage = apiErrorMessage(error, 'Unable to add the student.')
      });
  }

  private createEmptyStudent(): StudentRequest {
    return { rollNo: '', name: '', branch: '' };
  }
}
