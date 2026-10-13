import { Component, OnInit } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { finalize, forkJoin } from 'rxjs';
import { apiErrorMessage } from '../../core/utils/api-error';
import { Book, BookService } from '../../services/book.service';
import { IssueBookService } from '../../services/issue-book.service';
import { Student, StudentService } from '../../services/student.service';

@Component({
  selector: 'app-issue-book',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './issue-book.component.html',
  styleUrl: './issue-book.component.css'
})
export class IssueBookComponent implements OnInit {
  books: Book[] = [];
  students: Student[] = [];
  selectedBookId: number | null = null;
  selectedStudentId: number | null = null;
  successMessage = '';
  errorMessage = '';
  isLoading = true;
  isSubmitting = false;

  constructor(
    private readonly bookService: BookService,
    private readonly studentService: StudentService,
    private readonly issueBookService: IssueBookService
  ) {}

  ngOnInit(): void {
    this.loadOptions();
  }

  get availableBooks(): Book[] {
    return this.books.filter((book) => book.quantity > 0);
  }

  loadOptions(): void {
    this.isLoading = true;
    this.errorMessage = '';
    forkJoin({ books: this.bookService.getBooks(), students: this.studentService.getStudents() })
      .pipe(finalize(() => this.isLoading = false))
      .subscribe({
        next: ({ books, students }) => {
          this.books = books;
          this.students = students;
        },
        error: (error: unknown) => this.errorMessage = apiErrorMessage(error, 'Unable to load books and students.')
      });
  }

  submitIssue(form: NgForm): void {
    this.successMessage = '';
    this.errorMessage = '';
    if (form.invalid || this.isSubmitting || this.selectedBookId === null || this.selectedStudentId === null) {
      form.control.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    this.issueBookService.issueBook({ bookId: this.selectedBookId, studentId: this.selectedStudentId })
      .pipe(finalize(() => this.isSubmitting = false))
      .subscribe({
        next: () => {
          this.successMessage = 'Book issued successfully.';
          form.resetForm();
          this.selectedBookId = null;
          this.selectedStudentId = null;
          this.loadOptions();
        },
        error: (error: unknown) => this.errorMessage = apiErrorMessage(error, 'Unable to issue the book.')
      });
  }
}
