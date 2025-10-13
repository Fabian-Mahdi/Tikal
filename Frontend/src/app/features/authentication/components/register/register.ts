import { Component, ChangeDetectionStrategy, inject } from "@angular/core";
import { Router } from "@angular/router";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: "app-register",
  imports: [],
  templateUrl: "./register.html",
  styleUrl: "./register.scss",
})
export class Register {
  private readonly router: Router = inject(Router);

  onClick(): void {
    this.router.navigate([""]);
  }
}
