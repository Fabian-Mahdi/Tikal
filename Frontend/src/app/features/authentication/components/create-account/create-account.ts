import { Component, ChangeDetectionStrategy } from "@angular/core";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: "app-create-account",
  imports: [],
  templateUrl: "./create-account.html",
  styleUrl: "./create-account.scss",
})
export class CreateAccount {}
