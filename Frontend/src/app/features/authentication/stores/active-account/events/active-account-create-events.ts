import { type } from "@ngrx/signals";
import { eventGroup } from "@ngrx/signals/events";

export const activeAccountCreateEvents = eventGroup({
  source: "Create Account Overlay",
  events: {
    createAccount: type<string>(),
    cancel: type<void>(),
  },
});
