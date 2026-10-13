import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../core/config/api.config';

export interface IssuedBookSummary {
  id: number;
  bookId: number;
  studentId: number;
  isbn: string;
  bookName: string;
  author: string;
  rollNo: string;
  studentName: string;
  issuedOn: string;
}

export interface DashboardSummary {
  totalBooks: number;
  totalStudents: number;
  currentlyIssuedBooks: number;
  availableCopies: number;
  issuedBooks: IssuedBookSummary[];
}

@Injectable({ providedIn: 'root' })
export class DashboardService {
  constructor(private readonly http: HttpClient) {}

  getDashboard(): Observable<DashboardSummary> {
    return this.http.get<DashboardSummary>(`${API_BASE_URL}/api/dashboard`);
  }
}
