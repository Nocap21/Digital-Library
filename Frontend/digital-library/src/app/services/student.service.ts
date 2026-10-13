import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../core/config/api.config';

export interface Student {
  id: number;
  rollNo: string;
  name: string;
  branch: string;
}

export type StudentRequest = Omit<Student, 'id'>;

@Injectable({ providedIn: 'root' })
export class StudentService {
  private readonly endpoint = `${API_BASE_URL}/api/students`;

  constructor(private readonly http: HttpClient) {}

  getStudents(): Observable<Student[]> {
    return this.http.get<Student[]>(this.endpoint);
  }

  getStudent(id: number): Observable<Student> {
    return this.http.get<Student>(`${this.endpoint}/${id}`);
  }

  addStudent(student: StudentRequest): Observable<Student> {
    return this.http.post<Student>(this.endpoint, student);
  }

  updateStudent(id: number, student: StudentRequest): Observable<Student> {
    return this.http.put<Student>(`${this.endpoint}/${id}`, student);
  }
}
