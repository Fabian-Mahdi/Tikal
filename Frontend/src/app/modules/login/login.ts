import { Component, ChangeDetectionStrategy } from "@angular/core";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: "app-login",
  imports: [],
  templateUrl: "./login.html",
  styleUrl: "./login.scss",
})
export class Login {}
