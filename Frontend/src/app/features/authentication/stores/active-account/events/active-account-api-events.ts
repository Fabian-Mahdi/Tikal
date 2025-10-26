import { type } from "@ngrx/signals";
import { eventGroup } from "@ngrx/signals/events";
import { Account } from "../../../models/account";

export const activeAccountApiEvents = eventGroup({
  source: "Tikal API",
  events: {
    accountFound: type<Account>(),
    noAccount: type<void>(),
    error: type<unknown>(),
  },
});
