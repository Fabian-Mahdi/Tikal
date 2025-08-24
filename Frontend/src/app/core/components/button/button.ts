import {
  Component,
  ChangeDetectionStrategy,
  input,
  output,
} from "@angular/core";
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

  readonly clicked = output();

  onClicked(): void {
    this.clicked.emit();
  }

  get ButtonType(): typeof ButtonType {
    return ButtonType;
  }
}
