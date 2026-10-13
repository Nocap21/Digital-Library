import { Component, OnInit } from '@angular/core';
import { finalize } from 'rxjs';
import { apiErrorMessage } from '../../core/utils/api-error';
import { BookIssueReport, ReportService } from '../../services/report.service';

@Component({
  selector: 'app-report',
  standalone: true,
  templateUrl: './report.component.html',
  styleUrl: './report.component.css'
})
export class ReportComponent implements OnInit {
  issues: BookIssueReport[] = [];
  isLoading = true;
  errorMessage = '';

  constructor(private readonly reportService: ReportService) {}

  ngOnInit(): void {
    this.reportService.getBookIssueReport().pipe(finalize(() => this.isLoading = false)).subscribe({
      next: (issues) => this.issues = issues,
      error: (error: unknown) => this.errorMessage = apiErrorMessage(error, 'Unable to load the report.')
    });
  }
}
