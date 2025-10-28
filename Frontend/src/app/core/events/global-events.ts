import { type } from "@ngrx/signals";
import { eventGroup } from "@ngrx/signals/events";

export const globalEvents = eventGroup({
  source: "Global",
  events: {
    logout: type<void>(),
  },
});
