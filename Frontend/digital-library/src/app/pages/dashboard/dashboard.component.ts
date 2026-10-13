import { Component, OnInit } from '@angular/core';
import { finalize } from 'rxjs';
import { apiErrorMessage } from '../../core/utils/api-error';
import { DashboardService, DashboardSummary, IssuedBookSummary } from '../../services/dashboard.service';

interface PersonStatistic {
  rollNo: string;
  name: string;
  bookCount: number;
  percentage: number;
  color: string;
  piePath: string;
  labelX: number;
  labelY: number;
}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  private readonly chartColors = ['#4472C4', '#ED7D31', '#A5A5A5', '#FFC000', '#5B9BD5'];
  dashboard: DashboardSummary | null = null;
  isLoading = true;
  errorMessage = '';

  constructor(private readonly dashboardService: DashboardService) {}

  ngOnInit(): void {
    this.dashboardService.getDashboard().pipe(finalize(() => this.isLoading = false)).subscribe({
      next: (dashboard) => this.dashboard = dashboard,
      error: (error: unknown) => this.errorMessage = apiErrorMessage(error, 'Unable to load the dashboard.')
    });
  }

  get issuedBooks(): IssuedBookSummary[] {
    return this.dashboard?.issuedBooks ?? [];
  }

  get peopleStatistics(): PersonStatistic[] {
    const students = new Map<string, { rollNo: string; name: string; bookCount: number }>();

    for (const issuedBook of this.issuedBooks) {
      const student = students.get(issuedBook.rollNo);
      if (student) {
        student.bookCount += 1;
      } else {
        students.set(issuedBook.rollNo, {
          rollNo: issuedBook.rollNo,
          name: issuedBook.studentName,
          bookCount: 1
        });
      }
    }

    const totalIssuedBooks = this.issuedBooks.length;
    let startAngle = -90;

    return Array.from(students.values()).map((student, index) => {
      const share = student.bookCount / totalIssuedBooks;
      const endAngle = startAngle + share * 360;
      const middleAngle = startAngle + share * 180;
      const startPoint = this.piePoint(startAngle, 70);
      const endPoint = this.piePoint(endAngle, 70);
      const labelPoint = this.piePoint(middleAngle, 49);
      const largeArcFlag = share > 0.5 ? 1 : 0;
      const piePath = share === 1
        ? 'M 100 30 A 70 70 0 1 1 100 170 A 70 70 0 1 1 100 30 Z'
        : 'M 100 100 L ' + startPoint.x + ' ' + startPoint.y +
          ' A 70 70 0 ' + largeArcFlag + ' 1 ' + endPoint.x + ' ' + endPoint.y + ' Z';

      startAngle = endAngle;
      return {
        ...student,
        percentage: Math.round(share * 100),
        color: this.chartColors[index % this.chartColors.length],
        piePath,
        labelX: labelPoint.x,
        labelY: labelPoint.y
      };
    });
  }

  get maximumBookCount(): number {
    return Math.max(1, ...this.peopleStatistics.map((person) => person.bookCount));
  }

  private piePoint(angle: number, radius: number): { x: number; y: number } {
    const radians = (angle * Math.PI) / 180;
    return {
      x: Number((100 + radius * Math.cos(radians)).toFixed(1)),
      y: Number((100 + radius * Math.sin(radians)).toFixed(1))
    };
  }
}
