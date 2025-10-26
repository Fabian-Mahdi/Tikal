import { on, withReducer } from "@ngrx/signals/events";
import { activeAccountApiEvents } from "./events/active-account-api-events";
import { activeAccountHomeEvents } from "./events/active-account-home-events";
import { ActiveAccountStatus } from "./active-account-store";
import { SignalStoreFeature, signalStoreFeature } from "@ngrx/signals";

const LoadingStarted = on(activeAccountHomeEvents.getAccount, () => ({
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

const LoadingFailed = on(activeAccountApiEvents.loadingFailed, () => ({
  status: ActiveAccountStatus.failure,
}));

export function withActiveAccountReducer(): SignalStoreFeature {
  return signalStoreFeature(withReducer(LoadingStarted, LoadingFailed, AccountFound, NoAccount));
}
