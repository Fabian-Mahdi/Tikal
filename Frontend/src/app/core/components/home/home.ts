import { Component, ChangeDetectionStrategy, inject } from "@angular/core";
import { NgOptimizedImage } from "@angular/common";
import { Button } from "../button/button";
import { LoadingOverlay } from "../../overlays/loading-overlay/loading-overlay";
import {
  AccountLoadingStatus,
  ActiveAccountStore,
} from "../../../features/authentication/stores/active-account/active-account-store";
import { injectDispatch } from "@ngrx/signals/events";
import { activeAccountHomeEvents } from "../../../features/authentication/stores/active-account/events/active-account-home-events";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: "app-home",
  imports: [NgOptimizedImage, Button, LoadingOverlay],
  templateUrl: "./home.html",
  styleUrl: "./home.scss",
})
export class Home {
  readonly AccountLoadingStatus = AccountLoadingStatus;

  readonly accountStore = inject(ActiveAccountStore);

  private readonly dispatch = injectDispatch(activeAccountHomeEvents);

  onPlayOnlinePressed(): void {
    this.dispatch.loadAccount();
  }
}
