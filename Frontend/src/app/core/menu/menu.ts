import {
  Component,
  ChangeDetectionStrategy,
  inject,
  OnInit,
} from "@angular/core";
import { Router } from "@angular/router";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: "app-menu",
  imports: [],
  templateUrl: "./menu.html",
  styleUrl: "./menu.scss",
})
export class Menu implements OnInit {
  show = false;

  private readonly router: Router = inject(Router);

  ngOnInit(): void {
    this.show = true;
  }

  leave(): void {
    this.show = false;
    setTimeout(() => {
      this.router.navigate(["/"]);
    }, 200);
  }
}
