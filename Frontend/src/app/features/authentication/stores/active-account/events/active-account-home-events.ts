import { type } from "@ngrx/signals";
import { eventGroup } from "@ngrx/signals/events";

export const activeAccountHomeEvents = eventGroup({
  source: "Home Page",
  events: {
    getAccount: type<void>(),
  },
});
