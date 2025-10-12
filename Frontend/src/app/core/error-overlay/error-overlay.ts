import {
  Component,
  ChangeDetectionStrategy,
  inject,
  Signal,
} from "@angular/core";
import { ErrorOverlayService } from "./error-overlay-service";
import { toSignal } from "@angular/core/rxjs-interop";
import { Button } from "../components/button/button";
import { ButtonStyle } from "../components/button/button-type";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: "app-error-overlay",
  imports: [Button],
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

  closePressed(): void {
    this.errorOverlayService.hideErrorOverlay();
  }

  get ButtonStyle(): typeof ButtonStyle {
    return ButtonStyle;
  }
}
