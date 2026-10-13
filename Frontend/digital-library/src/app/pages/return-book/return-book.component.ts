import { Component, OnInit } from '@angular/core';
import { finalize } from 'rxjs';
import { apiErrorMessage } from '../../core/utils/api-error';
import { BookIssue, IssueBookService } from '../../services/issue-book.service';

@Component({
  selector: 'app-return-book',
  standalone: true,
  templateUrl: './return-book.component.html',
  styleUrl: './return-book.component.css'
})
export class ReturnBookComponent implements OnInit {
  issues: BookIssue[] = [];
  isLoading = true;
  returningId: number | null = null;
  successMessage = '';
  errorMessage = '';

  constructor(private readonly issueBookService: IssueBookService) {}

  ngOnInit(): void {
    this.loadIssues();
  }

  get activeIssues(): BookIssue[] {
    return this.issues.filter((issue) => issue.returnedOn === null);
  }

  loadIssues(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.issueBookService.getBookIssues().pipe(finalize(() => this.isLoading = false)).subscribe({
      next: (issues) => this.issues = issues,
      error: (error: unknown) => this.errorMessage = apiErrorMessage(error, 'Unable to load issue history.')
    });
  }

  returnBook(issue: BookIssue): void {
    if (this.returningId !== null) return;
    this.successMessage = '';
    this.errorMessage = '';
    this.returningId = issue.id;
    this.issueBookService.returnBook(issue.id).pipe(finalize(() => this.returningId = null)).subscribe({
      next: () => {
        this.successMessage = 'Book returned successfully.';
        this.loadIssues();
      },
      error: (error: unknown) => this.errorMessage = apiErrorMessage(error, 'Unable to return the book.')
    });
  }
}
