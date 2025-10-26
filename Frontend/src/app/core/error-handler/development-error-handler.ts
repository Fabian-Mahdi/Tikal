import { ErrorHandler, inject } from "@angular/core";
import { HttpErrorResponse } from "@angular/common/http";
import { Router } from "@angular/router";

export class DevelopmentErrorHandler implements ErrorHandler {
  private readonly router: Router = inject(Router);

  handleError(error: unknown): void {
    if (error instanceof HttpErrorResponse) {
      this.handleHttpError(error);
    }
  }

  private handleHttpError(error: HttpErrorResponse): void {
    if (error.status == 401) {
      this.router.navigate([{ outlets: { overlay: "login" } }]);
    }
  }
}
