import { type } from "@ngrx/signals";
import { eventGroup } from "@ngrx/signals/events";

export const tokenLoginEvents = eventGroup({
  source: "Login Page",
  events: {
    login: type<{ username: string; password: string }>(),
  },
});
