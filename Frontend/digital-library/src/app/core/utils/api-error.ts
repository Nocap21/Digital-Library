import { HttpErrorResponse } from '@angular/common/http';

export function apiErrorMessage(error: unknown, fallback = 'Something went wrong. Please try again.'): string {
  if (!(error instanceof HttpErrorResponse)) {
    return fallback;
  }

  const body = error.error as { message?: unknown; errors?: Record<string, unknown> } | null;
  if (typeof body?.message === 'string' && body.message.trim()) {
    return body.message;
  }

  if (body?.errors && typeof body.errors === 'object') {
    const validationMessage = Object.values(body.errors)
      .flatMap((messages) => Array.isArray(messages) ? messages : [messages])
      .find((message): message is string => typeof message === 'string' && !!message.trim());
    if (validationMessage) {
      return validationMessage;
    }
  }

  if (error.status === 400) {
    return 'Please check the entered information and try again.';
  }
  if (error.status === 404) {
    return 'The requested record was not found.';
  }
  if (error.status === 409) {
    return 'This request conflicts with the current library records.';
  }
  if (error.status === 0) {
    return 'Unable to connect to the library service. Please try again.';
  }

  return fallback;
}
