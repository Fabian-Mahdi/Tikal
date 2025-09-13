import {
  Component,
  ChangeDetectionStrategy,
  inject,
  Signal,
} from "@angular/core";
import { LoadingOverlayService } from "./loading-overlay-service";
import { toSignal } from "@angular/core/rxjs-interop";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: "app-loading-overlay",
  imports: [],
  templateUrl: "./loading-overlay.html",
  styleUrl: "./loading-overlay.scss",
})
export class LoadingOverlay {
  readonly loadingOverlayService: LoadingOverlayService = inject(
    LoadingOverlayService,
  );

  readonly showLoadingOverlay: Signal<boolean> = toSignal(
    this.loadingOverlayService.loadingOverlay,
    { initialValue: false },
  );
}
