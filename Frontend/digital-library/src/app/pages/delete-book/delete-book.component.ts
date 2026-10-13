import { Component, OnInit } from '@angular/core';
import { finalize } from 'rxjs';
import { apiErrorMessage } from '../../core/utils/api-error';
import { Book, BookService } from '../../services/book.service';

@Component({
  selector: 'app-delete-book',
  standalone: true,
  templateUrl: './delete-book.component.html',
  styleUrl: './delete-book.component.css'
})
export class DeleteBookComponent implements OnInit {
  books: Book[] = [];
  isLoading = true;
  deletingId: number | null = null;
  errorMessage = '';
  successMessage = '';

  constructor(private readonly bookService: BookService) {}

  ngOnInit(): void {
    this.loadBooks();
  }

  loadBooks(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.bookService.getBooks().pipe(finalize(() => this.isLoading = false)).subscribe({
      next: (books) => this.books = books,
      error: (error: unknown) => this.errorMessage = apiErrorMessage(error, 'Unable to load books.')
    });
  }

  deleteBook(book: Book): void {
    if (this.deletingId !== null) return;
    this.errorMessage = '';
    this.successMessage = '';
    this.deletingId = book.id;
    this.bookService.deleteBook(book.id).pipe(finalize(() => this.deletingId = null)).subscribe({
      next: () => {
        this.books = this.books.filter((item) => item.id !== book.id);
        this.successMessage = 'Book deleted successfully.';
      },
      error: (error: unknown) => this.errorMessage = apiErrorMessage(error, 'Unable to delete the book.')
    });
  }
}
