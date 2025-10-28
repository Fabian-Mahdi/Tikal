import { Component, ChangeDetectionStrategy, inject } from "@angular/core";
import { Button } from "../../../../core/components/button/button";
import { ButtonStyle } from "../../../../core/components/button/button-type";
import { Dispatcher } from "@ngrx/signals/events";
import { globalEvents } from "../../../../core/events/global-events";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: "app-lobbies",
  imports: [Button],
  templateUrl: "./lobbies.html",
  styleUrl: "./lobbies.scss",
})
export class Lobbies {
  readonly ButtonStyle = ButtonStyle;

  private readonly dispatcher = inject(Dispatcher);

  async onLogoutPressed(): Promise<void> {
    this.dispatcher.dispatch(globalEvents.logout());
  }
}
