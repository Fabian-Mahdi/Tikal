import { ChangeDetectionStrategy, Component } from "@angular/core";
import { RouterOutlet } from "@angular/router";
import { LoadingOverlay } from "./core/loading-overlay/loading-overlay";
import { ErrorOverlay } from "./core/error-overlay/error-overlay";

@Component({
  selector: "app-root",
  imports: [RouterOutlet, LoadingOverlay, ErrorOverlay],
  templateUrl: "./app.html",
  styleUrl: "./app.scss",
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class App {}
