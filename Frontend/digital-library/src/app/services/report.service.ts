import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../core/config/api.config';
import { BookIssue } from './issue-book.service';

export interface BookIssueReport extends BookIssue {}

export interface ReportFilters {
  bookId?: number;
  studentId?: number;
  fromDate?: string;
  toDate?: string;
  activeOnly?: boolean;
}

@Injectable({ providedIn: 'root' })
export class ReportService {
  constructor(private readonly http: HttpClient) {}

  getBookIssueReport(filters: ReportFilters = {}): Observable<BookIssueReport[]> {
    let params = new HttpParams();
    if (filters.bookId !== undefined) params = params.set('bookId', filters.bookId);
    if (filters.studentId !== undefined) params = params.set('studentId', filters.studentId);
    if (filters.fromDate) params = params.set('fromDate', filters.fromDate);
    if (filters.toDate) params = params.set('toDate', filters.toDate);
    if (filters.activeOnly !== undefined) params = params.set('activeOnly', filters.activeOnly);

    return this.http.get<BookIssueReport[]>(`${API_BASE_URL}/api/reports/book-issues`, { params });
  }
}
