import { Component, ChangeDetectionStrategy, inject } from "@angular/core";
import { NgOptimizedImage } from "@angular/common";
import { Button } from "../components/button/button";
import { ButtonStyle } from "../components/button/button-type";
import { Router } from "@angular/router";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: "app-home",
  imports: [NgOptimizedImage, Button],
  templateUrl: "./home.html",
  styleUrl: "./home.scss",
})
export class Home {
  private readonly router: Router = inject(Router);

  onPlayOnlinePressed(): void {
    this.router.navigate([{ outlets: { overlay: ["login"] } }]);
  }

  get ButtonType(): typeof ButtonStyle {
    return ButtonStyle;
  }
}
