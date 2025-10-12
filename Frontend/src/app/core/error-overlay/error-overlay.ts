import {
  Component,
  ChangeDetectionStrategy,
  inject,
  Signal,
} from "@angular/core";
import { ErrorOverlayService } from "./error-overlay-service";
import { toSignal } from "@angular/core/rxjs-interop";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: "app-error-overlay",
  imports: [],
  templateUrl: "./error-overlay.html",
  styleUrl: "./error-overlay.scss",
})
export class ErrorOverlay {
  private readonly errorOverlayService: ErrorOverlayService =
    inject(ErrorOverlayService);

  readonly showErrorOverlay: Signal<boolean> = toSignal(
    this.errorOverlayService.errorOverlay,
    { initialValue: false },
  );

  readonly errorMessage: Signal<string> = toSignal(
    this.errorOverlayService.errorMessage,
    { initialValue: "" },
  );
}
