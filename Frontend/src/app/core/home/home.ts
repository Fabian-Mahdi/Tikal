import { Component, ChangeDetectionStrategy, inject } from "@angular/core";
import { NgOptimizedImage } from "@angular/common";
import { Button } from "../components/button/button";
import { ButtonStyle } from "../components/button/button-type";
import { Router } from "@angular/router";
import { RefreshUseCase } from "../../features/authentication/usecases/refresh/refresh-usecase";
import { LoadingOverlayService } from "../loading-overlay/loading-overlay-service";
import { SetCurrentAccountUseCase } from "../../features/authentication/usecases/set-current-account/set-current-account-usecase";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: "app-home",
  imports: [NgOptimizedImage, Button],
  templateUrl: "./home.html",
  styleUrl: "./home.scss",
})
export class Home {
  private readonly router: Router = inject(Router);

  private readonly refresh: RefreshUseCase = inject(RefreshUseCase);

  private readonly setAccount: SetCurrentAccountUseCase = inject(
    SetCurrentAccountUseCase,
  );

  private readonly loadingOverlay: LoadingOverlayService = inject(
    LoadingOverlayService,
  );

  async onPlayOnlinePressed(): Promise<void> {
    this.loadingOverlay.showLoadingOverlay();

    const refreshResult = await this.refresh.call();

    if (refreshResult.isErr()) {
      this.loadingOverlay.hideLoadingOverlay();
      this.router.navigate([{ outlets: { overlay: ["login"] } }]);
      return;
    }

    const accountResult = await this.setAccount.call();

    if (accountResult.isErr()) {
      this.loadingOverlay.hideLoadingOverlay();
      this.router.navigate([{ outlets: { overlay: ["createaccount"] } }]);
      return;
    }

    this.loadingOverlay.hideLoadingOverlay();
  }

  get ButtonType(): typeof ButtonStyle {
    return ButtonStyle;
  }
}
