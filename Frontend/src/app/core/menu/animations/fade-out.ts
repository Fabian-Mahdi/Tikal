import { animate, animateChild, group, query, style, transition, trigger } from "@angular/animations";

export const backgroundFadeOut = trigger("backgroundFadeOut", [
  transition(":leave", group([animate("0.4s ease", style({ opacity: "0" })), query("@*", animateChild())])),
]);
