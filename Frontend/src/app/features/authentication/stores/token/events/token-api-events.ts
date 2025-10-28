import { type } from "@ngrx/signals";
import { eventGroup } from "@ngrx/signals/events";

export const tokenApiEvents = eventGroup({
  source: "Identity API",
  events: {
    authenticated: type<string>(),
    authenticationFailed: type<void>(),
    error: type<unknown>(),
  },
});
