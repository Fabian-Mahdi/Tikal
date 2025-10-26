import { type } from "@ngrx/signals";
import { eventGroup } from "@ngrx/signals/events";
import { Account } from "../../../models/account";

export const activeAccountApiEvents = eventGroup({
  source: "Tikal API",
  events: {
    // Create
    accountCreated: type<Account>(),
    duplicateAccount: type<void>(),
    createError: type<unknown>(),
    // Read
    accountLoaded: type<Account>(),
    noAccount: type<void>(),
    loadError: type<unknown>(),
  },
});
