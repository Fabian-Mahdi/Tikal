import { Component, ChangeDetectionStrategy, inject } from "@angular/core";
import { NgOptimizedImage } from "@angular/common";
import { Button } from "../components/button/button";
import { ButtonStyle } from "../components/button/button-type";
import { injectDispatch } from "@ngrx/signals/events";
import { activeAccountHomeEvents } from "../../features/authentication/stores/active-account/events/active-account-home-events";
import {
  ActiveAccountStatus,
  ActiveAccountStore,
} from "../../features/authentication/stores/active-account/active-account-store";
import { LoadingOverlay } from "../loading-overlay/loading-overlay";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: "app-home",
  imports: [NgOptimizedImage, Button, LoadingOverlay],
  templateUrl: "./home.html",
  styleUrl: "./home.scss",
})
export class Home {
  readonly ButtonStyle = ButtonStyle;

  readonly ActiveAccountStatus = ActiveAccountStatus;

  readonly accountStore = inject(ActiveAccountStore);

  private readonly dispatch = injectDispatch(activeAccountHomeEvents);

  async onPlayOnlinePressed(): Promise<void> {
    this.dispatch.getAccount();
  }
}
