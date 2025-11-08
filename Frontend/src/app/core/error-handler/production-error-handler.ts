import { HttpErrorResponse } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { SentryErrorHandler } from "@sentry/angular";

@Injectable({
  providedIn: "root",
})
export class ProductionErroHandler extends SentryErrorHandler {
  private readonly router: Router = inject(Router);

  constructor() {
    super({ logErrors: false });
  }

  override handleError(error: unknown): void {
    if (error instanceof HttpErrorResponse && error.status === 401) {
      this.router.navigate([{ outlets: { overlay: "login" } }], { skipLocationChange: true });
      return;
    }

    super.handleError(error);
  }
}
