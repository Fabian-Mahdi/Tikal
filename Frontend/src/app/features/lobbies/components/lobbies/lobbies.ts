import { Component, ChangeDetectionStrategy, inject } from "@angular/core";
import { Button } from "../../../../core/components/button/button";
import { ButtonStyle } from "../../../../core/components/button/button-type";
import { LogoutUseCase } from "../../../authentication/usecases/logout/logout-usecase";
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

  private readonly logout: LogoutUseCase = inject(LogoutUseCase);

  async onLogoutPressed(): Promise<void> {
    await this.logout.call();

    this.router.navigate([""]);
  }

  get ButtonStyle(): typeof ButtonStyle {
    return ButtonStyle;
  }
}

