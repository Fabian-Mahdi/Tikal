import { Component, ChangeDetectionStrategy, inject } from "@angular/core";
import { NgOptimizedImage } from "@angular/common";
import { Button } from "../components/button/button";
import { ButtonStyle } from "../components/button/button-type";
import { Router, RouterOutlet } from "@angular/router";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: "app-home",
  imports: [NgOptimizedImage, Button, RouterOutlet],
  templateUrl: "./home.html",
  styleUrl: "./home.scss",
})
export class Home {
  private readonly router: Router = inject(Router);

  onPlayOnlinePressed(): void {
    this.router.navigate(["login"]);
  }

  get ButtonType(): typeof ButtonStyle {
    return ButtonStyle;
  }
}
