import { Component, ChangeDetectionStrategy } from "@angular/core";
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
    mainLeaveAnimation,
    container1LeaveAnimation,
    container2LeaveAnimation,
    container3LeaveAnimation,
    container4LeaveAnimation,
  ],
})
export class Menu {}
