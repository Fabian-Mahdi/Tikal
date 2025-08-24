import { Component, ChangeDetectionStrategy, input } from "@angular/core";
import { ButtonType } from "./button-type";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: "app-button",
  imports: [],
  templateUrl: "./button.html",
  styleUrl: "./button.scss",
})
export class Button {
  readonly type = input.required<ButtonType>();

  readonly disabled = input<boolean>(false);

  get ButtonType(): typeof ButtonType {
    return ButtonType;
  }
}
