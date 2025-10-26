import { ChangeDetectionStrategy, Component } from "@angular/core";
import { RouterOutlet } from "@angular/router";
import { ErrorOverlay } from "./core/error-overlay/error-overlay";

@Component({
  selector: "app-root",
  imports: [RouterOutlet, ErrorOverlay],
  templateUrl: "./app.html",
  styleUrl: "./app.scss",
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class App {}
