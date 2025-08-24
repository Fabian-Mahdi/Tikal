import { animate, style, transition, trigger } from "@angular/animations";

export const mainLeaveAnimation = trigger("mainLeaveAnimation", [
  transition(
    ":leave",
    animate("0.4s ease", style({ transform: "translateY(-50%)" })),
  ),
]);

export const container1LeaveAnimation = trigger("container1LeaveAnimation", [
  transition(
    ":leave",
    animate("0.4s ease", style({ transform: "translate(100%, 40%)" })),
  ),
]);

export const container2LeaveAnimation = trigger("container2LeaveAnimation", [
  transition(
    ":leave",
    animate("0.4s ease", style({ transform: "translate(-90%, -45%)" })),
  ),
]);

export const container3LeaveAnimation = trigger("container3LeaveAnimation", [
  transition(
    ":leave",
    animate("0.4s ease", style({ transform: "translate(-105%, 40%)" })),
  ),
]);

export const container4LeaveAnimation = trigger("container4LeaveAnimation", [
  transition(
    ":leave",
    animate("0.4s ease", style({ transform: "translate(105%, -8%)" })),
  ),
]);
