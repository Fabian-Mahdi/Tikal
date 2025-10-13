import { Component, ChangeDetectionStrategy, input, output } from "@angular/core";
import { ButtonStyle } from "./button-type";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: "app-button",
  imports: [],
  templateUrl: "./button.html",
  styleUrl: "./button.scss",
})
export class Button {
  readonly style = input.required<ButtonStyle>();

  readonly disabled = input<boolean>(false);

  readonly type = input<string>("button");

  readonly form = input<string>("");

  readonly clicked = output();

  onClicked(): void {
    this.clicked.emit();
  }

  get ButtonStyle(): typeof ButtonStyle {
    return ButtonStyle;
  }
}
