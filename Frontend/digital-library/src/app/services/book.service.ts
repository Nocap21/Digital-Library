import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../core/config/api.config';

export interface Book {
  id: number;
  isbn: string;
  name: string;
  author: string;
  publication: string;
  details: string;
  quantity: number;
  price: number;
  branch: string;
}

export type BookRequest = Omit<Book, 'id'>;

@Injectable({ providedIn: 'root' })
export class BookService {
  private readonly endpoint = `${API_BASE_URL}/api/books`;

  constructor(private readonly http: HttpClient) {}

  getBooks(): Observable<Book[]> {
    return this.http.get<Book[]>(this.endpoint);
  }

  getBook(id: number): Observable<Book> {
    return this.http.get<Book>(`${this.endpoint}/${id}`);
  }

  addBook(book: BookRequest): Observable<Book> {
    return this.http.post<Book>(this.endpoint, book);
  }

  updateBook(id: number, book: BookRequest): Observable<Book> {
    return this.http.put<Book>(`${this.endpoint}/${id}`, book);
  }

  deleteBook(id: number): Observable<void> {
    return this.http.delete<void>(`${this.endpoint}/${id}`);
  }
}
