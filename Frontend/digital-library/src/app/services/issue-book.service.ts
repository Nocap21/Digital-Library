import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../core/config/api.config';

export interface BookIssue {
  id: number;
  bookId: number;
  studentId: number;
  isbn: string;
  bookName: string;
  author: string;
  rollNo: string;
  studentName: string;
  studentBranch: string;
  issuedOn: string;
  returnedOn: string | null;
}

export interface IssueBookRequest {
  bookId: number;
  studentId: number;
}

@Injectable({ providedIn: 'root' })
export class IssueBookService {
  private readonly endpoint = `${API_BASE_URL}/api/bookissues`;

  constructor(private readonly http: HttpClient) {}

  getBookIssues(): Observable<BookIssue[]> {
    return this.http.get<BookIssue[]>(this.endpoint);
  }

  issueBook(request: IssueBookRequest): Observable<BookIssue> {
    return this.http.post<BookIssue>(`${this.endpoint}/issue`, request);
  }

  returnBook(id: number): Observable<BookIssue> {
    return this.http.post<BookIssue>(`${this.endpoint}/${id}/return`, {});
  }
}
