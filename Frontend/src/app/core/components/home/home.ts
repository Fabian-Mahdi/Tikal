import { Component, ChangeDetectionStrategy, inject } from "@angular/core";
import { NgOptimizedImage } from "@angular/common";
import { Button } from "../button/button";
import { ButtonStyle } from "../button/button-type";
import { Router } from "@angular/router";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: "app-home",
  imports: [NgOptimizedImage, Button],
  templateUrl: "./home.html",
  styleUrl: "./home.scss",
})
export class Home {
  readonly ButtonStyle = ButtonStyle;

  private readonly router = inject(Router);

  async onPlayOnlinePressed(): Promise<void> {
    this.router.navigate(["lobbies"]);
  }
}
