import { Component, OnInit } from '@angular/core';
import { finalize } from 'rxjs';
import { BookService, Book } from '../../services/book.service';
import { apiErrorMessage } from '../../core/utils/api-error';

@Component({
  selector: 'app-view-books',
  standalone: true,
  templateUrl: './view-books.component.html',
  styleUrl: './view-books.component.css'
})
export class ViewBooksComponent implements OnInit {
  books: Book[] = [];
  isLoading = true;
  errorMessage = '';

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
}
