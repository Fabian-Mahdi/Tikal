import { on, withReducer } from "@ngrx/signals/events";
import { activeAccountApiEvents } from "./events/active-account-api-events";
import { activeAccountHomeEvents } from "./events/active-account-home-events";
import { ActiveAccountStatus } from "./active-account-store";
import { SignalStoreFeature, signalStoreFeature } from "@ngrx/signals";

const GetAccount = on(activeAccountHomeEvents.getAccount, () => ({
  status: ActiveAccountStatus.loading,
}));

const AccountFound = on(activeAccountApiEvents.accountFound, ({ payload: account }) => ({
  status: ActiveAccountStatus.success,
  activeAccount: account,
}));

const NoAccount = on(activeAccountApiEvents.noAccount, () => ({
  status: ActiveAccountStatus.noAccount,
  activeAccount: null,
}));

const Error = on(activeAccountApiEvents.error, () => ({
  status: ActiveAccountStatus.failure,
}));

export function withActiveAccountReducer(): SignalStoreFeature {
  return signalStoreFeature(withReducer(GetAccount, Error, AccountFound, NoAccount));
}
