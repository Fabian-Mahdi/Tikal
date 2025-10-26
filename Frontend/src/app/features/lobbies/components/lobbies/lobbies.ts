import { Component, ChangeDetectionStrategy, inject } from "@angular/core";
import { Button } from "../../../../core/components/button/button";
import { ButtonStyle } from "../../../../core/components/button/button-type";
import { Router } from "@angular/router";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: "app-lobbies",
  imports: [Button],
  templateUrl: "./lobbies.html",
  styleUrl: "./lobbies.scss",
})
export class Lobbies {
  private readonly router: Router = inject(Router);

  async onLogoutPressed(): Promise<void> {
    this.router.navigate([""]);
  }

  get ButtonStyle(): typeof ButtonStyle {
    return ButtonStyle;
  }
}
