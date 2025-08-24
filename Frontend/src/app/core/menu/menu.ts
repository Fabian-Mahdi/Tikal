import { Component, ChangeDetectionStrategy } from "@angular/core";
import { backgroundFadeOut } from "./animations/fade-out";
import {
  container1LeaveAnimation,
  container2LeaveAnimation,
  container3LeaveAnimation,
  container4LeaveAnimation,
  mainLeaveAnimation,
} from "./animations/leave-animations";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: "app-menu",
  imports: [],
  templateUrl: "./menu.html",
  styleUrl: "./menu.scss",
  animations: [
    backgroundFadeOut,
    mainLeaveAnimation,
    container1LeaveAnimation,
    container2LeaveAnimation,
    container3LeaveAnimation,
    container4LeaveAnimation,
  ],
})
export class Menu {}
