import { Component, ChangeDetectionStrategy, inject } from "@angular/core";
import { NgOptimizedImage } from "@angular/common";
import { Router } from "@angular/router";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: "app-home",
  imports: [NgOptimizedImage],
  templateUrl: "./home.html",
  styleUrl: "./home.scss",
})
export class Home {
  private readonly router: Router = inject(Router);

  constructor() {
    document.addEventListener(
      "keydown",
      () => {
        this.router.navigate(["test"]);
      },
      { once: true },
    );
  }
}
