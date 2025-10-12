import { ErrorHandler, inject } from "@angular/core";
import { TimeoutError } from "rxjs";
import { LoadingOverlayService } from "../loading-overlay/loading-overlay-service";
import { ErrorOverlayService } from "../error-overlay/error-overlay-service";

export class DevelopmentErrorHandler implements ErrorHandler {
  private readonly loadingOverlayService: LoadingOverlayService = inject(
    LoadingOverlayService,
  );

  private readonly errorOverlayService: ErrorOverlayService =
    inject(ErrorOverlayService);

  handleError(error: unknown): void {
    if (error instanceof TimeoutError) {
      this.handleTimeout();
    }
  }

  private handleTimeout(): void {
    this.loadingOverlayService.hideLoadingOverlay();
    this.errorOverlayService.showErrorOverlay("A timeout has occurred");
  }
}
