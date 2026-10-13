import { Component } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { finalize } from 'rxjs';
import { apiErrorMessage } from '../../core/utils/api-error';
import { BookRequest, BookService } from '../../services/book.service';

@Component({
  selector: 'app-add-book',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './add-book.component.html',
  styleUrl: './add-book.component.css'
})
export class AddBookComponent {
  book: Omit<BookRequest, 'quantity' | 'price'> & { quantity: number | null; price: number | null } = this.createEmptyBook();
  successMessage = '';
  errorMessage = '';
  isSubmitting = false;

  constructor(private readonly bookService: BookService) {}

  submitBook(form: NgForm): void {
    this.successMessage = '';
    this.errorMessage = '';
    if (form.invalid || this.isSubmitting) {
      form.control.markAllAsTouched();
      return;
    }

    const request: BookRequest = {
      ...this.book,
      quantity: Number(this.book.quantity),
      price: Number(this.book.price)
    };
    this.isSubmitting = true;
    this.bookService.addBook(request)
      .pipe(finalize(() => this.isSubmitting = false))
      .subscribe({
        next: () => {
          this.successMessage = 'Book added successfully.';
          form.resetForm();
          this.book = this.createEmptyBook();
        },
        error: (error: unknown) => this.errorMessage = apiErrorMessage(error, 'Unable to add the book.')
      });
  }

  private createEmptyBook(): Omit<BookRequest, 'quantity' | 'price'> & { quantity: number | null; price: number | null } {
    return {
      isbn: '',
      name: '',
      author: '',
      publication: '',
      details: '',
      quantity: null,
      price: null,
      branch: ''
    };
  }
}
