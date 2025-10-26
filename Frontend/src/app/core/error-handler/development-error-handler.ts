import { ErrorHandler, inject } from "@angular/core";
import { TimeoutError } from "rxjs";
import { LoadingOverlayService } from "../loading-overlay/loading-overlay-service";
import { ErrorOverlayService } from "../error-overlay/error-overlay-service";
import { HttpErrorResponse } from "@angular/common/http";
import { Router } from "@angular/router";

export class DevelopmentErrorHandler implements ErrorHandler {
  private readonly loadingOverlayService: LoadingOverlayService = inject(LoadingOverlayService);

  private readonly errorOverlayService: ErrorOverlayService = inject(ErrorOverlayService);

  private readonly router: Router = inject(Router);

  handleError(error: unknown): void {
    if (error instanceof HttpErrorResponse) {
      this.handleHttpError(error);
    } else if (error instanceof TimeoutError) {
      this.handleTimeout();
    }
  }

  private handleHttpError(error: HttpErrorResponse): void {
    if (error.status == 401) {
      this.router.navigate([{ outlets: { overlay: "login" } }]);
    }
  }

  private handleTimeout(): void {
    this.loadingOverlayService.hideLoadingOverlay();
    this.errorOverlayService.showErrorOverlay("A timeout has occurred");
  }
}
