import { Component } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import { AuthService } from '../../core/services/auth.service';
import { apiErrorMessage } from '../../core/utils/api-error';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  submitted = false;
  isSubmitting = false;
  errorMessage = '';

  constructor(private readonly authService: AuthService, private readonly router: Router) {}

  onSubmit(form: NgForm): void {
    this.submitted = true;
    this.errorMessage = '';

    if (form.invalid || this.isSubmitting) {
      return;
    }

    this.isSubmitting = true;
    this.authService.login({ username: form.value.username, password: form.value.password })
      .pipe(finalize(() => this.isSubmitting = false))
      .subscribe({
        next: () => void this.router.navigateByUrl('/dashboard'),
        error: (error: unknown) => {
          this.errorMessage = apiErrorMessage(error, 'Unable to sign in. Please try again.');
        }
      });
  }
}
